using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Abstract;
using EWK_Server.TeamGehem.Manager;
using WebSocketSharp;
using TeamGehem.DataModels.Protocols;

namespace EWK_Server.TeamGehem.Channel
{
    class Lobby : WebSocketServiceAbs
    {
        public Lobby()
        {

        }

        protected override void OnOpen()
        {
            base.OnOpen();

            //SendToSelf(EwkProtoFactory.CreateIEwkProtocol<SceneListEnum>(ProtocolEnum.Res_Change_Scene, SceneListEnum.Duel_Menu_Scene).GetBytes);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Type == Opcode.Binary)
            {
                IEwkProtocol ewk = EwkProtoSerilazer.DeserializeForProtobuf(e.RawData);
                if (ewk.Protocol_Enum == ProtocolEnum.Req_Change_Scene)
                {
                    SendToSelf(EwkProtoFactory.CreateIEwkProtocol<SceneListEnum>(ProtocolEnum.Res_Change_Scene, ewk.GetData<SceneListEnum>()).GetBytes);
                }
            }
            //else
            //{
            //    log_.Debug("e.Data = {0}", e.Data);
            //}
        }

        public override Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_lobby; }
        }

        public override LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }
    }
}
