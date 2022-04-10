using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using CommonTool;

namespace Hub
{
    public class CD2SQLiteUtility
    {
        SQLiteConnection conn;

        public CD2SQLiteUtility()
        {
            string connString = @"Data Source=:memory:";
            conn = new SQLiteConnection(connString);
            conn.Open();
        }

        public void Initialize()
        {
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS log(src STRING, timestamp INTEGER, account STRING, data_length INTEGER, isSend BOOLEAN DEFAULT false, PRIMARY KEY(src, timestamp))";
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
 
            }
        }

        public bool Log(long id, string account, DataProtocol dp)
        {
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM log WHERE src = @src AND timestamp = @timestamp";
                cmd.Parameters.Add("@src", System.Data.DbType.String).Value = dp.Src;
                cmd.Parameters.Add("@timestamp", System.Data.DbType.Int64).Value = dp.TS;
                using (SQLiteDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.HasRows)
                        return false;
                }
                cmd.CommandText = "INSERT INTO log(src, timestamp, account, data_length) VALUES(@src, @timestamp, @account, @data_length)";
                cmd.Parameters.Add("@src", System.Data.DbType.String).Value = dp.Src;
                cmd.Parameters.Add("@timestamp", System.Data.DbType.Int64).Value = dp.TS;
                cmd.Parameters.Add("@account", System.Data.DbType.String).Value = account;
                cmd.Parameters.Add("@data_length", System.Data.DbType.Int32).Value = dp.Content.Length;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        public List<LogData> LoadUnSendLog(int count)
        {
            try
            {
                List<LogData> logs = new List<LogData>();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM log WHERE isSend = 'false'";
                if (count != 0)
                {
                    cmd.CommandText += " LIMIT 0, @count";
                    cmd.Parameters.Add("@count", System.Data.DbType.Int32).Value = count;
                }

                SQLiteDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    LogData log = new LogData();
                    log.Src = rd["src"].ToString();
                    log.TS = Convert.ToInt64(rd["timestamp"]);
                    log.Account = rd["account"].ToString();
                    log.DataLength = Convert.ToInt32(rd["data_length"]);
                    logs.Add(log);
                }

                foreach (LogData log in logs)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE log SET isSend = 'true' WHERE src = @src AND timestamp = @timestamp";
                    cmd.Parameters.Add("@src", System.Data.DbType.String).Value = log.Src;
                    cmd.Parameters.Add("@timestamp", System.Data.DbType.Int64).Value = log.TS;
                    cmd.ExecuteNonQuery();
                }
                return logs;
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        public void DeleteSendedLog()
        {
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM log WHERE isSend = 'true'";
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {

            }
        }
    }
}
