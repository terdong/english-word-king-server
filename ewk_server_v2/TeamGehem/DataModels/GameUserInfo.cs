using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.DataModels
{
    public class GameUserInfo
    {
        private int decrease_hp_amount_ = 10;
        private int increase_score_amount_ = 100;

        public int Guest_Id_ { get; private set; }
        public int Hp_ { get; set; }                    // 체력
        public int Score_ { get; set; }                 // 점수
        public int Num_Of_Combo_ { get; set; }          // 연속 정답 맞춘 개수

        public float Num_Of_Right_Answer_ { get; set; } //정답개수
        public float Num_Of_Wrong_Answer_ { get; set; } //오답개수
        public float Num_Of_Miss_Answer_ { get; set; }  //놓친문제개수

        public int Num_Of_Total_Turn_ { get; set; }     // 총 턴
        public string Fight_Time_ { get; set; }         // 총 플레이 시간

        public int Num_Of_Win { get; set; }
        public int Num_Of_Lose { get; set; }
        public int Num_Of_Draw { get; set; }

        public int Avartar_Direction { get; private set; }

        public static GameUserInfo CreateGameUserInfo( int guest_id, int full_hp, int avartar_direction )
        {
            return new GameUserInfo(guest_id, full_hp, avartar_direction);
        }

        public GameUserInfo SetAmountVariable(int decrease_hp_amount, int increase_score_amount)
        {
            this.decrease_hp_amount_ = decrease_hp_amount;
            this.increase_score_amount_ = increase_score_amount;

            return this;
        }

        public void IncreaseScore() { Score_ += increase_score_amount_; }
        public void DecreaseHp() { Hp_ = Math.Max(0, Hp_ - decrease_hp_amount_); }
        public bool IsDead() { return Hp_ <= 0; }

        public void IncreaseWin() { ++Num_Of_Win; }
        public void IncreaseLose() { ++Num_Of_Lose; }
        public void DecreaseLose() { --Num_Of_Lose; }
        public void IncreaseDraw() { ++Num_Of_Draw; }

        private GameUserInfo(int guest_id, int full_hp, int avartar_direction)
        {
            Initialize( guest_id,  full_hp, avartar_direction);
        }

        void Initialize(int guest_id, int full_hp, int avartar_direction)
        {
            Guest_Id_ = guest_id;
            Hp_ = full_hp;
            Score_ = 0;

            Num_Of_Combo_ = 0;
            Num_Of_Right_Answer_ = 0;
            Num_Of_Wrong_Answer_ = 0;
            Num_Of_Miss_Answer_  = 0;
            Num_Of_Total_Turn_ = 0;

            Num_Of_Win = 0;
            Num_Of_Lose = 0;
            Num_Of_Draw = 0;

            Fight_Time_ = null;

            Avartar_Direction = avartar_direction;
        }
    }
}
