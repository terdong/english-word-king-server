using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Abstract;
using EWK_Server.TeamGehem.Utility;
using EWK_Server.TeamGehem.DataModels;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;
using ewk_server_v2;

namespace EWK_Server.TeamGehem.Manager
{
    public class ConfigManager : Singleton<ConfigManager>, LoggerParent
    {
        ServerInfo server_info_;
        public static ServerInfo Server_Info { get { return Instance.server_info_; }           private set { } }
        public static LogInfo Log_Info       { get { return Instance.server_info_.log_info_; } private set { } }

        public ConfigManager()
        {
        }

        public void SetupServerConfigFile()
        {
            FileInfo file_info = new FileInfo( SystemStr.Server_Config_File );
            if ( file_info.Exists == false )
            {
                var server_info = InitializeConfig();
                WriterConfigFile( SystemStr.Server_Config_File, server_info );
            }

            server_info_ = ReadConfigFile<ServerInfo>(SystemStr.Server_Config_File);
            if ( server_info_ == null )
            {
                server_info_ = new ServerInfo();
                WriterConfigFile( SystemStr.Server_Config_File, server_info_ );
            }

            Console.WriteLine( server_info_.ToString() );
        }

        private ServerInfo InitializeConfig()
        {
            return new ServerInfo();
        }

        private T ReadConfigFile<T>( string read_file_path )
        {
            try
            {
                using ( StreamReader input = new StreamReader( read_file_path ) )
                {
                    var deserializer = new Deserializer( namingConvention: new UnderscoredNamingConvention() );
                    return deserializer.Deserialize<T>( input );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.ToString() );
            }
            return default(T);
        }

        private void WriterConfigFile(string create_path, Object graph)
        {
            using ( StreamWriter sw = File.CreateText( create_path ) )
            {
                var server_info = InitializeConfig();

                var serializer = new Serializer( namingConvention: new UnderscoredNamingConvention() );
                serializer.Serialize( sw, graph );
            }
        }

        #region LoggerParent 멤버

        public Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_system; }
        }

        public WebSocketSharp.LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        #endregion
    }
}
