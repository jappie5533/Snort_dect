using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceSDK;

namespace RAA
{
    public class RAASvc : ServiceBase
    {
        private string pas;

        public RAASvc() { }

        protected override void preExecute()
        {
            Console.WriteLine("PreExecute: Handling params...");
            pas = "";

            foreach (string s in Params)
                pas += s + " ";
        }

        protected override void onExecute()
        {
            Console.WriteLine("OnExecute: Executing Service...");
            Console.WriteLine("Virtual Gateway Run with Params: " + pas);
        }

        protected override void postExecute()
        {
            Console.WriteLine("PostExecute: Seting Result...");
            Result = "Exected.";
        }
    }
}
