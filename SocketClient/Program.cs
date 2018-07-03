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
                Console.Write("Send a request: ");
                string request = Console.ReadLine();
                if (request != null) client.SendRequest(request);
                Console.WriteLine(client.ReceiveResponse());
            }
        }
    }
}