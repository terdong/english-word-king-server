
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Utility;

namespace EWK_Server.TeamGehem.DataModels
{
    public class GameInfo
    {
        public int Current_Turn_ { get; set; }

        private int quiz_turn_max_num_;

        /// <summary>
            /// 문제 번호 및 정답 인덱스.
            /// </summary>
        int[] quiz_index_array_;
        /// <summary>
        /// 문제 답안 목록.
        /// </summary>
        int[][] quiz_example_array_;

        /// <summary>
        /// 답안 중에 정답인 문제 index;
        /// </summary>
        int[] right_answer_index_array_;

        public static GameInfo CreateGameInfo( int quiz_turn_max_num, int quiz_words_count, int quiz_example_num )
        {
            return new GameInfo( quiz_turn_max_num, quiz_words_count, quiz_example_num );
        }

        public int NextQuizQuestionIndex()
        {
            IncreaseCurrentTurn();
            return quiz_index_array_[Current_Turn_];
        }

        public int[] NextQuizExampleIndexArray()
        {
            return quiz_example_array_[Current_Turn_];
        }

        public void IncreaseCurrentTurn()
        {
            ++Current_Turn_;
            if ( Current_Turn_ > quiz_turn_max_num_ )
            {
                throw new Exception(
                    string.Format( "current_turn_({0})이 Turn_Max_Num({1}) 개수를 초과했습니다.", Current_Turn_, quiz_turn_max_num_ )
                );
            }
        }

        public bool IsRightAnswer(int answer_example_index)
        {
            return right_answer_index_array_[Current_Turn_] == answer_example_index;
        }

        /// <summary>
        /// </summary>
        /// <param name="Quiz_Turn_Max_Num">게임 최대 턴.</param>
        /// <param name="Quiz_Words_Count">문제들 최대 개수.</param>
        /// <param name="Quiz_Example_Num">답안 개수.</param>
        private GameInfo( int Quiz_Turn_Max_Num, int Quiz_Words_Count, int Quiz_Example_Num )
        {
            Current_Turn_ = 0;
            quiz_turn_max_num_ = Quiz_Turn_Max_Num;

            quiz_index_array_ = RandomExtention.GetNotOverlapRandNumberArray( 0, Quiz_Words_Count - 1, 20 );
            right_answer_index_array_ = new int[Quiz_Turn_Max_Num];
            quiz_example_array_ = new int[quiz_index_array_.Length][];

            for ( int i = 0; i < quiz_example_array_.Length; ++i )
            {
                quiz_example_array_[i] = new int[Quiz_Example_Num];
                int right_answer_index = RandomExtention.Rand_.Next( Quiz_Example_Num );
                right_answer_index_array_[i] = right_answer_index;
                quiz_example_array_[i][right_answer_index] = quiz_index_array_[i];

                int[] wrong_example_index_array = RandomExtention.GetNotOverlapRandNumberArray_Lib(
                    0,
                    Quiz_Turn_Max_Num, Quiz_Example_Num - 1
                    );
                int wrong_example_index_count = 0;
                for ( int j = 0; j < Quiz_Example_Num; ++j )
                {
                    if ( j != right_answer_index )
                    {
                        quiz_example_array_[i][j] = wrong_example_index_array[wrong_example_index_count++];
                    }
                }
            }
        }
    }
}
