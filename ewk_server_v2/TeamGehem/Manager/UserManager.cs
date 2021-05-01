using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Utility;
using System.Collections.Concurrent;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Abstract;
using WebSocketSharp;
using TeamGehem.DataModels.Protocols;
using ewk_server_v2;

namespace EWK_Server.TeamGehem.Manager
{
    class UserManager : Singleton<UserManager>, LoggerParent
    {
        private static readonly int Max_Guest_Id_Number = 1000;

        LoggerWrapper log_;

        ConcurrentBag<int> guest_id_bag_;

        /// <summary>
        /// int : guest_id
        /// </summary>
        ConcurrentDictionary<int, UserInfo> player_dic_;

        public UserManager()
        {
            log_ = LogManager.CreateLogger( "UserManager", Log_Collection_, Log_Level_ );
            TempCreateUserId();
        }

        public UserInfo TakeUserInfo(string session_id)
        {
            int get_id = -1;

            guest_id_bag_.TryTake( out get_id );

            //TODO: channel_url_path는 임시로. [by Terdong : 2014-12-01]
            string channel_url_path = string.Format("{0}{1}:{2}{3}", Program.Ws_Header, Program.My_IP, Program.Port, SystemStr.Channel_Lobby);

            UserInfo user_info = UserInfo.CreateUserInfo(get_id, string.Format(SystemStr.Temp_Guest, get_id), session_id, channel_url_path);
            player_dic_.TryAdd( get_id, user_info );

            return user_info;
        }

        public bool IsValidSessionId( int key, string id )
        {
            return player_dic_[key].First_Session_Id.Equals( id );
        }

        public UserInfo GetUserInfo( int key )
        {
            return player_dic_[key];
        }

        private void TempCreateUserId()
        {
            guest_id_bag_ = new ConcurrentBag<int>();

            player_dic_ = new ConcurrentDictionary<int, UserInfo>();

            InitializeGuestId();
        }

        private void InitializeGuestId()
        {
            if ( guest_id_bag_.IsEmpty )
            {
                for ( int i = 0; i < Max_Guest_Id_Number; ++i )
                {
                    guest_id_bag_.Add( i );
                }
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
