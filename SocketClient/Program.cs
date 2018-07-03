using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketClient
{
    class Program
    {
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 100;

        static void Main()
        {
            Console.Title = "Client";

            SocketClient client = new SocketClient(100);
        }
    }
}