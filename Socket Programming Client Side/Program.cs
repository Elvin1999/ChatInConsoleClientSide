using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Socket_Programming_Client_Side
{
    class Program
    {
        static string Name;
        static void Main(string[] args)
        {
            Console.WriteLine("Write your name");
            Name = Console.ReadLine();
            #region ClientMessage
            Console.WriteLine("======================CLIENT====================");
            string ipAddress = "172.20.28.56"; // server ip
            int port = 1031;
            byte[] buffer = new byte[1024];
            IPEndPoint endp = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (!socket.Connected)
            {
                try
                {
                    socket.Connect(endp);
                }
                catch (Exception)
                {
                    Console.WriteLine("NOT CONNECTED");
                    continue;
                }
                Console.WriteLine("CONNECTED");
                break;
            }
            Task senderTask = Task.Run(() =>
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    socket.Send(Encoding.ASCII.GetBytes(Name + " - >" + message));
                    Console.WriteLine("Client  : " + message);
                }
            });
            Task receiverTask = Task.Run(() =>
            {
                while (true)
                {
                    int length = socket.Receive(buffer);
                    if (length != 0)
                    {
                        Console.WriteLine("Server 1: " + Encoding.ASCII.GetString(buffer, 0, length));
                    }
                }
            });
            Task.WaitAll(senderTask, receiverTask);
            #endregion
           
          
        }
    

    }
}
