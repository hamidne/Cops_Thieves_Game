using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketClient
{
    class Program
    {
        private const int PORT = 100;
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private byte ID;
        private string name;

        static void Main()
        {
            Console.Title = "Client";
            SocketClient client = new SocketClient(100);
        }
    }
}