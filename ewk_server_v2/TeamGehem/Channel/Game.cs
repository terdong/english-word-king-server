using EWK_Server.TeamGehem.Abstract;
using EWK_Server.TeamGehem.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.GameScene;
using TeamGehem.DataModels.Protocols;
using EWK_Server.TeamGehem.Utility;
using ewk_server_v2;
namespace EWK_Server.TeamGehem.Channel
{
    class Game : WebSocketServiceAbs
    {
        Room room_ = null;
        UserInfo user_info_;

        public Game()
        {
        }
        public override Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_game; }
        }

        public override LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        //protected override string GetName()
        //{
            // TODO : 나중에 google play 인증 아이디로 불러올수있게 변경!!!
        //    return ID;
        //}
        protected override void OnOpen()
        {
            base.OnOpen();

            user_info_ = UserManager.Instance.GetUserInfo( System.Convert.ToInt32(Context.User.Identity.Name));

            // TODO : Game 채널을 안열고 Chat에서 방 검사하게끔 작업해야함. 리소스낭비임.
            // 해결책 : 방이 다 찼으면, 암호키를 쿠키형태로 클라에 전송. Game 채널 생성시 대조해봄.
            // 방이 다 안찼으면, 강제로 소켓 닫아버린다.[Terdong : 2014-09-25]
            if (!RoomManager.Instance.GetRoomToStartGame(user_info_.Room_Index, out room_))
            {
                SendToSelf(Resources.Game_Open_Error_NotCompleteRoom);
                this.Context.WebSocket.Close();
                return;
            }

            room_.AddGameSessionId( user_info_.Guest_Id, ID );

            // 게임준비 되면, 게임로직 실행.
            if (room_.CheckGameReadyAndMakeGameLogic())
            {
                GameLogic game_logic = room_.Game_Logic;
                game_logic.Boradcast_Message_ += BroadcastMessage;
                game_logic.Broadcast_Data_ += BroadcastMessage;
                game_logic.Send_Data_To_OneId_ += SendMessageToOneId;
                game_logic.Send_Message_To_OneId_ += SendMessageToOneId;
            }

            SendToSelf( EwkProtoFactory.CreateIEwkProtocol(ProtocolEnum.Res_Game_Ready_Ok).GetBytes);
        }

        protected override void OnMessage( MessageEventArgs e )
        {
            //Send( "echo response = " + e.Data );
            if(e.Type == Opcode.Binary)
            {
                IEwkProtocol protocol = EwkProtoSerilazer.DeserializeForProtobuf( e.RawData );
                log_.Debug( protocol.ToString() );
                if(protocol.Protocol_Enum == ProtocolEnum.RightAnswer)
                {
                    int answer_index = protocol.GetData<int>();
                    log_.Debug( "ansewr_index = {0}", answer_index );
                    room_.Game_Logic.RequestRightAnswer( user_info_.Guest_Id, answer_index );
                }
                //if ( protocol.Protocol_Enum == ProtocolEnum.None )
                //{
                //    stream.Position = 0;
                //    Person person = ProtoBuf.Serializer.Deserialize<Person>( stream );
                //    log_.Debug( "person.name = {0}", person.name );
                //}
            }
        }

        //protected override void OnClose( CloseEventArgs e )
        //{
        //}
        //protected override void OnError( WebSocketSharp.ErrorEventArgs e )
        //{
        //}

        void SendMessageToOneId(string id, string message)
        {
            Sessions.SendTo(message, id );
        }

        void SendMessageToOneId(string id, byte[] binary_data)
        {
            Sessions.SendTo(binary_data ,id);
        }

        protected override void BroadcastMessage( string message )
        {
            ICollection<string> id_set = room_.Game_Session_Id_Set.Values;

            foreach ( string id in id_set )
            {
                Sessions.SendTo( message, id );
            }
        }

        protected override void BroadcastMessage( byte[] binary_data )
        {
            ICollection<string> id_set = room_.Game_Session_Id_Set.Values;

            foreach ( string id in id_set )
            {
                Sessions.SendTo( binary_data, id);
            }
        }
    }
}
