using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketClient
{
    class Program
    {

        static void Main()
        {
            Console.Title = "Client";
            SocketClient client = new SocketClient(100);
            Console.WriteLine(client.ConnectToServer());
            while (true)
            {
                string request = Console.ReadLine();
                client.SendRequest(request);
                Console.WriteLine(client.ReceiveResponse());
            }
        }
    }
}