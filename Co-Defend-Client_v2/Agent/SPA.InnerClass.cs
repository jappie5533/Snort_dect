using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.Runtime.InteropServices;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Base;
using PcapDotNet.Packets.Arp;
using CommonTool;
using System.Net;
using System.Threading;

namespace Co_Defend_Client_v2.Agent
{
    partial class SPA
    {
        internal class DiscoverNetworkPC
        {
            internal event EventHandler<EventArgs> onDetectionComplete;

            [DllImport("Ws2_32.dll", EntryPoint = "inet_addr", CharSet = CharSet.Ansi)]
            private static extern Int32 inet_addr(string ipaddr);

            [DllImport("Iphlpapi.dll", EntryPoint = "SendARP", CharSet = CharSet.Ansi)]
            private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);

            private BackgroundWorker t_FilterARPPackets;
            private PacketCommunicator communicator;
            private PacketDevice selectedDevice;
            private Timer timer;

            private Dictionary<string, string> _detectionPCList;
            private string _localIPAddress;
            private string _localMacAddress;
            private string tid;

            internal List<string> DetectionResult;

            internal void Initialize(string IP, string Mask, string tid)
            {
                this.tid = tid;

                if (t_FilterARPPackets == null)
                {
                    t_FilterARPPackets = new BackgroundWorker();
                    t_FilterARPPackets.DoWork += new DoWorkEventHandler(t_FilterARPPackets_DoWork);
                }

                IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
                foreach (var device in allDevices)
                {
                    foreach (DeviceAddress address in device.Addresses)
                        if (address.Address.ToString() == string.Format("Internet {0}", IP))
                            selectedDevice = device;
                }
                communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);

                this._localIPAddress = IP;
                this._localMacAddress = GetMacAddress(_localIPAddress);

                _detectionPCList = new Dictionary<string, string>();
                // Detection range...
                string networkAddress = DataUtility.NetworkAddress(IP, Mask);
                uint range = uint.MaxValue - (Convert.ToUInt32(Mask.Split('.')[0]) * 16777216
                                            + Convert.ToUInt32(Mask.Split('.')[1]) * 65536
                                            + Convert.ToUInt32(Mask.Split('.')[2]) * 256
                                            + Convert.ToUInt32(Mask.Split('.')[3]));

                int[] ips = new int[4];
                for (int j = 0; j < 4; j++)
                    ips[j] = int.Parse(networkAddress.Split('.')[j]);

                for (int i = 0; i < range; i++)
                {
                    ips[3]++;
                    if (ips[3] > 255) { ips[2]++; ips[3] = 0; }
                    if (ips[2] > 255) { ips[1]++; ips[2] = 0; }

                    if (ips[3] == 0 || ips[3] == 255)
                        continue;

                    _detectionPCList[string.Format("{0}.{1}.{2}.{3}", ips[0], ips[1], ips[2], ips[3])] = "#";
                }
            }

            internal void StartDetection()
            {
                if (!t_FilterARPPackets.IsBusy)
                    t_FilterARPPackets.RunWorkerAsync();

                using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
                {
                    Dictionary<string, string> tmp = new Dictionary<string, string>(_detectionPCList);
                    foreach (var kvp in tmp)
                    {
                        EthernetLayer ethernetLayer = new EthernetLayer
                        {
                            Source = new MacAddress(_localMacAddress),
                            Destination = new MacAddress("ff:ff:ff:ff:ff:ff"),
                            EtherType = EthernetType.None
                        };

                        ArpLayer arpLayer = new ArpLayer
                        {
                            ProtocolType = EthernetType.IpV4,
                            Operation = ArpOperation.Request,
                            SenderHardwareAddress = new byte[] 
                        {
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
                            Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
                        }.AsReadOnly(),
                            SenderProtocolAddress = new byte[]
                        {
                            Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[0])),
                            Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[1])),
                            Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[2])),
                            Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[3])),
                        }.AsReadOnly(),
                            TargetHardwareAddress = new byte[] { 0, 0, 0, 0, 0, 0 }.AsReadOnly(),
                            TargetProtocolAddress = new byte[] 
                        {
                            Convert.ToByte(kvp.Key.Split('.')[0]),
                            Convert.ToByte(kvp.Key.Split('.')[1]),
                            Convert.ToByte(kvp.Key.Split('.')[2]),
                            Convert.ToByte(kvp.Key.Split('.')[3])
                        }.AsReadOnly()
                        };

                        PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
                        communicator.SendPacket(builder.Build(DateTime.Now));
                    }
                }

                timer = new Timer(new TimerCallback(delegate(object obj)
                                     {
                                         Completed();
                                     }),
                                     null, CD2Constant.Hub_Select_Timeout, 0
                                 );
            }

            #region Not use forever?!
            //private void Detect(string IP)
            //{
            //    using (PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000))
            //    {
            //        EthernetLayer ethernetLayer = new EthernetLayer
            //        {
            //            Source = new MacAddress(_localMacAddress),
            //            Destination = new MacAddress("ff:ff:ff:ff:ff:ff"),
            //            EtherType = EthernetType.None
            //        };

            //        ArpLayer arpLayer = new ArpLayer
            //        {
            //            ProtocolType = EthernetType.IpV4,
            //            Operation = ArpOperation.Request,
            //            SenderHardwareAddress = new byte[] 
            //            {
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[0], System.Globalization.NumberStyles.HexNumber)),
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[1], System.Globalization.NumberStyles.HexNumber)),
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[2], System.Globalization.NumberStyles.HexNumber)),
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[3], System.Globalization.NumberStyles.HexNumber)),
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[4], System.Globalization.NumberStyles.HexNumber)),
            //                Convert.ToByte(Int32.Parse(_localMacAddress.Split(':')[5], System.Globalization.NumberStyles.HexNumber))
            //            }.AsReadOnly(),
            //                SenderProtocolAddress = new byte[]
            //            {
            //                Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[0])),
            //                Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[1])),
            //                Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[2])),
            //                Convert.ToByte(Convert.ToInt32(_localIPAddress.Split('.')[3])),
            //            }.AsReadOnly(),
            //                TargetHardwareAddress = new byte[] { 0, 0, 0, 0, 0, 0 }.AsReadOnly(),
            //                TargetProtocolAddress = new byte[] 
            //            {
            //                Convert.ToByte(IP.Split('.')[0]),
            //                Convert.ToByte(IP.Split('.')[1]),
            //                Convert.ToByte(IP.Split('.')[2]),
            //                Convert.ToByte(IP.Split('.')[3])
            //            }.AsReadOnly()
            //        };

            //        PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);
            //        communicator.SendPacket(builder.Build(DateTime.Now));
            //    }
            //}
            #endregion

            private void t_FilterARPPackets_DoWork(object sender, DoWorkEventArgs e)
            {
                communicator.ReceivePackets(0, PacketHandler);
            }

            private void PacketHandler(Packet packet)
            {
                if (packet.Ethernet.EtherType == EthernetType.Arp && packet.Ethernet.Destination.ToString() == _localMacAddress.ToUpper())
                {
                    string ip = string.Format("{0}.{1}.{2}.{3}", packet.Ethernet.Arp.SenderProtocolAddress[0],
                                                                 packet.Ethernet.Arp.SenderProtocolAddress[1],
                                                                 packet.Ethernet.Arp.SenderProtocolAddress[2],
                                                                 packet.Ethernet.Arp.SenderProtocolAddress[3]);

                    if (_detectionPCList.ContainsKey(ip))
                    {
                        if (_detectionPCList[ip] != string.Empty && _detectionPCList[ip] != packet.Ethernet.Source.ToString())
                        {
                            //TODO: arp spoofing exist, invoke event?!

                        }

                        _detectionPCList[ip] = packet.Ethernet.Source.ToString();

                        if (!_detectionPCList.Values.Contains("#"))
                            Completed();
                    }
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

            private void Completed()
            {
                DetectionResult = _detectionPCList.Where(x => x.Value != "#").Select(pair => pair.Key).ToList();
                DetectionResult.Add(tid);

                // Invoke complete event..
                if (onDetectionComplete != null)
                    onDetectionComplete.Invoke(this, new EventArgs());

                // Stop the packet listener
                communicator.Break();

                if (t_FilterARPPackets != null)
                    t_FilterARPPackets.Dispose();

                if (timer != null)
                    timer.Dispose();
            }
        }
    }
}

