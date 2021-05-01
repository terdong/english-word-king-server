using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
namespace TeamGehem.DataModels.Protocols
{
    [ProtoContract]
    public enum ProtocolEnum
    {
        // common
        [ProtoEnum]
        None = 0,
        [ProtoEnum]
        Ready,

        // server -> client
        [ProtoEnum]
        UserInfo = 1000,
        [ProtoEnum]
        QuizInfo,
        [ProtoEnum]
        FightInfo,
        [ProtoEnum]
        AvatarInfo,
        [ProtoEnum]
        Avatar_Direction_Left,
        [ProtoEnum]
        Avatar_Direction_Right,
        [ProtoEnum]
        Res_Game_Ready_Ok,
        [ProtoEnum]
        Res_Game_Change_Game_Mode,
        [ProtoEnum]
        Res_Game_Change_Result_Mode,
        [ProtoEnum]
        Res_Change_Scene,   // Scene 이동 시 프로토콜.

        // client -> server
        [ProtoEnum]
        RightAnswer = 2000,
        [ProtoEnum]
        Req_Notify_Game_Ready,
        [ProtoEnum]
        Req_Change_Scene,
        [ProtoEnum]
        Req_Change_Scene_Duel_Game_Room,

        // server -> server
        [ProtoEnum]
        SReq_Give_SessionKeys = 3000,
        [ProtoEnum]
        SRes_Give_SessionKeys = 4000,
    }
}
