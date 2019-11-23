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

        public static string SendBoardData(string sendData)
        {

            //TODO: When the server turns off while we are connect and reboots...
            //  If we try and send data it will dump our app.
            //  If we use try it wont dump our app but it also fails to recreate the connection
            //      and send the user data.

            try
            {
                string responce = string.Empty;
                // If the client is connected to the server
                if (clientSocket.Connected)
                {
                    byte[] request = Encoding.ASCII.GetBytes(sendData);
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
                        _ = SendBoardData(sendData);
                    }
                    else
                    {
                        responce = "Connection Failure, Request could not be sent.";
                    }
                }
                return responce;
            }
            catch 
            {
                return null;
            }            
        }

        private static void AttemptConnect()
        {            
            try
            {
                clientSocket.Connect("129.21.52.2", 6969);
            }
            catch { }
        }
    }
}
