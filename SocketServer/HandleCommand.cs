using System;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SocketServer
{
    public static class HandleCommand
    {
        private static string _text;
        private static Socket _current;
        private static List<User> _users;

        // Register new commad in this method
        public static void Handle(Socket current, List<User> users, string text)
        {
            _text = text;
            _users = users;
            _current = current;

            if (_text == "get time")
                GetTimeCommand();
            else if (_text.StartsWith("connect"))
                ConnectCommand();
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

        // connect $username => connected $id
        private static void ConnectCommand()
        {
            Match match = Regex.Match(_text, @"^connect (\w+)$");
            if (match.Success)
            {
                if (!_users.Exists(user => user.Name == match.Groups[1].Value))
                {
                    Console.WriteLine("Text is a join to game request");
                    _users.Add(new User(match.Groups[1].Value));
                    SendMessage("connected " + _users.Count + ":" + _users.Last().Name);
                    Console.WriteLine("Accept join to game request");
                    Console.WriteLine("User " + _users.Last().Name + " login to server");
                }
                else
                {
                    Console.WriteLine("Text is a join to game request");
                    SendMessage("This username is already exist plase set new username");
                    Console.WriteLine("Not accept join to game request");
                }
            }
            else
            {
                SendMessage("Invalid parameters for connect command");
                Console.WriteLine("Accept join to game request");
            }
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

            //for(int i = 0; i < Program.ClientSockets.Count; i++)
            //{
            //    Program.ClientSockets[i].Send(data);
            //}
            _current.Send(data);
        }
    }
}