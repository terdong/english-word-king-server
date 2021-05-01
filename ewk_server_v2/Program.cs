/*********************************************************************
 * English Word King Server
 * Copyrightⓒ2014 TeamGehem. All rights reserved.
 * Make by DongHee Kim
 *********************************************************************/

// 확인용 define 문구.
//#define TG_DEBUG // #if DEBUG 체크로 대체.

using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using EWK_Server.TeamGehem.Channel;
using EWK_Server.TeamGehem.Test;
using EWK_Server.TeamGehem.DataModels;
using EWK_Server.TeamGehem.Utility;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using EWK_Server.TeamGehem.Manager;
using EWK_Server.TeamGehem.GameScene;
using TeamGehem.DataModels.Protocols;
using System.Collections;
using ewk_server_v2;

namespace EWK_Server
{
    public class Program
    {
        public static readonly string Ws_Header = "ws://";
        public static readonly string Main_Path = "ws://127.0.0.1";
        public static readonly int Port = 10012;
        public static string My_IP = null;

        static void Main(string[] args)
        {
            //TestFunc.RunTestFunc();
            StartServer();

        }

        private static void StartServer()
        {
            Initialize();
            StartEwkServer();
            ReleaseSingletons();
        }

        private static void Initialize()
        {
            ConfigManager.Instance.SetupServerConfigFile();

            My_IP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
        }

        private static void ReleaseSingletons()
        {
            RoomManager.Instance.Release();
            UserManager.Instance.Release();
            LogManager.Instance.Release();
            ConfigManager.Instance.Release();
        }

        static void StartEwkServer()
        {
            RoomManager rm = RoomManager.Instance;

            var wssv = new WebSocketServer(Port, false);

            // For Secure Connection
            //            var cert = ConfigurationManager.AppSettings["ServerCertFile"];
            //            var password = ConfigurationManager.AppSettings["CertFilePassword"];
            //           wssv.Certificate = new X509Certificate2( cert, password );

            //For HTTP Authentication (Basic/Digest)
            wssv.AuthenticationSchemes = AuthenticationSchemes.Digest;
            wssv.Realm = "EWK_000";
            wssv.UserCredentialsFinder = identity =>
            {
                // TODO : 구글 플레이와 연동시, id는 구글 id.
                // password는 토큰값으로 판별. [Terdong : 2014-09-15]
                if (identity.Name.Equals("login"))
                {
                    //계정 없으면 생성 코드.
                    if (GenericCRUD.IsEqualEmail(identity.AuthenticationType))
                    {
                        GenericCRUD.InsertEntityBySP<account>(identity.AuthenticationType);
                    }

                    return new NetworkCredential("login", identity.AuthenticationType);
                }
                else
                {
                    UserInfo user_info = UserManager.Instance.GetUserInfo(System.Convert.ToInt32(identity.Name));
                    return user_info != null ? new NetworkCredential(user_info.Guest_Id.ToString(), user_info.First_Session_Id) : null;
                }

                //var expected = identity.Name;
                //return identity.Name == expected
                //       ? new NetworkCredential( expected, "password" )
                //       : null;
            };

            wssv.AddWebSocketService<Login>(SystemStr.Channel_Login);
            wssv.AddWebSocketService<Lobby>(SystemStr.Channel_Lobby);
            wssv.AddWebSocketService<Game>(SystemStr.Channel_Game);
            //wssv.AddWebSocketService<Chat>( "/chat" );
            wssv.AddWebSocketService<Chat>(SystemStr.Channel_Chat, () => new Chat("Anon#", rm.GetRoom()) { });
            //wssv.AddWebSocketService<SessionProvider>( SystemStr.Channel_SessionProvider );
            /* Protocol = "chat",
             // Checking Origin header
             OriginValidator = value => {
               Uri origin;
               return !value.IsNullOrEmpty () &&
                      Uri.TryCreate (value, UriKind.Absolute, out origin) &&
                      origin.Host == "localhost";
             },
             // Checking Cookies
             CookiesValidator = (req, res) => {
               foreach (Cookie cookie in req) {
                 cookie.Expired = true;
                 res.Add (cookie);
               }
               return true;
             }
           });
          */

            //TODO: 로그파일 날짜별 지정하게 작업.[Terdong : 2014-08-14]
            wssv.Log.Level = LogLevel.Debug;
            wssv.Log.File = ConfigManager.Log_Info.log_file_name;
            wssv.Log.Output = LogManager.Instance.EwkOutput;
            wssv.Start();
            if (wssv.IsListening)
            {
                Console.WriteLine(
                  "A WebSocket server listening on port: {0}, providing services:", wssv.Port);

                foreach (var path in wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            wssv.Log.Info("E.W.K Server Start!");

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();
            wssv.Stop();
        }
    }
}
