using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CommonTool;
using System.Data;
using IP = System.String;
using PublicKey = System.String;
using String = System.String;
using HubLogInfo = BootstrapTools.CD2BSObject.HubLogInfo;
using System.ComponentModel;
using System.Net;

namespace BootstrapTools
{
    public class CD2SQLUtility
    {
        private string connString;
        private int agentColumnCount = 11;

        public CD2SQLUtility()
        {
            connString = string.Format("Data source={0};Initial Catalog=CD2DB;User ID=sa;Password=a205EIAB", CD2Constant.SQLServer);
        }

        public CD2SQLUtility(string server, string db, string user, string pwd)
        {
            connString = "Data source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pwd;
        }

        public BootstrapTools.CD2BSObject.GeoInformation GetHubGeoInfo(string ip)
        {
            try
            {
                string[] splitedIP = ip.Split('.');
                long integer_ip = Convert.ToInt64(splitedIP[0]) * 16777216
                                + Convert.ToInt64(splitedIP[1]) * 65536
                                + Convert.ToInt64(splitedIP[2]) * 256
                                + Convert.ToInt64(splitedIP[3]);

                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT city, country, latitude, longitude FROM GeoIPBlocks, GeoIPLocation WHERE @integer_ip BETWEEN startIpNum AND endIpNum AND GeoIPBlocks.locId = GeoIPLocation.locId";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@integer_ip", SqlDbType.BigInt).Value = integer_ip;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (rd.HasRows)
                    {
                        rd.Read();
                        BootstrapTools.CD2BSObject.GeoInformation Geo = new BootstrapTools.CD2BSObject.GeoInformation
                                                                            {
                                                                                City = rd["city"].ToString(),
                                                                                Country = rd["country"].ToString(),
                                                                                Latitude = Convert.ToDouble(rd["latitude"]),
                                                                                Longitude = Convert.ToDouble(rd["longitude"])
                                                                            };
                        rd.Close();

                        return Geo;
                    }
                    else
                        return null;
                }
            }
            catch(SqlException sqlEx)
            {
                return null;
            }
        }

        public void SetHubInfo(ref BootstrapTools.CD2BSObject.HubInformation hi)
        {
            try
            {
                string connections = string.Empty;
                foreach (var kvp in hi.ConnectionList)
                    if (string.IsNullOrEmpty(connections))
                        connections = kvp.Key;
                    else
                        connections = string.Format("{0},{1}", connections, kvp.Key);

                hi.GeoInfo = GetHubGeoInfo(hi.IP.ToString());
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    _conn.Open();
                    string cmd = "INSERT INTO Hub(id, ip, public_key, private_key, timestamp, account, city, country, connections) VALUES(@id, @ip, @public_key, @private_key, @timestamp, @account, @city, @country, @connections)";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = hi.ID;
                    sqlcmd.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = hi.IP.ToString();
                    sqlcmd.Parameters.Add("@public_key", SqlDbType.Text).Value = hi.KP.PublicKey;
                    sqlcmd.Parameters.Add("@private_key", SqlDbType.Text).Value = hi.KP.PrivateKey;
                    sqlcmd.Parameters.Add("@timestamp", SqlDbType.BigInt).Value = hi.TS;
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = hi.Account;
                    sqlcmd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(hi.GeoInfo.City) ? "Uknown" : hi.GeoInfo.City;
                    sqlcmd.Parameters.Add("@country", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(hi.GeoInfo.Country) ? "Uknown" : hi.GeoInfo.Country;
                    sqlcmd.Parameters.Add("@connections", SqlDbType.Text).Value = connections;
                    sqlcmd.ExecuteNonQuery();

                    // Log hub start up information
                    cmd = "INSERT INTO HubLog(id, ip, start_timestamp, account, city, country) VALUES(@id, @ip, @start_timestamp, @account, @city, @country)";
                    sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = hi.ID;
                    sqlcmd.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = hi.IP.ToString();
                    sqlcmd.Parameters.Add("@start_timestamp", SqlDbType.BigInt).Value = hi.StartTime;
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = hi.Account;
                    sqlcmd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = hi.GeoInfo.City;
                    sqlcmd.Parameters.Add("@country", SqlDbType.VarChar, 20).Value = hi.GeoInfo.Country;
                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                foreach (SqlError err in sqlEx.Errors)
                {
                    if (err.Number == 2627) // Violation of primary key constraint. Cannot insert duplicate key.
                    {
                        DelHubInfo(hi);
                        SetHubInfo(ref hi);
                        break;
                    }
                }
            }
        }

        public void DelHubInfo(BootstrapTools.CD2BSObject.HubInformation hi)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    _conn.Open();
                    //TODO: The end_timestamp of the hub is null, must update it when the hub re-regist
                    // Log Hub end time
                    string cmd = "UPDATE HubLog SET end_timestamp = @endTime WHERE id = @id AND start_timestamp = @startTime";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@endTime", SqlDbType.BigInt).Value = DateTime.Now.ToBinary();
                    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = hi.ID;
                    sqlcmd.Parameters.Add("@startTime", SqlDbType.BigInt).Value = hi.StartTime;
                    sqlcmd.ExecuteNonQuery();

                    // Delete hub from hublist
                    cmd = "DELETE FROM Hub WHERE id = @id";
                    sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = hi.ID;
                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
            }
        }

        public Dictionary<string, string> GetHubsPubKey(Dictionary<string, string> dic)
        {
            try
            {
                if (dic.Count > 0)
                {
                    using (SqlConnection _conn = new SqlConnection(connString))
                    {
                        string cmd = "SELECT ip, public_key FROM Hub WHERE";
                        for (int i = 0; i < dic.Count; i++)
                        {
                            cmd += " ip = @ip" + i;
                            if (i + 1 != dic.Count)
                                cmd += " OR";
                        }
                        SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                        int idx = 0;
                        foreach (var kvp in dic)
                        {
                            sqlcmd.Parameters.Add("@ip" + idx, SqlDbType.VarChar, 15).Value = kvp.Key;
                            idx++;
                        }

                        _conn.Open();
                        SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (rd.Read())
                        {
                            dic[rd["ip"].ToString()] = rd["public_key"].ToString();
                        }
                        rd.Close();
                    }
                    return dic;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public void GetHubsInfo(ref Dictionary<long, BootstrapTools.CD2BSObject.HubInformation> hubs)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT * FROM Hub";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    long rebirth = DateTime.Now.ToBinary();

                    while (rd.Read())
                    {
                        BootstrapTools.CD2BSObject.HubInformation hi = new CD2BSObject.HubInformation
                                                                            {
                                                                                TS = rebirth,
                                                                                ID = Convert.ToInt64(rd["id"]),
                                                                                KP = new CD2BSObject.KeyPair
                                                                                {
                                                                                    PublicKey = rd["public_key"].ToString(),
                                                                                    PrivateKey = rd["private_key"].ToString()
                                                                                },
                                                                                IP = IPAddress.Parse(rd["ip"].ToString()),
                                                                                GeoInfo = new CD2BSObject.GeoInformation
                                                                                {
                                                                                    Country = rd["country"].ToString(),
                                                                                    City = rd["city"].ToString()
                                                                                },
                                                                                Account = rd["account"].ToString(),
                                                                                ConnectionList = new Dictionary<String,String>()
                                                                            };

                        foreach (string s in rd["connections"].ToString().Split(','))
                            hi.ConnectionList[s] = string.Empty;

                        hi.ConnectionList = GetHubsPubKey(hi.ConnectionList);

                        hubs.Add(hi.ID, hi);
                    }
                    rd.Close();
                }
            }
            catch (SqlException sqlEx)
            {
            }
        }

        public List<BootstrapTools.CD2BSObject.HubInformation> GetHubsInformation()
        {
            try
            {
                List<BootstrapTools.CD2BSObject.HubInformation> hubs = new List<CD2BSObject.HubInformation>();
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    SqlCommand sqlcmd = new SqlCommand("SELECT * FROM Hub", _conn);
                    _conn.Open();


                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (rd.Read())
                    {
                        BootstrapTools.CD2BSObject.HubInformation hi = new CD2BSObject.HubInformation
                        {
                            TS = Convert.ToInt64(rd["timestamp"]),
                            ID = Convert.ToInt64(rd["id"]),
                            KP = new CD2BSObject.KeyPair
                            {
                                PublicKey = rd["public_key"].ToString(),
                                PrivateKey = rd["private_key"].ToString()
                            },
                            IP = IPAddress.Parse(rd["ip"].ToString()),
                            GeoInfo = new CD2BSObject.GeoInformation
                            {
                                Country = rd["country"].ToString(),
                                City = rd["city"].ToString()
                            },
                            Account = rd["account"].ToString(),
                            ConnectionList = new Dictionary<String, String>()
                        };

                        foreach (string s in rd["connections"].ToString().Split(','))
                            hi.ConnectionList[s] = string.Empty;

                        hi.ConnectionList = GetHubsPubKey(hi.ConnectionList);

                        hubs.Add(hi);
                    }

                    return hubs;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public BootstrapTools.CD2BSObject.HubInformation GetHubInformation(string ip)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    SqlCommand sqlcmd = new SqlCommand("SELECT * FROM Hub WHERE ip = @ip", _conn);
                    sqlcmd.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = ip;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (rd.HasRows)
                    {
                        rd.Read();

                        BootstrapTools.CD2BSObject.HubInformation hi = new CD2BSObject.HubInformation
                        {
                            TS = Convert.ToInt64(rd["timestamp"]),
                            ID = Convert.ToInt64(rd["id"]),
                            KP = new CD2BSObject.KeyPair
                            {
                                PublicKey = rd["public_key"].ToString(),
                                PrivateKey = rd["private_key"].ToString()
                            },
                            IP = IPAddress.Parse(rd["ip"].ToString()),
                            GeoInfo = new CD2BSObject.GeoInformation
                            {
                                Country = rd["country"].ToString(),
                                City = rd["city"].ToString()
                            },
                            Account = rd["account"].ToString(),
                            ConnectionList = new Dictionary<String, String>()
                        };

                        foreach (string s in rd["connections"].ToString().Split(','))
                            hi.ConnectionList[s] = string.Empty;

                        hi.ConnectionList = GetHubsPubKey(hi.ConnectionList);

                        rd.Close();
                        return hi;
                    }

                    return null;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public BootstrapTools.CD2BSObject.HubInformation GetHubInformation(long id)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    SqlCommand sqlcmd = new SqlCommand("SELECT * FROM Hub WHERE id = @id", _conn);
                    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (rd.HasRows)
                    {
                        rd.Read();

                        BootstrapTools.CD2BSObject.HubInformation hi = new CD2BSObject.HubInformation
                                                                            {
                                                                                TS = Convert.ToInt64(rd["timestamp"]),
                                                                                ID = Convert.ToInt64(rd["id"]),
                                                                                KP = new CD2BSObject.KeyPair
                                                                                {
                                                                                    PublicKey = rd["public_key"].ToString(),
                                                                                    PrivateKey = rd["private_key"].ToString()
                                                                                },
                                                                                IP = IPAddress.Parse(rd["ip"].ToString()),
                                                                                GeoInfo = new CD2BSObject.GeoInformation
                                                                                {
                                                                                    Country = rd["country"].ToString(),
                                                                                    City = rd["city"].ToString()
                                                                                },
                                                                                Account = rd["account"].ToString(),
                                                                                ConnectionList = new Dictionary<String,String>()
                                                                            };

                        foreach (string s in rd["connections"].ToString().Split(','))
                            hi.ConnectionList[s] = string.Empty;

                        hi.ConnectionList = GetHubsPubKey(hi.ConnectionList);

                        rd.Close();
                        return hi;
                    }

                    return null;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public BootstrapTools.CD2BSObject.KeyPair GetAgentKey(string account)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT public_key, private_key FROM [User] WHERE account = @account";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (rd.HasRows)
                    {
                        rd.Read();
                        BootstrapTools.CD2BSObject.KeyPair kp = new CD2BSObject.KeyPair 
                                                                    { 
                                                                        PublicKey = rd["public_key"].ToString(),
                                                                        PrivateKey = rd["private_key"].ToString()
                                                                    };
                        rd.Close();
                        return kp;
                    }
                    return null;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        //public string GetAgentConnectedHub(string account)
        //{
        //    BootstrapTools.CD2BSObject.AgentInformation agent = GetUserVerboseInfo(account);
        //    return agent == null ? string.Empty : agent.Hub.IP.ToString();
        //}

        //public BootstrapTools.CD2BSObject.AgentInformation GetUserVerboseInfo(long ID)
        //{
        //    SqlConnection _conn = new SqlConnection(connString);
        //    string hubColumn = "Hub.ip AS HubIP, Hub.public_key AS HubPubKey";
        //    string userColumn = "[User].account, [User].uid, [User].email, [User].id, [User].ip, [User].public_key, [User].private_key, [User].agent_type, [User].hub_id, ";
        //    SqlCommand sqlcmd = new SqlCommand("SELECT " + userColumn + hubColumn + " FROM [User], Hub WHERE id = @id AND [User].hub_id = Hub.id", _conn);
        //    sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = ID;
        //    return _getUserInfo(sqlcmd, _conn);
        //}

        public SqlDataReader GetUserInfoWithDataReader(string account)
        {
            SqlConnection _conn = new SqlConnection(connString);
            SqlCommand sqlcmd = new SqlCommand("SELECT * FROM [User] WHERE account = @account", _conn);
            sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;

            _conn.Open();
            return sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public BootstrapTools.CD2BSObject.AgentInformation GetUserInfo(string account)
        {
            SqlConnection _conn = new SqlConnection(connString);
            SqlCommand sqlcmd = new SqlCommand("SELECT * FROM [User] WHERE account = @account", _conn);
            sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
            return _getUserInfo(sqlcmd, _conn);
        }

        //public BootstrapTools.CD2BSObject.AgentInformation GetUserVerboseInfo(string account)
        //{
        //    SqlConnection _conn = new SqlConnection(connString);
        //    string hubColumn = "Hub.ip AS HubIP, Hub.public_key AS HubPubKey";
        //    string userColumn = "[User].account, [User].uid, [User].email, [User].id, [User].ip, [User].public_key, [User].private_key, [User].agent_type, [User].hub_id, ";
        //    SqlCommand sqlcmd = new SqlCommand("SELECT " + userColumn + hubColumn + " FROM [User], Hub WHERE account = @account AND [User].hub_id = Hub.id", _conn);
        //    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
        //    return _getUserInfo(sqlcmd, _conn);
        //}

        private BootstrapTools.CD2BSObject.AgentInformation _getUserInfo(SqlCommand sqlcmd, SqlConnection _conn)
        {
            try
            {
                BootstrapTools.CD2BSObject.AgentInformation user = new BootstrapTools.CD2BSObject.AgentInformation();
                _conn.Open();
                SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rd.HasRows)
                {
                    rd.Read();
                    user.Type = Convert.ToInt32(rd["agent_type"]);
                    user.Key.PublicKey = rd["public_key"].ToString();
                    user.Key.PrivateKey = rd["private_key"].ToString();
                    user.Account = rd["account"].ToString();
                    user.ID = Convert.ToInt64(rd["id"]);
                    user.IP = rd["ip"].ToString();
                    user.Email = rd["email"].ToString();
                    user.UID = Convert.ToInt32(rd["UID"]);
                    if (rd.FieldCount > agentColumnCount) // TODO: Fixed here... 
                    {
                        user.Hub.ID = Convert.ToInt64(rd["hub_id"]); //Bug?! rd["hub_id"] == null
                        user.Hub.IP = IPAddress.Parse(rd["HubIP"].ToString());
                        user.Hub.KP.PublicKey = rd["HubPubKey"].ToString();
                    }
                    rd.Close();
                    return user;
                }
                _conn.Close();
                return null;
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public bool SetAgentInfo(AgentRegistRequestContent agent, BootstrapTools.CD2BSObject.KeyPair kp, long ID, string IP)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    _conn.Open();
                    if (UserAuthenticate(agent.Account, agent.Password))
                    {
                        string cmd = "UPDATE [User] SET id = @id, ip = @ip, public_key = @public_key, private_key = @private_key WHERE account = @account";
                        SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                        sqlcmd.Parameters.Add("@id", SqlDbType.BigInt).Value = ID;
                        sqlcmd.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = IP;
                        sqlcmd.Parameters.Add("@public_key", SqlDbType.Text).Value = kp.PublicKey;
                        sqlcmd.Parameters.Add("@private_key", SqlDbType.Text).Value = kp.PrivateKey;
                        sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = agent.Account;

                        sqlcmd.ExecuteNonQuery();
                        return true;
                    }
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        public bool UserSignUp(string account, string password, string email)
        {
            try 
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "INSERT INTO [User](account, password, email) VALUES(@account, @password, @email)";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    sqlcmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = password;
                    sqlcmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

                    _conn.Open();
                    if (sqlcmd.ExecuteNonQuery() == 1)
                        return true;
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        public bool UserChangePassword(string account, string newPassword, string oldPassword)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "UPDATE [User] SET password = @newpassword WHERE account = @account AND password = @oldpassword";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@newpassword", SqlDbType.VarChar, 50).Value = newPassword;
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    sqlcmd.Parameters.Add("@oldpassword", SqlDbType.VarChar, 50).Value = oldPassword;

                    _conn.Open();
                    if (sqlcmd.ExecuteNonQuery() == 1)
                        return true;
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        public bool UserAuthenticate(string account, string password)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT count(*) FROM [User] WHERE account = @account AND password = @password";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    sqlcmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = password;

                    _conn.Open();
                    if ((int)sqlcmd.ExecuteScalar() > 0) 
                        return true;
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        public void SetUserLogin(string account, long hubID)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "UPDATE [User] SET hub_id = @hub_id WHERE account = @account";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@hub_id", SqlDbType.BigInt).Value = hubID;
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;

                    _conn.Open();
                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
            }
        }

        public string GetUserNameByEmail(string email)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString)) 
                {
                    string cmd = "SELECT account FROM [User] WHERE email = @email";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (rd.HasRows)
                    {
                        rd.Read();
                        return rd["account"].ToString();
                    }
                    return string.Empty;
                }
            }
            catch (SqlException sqlEx)
            {
                return string.Empty;
            }
        }

        public bool IsAdmin(string account)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT isAdm FROM [User] WHERE account = @account";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (rd.HasRows)
                    {
                        rd.Read();
                        return Convert.ToBoolean(rd["isAdm"]);
                    }
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        public List<BootstrapTools.CD2BSObject.AgentInformation> GetUsersConnected()
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT account, email, ip, hub_id FROM [User]";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader();
                    List<BootstrapTools.CD2BSObject.AgentInformation> users = new List<BootstrapTools.CD2BSObject.AgentInformation>();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            BootstrapTools.CD2BSObject.AgentInformation user = new BootstrapTools.CD2BSObject.AgentInformation();
                            user.Account = rd["account"].ToString();
                            user.Email = rd["email"].ToString();
                            user.IP = rd["ip"].ToString();
                            if (!string.IsNullOrEmpty(rd["hub_id"].ToString()))
                            {
                                BootstrapTools.CD2BSObject.HubInformation hi = GetHubInformation(Convert.ToInt64(rd["hub_id"]));
                                if (hi != null)
                                    user.Hub.IP = hi.IP;
                            }
                            users.Add(user);
                        }
                    }
                    return users;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public List<BootstrapTools.CD2BSObject.AgentInformation> GetUsersInfo(string keyword)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT * FROM [User]";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        sqlcmd.CommandText += " WHERE account LIKE '%' + @keyword + '%' OR email LIKE '%' + @keyword + '%'";
                        sqlcmd.Parameters.Add("@keyword", SqlDbType.VarChar, 50).Value = keyword;
                    }

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader();
                    List<BootstrapTools.CD2BSObject.AgentInformation> users = new List<BootstrapTools.CD2BSObject.AgentInformation>();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            BootstrapTools.CD2BSObject.AgentInformation user = new BootstrapTools.CD2BSObject.AgentInformation();
                            user.UID = Convert.ToInt32(rd["uid"]);
                            user.Account = rd["account"].ToString();
                            user.Email = rd["email"].ToString();
                            user.Type = Convert.ToInt32(rd["agent_type"]);
                            user.IP = rd["ip"].ToString();
                            user.ID = Convert.ToInt64(rd["id"]);
                            users.Add(user);
                        }
                    }
                    return users;
                }
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public void UpdateUserInfo(int uid, string account, string email, int agentType)
        {
            try
            {
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "UPDATE [User] SET account = @account, email = @email, agent_type = @agent_type WHERE uid = @uid";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    sqlcmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                    sqlcmd.Parameters.Add("@agent_type", SqlDbType.Int).Value = agentType;
                    sqlcmd.Parameters.Add("@uid", SqlDbType.Int).Value = uid;

                    _conn.Open();
                    sqlcmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
 
            }
        }

        public void Log(List<LogData> logs)
        {
            try
            {
                DataTable dt = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(LogData));
                foreach (PropertyDescriptor p in props)
                    if (p.Name.CompareTo("DT") != 0)
                        dt.Columns.Add(p.Name, p.PropertyType);

                object[] values = new object[props.Count - 1];
                foreach (LogData log in logs)
                {
                    for (int i = 0; i < props.Count; i++)
                        if (props[i].Name.CompareTo("DT") != 0)
                            values[i] = props[i].GetValue(log);
                    dt.Rows.Add(values);
                }
                    
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    _conn.Open();
                    SqlTransaction trans = _conn.BeginTransaction();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_conn, SqlBulkCopyOptions.KeepIdentity, trans))
                    {
                        bulkCopy.DestinationTableName = "Log";
                        bulkCopy.WriteToServer(dt);
                    }

                    trans.Commit();
                }
            }
            catch (SqlException sqlEx)
            {
            }
        }

        public List<LogData> GetDateLog()
        {
            return GetDateLog(string.Empty, new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Local), DateTime.Now.AddHours(24));
        }

        public List<LogData> GetDateLog(string account, DateTime lowbound, DateTime upbound)
        {
            List<LogData> logs = GetLog(account, lowbound, upbound);
            var gLog = from log in logs
                       group log by new { log.Src, log.DT, log.Account } into g
                       select new LogData
                       {
                           Src = g.Key.Src,
                           Account = g.Key.Account,
                           DT = g.Key.DT,
                           DataLength = g.Sum(log => log.DataLength)
                       };

            return gLog.ToList();
        }

        public List<LogData> GetLog()
        {
            return GetLog(string.Empty, new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Local), DateTime.Now.AddHours(24));
        }

        public List<LogData> GetLog(string account, DateTime lowbound, DateTime upbound)
        {
            try
            {
                List<LogData> logs = new List<LogData>();
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    string cmd = "SELECT * FROM Log WHERE timestamp BETWEEN @lowbound AND @upbound";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    sqlcmd.Parameters.Add("@lowbound", SqlDbType.BigInt).Value = lowbound.ToBinary(); /*lowbound.ToTimeStamp();*/
                    sqlcmd.Parameters.Add("@upbound", SqlDbType.BigInt).Value = upbound.ToBinary(); /* Bug is here. hey hey A_A */
                    if (!string.IsNullOrEmpty(account))
                    {
                        sqlcmd.CommandText += " AND account = @account";
                        sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    }
                    sqlcmd.CommandText += " ORDER BY timestamp DESC";

                    _conn.Open();
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (rd.Read())
                    {
                        LogData log = new LogData();
                        log.Account = rd["account"].ToString();
                        log.Src = rd["src"].ToString();
                        log.TS = Convert.ToInt64(rd["timestamp"]);
                        log.DataLength = Convert.ToInt32(rd["data_length"]);
                        logs.Add(log);
                    }
                }

                return logs;
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }

        public List<HubLogInfo> GetHubLog(string account)
        {
            try
            {
                List<HubLogInfo> logs = new List<HubLogInfo>();
                using (SqlConnection _conn = new SqlConnection(connString))
                {
                    _conn.Open();
                    string cmd = "SELECT * FROM HubLog";
                    SqlCommand sqlcmd = new SqlCommand(cmd, _conn);
                    if (!string.IsNullOrEmpty(account))
                    {
                        sqlcmd.CommandText += " WHERE account = @account";
                        sqlcmd.Parameters.Add("@account", SqlDbType.VarChar, 50).Value = account;
                    }
                    SqlDataReader rd = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (rd.Read())
                    {
                        HubLogInfo log = new HubLogInfo();
                        log.IP = rd["ip"].ToString();
                        log.StartTime = DateTime.FromBinary(Convert.ToInt64(rd["start_timestamp"])).ToString();
                        if (!string.IsNullOrEmpty(rd["end_timestamp"].ToString()))
                            log.TimePeriod = (DateTime.FromBinary(Convert.ToInt64(rd["end_timestamp"])) - DateTime.FromBinary(Convert.ToInt64(rd["start_timestamp"]))).Seconds;
                        else
                            log.TimePeriod = (DateTime.Now - DateTime.FromBinary(Convert.ToInt64(rd["start_timestamp"]))).Seconds;
                        log.City = rd["city"].ToString();
                        log.Country = rd["country"].ToString();
                        log.Account = rd["account"].ToString();
                        logs.Add(log);
                    }
                }

                return logs;
            }
            catch (SqlException sqlEx)
            {
                return null;
            }
        }
    }
}
