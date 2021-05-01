using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.Utility
{
    public class RandomExtention
    {
        public static readonly Random Rand_ = new Random();

        public static int[] GetNotOverlapRandNumberArray_Lib( int min_range, int max_range, int array_num )
        {
            int[] value = new int[array_num];
            int[] rand_values = Enumerable.Range( min_range, max_range ).OrderBy( o => Rand_.Next() ).ToArray();
            for(int i=0; i<array_num; ++i)
            {
                value[i] = rand_values[i];
            }
            return value;
        }

        public static int[] GetNotOverlapRandNumberArray( int min_range, int max_range, int array_num )
        {
            int[] value = new int[array_num];

            KeyValuePair<int, int>[] temp_rand_value = new KeyValuePair<int, int>[( max_range - min_range ) + 1];

            int adjusted_max_range = max_range + 1;

            for ( int i = 0; i < temp_rand_value.Length; ++i )
            {
                temp_rand_value[i] = new KeyValuePair<int, int>( i, Rand_.Next( min_range, adjusted_max_range ) );
            }

            temp_rand_value = temp_rand_value.OrderBy( kv => kv.Value ).ToArray();

            for(int i=0; i<array_num; ++i)
            {
                value[i] = temp_rand_value[i].Key;
            }

            return value;
        }

    }
}
