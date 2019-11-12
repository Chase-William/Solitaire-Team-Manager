using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Solitaire
{
    public static class ClientManager
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void InitClientSocket()
        {
            AttemptConnect();
        }

        public static string SendRequest(string _command)
        {
            string responce = string.Empty;
            // If the client is connected to the server
            if (clientSocket.Connected)
            {
                byte[] request = Encoding.ASCII.GetBytes(_command);
                clientSocket.Send(request);

                byte[] recievedBuf = new byte[1024];
                int rec = clientSocket.Receive(recievedBuf);
                byte[] data = new byte[rec];
                Array.Copy(recievedBuf, data, rec);
                // Passing our data as a string back to the source
                responce = Encoding.ASCII.GetString(data);
            }
            else
            {
                AttemptConnect();
                if (clientSocket.Connected)
                {
                    _ = SendRequest(_command);
                }
                else
                {
                    responce = "Connection Failure, Request could not be sent.";
                }                
            }
            return responce;
        }

        private static void AttemptConnect()
        {
            try
            {
                clientSocket.Connect("129.21.52.2", 6969);
            }
            catch (SocketException)
            {
                Console.WriteLine();
            }
        }
    }

    //public static class ClientManager
    //{
    //    // This apps socket connection the server
    //    private static Socket clientSocket;

    //    // Tring to establish a connection
    //    public static bool TryServerConnection()
    //    {
    //        // We want to send data as well as recieve, will be connecting to host via IP
    //        clientSocket = new Socket(SocketType.Stream, ProtocolType.IP);

    //        try
    //        {
    //            clientSocket.Connect("129.21.52.2", 6969);
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }

    //    public static void SendMessage()
    //    {
    //        string request = "Hey my phone is saying hi";
    //        Byte[] bytesSent = Encoding.ASCII.GetBytes(request);

    //        // Send request to the server.
    //        clientSocket.Send(bytesSent, bytesSent.Length, 0);
    //    }
    //}
}
