using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SocketServer
{
    class Program
    {
        private const int PORT = 100;
        private const int BUFFER_SIZE = 2048;
        private static List<User> Users = new List<User>();
        private static readonly byte[] Buffer = new byte[BUFFER_SIZE];
        public static readonly List<Socket> ClientSockets = new List<Socket>();

        private static readonly Socket ServerSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main()
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine(); // When we press enter close everything
            CloseAllSockets();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            ServerSocket.Listen(0);
            ServerSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }

        private static void CloseAllSockets()
        {
            foreach (Socket socket in ClientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            ServerSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = ServerSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            ClientSockets.Add(socket);
            socket.BeginReceive(Buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Console.WriteLine("Client connected, waiting for request...");
            ServerSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket) AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                ClientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(Buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf).Trim().ToLower();
            Console.WriteLine("Received Text: " + text);

            
            if (text == "exit")
            {
                // Always Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                ClientSockets.Remove(current);
                Console.WriteLine("Client disconnected");
                return;
            }

            HandleCommand.Handle(current, Users, text);
            current.BeginReceive(Buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }
    }
}