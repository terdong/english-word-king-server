using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using WebSocketSharp;

namespace EWK_Server.TeamGehem.DataModels
{
    public class ServerInfo
    {
        [YamlMemberAttribute()]
        public LogInfo log_info_ {get;set;}

        public ServerInfo()
        {
            log_info_ = new LogInfo();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( log_info_ );

            return sb.ToString();
        }
    }

    public class LogInfo
    {
        public LogLevel log_level { get; set; }
        public string log_db_url { get; set; }
        public string log_db_name { get; set; }
        public string log_file_name { get; set; }

        public LogInfo()
        {
            log_file_name = "log_file_name";
            log_level = LogLevel.Debug;
            log_db_url = "mongodb://localhost";
            log_db_name = "ewk_log_db";
        }

        public override string ToString()
        {
            return string.Format(
                "Log_Info:\n\tlog_level: {0}\n\tlog_db_url: {1}\n\tlog_db_name: {2}\n\tlog_file_name: {3}\n"
                , log_level.ToString(), log_db_url, log_db_name, log_file_name
                );
        }
    }
}
