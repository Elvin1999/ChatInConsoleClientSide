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
            string ipAddress = "192.168.1.103"; // server ip
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
            //Parallel.Invoke(() => Send(socket), () => Receive(socket));
            #endregion
            #region client

            //byte[] buffer = new byte[256];
            //IPEndPoint endp = new IPEndPoint(IPAddress.Parse("192.168.10.44"), 1031);
            //Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //sender.Connect(endp);

            //if (sender.Connected) Console.WriteLine("connected");

            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    buffer = Encoding.ASCII.GetBytes(message);
            //    sender.Send(buffer);
            //}

            #endregion
            #region Messaging with Server

            //byte[] buffer = new byte[256];
            //IPEndPoint endp = new IPEndPoint(IPAddress.Parse("192.168.10.44"), 1031);

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Connect(endp);
            //if (socket.Connected) Console.WriteLine("connected");


            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    buffer = Encoding.ASCII.GetBytes(message);
            //    socket.Send(buffer);
            //    buffer = new byte[256];
            //    int length = socket.Receive(buffer);
            //    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, length));
            //}

            #endregion
            //IPHostEntry host = Dns.GetHostEntry("facebook.com");
        }
        public static void Send(Socket socket)
        {
            while (true)
            {
                string message = Console.ReadLine();
                socket.Send(Encoding.ASCII.GetBytes(message));
                Console.WriteLine(Name + " - > " + message);
            }
        }
        public static void Receive(Socket socket)
        {
            while (true)
            {
                byte[] buffer = new byte[256];
                int length = socket.Receive(buffer);
                Console.WriteLine("Server 1: " + Encoding.ASCII.GetString(buffer, 0, length));
            }
        }

    }
}
