using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitaire
{
    public static class ClientManager
    {
        // This apps socket connection the server
        private static Socket clientSocket;

        // Tring to establish a connection
        public static bool TryServerConnection()
        {
            // We want to send data as well as recieve, will be connecting to host via IP
            clientSocket = new Socket(SocketType.Stream, ProtocolType.IP);

            try
            {
                clientSocket.Connect("129.21.52.2", 6969);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SendMessage()
        {
            string request = "Hey my phone is saying hi";
            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);

            // Send request to the server.
            clientSocket.Send(bytesSent, bytesSent.Length, 0);
        }
    }
}