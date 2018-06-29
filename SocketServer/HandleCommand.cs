using System;
using System.Text;
using System.Net.Sockets;

namespace SocketServer
{
    public static class HandleCommand
    {
        private static string _text;
        private static Socket _current;

        public static void Handle(Socket current, string text)
        {
            _text = text;
            _current = current;
            
            if (_text == "get time")
                GetTimeCommand();
            else if (_text.StartsWith("connect"))
                ConnectCommand(_text);
            else
                UnknownCommand();
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