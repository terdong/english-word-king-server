using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using EWK_Server.TeamGehem.Abstract;
using EWK_Server.TeamGehem.Manager;
using WebSocketSharp;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Utility;
using TeamGehem.DataModels.Protocols;
using ewk_server_v2;

namespace EWK_Server.TeamGehem.GameScene
{
    public class GameLogic : LoggerParent
    {
        private static readonly int Game_User_Max_Num = 2;
        private static readonly int Quiz_Turn_Max_Num = 20;
        private static readonly int Quiz_Example_Num = 4;
        private static readonly int Default_Full_HP = 100;
        private static readonly int Default_Damage_Amount = 10;
        private static readonly int Default_Reward_Score_Amount = 100;

        #region Sample Word
        protected readonly string[] Sample_Quiz_Words = {
                                    "perfect",
                                    "complete",
                                    "gather",
                                    "collect",
                                    "act",
                                    "deed",
                                    "help",
                                    "assist",
                                    "goal",
                                    "purpose",
                                    "better",
                                    "improve",
                                    "study",
                                    "examine",
                                    "hurt",
                                    "pain",
                                    "live",
                                    "alive",
                                    "use",
                                    "utilize",
                                    "about",
                                    "around",
                                    "view",
                                    "scene"};
        protected readonly string[] Sample_Quiz_Means = {
                                                          "완전한",
"완전한",
"수집",
"수집",
"행동",
"행위",
"도움",
"지원",
"목적",
"목적",
"더 나은",
"개선",
"연구",
"검토",
"상처",
"고통",
"살다",
"살아있는",
"사용",
"활용",
"약",
"주위에",
"전망",
"장면"};
        #endregion

        double event_interval_ = 100;
        public double Event_Interval
        {
            get { return event_interval_; }
            set
            {
                event_interval_ = value;
                timer.Interval = event_interval_;
            }
        }
        readonly double fixed_second_interval_ = 1000;
        public double Fixed_Second_Interval
        {
            get { return fixed_second_interval_; }
            private set { }
        }

        double global_seconds_;
        double event_seconds_;

        bool is_quizevent_;

        GameInfo game_info_;
        IDictionary<int, GameUserInfo> game_user_dic_;
        IDictionary<int, string> game_session_id_dic_;
        //GameUserInfo[] game_users_info_;

        Queue<KeyValuePair<int, int>> request_right_answer_queue_;

        private Timer timer;

        private LoggerWrapper log_;

        public delegate void BroadcastMessage( string message );
        public delegate void BroadcastData(byte[] data);
        public delegate void SendMessageToOneId(string id, string message);
        public delegate void SendDataToOneId(string id, byte[] data);
        public BroadcastMessage Boradcast_Message_;
        public BroadcastData Broadcast_Data_;
        public SendMessageToOneId Send_Message_To_OneId_;
        public SendDataToOneId Send_Data_To_OneId_;

        public static GameLogic CreateGameLogin(IDictionary<int, string> game_session_id_dic)
        {
            if (game_session_id_dic.Count > GameLogic.Game_User_Max_Num)
            {
                throw new Exception(string.Format("guest_id.Length({0})이 Game_User_Max_Number({1}) 개수를 초과했습니다.", game_session_id_dic.Count, Game_User_Max_Num));
            }
            return new GameLogic(game_session_id_dic);
        }

        public void StartGameLogic()
        {
            ICollection<int> keys = game_user_dic_.Keys;
            foreach (int key in keys)
            {
                GameUserInfo game_user_info = game_user_dic_[key];
                string game_session_id = game_session_id_dic_[key];
                Send_Data_To_OneId_(
                                    game_session_id,
                                    EwkProtoFactory.CreateIEwkProtocol(game_user_info.Avartar_Direction == 0 ?
                                    ProtocolEnum.Avatar_Direction_Left : ProtocolEnum.Avatar_Direction_Right).GetBytes
                                    );
            }
            timer.Start();
        }

        public void StopGameLogic()
        {
            timer.Stop();
        }

        public void RequestRightAnswer(int guest_id, int selected_example_answer_index)
        {
            request_right_answer_queue_.Enqueue(new KeyValuePair<int,int>(guest_id, selected_example_answer_index));
        }

        /// <summary>
        /// 정답 체크.
        /// </summary>
        /// <param name="guest_id"></param>
        /// <param name="example_answer_index"></param>
        void RequestRightAnswer_(int guest_id, int selected_example_answer_index)
        {
            if ( game_info_.IsRightAnswer( selected_example_answer_index ) )
            {
                request_right_answer_queue_.Clear();
                ChangeFightEvent(guest_id);
            }
            else
            {
                GameUserInfo game_user_info = game_user_dic_[guest_id];
                game_user_info.DecreaseHp();

                BroadcastGameUserInfo( game_user_dic_.Values );
            }
        }

        private GameLogic(IDictionary<int, string> game_session_id_dic)
        {
            log_ = LogManager.CreateLogger( Log_Collection.ewk_log_game_logic.ToString(), Log_Collection.ewk_log_game_logic, LogLevel.Debug );

            game_session_id_dic_ = game_session_id_dic;

            global_seconds_ = 0;
            event_seconds_ = GetEventCount(3);

            timer = new Timer( Event_Interval );
            timer.Elapsed += MainEvent;
            timer.Elapsed += IntroEvent;
            //timer.Start();

            game_info_ = GameInfo.CreateGameInfo( Quiz_Turn_Max_Num, Sample_Quiz_Words.Length, Quiz_Example_Num );

            int guest_id_length = game_session_id_dic_.Count;
            game_user_dic_ = new Dictionary<int, GameUserInfo>();

            int avatar_direction = 0; // default = left
            foreach(int guest_id in game_session_id_dic_.Keys)
            {
                game_user_dic_[guest_id] = GameUserInfo.CreateGameUserInfo(guest_id, Default_Full_HP, avatar_direction++).SetAmountVariable(Default_Damage_Amount, Default_Reward_Score_Amount);
            }

            request_right_answer_queue_ = new Queue<KeyValuePair<int, int>>();

            is_quizevent_ = false;
        }

        void ChangeFightEvent(int guest_id)
        {
            foreach ( GameUserInfo game_user_info in game_user_dic_.Values )
            {
                if ( game_user_info.Guest_Id_ == guest_id )
                {
                    game_user_info.IncreaseScore();
                }else
                {
                    game_user_info.DecreaseHp();
                }
            }

            BroadcastGameUserInfo( game_user_dic_.Values );

            event_seconds_ = GetEventCount(2);

            is_quizevent_ = false;
            timer.Elapsed -= QuizEvent;
            timer.Elapsed += FightActionEvent;
        }

        private void BroadcastGameUserInfo( ICollection<GameUserInfo> game_info_list )
        {
            GameUserInfo[] game_info_array = game_info_list.ToArray();
            int[] left_info;
            int[] right_info;
            if ( game_info_array[0].Avartar_Direction == 0 )
            {
                left_info = new int[] { game_info_array[0].Hp_, game_info_array[0].Score_ };
                right_info = new int[] { game_info_array[1].Hp_, game_info_array[1].Score_ };
            }
            else
            {
                left_info = new int[] { game_info_array[1].Hp_, game_info_array[1].Score_ };
                right_info = new int[] { game_info_array[0].Hp_, game_info_array[0].Score_ };
            }

            FightInfo fight_info = FightInfo.CreateFightInfo( left_info, right_info );
            Broadcast_Data_( EwkProtoFactory.CreateIEwkProtocol<FightInfo>(ProtocolEnum.FightInfo,fight_info).GetBytes);
        }

        void MainEvent( object sender, ElapsedEventArgs e )
        {
            global_seconds_ += Event_Interval;
            string str = Convert.ToString( TimeSpan.FromMilliseconds( global_seconds_ ) );

            if (is_quizevent_ && request_right_answer_queue_.Count > 0)
            {
                KeyValuePair<int,int> answer = request_right_answer_queue_.Dequeue();
                RequestRightAnswer_( answer.Key, answer.Value );
            }

            //Console.WriteLine( "test" + sender + ", " + e.SignalTime + ", " + str );
        }

        /// <summary>
        /// 3초후 게임시작 멘트.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IntroEvent( object sender, ElapsedEventArgs e )
        {
            event_seconds_ -= Event_Interval;
            if ( event_seconds_ % Fixed_Second_Interval == 0 )
            {
                Boradcast_Message_( string.Format( Resources.Game_Start_Count, ( (event_seconds_ / Fixed_Second_Interval) + 1 ).ToString() ) );
            }
            if ( event_seconds_ <= 0 )
            {
                event_seconds_ = GetEventCount(2);

                // TODO : 나중엔 모두 로딩이 끝나면 그때 QuizEvent로 시작하게 해야함.
                timer.Elapsed -= IntroEvent;
                timer.Elapsed += WaitEvent;
            }
        }

        void WaitEvent( object sender, ElapsedEventArgs e )
        {
            event_seconds_ -= Event_Interval;

            if (event_seconds_ <= 0)
            {
                event_seconds_ = GetEventCount(5);

                Broadcast_Data_(EwkProtoFactory.CreateIEwkProtocol(ProtocolEnum.Res_Game_Change_Game_Mode).GetBytes);

                // TODO : 여기 이상하게 짰음. 나중에 고칠것.
                // 1차 수정완료. [Terdong : 2014-10-02]
                // 게임 시작 전 아바타 정보(전적 등) 보냄.
                int[][] avatar_info_array = new int[2][];
                int temp_count = 0;
                ICollection<int> keys = game_user_dic_.Keys;
                foreach (int key in keys)
                {
                    GameUserInfo game_user_info = game_user_dic_[key];
                    string game_session_id = game_session_id_dic_[key];
                    avatar_info_array[temp_count] = new int[3];
                    avatar_info_array[temp_count][0] = game_user_info.Num_Of_Win;
                    avatar_info_array[temp_count][1] = game_user_info.Num_Of_Lose;
                    avatar_info_array[temp_count][2] = game_user_info.Num_Of_Draw;
                    ++temp_count;
                }

                AvatarInfo avatar_info = AvatarInfo.CreateAvatarInfo( avatar_info_array[0], avatar_info_array[1] );
                Broadcast_Data_(
                        EwkProtoFactory.CreateIEwkProtocol<AvatarInfo>(
                            ProtocolEnum.AvatarInfo, avatar_info
                        ).GetBytes
                    );

                SetQuizQuestion();

                timer.Elapsed -= WaitEvent;
                timer.Elapsed += QuizEvent;

                is_quizevent_ = true;
            }
        }

        void QuizEvent( object sender, ElapsedEventArgs e )
        {
            event_seconds_ -= Event_Interval;
            if ( event_seconds_ % Fixed_Second_Interval == 0 )
            {
                SendQuizInfo();
            }
            if ( event_seconds_ <= 0 )
            {
                ChangeFightEvent(-1);
            }
        }

        void FightActionEvent(object sender, ElapsedEventArgs e)
        {
            event_seconds_ -= Event_Interval;
            if (event_seconds_ < 0)
            {
                event_seconds_ = GetEventCount(5);

                bool is_somebody_dead = false;
                foreach ( GameUserInfo game_user_info in game_user_dic_.Values )
                {
                    is_somebody_dead = game_user_info.IsDead();
                }

                if ( game_info_.Current_Turn_ + 1 < Quiz_Turn_Max_Num && !is_somebody_dead)
                {
                    SetQuizQuestion();

                    is_quizevent_ = true;
                    timer.Elapsed -= FightActionEvent;
                    timer.Elapsed += QuizEvent;
                }
                else
                {
                    timer.Elapsed -= FightActionEvent;
                    timer.Elapsed += ResultEvent;
                }
            }
        }

        // TODO : 여기도 좀 이상함. 고치자.
        void ResultEvent( object sender, ElapsedEventArgs e )
        {
            int dead_count = 0;
            foreach ( GameUserInfo game_user_info in game_user_dic_.Values )
            {
                if (game_user_info.IsDead() )
                {
                    ++dead_count;
                    game_user_info.IncreaseLose();
                }
            }
            if ( dead_count >= Game_User_Max_Num )
            {
                foreach ( GameUserInfo game_user_info2 in game_user_dic_.Values )
                {
                    game_user_info2.DecreaseLose();
                    game_user_info2.IncreaseDraw();
                }
            }

            Broadcast_Data_( EwkProtoFactory.CreateIEwkProtocol(ProtocolEnum.Res_Game_Change_Result_Mode).GetBytes);

            timer.Elapsed -= ResultEvent;

            timer.Stop();
        }

        double GetEventCount(double val)
        {
            return val * Fixed_Second_Interval;
        }

        void SetQuizQuestion()
        {
            string quiz_question = Sample_Quiz_Words[game_info_.NextQuizQuestionIndex()];
            string[] quiz_example = new string[Quiz_Example_Num];
            int[] quiz_example_index_array = game_info_.NextQuizExampleIndexArray();
            for (int i = 0; i < quiz_example.Length; ++i)
            {
                quiz_example[i] = Sample_Quiz_Means[quiz_example_index_array[i]];
            }
            SendQuizInfo(quiz_question, quiz_example);
        }

        private void SendQuizInfo()
        {
            QuizInfo quiz_info = QuizInfo.CreateUserInfo(TimeSpan.FromMilliseconds(event_seconds_).Seconds);
            log_.Debug( quiz_info.ToString() );
            Broadcast_Data_( EwkProtoFactory.CreateIEwkProtocol<QuizInfo>( ProtocolEnum.QuizInfo, quiz_info ).GetBytes );
        }

        private void SendQuizInfo(string quiz_question, string[] quiz_example)
        {
            QuizInfo quiz_info = QuizInfo.CreateUserInfo(
                quiz_question,
                quiz_example,
                TimeSpan.FromMilliseconds(event_seconds_).Seconds,
                game_info_.Current_Turn_
                );

            Broadcast_Data_(EwkProtoFactory.CreateIEwkProtocol<QuizInfo>(ProtocolEnum.QuizInfo, quiz_info).GetBytes);
        }

        #region LoggerParent 멤버

        public Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_game; }
        }

        public LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        #endregion
    }
}
