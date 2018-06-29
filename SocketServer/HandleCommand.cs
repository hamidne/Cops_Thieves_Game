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
        
        private static void GetTimeCommand()    // get time => $time
        {
            Console.WriteLine("Text is a get time request");
            SendMessage(DateTime.Now.ToLongTimeString());
            Console.WriteLine("Time sent to client");
        }

        private static void ConnectCommand(string text)    // connect $username => connect $id
        {
            Console.WriteLine("Text is a join to game request");
            SendMessage(DateTime.Now.ToLongTimeString());
            Console.WriteLine("Accept join to game request");
        }
        
        private static void UnknownCommand()
        {
            Console.WriteLine("Text is an invalid request");
            SendMessage("Invalid request");
            Console.WriteLine("Warning Sent");
        }

        private static void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            _current.Send(data);
        }
    }
}