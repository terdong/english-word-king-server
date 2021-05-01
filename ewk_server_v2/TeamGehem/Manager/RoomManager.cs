using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.Manager;
using EWK_Server.TeamGehem.Utility;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Abstract;
using WebSocketSharp;

namespace EWK_Server.TeamGehem.Manager
{
    public class RoomManager : Singleton<RoomManager>, LoggerParent
    {
        private static readonly int Max_Create_Room = 100;

        /// <summary>
        /// 한명 이상 대기중인 방들.
        /// </summary>
        ConcurrentQueue<Room> incomplete_room_queue_;
        /// <summary>
        /// 게임시작 준비가 된 방들.
        /// int : guest_id
        /// </summary>
        ConcurrentDictionary<int, Room> complete_room_dic_;
        ObjectPool<Room> room_pool_;

        LoggerWrapper log_;

        public RoomManager()
        {
            room_pool_ = new ObjectPool<Room>( () => new Room() );
            incomplete_room_queue_ = new ConcurrentQueue<Room>();
            complete_room_dic_ = new ConcurrentDictionary<int, Room>();

            log_ = LogManager.CreateLogger( "RoomManager", Log_Collection.ewk_log_system, WebSocketSharp.LogLevel.Info);

            CreateRooms( Max_Create_Room );
        }

        public bool GetRoomToStartGame( int room_index, out Room room)
        {
            return complete_room_dic_.TryGetValue(room_index, out room) && room != null;
        }

        /// <summary>
        /// 현재 한명만 있는 활성화 방 부터 줌.
        /// 없으면 room_pool에서 꺼내서 줌.
        /// </summary>
        /// <returns>Room</returns>
        public Room GetRoom()
        {
            Room room = null;
            if (incomplete_room_queue_.IsEmpty)
            {
                room = GetRoomFromRoomPool();
            }else
            {
                if ( incomplete_room_queue_.TryDequeue( out room ) )
                {
                    switch(room.UserCount())
                    {
                        case 0:
                            incomplete_room_queue_.Enqueue( room );
                            break;
                        case 1: // 이제 곧 카운트가 2가 될것이므로 미리 셋팅.
                            complete_room_dic_[room.Index_] = room;
                            break;
                        case 2:
                            log_.Error( "room.count can't have 2 Count" );
                            return GetRoom();
                    }
                }
                else
                {
                    log_.Error( "actived_room_queue_: TryDequeue failed when it should have succeeded" );
                    room = GetRoomFromRoomPool();
                }
            }
            return room;
        }

        public void GiveBackRoom(Room room)
        {
            complete_room_dic_[room.Index_] = null;
            if(room.UserCount() == 0)
            {
                room.Clear();
                lock ( room_pool_ )
                {
                    room_pool_.PutObject( room );
                }

            }else if(room.UserCount() == 1)
            {
                incomplete_room_queue_.Enqueue( room );
            }
        }

        Room GetRoomFromRoomPool()
        {
            Room room = null;
            lock ( room_pool_ )
            {
                room = room_pool_.GetObject();
            }
            incomplete_room_queue_.Enqueue( room );
            return room;
        }

        void CreateRooms(int max_num)
        {
            Room[] rooms = new Room[max_num];
            for (int i = 0; i < max_num; ++i)
            {
                Room room = room_pool_.GetObject();
                rooms[i] = room;
                //Console.WriteLine("room index = {0}", room.Index_);
            }

            for (int i = 0; i < max_num; ++i)
            {
                room_pool_.PutObject(rooms[i]);
                rooms[i] = null;
            }
            rooms = null;
        }

        #region LoggerParent 멤버

        public Log_Collection Log_Collection_
        {
            get { return Log_Collection.ewk_log_system; }
        }

        public WebSocketSharp.LogLevel Log_Level_
        {
            get { return LogManager.Common_Log_Level; }
        }

        #endregion
    }
}
