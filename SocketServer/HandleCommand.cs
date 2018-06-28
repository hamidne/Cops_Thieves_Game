using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SocketServer
{
    public class HandleCommand
    {
        private Socket current;
        private readonly string _text;
        private List<Socket> ClientSockets;

        public HandleCommand(string text, Socket current, List<Socket> clientSockets)
        {
            _text = text.ToLower();
            this.current = current;
            ClientSockets = clientSockets;
            Handle();
        }

        private void Handle()
        {
            if (_text == "get time")
                GetTimeCommand();
            else if (_text == "connect")
                ConnectCommand();
            else if (_text == "exit")
                ExitCommand();
            else
                UnknownCommand();
        }

        private void ExitCommand()
        {
            current.Shutdown(SocketShutdown.Both);
            current.Close();
            ClientSockets.Remove(current);
            Console.WriteLine("Client disconnected");
            return;
        }

        private void UnknownCommand()
        {
            Console.WriteLine("Text is an invalid request");
            byte[] data = Encoding.ASCII.GetBytes("Invalid request");
            current.Send(data);
            Console.WriteLine("Warning Sent");
        }

        private void ConnectCommand()
        {
            Console.WriteLine("Text is a get time request");
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            current.Send(data);
            Console.WriteLine("Time sent to client");
        }

        private void GetTimeCommand()
        {
            Console.WriteLine("Text is a get time request");
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            current.Send(data);
            Console.WriteLine("Time sent to client");
        }
    }
}