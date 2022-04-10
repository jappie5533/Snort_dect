using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CommonTool;
using System.Data.SQLite;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.ComponentModel;
using System.Data.Common;
using System.Data;

namespace Co_Defend_Client_v2.Utils
{
    internal class SuspiciousTarget
    {
        internal string Target { get; set; }
        internal string Protocol { get; set; }
        internal string Date { get; set; }
        internal string Count { get; set; }
    }

    internal class SuspiciousCondition
    {
        internal string Where { get; set; }
        internal int LowerBound { get; set; }
    }

    internal class AgentAnalysisUtility
    {
        internal static AgentAnalysisUtility Instance
        { 
           get 
           {
               if (_instance == null)
                   _instance = new AgentAnalysisUtility();

               return _instance;
           } 
        }

        internal event EventHandler<EventArgs> onPcapToSQLiteCompleted;

        private static AgentAnalysisUtility _instance;

        private BackgroundWorker _worker;
        private SQLiteConnection _conn;
        private string _tableName;

        private AgentAnalysisUtility()
        {
            Initalize();   
        }

        /// <summary>
        /// Let Pcap file which with specific path writing to SQLite DB with specific table name.
        /// </summary>
        /// <param name="TableName">The SQLite database table name.</param>
        /// <param name="FilePath">The pcap file path.</param>
        /// <returns>if the background worker is busy, it will return false; otherwise return true.</returns>
        internal bool PcapToSQLite(string TableName, string FilePath)
        {
            if (!_worker.IsBusy)
            {
                _tableName = TableName;
                _worker.RunWorkerAsync(FilePath);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Let Pcap files which with specific path list writing to SQLite DB with specific table name.
        /// </summary>
        /// <param name="TableName">The SQLite database table name.</param>
        /// <param name="FileNames">The pcap file path lists.</param>
        /// <returns>if the background worker is busy, it will return false; otherwise return true.</returns>
        internal bool PcapToSQLite(string TableName, List<string> FileNames)
        {
            if (!_worker.IsBusy)
            {
                _tableName = TableName;
                _worker.RunWorkerAsync(FileNames);
                
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Obtain google data table's json format from SQLite database with specifc SQL command.
        /// </summary>
        /// <param name="sqlCmd">The specifc SQLite command.</param>
        /// <returns>Google data table's json string.</returns>
        internal string ObtainSQLiteDataWithSQLCommand(string sqlCmd)
        {
            DataTable dt;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            bool hasRows = false;

            using (_conn = new SQLiteConnection("Data Source = " + CD2Constant.Agent_PCAP_DB_Locate))
            {
                _conn.Open();

                cmd = _conn.CreateCommand();
                dt = new DataTable();

                cmd.CommandText = sqlCmd;
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    hasRows = true;
                    dt.Load(reader);
                }

                reader.Close();
                _conn.Close();
            }

            if (hasRows)
                return new Bortosky.Google.Visualization.GoogleDataTable(dt).GetJson();
            else
                return string.Empty;
        }

        internal DataTable ObtainSuspiciousTargetTable()
        {
            DataTable dataTable = new DataTable();
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            string tableName = ObtainRecentTableName();

            if (string.IsNullOrEmpty(tableName))
                return dataTable;

            dataTable.Columns.Add(new DataColumn("Blocked", typeof(bool)));
            
            using (_conn = new SQLiteConnection("Data Source = " + CD2Constant.Agent_PCAP_DB_Locate))
            {
                _conn.Open();

                // Select suspicious target of port scanning.
                using (cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        "SELECT Source_IP,Source_MAC,Destination_IP,count(*) as Port_Scan_Count FROM (SELECT * from {0} GROUP BY Source_IP,Source_MAC,Destination_IP,Destination_Port) GROUP BY Source_IP,Source_MAC,Destination_IP HAVING Port_Scan_Count > 5 ORDER BY Port_Scan_Count DESC", 
                        tableName);

                    reader = cmd.ExecuteReader();

                    dataTable.Load(reader);
                }

                // Select suspicious target of flooding.
                using (cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        "SELECT Source_IP,Source_MAC,Destination_IP,count(*) as Flooding_Count FROM (SELECT Time,Source_IP,Source_MAC,Destination_IP from {0} GROUP BY Time,Source_IP,Source_MAC,Destination_IP having count(*) >= 5) GROUP BY Source_IP,Source_MAC,Destination_IP HAVING Flooding_Count >= 3 ORDER BY Flooding_Count DESC",
                        tableName);

                    reader = cmd.ExecuteReader();

                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }

        internal string ObtainRecentTableName()
        {
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            string retVal = string.Empty;

            using (_conn = new SQLiteConnection("Data Source = " + CD2Constant.Agent_PCAP_DB_Locate))
            {
                _conn.Open();

                using (cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' and sql like '%(Time STRING, Source_IP STRING, Source_Port INT, Destination_IP STRING, Destination_Port INT, Protocol STRING, Length INT, TCP_Flag STRING, ICMP_Code STRING, ICMP_Type STRING, Source_MAC STRING)' ORDER BY name DESC limit 1";
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                        retVal = reader["name"].ToString();
                }
            }

            return retVal;
        }

        internal List<string> ObtainTableNames()
        {
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            List<string> retVal = new List<string>();

            using (_conn = new SQLiteConnection("Data Source = " + CD2Constant.Agent_PCAP_DB_Locate))
            {
                _conn.Open();

                using (cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' and sql like '%(Time STRING, Source_IP STRING, Source_Port INT, Destination_IP STRING, Destination_Port INT, Protocol STRING, Length INT, TCP_Flag STRING, ICMP_Code STRING, ICMP_Type STRING, Source_MAC STRING)' ORDER BY name DESC";
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retVal.Add(reader["name"].ToString());
                    }
                }
            }

            return retVal;
        }

        private void Initalize()
        {
            // Create background worker instance.
            _worker = new BackgroundWorker();

            // Check db file.
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, CD2Constant.Agent_PCAP_DB_Locate)))
                SQLiteConnection.CreateFile(Path.Combine(Environment.CurrentDirectory, CD2Constant.Agent_PCAP_DB_Locate));

            // Event regist.
            _worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                List<string> files = e.Argument as List<string>;
                string file = e.Argument as string;

                using (_conn = new SQLiteConnection("Data Source = " + CD2Constant.Agent_PCAP_DB_Locate))
                {
                    _conn.Open();

                    // Check Table
                    SQLiteCommand cmd = _conn.CreateCommand();
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + _tableName + "(Time STRING, Source_IP STRING, Source_Port INT, Destination_IP STRING, Destination_Port INT, Protocol STRING, Length INT, TCP_Flag STRING, ICMP_Code STRING, ICMP_Type STRING, Source_MAC STRING)";
                    cmd.ExecuteNonQuery();

                    DbTransaction trans = _conn.BeginTransaction();

                    try
                    {
                        if (files != null)
                        {
                            foreach (string f in files)
                            {
                                OfflinePacketDevice selectDevice = new OfflinePacketDevice(f);

                                using (PacketCommunicator communicator = selectDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                                {
                                    communicator.ReceivePackets(0, PacketHandler);
                                }
                            }
                        }
                        else if (file != null)
                        {
                            OfflinePacketDevice selectDevice = new OfflinePacketDevice(file);

                            using (PacketCommunicator communicator = selectDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                            {
                                communicator.ReceivePackets(0, PacketHandler);
                            }
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        _conn.Close();
                    }
                }
            };

            _worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (onPcapToSQLiteCompleted != null)
                    onPcapToSQLiteCompleted.Invoke(this, e);
            };
        }

        private void PacketHandler(Packet packet)
        {
            /*
             *             Time: packet.Timestamp
             *        Source IP: packet.Ethernet.IpV4.Source
             *      Source Port: packet.Ethernet.IpV4.Tcp.SourcePort / packet.Ethernet.IpV4.Udp.SourcePort
             *   Destination IP: packet.Ethernet.IpV4.Destination
             * Destination Port: packet.Ethernet.IpV4.Tcp.DestinationPort / packet.Ethernet.IpV4.Udp.DestinationPort
             *         Protocol: packet.Ethernet.IpV4.Protocol
             *           Length: packet.Length
             *         TCP Flag: packet.Ethernet.IpV4.Tcp.ControlBits
             *        ICMP code: packet.Ethernet.IpV4.Icmp.Code
             *        ICMP Type: packet.Ethernet.IpV4.Icmp.MessageType
             */

            if (packet.Ethernet.EtherType == PcapDotNet.Packets.Ethernet.EthernetType.IpV4)
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO " + _tableName + "(Time, Source_IP, Source_Port, Destination_IP, Destination_Port, Protocol, Length, TCP_Flag, ICMP_Code, ICMP_Type, Source_MAC) VALUES(@time, @sourceip, @sourceport, @destinationip, @destinationport, @protocol, @length, @tcpflag, @icmpcode, @icmptype, @sourcemac)";
                cmd.Parameters.Add("@time", System.Data.DbType.String).Value = packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff");
                cmd.Parameters.Add("@sourceip", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Source;
                cmd.Parameters.Add("@destinationip", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Destination;
                cmd.Parameters.Add("@protocol", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Protocol;
                cmd.Parameters.Add("@length", System.Data.DbType.Int32).Value = packet.Length;
                cmd.Parameters.Add("@sourcemac", System.Data.DbType.String).Value = packet.Ethernet.Source;

                switch (packet.Ethernet.IpV4.Protocol)
                {
                    case PcapDotNet.Packets.IpV4.IpV4Protocol.Tcp:
                        cmd.Parameters.Add("@sourceport", System.Data.DbType.Int32).Value = packet.Ethernet.IpV4.Tcp.SourcePort;
                        cmd.Parameters.Add("@destinationport", System.Data.DbType.Int32).Value = packet.Ethernet.IpV4.Tcp.DestinationPort;
                        cmd.Parameters.Add("@tcpflag", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Tcp.ControlBits.ToString();
                        cmd.Parameters.Add("@icmpcode", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmptype", System.Data.DbType.String).Value = string.Empty;
                        break;
                    case PcapDotNet.Packets.IpV4.IpV4Protocol.Udp:
                        cmd.Parameters.Add("@sourceport", System.Data.DbType.Int32).Value = packet.Ethernet.IpV4.Udp.SourcePort;
                        cmd.Parameters.Add("@destinationport", System.Data.DbType.Int32).Value = packet.Ethernet.IpV4.Udp.DestinationPort;
                        cmd.Parameters.Add("@tcpflag", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmpcode", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmptype", System.Data.DbType.String).Value = string.Empty;
                        break;
                    case PcapDotNet.Packets.IpV4.IpV4Protocol.InternetControlMessageProtocol:
                        cmd.Parameters.Add("@sourceport", System.Data.DbType.Int32).Value = null;
                        cmd.Parameters.Add("@destinationport", System.Data.DbType.Int32).Value = null;
                        cmd.Parameters.Add("@tcpflag", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmpcode", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Icmp.Code;
                        cmd.Parameters.Add("@icmptype", System.Data.DbType.String).Value = packet.Ethernet.IpV4.Icmp.MessageType;
                        break;
                    default:
                        cmd.Parameters.Add("@sourceport", System.Data.DbType.Int32).Value = null;
                        cmd.Parameters.Add("@destinationport", System.Data.DbType.Int32).Value = null;
                        cmd.Parameters.Add("@tcpflag", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmpcode", System.Data.DbType.String).Value = string.Empty;
                        cmd.Parameters.Add("@icmptype", System.Data.DbType.String).Value = string.Empty;
                        break;
                }

                cmd.ExecuteNonQuery();
            }
        }
    }
}
