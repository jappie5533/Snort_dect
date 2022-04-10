using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool
{
    public class CommandUtility
    {
        public static string ToJson(string str, out DPType type)
        {
            Command cmd = ToCommand(str);

            // Parsing msg command format: msg SENDER MESSAGE
            if (cmd.Cmd == "msg")
            {
                MessageContent mc = new MessageContent();

                if (cmd.Param.Length > 0)
                {
                    mc.Sender = cmd.Param[0].ToString();
                    if (cmd.Param.Length > 1)
                    {
                        mc.Content = cmd.Param[1].ToString();
                        for (int i = 2; i < cmd.Param.Length; i++)
                            mc.Content += " " + cmd.Param[i].ToString();
                    }

                    type = DPType.Msg;
                    return mc.ToJson();
                }
            }
            // Parsing run command format: run SERVICE LOG SENDER_TARGET PARAMS
            else if (cmd.Cmd == "run")
            {
                RunContent rc = new RunContent();
                try
                {
                    if (cmd.Param.Length >= 4)
                    {
                        rc.Service = cmd.Param[0].ToString();
                        rc.BackLog = cmd.Param[1].ToString();
                        rc.SenderTarget = cmd.Param[2].ToString();
                        rc.Params = cmd.Param[3].ToString();
                        for (int i = 4; i < cmd.Param.Length; i++)
                            rc.Params += " " + cmd.Param[i].ToString();

                        type = DPType.Run;
                        return rc.ToJson();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex.Message, ex.StackTrace);
                }
            }
            // Parsing pub command format: pub IP_ADDR {FILE_NAME, FILE_NAME, ...}
            else if (cmd.Cmd == "pub")
            {
                PublishContent pc = new PublishContent();

                if (cmd.Param.Length >= 2)
                {
                    pc.Target_IP_or_ID = cmd.Param[0].ToString();
                    for (int i = 1; i < cmd.Param.Length; i++)
                        pc.FileNames.Add(cmd.Param[i].ToString());

                    type = DPType.Publish_Service;
                    return pc.ToJson();
                }
            }

            type = DPType.None;
            return null;
        }

        public static string ToString(Command cmd)
        {
            string str_cmd = cmd.Cmd;

            foreach (object obj in cmd.Param)
            {
                str_cmd += " " + obj.ToString();
            }

            return str_cmd;
        }

        public static Command ToCommand(string str_cmd)
        {
            Command cmd = new Command();
            // TODO: Exception.
            List<object> param = new List<object>(str_cmd.Split(new Char[] { ' ' }));

            cmd.Cmd = param[0].ToString();
            param.Remove(param[0]);
            cmd.Param = param.ToArray();

            return cmd;
        }

        public class Command
        {
            public string Cmd { get; set; }
            public object[] Param { get; set; }
        }
    }
}
