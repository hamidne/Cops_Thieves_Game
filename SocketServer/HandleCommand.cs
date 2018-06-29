using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SocketServer
{
    public static class HandleCommand
    {
        private static string _text;
        private static Socket _current;
        private static List<Socket> _clientSockets;

        public static void Handle(List<Socket> clientSockets, Socket current, string text)
        {
            _text = text.Trim().ToLower();
            _current = current;
            _clientSockets = clientSockets;
            
            if (_text == "get time")
                GetTimeCommand();
            else if (_text.StartsWith("connect"))
                ConnectCommand(_text);
            else if (_text == "exit")
                ExitCommand();
            else
                UnknownCommand();
        }

        private static int ExitCommand()
        {
            _current.Shutdown(SocketShutdown.Both);
            _current.Close();
            _clientSockets.Remove(_current);
            Console.WriteLine("Client disconnected");
            return 0;
        }

        private static void UnknownCommand()
        {
            Console.WriteLine("Text is an invalid request");
            byte[] data = Encoding.ASCII.GetBytes("Invalid request");
            _current.Send(data);
            Console.WriteLine("Warning Sent");
        }

        private static void ConnectCommand(string text)
        {
            Console.WriteLine("Text is a get time request");
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            _current.Send(data);
            Console.WriteLine("Time sent to client");
        }

        private static void GetTimeCommand()
        {
            Console.WriteLine("Text is a get time request");
            byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            _current.Send(data);
            Console.WriteLine("Time sent to client");
        }
    }
}