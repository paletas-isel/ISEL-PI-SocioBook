using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;
using SuperWebSocket;
using WebServerMVC.Models;
using System.Linq;

namespace WebServerMVC.ChatServer
{
    public class ChatServer
    {
        public static void Start()
        {
            var chatServer = new ChatServer();
            chatServer.StartSuperWebSocketByConfig();
        }

        public static void Stop()
        {
            SocketServerManager.Stop();
        }

        void StartSuperWebSocketByConfig()
        {
            var serverConfig = ConfigurationManager.GetSection("socketServer") as SocketServiceConfig;
            if (!SocketServerManager.Initialize(serverConfig))
                return;

            var socketServer = SocketServerManager.GetServerByName("SuperWebSocket") as WebSocketServer;

            if (socketServer != null)
            {
                socketServer.CommandHandler += socketServer_CommandHandler;
                socketServer.NewSessionConnected += socketServer_NewSessionConnected;
                socketServer.SessionClosed += socketServer_SessionClosed;
            }

            if (!SocketServerManager.Start())
                SocketServerManager.Stop();
        }


        private readonly LinkedList<ChatUser> _sessions = new LinkedList<ChatUser>();

        private void socketServer_CommandHandler(WebSocketSession session, WebSocketCommandInfo commandinfo)
        {
            var username = ObtainUsername(session);

            lock (_sessions)
            {
                Notify(CommandType.MESSAGE, string.Format("{0}:{1}", username, commandinfo.Data), _sessions.Where(p => !p.Session.Equals(session)).ToArray());
            }
        }

        private void Notify(string type, string data, params ChatUser[] toNotify)
        {
            ThreadPool.QueueUserWorkItem(p =>
            {
                foreach (var session in toNotify.Select(c => c.Session))
                {
                    session.SendResponseAsync(String.Format("{0}{1}", type, data));
                }
            });
        }

        private void socketServer_SessionClosed(WebSocketSession session, CloseReason reason)
        {
            lock (_sessions)
            {
                var user = _sessions.First(p => p.Session.Equals(session));
                _sessions.Remove(user);

                Notify(CommandType.USERLEFT, user.Username, _sessions.ToArray());
            }
        }

        private void socketServer_NewSessionConnected(WebSocketSession session)
        {
            var username = ObtainUsername(session);

            if (username != null)
            {
                lock (_sessions)
                {
                    Notify(CommandType.USERJOIN, username, _sessions.ToArray());

                    var chatuser = new ChatUser(username, session);
                    _sessions.AddLast(chatuser);

                    Notify(CommandType.USERLIST, GenerateUserList(_sessions), chatuser);
                }
            }
        }

        private static string ObtainUsername(WebSocketSession session)
        {
            var cookie = session.Cookies["AuthCookie"];
            int firstEqual = cookie.IndexOf('=');
            var username = cookie.Substring(firstEqual + 1, cookie.IndexOf('&') - firstEqual - 1);
            int secondEqual = cookie.LastIndexOf('=');
            var token = cookie.Substring(secondEqual + 1);

            if (AuthModule.AuthModule.CheckTokenValidity(username, token))
                return username;
            return null;
        }

        private static string GenerateUserList(IEnumerable<ChatUser> sessions)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var chatUser in sessions)
            {
                sb.Append(String.Format("{0};", chatUser.Username));
            }

            return sb.ToString(0, sb.Length - 1);
        }
    }
}