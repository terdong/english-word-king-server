using EWK_Server.TeamGehem.Manager;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EWK_Server.TeamGehem.Abstract
{
    public abstract class WebSocketServiceAbs : WebSocketBehavior, LoggerParent
    {
        protected LoggerWrapper log_;

        protected WebSocketServiceAbs()
        {
        }

        //TODO : 필요없는것 같다.
        //protected abstract string GetName();
        public abstract Log_Collection Log_Collection_
        {
            get;
        }
        public abstract LogLevel Log_Level_
        {
            get;
        }

        /// <summary>
        /// 항상 오버라이드 해서 쓰자.
        /// 이거 그대로 쓰면 모든 사용자에게 메시지가 감.
        /// 운영용으로만 쓰일듯.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void BroadcastMessage(string message)
        {
            Sessions.Broadcast(message);
        }

        protected virtual void BroadcastMessage( byte[] binary_data)
        {
            Sessions.Broadcast( binary_data );
        }

        protected override void OnOpen()
        {
            log_ = LogManager.CreateLogger("User_Name", Log_Collection_, Log_Level_);

            log_.Debug("{0}, ID = {1}, Session.count = {2},",Log_Collection_.ToString(),  ID, Sessions.Count);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            //log_.Dispose();
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            log_.Error(e.Message);
        }

        protected void SendToSelf(string message)
        {
            Sessions.SendTo(message, ID);
        }

        protected void SendToSelf( byte[] binary_data)
        {
            Sessions.SendTo( binary_data , ID);
        }
    }
}
