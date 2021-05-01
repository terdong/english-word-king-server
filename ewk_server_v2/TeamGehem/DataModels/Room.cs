using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EWK_Server.TeamGehem.Abstract;
using TeamGehem.DataModels.Protocols;
using EWK_Server.TeamGehem.GameScene;

namespace EWK_Server.TeamGehem.DataModels
{
    public class Room
    {
        private static readonly int Max_Users_Number = 2;

        private static Object lock_object_ = new Object();
        private static int index_ = 0;

        public int Index_ { get; private set; }
        public GameLogic Game_Logic { get; private set; }

        //HashSet<WebSocketServiceAbsh> socket_set_;

        /// <summary>
        /// 나중에 string 으로 바꿔야함.(google play)
        /// </summary>
        HashSet<int> user_key_set_;

        HashSet<UserInfo> user_info_set_;

        HashSet<string> chat_session_id_set_;

        public HashSet<string> Chat_Session_Id_Set
        {
            private set { chat_session_id_set_ = value; }
            get { return chat_session_id_set_; }
        }

        /// <summary>
        /// int : guest_key
        /// string : game_session_id
        /// </summary>
        IDictionary <int, string> game_session_id_dic_;
        public IDictionary<int, string> Game_Session_Id_Set
        {
            private set { game_session_id_dic_ = value; }
            get { return game_session_id_dic_; }
        }

        public Room()
        {
            user_key_set_ = new HashSet<int>();
            user_info_set_ = new HashSet<UserInfo>();
            chat_session_id_set_ = new HashSet<string>();
            game_session_id_dic_ = new Dictionary<int, string>();
            //socket_set_ = new HashSet<WebSocketServiceAbs>();
            lock (lock_object_)
            {
                Index_ = index_;
            }
            Interlocked.Increment(ref index_);
        }

        public void Clear()
        {
            user_key_set_.Clear();
            user_info_set_.Clear();
            chat_session_id_set_.Clear();
            game_session_id_dic_.Clear();
            Game_Logic.StopGameLogic();
            Game_Logic = null;
        }

        public int UserCount()
        {
            return user_key_set_.Count;
        }

        public bool CheckGameReadyAndMakeGameLogic()
        {
            bool is_game_ready = IsGamePlayReady();
            if ( is_game_ready )
            {
                MakeGameLogic();
            }
            return is_game_ready;
        }

        public bool IsGamePlayReady()
        {
            return game_session_id_dic_.Count == Max_Users_Number;
        }

        public bool IsRoomFull()
        {
            return chat_session_id_set_.Count == Max_Users_Number;
        }

        public void AddGameSessionId(int guest_id, string gamme_setssion_id)
        {
            lock (game_session_id_dic_)
            {
                if (game_session_id_dic_.Count < Max_Users_Number)
                {
                    game_session_id_dic_.Add(guest_id, gamme_setssion_id);
                }
                else
                {
                    throw new OverflowException("Max_Users_Number 보다 많은 UserInfo를 저장하려 한다.");
                }

            }
        }

        public void AddChatSessionIdAndUserInfo(string chat_session_id, UserInfo user_info)
        {
            lock (user_key_set_)
            {
                if (user_key_set_.Count < Max_Users_Number)
                {
                    user_key_set_.Add(user_info.Guest_Id);
                    user_info_set_.Add(user_info);
                    chat_session_id_set_.Add(chat_session_id);
                }
                else
                {
                    throw new OverflowException("Max_Users_Number 보다 많은 UserInfo를 저장하려 한다.");
                }
            }
        }

        public void RemoveUserInfo(int id)
        {
            lock (user_key_set_)
            {
                user_key_set_.Remove(id);
                foreach (UserInfo user_info in user_info_set_)
                {
                    if (user_info.Guest_Id == id)
                    {
                        user_info_set_.Remove(user_info);
                        break;
                    }
                }
            }
        }

        void MakeGameLogic()
        {
            Game_Logic = GameLogic.CreateGameLogin(Game_Session_Id_Set);
        }
    }
}
