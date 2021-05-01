using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Abstract;
using WebSocketSharp;
using EWK_Server.TeamGehem.Manager;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Utility;
using System.IO;
using EWK_Server.TeamGehem.GameScene;
using TeamGehem.DataModels.Protocols;
using WebSocketSharp.Net;
using ewk_server_v2;

namespace EWK_Server.TeamGehem.Channel
{
    class Login : WebSocketServiceAbs
    {
        public Login()
        {
        }

        public override Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_login; }
        }

        public override LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            // Session key 생성. [by Terdong : 2014-12-01]
            string session_key = GetSessionKeys();

            // 최초 UserInfo 생성.(session key 포함). [by Terdong : 2014-12-01]
            UserInfo user_info = UserManager.Instance.TakeUserInfo( session_key );
            
            // UserInfo(data)와 인사말(string)을 해당 client에게 전송. [by Terdong : 2014-12-01]
            SendToSelf(EwkProtoFactory.CreateIEwkProtocol<UserInfo>(ProtocolEnum.UserInfo, user_info).GetBytes);
            SendToSelf( string.Format( Resources.Login_Open_Message, user_info.Guest_Name ) );
            
        }

        protected override void OnMessage( MessageEventArgs e )
        {
            //TODO: UserInfo를 받은 client가 알아서 Login채널 연결 해제. [by Terdong : 2014-12-01]
            //if (e.Type == Opcode.Binary)
            //{
            //    IEwkProtocol ewk = EwkProtoSerilazer.DeserializeForProtobuf(e.RawData);
            //    if (ewk.Protocol_Enum == ProtocolEnum.Req_Change_Scene)
            //    {
            //        string changing_scene_name = ewk.GetData<string>();
            //        ewk.Protocol_Enum = ProtocolEnum.Res_Change_Scene;
            //        SendToSelf(ewk.GetBytes);
            //    }
            //}
            //else
            {
                log_.Debug( "e.Data = {0}", e.Data );
            }
        }

        protected override void OnClose( CloseEventArgs e )
        {
            //TODO: 혹시 e.Code 값이 normal이 아닐 경우의 예외 처리를 할 필요가 있을까?  [by Terdong : 2014-12-01]
            base.OnClose( e );
        }

        private string GetSessionKeys()
        {
            #region legacy_굳이 session key를 동기화할 필요가없다(확률적인 요인)
            /*
            if (session_keys_ == null || session_keys_.Count == 0)
            {
                using ( var ws = new WebSocket( "ws://localhost:10012/sessionprovider" ) )
                {
                    bool result = false;
                    // To set the WebSocket events.
                    ws.OnOpen += ( sender, e ) =>
                        {
                            ws.Send( EwkProtoFactory.CreateEwkProtocol( ProtocolEnum.SReq_Give_SessionKeys ).GetBytes );
                        };

                    ws.OnMessage += ( sender, e ) =>
                        {
                            if ( e.Type == Opcode.Binary )
                            {
                                IEwkProtocol ewk = EwkProtoSerilazer.DeserializeForProtobuf( e.RawData );
                                if ( ewk.Protocol_Enum == ProtocolEnum.SRes_Give_SessionKeys )
                                {
                                    session_keys_ = ewk.GetData<Queue<string>>();
                                    //ws.Close();
                                    result = true;
                                }
                            }
                        };

                    ws.OnError += (sender, e) => { result = true; };

                    ws.OnClose += ( sender, e ) => { };
                    ws.Connect();
                    while (!result) ;
                }
            }
            return session_keys_.Dequeue();*/
            #endregion
            return Guid.NewGuid().ToString("N");
        }
    }
}
