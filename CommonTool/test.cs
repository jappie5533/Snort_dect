using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Policy;
using System.Globalization;

namespace CommonTool
{
    /// <summary>
    /// Loads an assembly into a new AppDomain and obtains all the
    /// namespaces in the loaded Assembly, which are returned as a 
    /// List. The new AppDomain is then Unloaded.
    /// 
    /// This class creates a new instance of a 
    /// <c>AssemblyLoader</c> class
    /// which does the actual ReflectionOnly loading 
    /// of the Assembly into
    /// the new AppDomain.
    /// </summary>
    public class SeperateAppDomainAssemblyLoader
    {
        #region Public Methods
        /// <summary>
        /// Loads an assembly into a new AppDomain and obtains all the
        /// namespaces in the loaded Assembly, which are returned as a 
        /// List. The new AppDomain is then Unloaded
        /// </summary>
        /// <param name="assemblyLocation">The Assembly file 
        /// location</param>
        /// <returns>A list of found namespaces</returns>
        public Type[] LoadAssembly(FileInfo assemblyLocation)
        {
            if (string.IsNullOrEmpty(assemblyLocation.Directory.FullName))
            {
                throw new InvalidOperationException(
                    "Directory can't be null or empty.");
            }

            if (!Directory.Exists(assemblyLocation.Directory.FullName))
            {
                throw new InvalidOperationException(
                   string.Format(CultureInfo.CurrentCulture,
                   "Directory not found {0}",
                   assemblyLocation.Directory.FullName));

            }

            AppDomain childDomain = BuildChildDomain(
                AppDomain.CurrentDomain);

            try
            {
                Type loaderType = typeof(AssemblyLoader);
                if (loaderType.Assembly != null)
                {
                    var loader =
                        (AssemblyLoader)childDomain.
                            CreateInstanceFrom(
                            loaderType.Assembly.Location,
                            loaderType.FullName).Unwrap();

                    loader.LoadAssembly(
                        assemblyLocation.FullName);

                    return loader.GetTypes(
                        assemblyLocation.Directory.FullName);
                }
                return null;
            }

            finally
            {

                AppDomain.Unload(childDomain);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a new AppDomain based on the parent AppDomains 
        /// Evidence and AppDomainSetup
        /// </summary>
        /// <param name="parentDomain">The parent AppDomain</param>
        /// <returns>A newly created AppDomain</returns>
        private AppDomain BuildChildDomain(AppDomain parentDomain)
        {
            Evidence evidence = new Evidence(parentDomain.Evidence);
            AppDomainSetup setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain("DiscoveryRegion",
                evidence, setup);
        }
        #endregion


        /// <summary>
        /// Remotable AssemblyLoader, this class 
        /// inherits from <c>MarshalByRefObject</c> 
        /// to allow the CLR to marshall
        /// this object by reference across 
        /// AppDomain boundaries
        /// </summary>
        class AssemblyLoader : MarshalByRefObject
        {
            #region Private/Internal Methods
            /// <summary>
            /// Gets namespaces for ReflectionOnly Loaded Assemblies
            /// </summary>
            /// <param name="path">The path to the Assembly</param>
            /// <returns>A List of namespace strings</returns>
            [SuppressMessage("Microsoft.Performance",
                "CA1822:MarkMembersAsStatic")]
            internal Type[] GetTypes(string path)
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                ResolveEventHandler resolveEventHandler =
                    (s, e) =>
                    {
                        return OnReflectionOnlyResolve(
                            e, directory);
                    };

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve
                    += resolveEventHandler;

                Assembly reflectionOnlyAssembly =
                    AppDomain.CurrentDomain.
                        ReflectionOnlyGetAssemblies().First();

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve
                    -= resolveEventHandler;

                return reflectionOnlyAssembly.GetTypes();

            }

            /// <summary>
            /// Attempts ReflectionOnlyLoad of current 
            /// Assemblies dependants
            /// </summary>
            /// <param name="args">ReflectionOnlyAssemblyResolve 
            /// event args</param>
            /// <param name="directory">The current Assemblies 
            /// Directory</param>
            /// <returns>ReflectionOnlyLoadFrom loaded
            /// dependant Assembly</returns>
            private Assembly OnReflectionOnlyResolve(
                ResolveEventArgs args, DirectoryInfo directory)
            {

                Assembly loadedAssembly =
                    AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                        .FirstOrDefault(
                          asm => string.Equals(asm.FullName, args.Name,
                              StringComparison.OrdinalIgnoreCase));

                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                AssemblyName assemblyName =
                    new AssemblyName(args.Name);
                string dependentAssemblyFilename =
                    Path.Combine(directory.FullName,
                    assemblyName.Name + ".dll");

                if (File.Exists(dependentAssemblyFilename))
                {
                    return Assembly.ReflectionOnlyLoadFrom(
                        dependentAssemblyFilename);
                }
                return Assembly.ReflectionOnlyLoad(args.Name);
            }


            /// <summary>
            /// ReflectionOnlyLoad of single Assembly based on 
            /// the assemblyPath parameter
            /// </summary>
            /// <param name="assemblyPath">The path to the Assembly</param>
            [SuppressMessage("Microsoft.Performance",
                "CA1822:MarkMembersAsStatic")]
            internal void LoadAssembly(String assemblyPath)
            {
                try
                {
                    Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                }
                catch (FileNotFoundException)
                {
                    /* Continue loading assemblies even if an assembly
                     * can not be loaded in the new AppDomain. */
                }
            }
            #endregion
        }
    }
}
