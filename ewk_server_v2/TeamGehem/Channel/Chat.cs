using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.IO;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Manager;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Generic;
using EWK_Server.TeamGehem.Abstract;
using System.Linq;
using TeamGehem.DataModels.Protocols;
using EWK_Server.TeamGehem.Utility;
using ewk_server_v2;

namespace EWK_Server.TeamGehem.Channel
{
    public class Chat : WebSocketServiceAbs
    {
        private static int _num = 0;

        private string _name;
        private string _prefix;

        Room room_;

        int user_key_;

        public Chat()
            : this(null)
        {
            Console.WriteLine("Constructor");
        }

        public Chat(string prefix)
        {
            _prefix = !prefix.IsNullOrEmpty() ? prefix : "anon#";
        }

        public Chat(string prefix, Room room)
            : this(prefix)
        {
            room_ = room;
        }

        public override Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_chat; }
        }

        public override WebSocketSharp.LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        protected string GetName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty() ? name : (_prefix + GetNum());
        }

        private static int GetNum()
        {
            return Interlocked.Increment(ref _num);
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            _name = GetName();
            user_key_ = System.Convert.ToInt32(Context.User.Identity.Name);

            UserInfo user_info = UserManager.Instance.GetUserInfo( user_key_ );
            user_info.Room_Index = room_.Index_;

            room_.AddChatSessionIdAndUserInfo( ID, user_info);

            string str1 = Resources.Chat_Join_Welcome;
            string str2 = string.Format(Resources.Chat_Join_User, _name, room_.UserCount());

            SendToSelf( str1);
            BroadcastMessage( str2 );
            if (room_.IsRoomFull())
            {
                BroadcastMessage(Resources.Chat_Join_Full);
            }

            log_.Info("{0}, {1}", str1, str2);
            log_.Debug("room_.Index = {0}", room_.Index_);
        }

        // TODO : 임시 게임로직 시작 플래그
        private bool is_started = false;
        protected override void OnMessage(MessageEventArgs e)
        {
            if(e.Type == Opcode.Binary)
            {
                IEwkProtocol protocol = EwkProtoSerilazer.DeserializeForProtobuf( e.RawData );
                if(protocol.Protocol_Enum == ProtocolEnum.Req_Notify_Game_Ready)
                {
                    BroadcastMessage( string.Format( Resources.Chat_Game_Ready_Notification, _name ) );

                    // TODO : 이 코드는 굉장히 불안전(해킹에 취약)하고 불안정하다.
                    // 나중에 다시 GameLogic 으로 다시 되돌려야함.
                    if(room_.IsGamePlayReady() && !is_started)
                    {
                        is_started = true;
                        BroadcastMessage(Resources.Chat_Game_Start_Notification);
                        room_.Game_Logic.StartGameLogic();
                    }
                }
            }
            else if (e.Type == Opcode.Text)
            {
                string message = String.Format(Resources.Chat_Chatting_Color, _name, e.Data);

                BroadcastMessage(message);
                //Sessions.Broadcast( message );

                // Example [Terdong : 2014-08-14]
                //Person person = new Person();
                //person.id = 100;
                //person.name = "terdong";
                //MemoryStream serialize = new MemoryStream();
                //ProtoBuf.Serializer.Serialize<Person>(serialize, person);
                //byte[] byteData = serialize.ToArray();
                //Sessions.SendTo(ID, byteData);
                //Sessions.SendTo( ID, ID + " = hey!" );

                log_.Info(message);
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            room_.RemoveUserInfo( user_key_ );
            RoomManager room_manager = RoomManager.Instance;
            room_manager.GiveBackRoom(room_);

            string log = string.Format( Resources.Chat_Leave_User, _name, room_.UserCount() );
            BroadcastMessage( log );
            log_.Info(log);



            base.OnClose(e);
        }

        protected override void BroadcastMessage(string message)
        {
            HashSet<string> ids_set = room_.Chat_Session_Id_Set;

            foreach ( string id in ids_set )
            {
                Sessions.SendTo( message , id );
            }
        }
    }
}
