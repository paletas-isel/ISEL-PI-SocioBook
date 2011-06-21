using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.ServiceModel.WebSockets;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sh = new WebSocketsHost<ChatService>(new Uri("ws://localhost:4500/chat"));
            //sh.AddWebSocketsEndpoint();
            //sh.Open();

            //Console.WriteLine("Websocket chat server listening on " + sh.Description.Endpoints[0].Address.Uri.AbsoluteUri);
            //Console.WriteLine();
            //Console.WriteLine("Press Ctrl-C to terminate the chat server...");

            //using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
            //{
            //    manualResetEvent.WaitOne();
            //}

            //sh.Close();

            
        }
    }
}
