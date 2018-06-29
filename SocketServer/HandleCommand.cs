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
        private static List<User> _users;

        public static void Handle(Socket current, List<User> users, string text)
        {
            _text = text;
            _users = users;
            _current = current;

            if (_text == "get time")
                GetTimeCommand();
            else if (_text.StartsWith("connect"))
                ConnectCommand(_text);
            else
                UnknownCommand();
        }

        // get time => $time
        private static void GetTimeCommand()
        {
            Console.WriteLine("Text is a get time request");
            SendMessage(DateTime.Now.ToLongTimeString());
            Console.WriteLine("Time sent to client");
        }

        // connect $username => connect $id
        private static void ConnectCommand(string text)
        {
            Console.WriteLine("Text is a join to game request");
            _users.Add(new User(1, "asdasd"));
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