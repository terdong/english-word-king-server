using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Abstract;
using EWK_Server.TeamGehem.Manager;
using WebSocketSharp;
using TeamGehem.DataModels.Protocols;
using System.Collections;

namespace EWK_Server.TeamGehem.Channel
{
    // 더 이상 사용하지 않는다. [11/30/2014 terdong]
    /*
    class SessionProvider : WebSocketServiceAbs
    {
        private static readonly int Default_Session_Key_Count = 100;

        public SessionProvider()
        {
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnMessage( MessageEventArgs e )
        {
            if ( e.Type == Opcode.Binary )
            {
                IEwkProtocol ewk = EwkProtoSerilazer.DeserializeForProtobuf( e.RawData );
                if ( ewk.Protocol_Enum == ProtocolEnum.SReq_Give_SessionKeys )
                {
                    Queue<string> session_list = new Queue<string>();

                    for(int i=0; i<Default_Session_Key_Count; ++i)
                    {
                        session_list.Enqueue( Guid.NewGuid().ToString( "N" ) );
                    }

                    ewk = EwkProtoFactory.CreateIEwkProtocol<Queue<string>>( ProtocolEnum.SRes_Give_SessionKeys, session_list );
                    SendToSelf( ewk.GetBytes );
                }
            }
        }

        protected override void OnClose( CloseEventArgs e )
        {
            base.OnClose( e );
        }

        public override Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_system; }
        }

        public override LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }
    }

    */
}
