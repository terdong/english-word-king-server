using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using TeamGehem.DataModels.Protocols;
using EWK_Server.TeamGehem.Utility;
using EWK_Server.TeamGehem.Test;
using EWK_Server.TeamGehem.DataModels;
using StackExchange.Redis;
using LinqToDB;
using LinqToDB.Data;

namespace EWK_Server
{
    class TestFunc
    {
        public static void RunTestFunc()
        {
            TestFunc tf = new TestFunc();
            //tf.TestGUID();
            //tf.TestRedis();
            tf.TestLinqToDbWithAccount();

            Console.ReadLine();
        }
        void TestGUID()
        {
            for ( int i = 0; i < 100; ++i )
            {
                Console.WriteLine( i + " = " + Guid.NewGuid().ToString("N") );
            }
        }
        void TestRedis()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect( "localhost" );

            IDatabase db = redis.GetDatabase();

            db.StringSet( "000_test", "value1234" );
            db.StringSet( "000_test1", "value1234" );
            db.StringSet( "000_test2", "value1234" );
            db.StringSet( "000_test3", "value1234" );

            Console.WriteLine( "000_test = " + db.StringGet( "000_test" ));

            IServer server = redis.GetServer( "localhost", 6379 );
            foreach ( var key in server.Keys( pattern: "000_*" ) )
            {
                Console.WriteLine( "Key = " + key.ToString() );
            }


            //server.FlushDatabase();

            //db.CreateTransaction()

        }
        void TestYaml()
        {
            //ConfigManager.Instance.SetupServerConfigFile();
        }
        void TestProtobuf()
        {
            // var model = new EwkProtocol<Person>();
            //Object obj = new Object();
            IDictionary<string, Object> dic_ = new Dictionary<string, Object>();
            dic_["aaa"] = 7777 as Object;
            dic_["bbb"] = 9999 as Object;
            dic_["ccc"] = new List<int> { 1, 2, 3 } as Object;
            //dic_["ccc"] = 9999;
            //dic_["ddd"] = 9999;
            //dic_["eee"] = 9999;
            //dic_["fff"] = 9999;
            //dic_["ggg"] = 9999;

            Hashtable htable_ = new Hashtable();
            dic_["aaa"] = 9999;
            dic_["bbb"] = 9999;
            dic_["ccc"] = 9999;
            dic_["ddd"] = 9999;
            dic_["eee"] = 9999;
            dic_["fff"] = 9999;
            dic_["ggg"] = 9999;

            IList<string> list_ = new List<string>();
            list_.Add( "hello" );
            list_.Add( "asdf" );
            list_.Add( "asdf111" );
            list_.Add( "asdf222" );
            list_.Add( "asdf333" );

            QuizInfo quiz_info = QuizInfo.CreateUserInfo( 100 );


            //AvatarInfo ai = AvatarInfo.CreateAvatarInfo(9999,9999,9999,9999,9999,9999);
            //EwkProtocol ewk_ = EwkProtocol.CreateEwkProtocol( ProtocolEnum.Res_Game_Change_Game_Mode, ai );
            // string base64_ = ewk_.Input_Parameters_Base64;
            // byte[] string_bytes = Encoding.UTF8.GetBytes( base64_ );
            //byte[] bytes = ProtocolUtility.SerializeForProtobuf<AvatarInfo>( ai );
            //byte[] bytes = ProtocolUtility.SerializeForProtobuf<EwkProtocol>(
            //EwkProtocol.CreateEwkProtocol( ProtocolEnum.Res_Game_Change_Game_Mode, dic_ ) );
            //ewk_ );

            //IEwkProtocol ewkp = ProtocolUtility.DeserializeForProtobuf<EwkProtocol>( bytes );

            // 비 generic collection은 직렬화안됨.
            //IEwkProtocol gewk = EwkProtocolFactory.CreateEwkProtocol<Hashtable>( ProtocolEnum.Avatar_Direction_Left, htable_ );

            //IEwkProtocol gewk = EwkProtoFactory.CreateEwkProtocol<IList<string>>( ProtocolEnum.Ready, list_ );
            IEwkProtocol gewk = EwkProtoFactory.CreateIEwkProtocol<QuizInfo>( ProtocolEnum.QuizInfo, quiz_info );
            //IEwkProtocol gewk = EwkProtoFactory.CreateEwkProtocol<IList<string>>( ProtocolEnum.Avatar_Direction_Left, list_ );
            //IEwkProtocol ewk = EwkProtoFactory.CreateEwkProtocol( ProtocolEnum.Avatar_Direction_Left );

            byte[] bytes = gewk.GetBytes;
            gewk = EwkProtoSerilazer.DeserializeForProtobuf( bytes );
            //EwkProtocol<QuizInfo> gewkp = gewk as EwkProtocol<QuizInfo>;
            //QuizInfo gewkpp = gewkp.Data;

            QuizInfo gewkpp = gewk.GetData<QuizInfo>();
            //QuizInfo gewkpp = EwkProtoSerilazer.DeserializeForData<QuizInfo>( bytes );

            //QuizInfo quiz_info = QuizInfo.CreateUserInfo(null, null, 100, 10);
            //Person p = new Person();
            ////p.Protocol_Enum = ProtocolEnum.QuizInfo;

            //byte[] bytes = ProtocolUtility.SerializeForProtobuf<Person>(p);
            //EwkProtocol ewk = ProtocolUtility.DeserializeForProtobuf<EwkProtocol>(bytes);

            ////quiz_info = BinarySerializationExtension.DeserializeForProtobuf<QuizInfo>(bytes);
            //p = ProtocolUtility.DeserializeForProtobuf<Person>(bytes);
        }
        void TestSortedRandomArray()
        {
            int[] values = RandomExtention.GetNotOverlapRandNumberArray( 0, 10, 5 );
            foreach ( int value in values )
                Console.WriteLine( value );

            values = RandomExtention.GetNotOverlapRandNumberArray_Lib( 0, 10, 5 );
            foreach ( int value in values )
                Console.WriteLine( value );
        }
        void TestMongoDb()
        {
            new MongoDBTest().TestTutoril();
        }
        void TestMongoDb2()
        {
            //LoggerWrapper lw = LogManager.CreateLogger( "test_user" );
            //lw.Info( "Hello World!" );
        }
        void TestLinqToDbWithAccount()
        {
            using (var db = new ewkDB())
            {
                //var id = db.InsertWithIdentity<Account>(new Account() { EMail = "test1@test.com", FirstSignedDate = DateTime.Now });
                //Console.WriteLine("id = {0}", id);

                db.ExecuteProc("test_insert", DataParameter.VarChar("email", "h11e123llo11@hello.com"));

                List<account> list;// = TestLinq2Db.All();

                var query = from p in db.accounts
                            //where p.ProductID > 25
                            select p;
                list = query.ToList();

                foreach (account p in list)
                {
                    Console.WriteLine(p.email);
                }
            }
        }
        void TestLinqToDb()
        {
            //List<product > list;// = TestLinq2Db.All();

            //using ( var db = new ewkDB() )
            //{
            //    //db.CreateTable<Product>( "products" );

            //    product product_ = new product();
            //    product_.productname = "asdf3211";
            //    int id = System.Convert.ToInt32(db.InsertWithIdentity<product>(product_));
            //    Console.WriteLine("id = {0}", id);
            //    var query = from p in db.products
            //                //where p.ProductID > 25
            //                orderby p.productname descending
            //                select p;
            //    list = query.ToList();
            //}

            //Console.WriteLine("list count = {0}", list.Count);
            //foreach (product p in list)
            //{
            //    Console.WriteLine( p );
            //}
        }
    }
}
