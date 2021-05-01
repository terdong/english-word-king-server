using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TeamGehem.DataModels.Protocols;

namespace EWK_Server.TeamGehem.Utility
{
    public static class ProtocolUtility
    {
        /// <summary>
        /// EwkProtocol Class를 상속받은 Class(T형) 만 Serialization 이 가능함.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        [Obsolete("This method was Deprecated", true )]
        public static byte[] SerializeForProtobuf<T>(T target)
        {
            //String str = ProtoBuf.Serializer.GetProto<EwkProtocol>();

            MemoryStream serialize = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(serialize, target);

            Console.WriteLine("SerializeForProtobuf size = {0}", serialize.ToArray().LongLength);

            byte[] byte_array = serialize.ToArray();
            serialize.Dispose();
            return byte_array;
        }

        [Obsolete( "This method was Deprecated", true )]
        public static T DeserializeForProtobuf<T>( byte[] binary_data )
        {
            Console.WriteLine("DeserializeForProtobuf size = {0}", binary_data.LongLength);

            Stream stream = new MemoryStream( binary_data );

            T t_object = ProtoBuf.Serializer.Deserialize<T>(stream);
            stream.Dispose();

            return t_object;
        }

        //public static T DeserializeForProtobuf<T>( byte[] binary_data, ProtocolEnum protocol )
        //{
        //    //EwkProtocol ewk_protocol = DeserializeForProtobuf<EwkProtocol>( binary_data );
        //    //if ( ewk_protocol.Protocol_Enum == protocol )
        //    try
        //    {
        //        {
        //            return DeserializeForProtobuf<T>( binary_data );
        //        }
        //    }
        //    catch ( Exception e )
        //    {
        //        throw new NullReferenceException( string.Format( "받은 데이터 객체에 해당 {0}이 지정되지 않았습니다. Exception = {1}", protocol.ToString(), e.Message) );
        //    }
        //}
        //public static Stream Serialize( this object source )
        //{
        //    var formatter = new BinaryFormatter();
        //    var stream = new MemoryStream();
        //    formatter.Serialize( stream, source );
        //    return stream;
        //}

        //public static T Deserialize<T>( this Stream stream )
        //{
        //    var formatter = new BinaryFormatter();
        //    stream.Position = 0;
        //    return ( T )formatter.Deserialize( stream );
        //}
    }
}
