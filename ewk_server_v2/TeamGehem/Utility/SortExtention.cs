using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.Utility
{
    public class SortExtention
    {
        struct QuickPosInfo
        {
            public int left;
            public int right;
        };

        public static void QuickSort_Iterative( int[] numbers, int left, int right )
        {

            if ( left >= right )
                return; // Invalid index range

            List<QuickPosInfo> list = new List<QuickPosInfo>();

            QuickPosInfo info;
            info.left = left;
            info.right = right;
            list.Insert( list.Count, info );

            while ( true )
            {
                if ( list.Count == 0 )
                    break;

                left = list[0].left;
                right = list[0].right;
                list.RemoveAt( 0 );

                int pivot = Partition( numbers, left, right );

                if ( pivot > 1 )
                {
                    info.left = left;
                    info.right = pivot - 1;
                    list.Insert( list.Count, info );
                }

                if ( pivot + 1 < right )
                {
                    info.left = pivot + 1;
                    info.right = right;
                    list.Insert( list.Count, info );
                }
            }
        }

        private static int Partition( int[] numbers, int left, int right )
        {
            int pivot = numbers[left];
            while ( true )
            {
                while ( numbers[left] < pivot )
                    left++;

                while ( numbers[right] > pivot )
                    right--;

                if ( left < right )
                {
                    int temp = numbers[right];
                    numbers[right] = numbers[left];
                    numbers[left] = temp;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
