using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EWK_Server.TeamGehem.DataModels;
using WebSocketSharp;
using EWK_Server.TeamGehem.Utility;

namespace EWK_Server.TeamGehem.Manager
{
    public class LoggerWrapper
    {
        private Logger logger_;
        private MongoCollection<LogEntity> mongocl_;
        private LogEntity log_entity_;

        public LoggerWrapper( string user_name, string collection_name, LogLevel log_level)
        {
            InitializeMongoDb( collection_name );
            InitializeLogger(user_name, log_level);
        }

        public LoggerWrapper( string user_name, string collection_name )
            : this( user_name, collection_name, LogLevel.Info ) { }

        public void Info( string string_format, params object[] objs )
        {
            string message = string.Format( string_format, objs );
            logger_.Info(message);
        }

        public void Debug( string string_format, params object[] objs )
        {
            string message = string.Format( string_format, objs );
            logger_.Debug( message );
        }

        public void Warn( string string_format, params object[] objs )
        {
            string message = string.Format( string_format, objs );
            logger_.Warn( message );
        }

        public void Error( string string_format, params object[] objs )
        {
            string message = string.Format( string_format, objs );
            logger_.Error( message );
        }

        public void Fatal( string string_format, params object[] objs )
        {
            string message = string.Format( string_format, objs );
            logger_.Fatal( message );
        }

        private LoggerWrapper() { }
        private void InitializeLogger(string a_tag, LogLevel log_level)
        {
            log_entity_ = new LogEntity();
            log_entity_.tag = a_tag;
            logger_ = new Logger(log_level);
            logger_.Output = EwkOutput;
        }

        private void InitializeMongoDb( string collection_name )
        {
            mongocl_ = LogManager.GetMongoCollection<LogEntity>( collection_name );
        }

        public void EwkOutput(LogData data, string path)
        {
            var log = data.ToString();
            Console.WriteLine(log);
            WriteDb(log);
        }

        private void WriteDb(string message)
        {
            if ( string.IsNullOrEmpty( message ) ) return;

            LogEntity entity = log_entity_.Clone() as LogEntity;
            entity.log = message;

            try
            {
                mongocl_.Insert(entity);
            }
            catch (Exception e) { Console.WriteLine("[Error] Can't connect MongoDb, Exception = {0}", e.Message); }
        }
    }

    public enum Log_Collection
    {
        ewk_log_login,
        ewk_log_lobby,
        ewk_log_chat,
        ewk_log_game,
        ewk_log_system,
        ewk_log_item,
        ewk_log_reward,
        ewk_log_game_logic,
    }

    public class LogManager : Singleton<LogManager>
    {
        // TODO : 서비스 시작전에 공용 로그 레벨을 한단계 낮춰준다.
        public static LogLevel Common_Log_Level { private set { } get { return LogLevel.Debug; } }// ; } }
        //internal static readonly string Connection_Uri = "mongodb://[admin:$K1JfI9Ojd+3_l]@localhost";

        private MongoClient mongo_client_;
        private MongoDatabase mongo_db_;
        private MongoCollection<LogEntity> mongocl_;
        private LogEntity log_entity_;

        public static MongoCollection<T> GetMongoCollection<T>( string collection_name )
        {
            MongoDatabase mongo_db = Instance.mongo_db_;
            return mongo_db.GetCollection<T>( collection_name );
        }

        public LogManager()
        {
            Initialize();
        }

        public void EwkOutput( LogData data, string path )
        {
            var log = data.ToString();
            Console.WriteLine( log );
            WriteDb( log );
        }

        private void WriteDb( string message )
        {
            if ( string.IsNullOrEmpty( message ) ) return;

            LogEntity entity = log_entity_.Clone() as LogEntity;
            entity.log = message;

            try
            {
                mongocl_.Insert( entity );
            }
            catch ( Exception e ) { Console.WriteLine( "[Error] Can't connect MongoDb, Exception = {0}", e.Message ); }
        }

        private void Initialize()
        {
            LogInfo log_info = ConfigManager.Log_Info;
            log_entity_ = new LogEntity();
            log_entity_.tag = "LogManager";

            mongo_client_ = new MongoClient( log_info.log_db_url );
            var server = mongo_client_.GetServer();
            mongo_db_ = server.GetDatabase( log_info.log_db_name );

            mongocl_ = mongo_db_.GetCollection<LogEntity>( Log_Collection.ewk_log_system.ToString() );
        }

        public static LoggerWrapper CreateLogger( string user_name, Log_Collection log_collection, LogLevel log_level )
        {
            LoggerWrapper logger = new LoggerWrapper( user_name, log_collection.ToString(), log_level );
            return logger;
        }

        public static LoggerWrapper CreateLogger( string user_name, Log_Collection log_collection )
        {
            LoggerWrapper logger = new LoggerWrapper( user_name, log_collection.ToString() );
            return logger;
        }

        public static LoggerWrapper CreateLogger( Log_Collection log_collection, LogLevel log_level )
        {
            LoggerWrapper logger = new LoggerWrapper( "none", log_collection.ToString(), log_level);
            return logger;
        }
    }
}
