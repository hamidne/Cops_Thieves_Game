using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            else if (_text.StartsWith("create"))
                CreateGameCommand(_text);
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

        private static void CreateGameCommand(string text)
        {
            Match match = Regex.Match(text, @"^create (\w+):(\w+):(\w+):(\w+):(\w+)$");
            if (match.Success)
            {
                if (!_users.Exists(user => user.Name == match.Groups[1].Value) && _users.Count == 0)
                {
                    Console.WriteLine("Text is a create the game request");
                    if (match.Groups[5].Value == "cop")
                        _users.Add(new User(match.Groups[1].Value, _users.Count + 1, true, false));
                    else
                        _users.Add(new User(match.Groups[1].Value, _users.Count + 1, false, true));

                    //set the playground 
                    Program.playGround = new int[Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value)];
                    Program.width = Convert.ToInt32(match.Groups[3].Value);
                    Program.height = Convert.ToInt32(match.Groups[4].Value);
                    for (int i = 0; i < Program.width; i++)
                    {
                        for (int j = 0; j < Program.height; j++)
                        {
                            Program.playGround[i, j] = 0;
                        }
                    }
                    //set num of players
                    Program.numberOfPlayers = Convert.ToInt32(match.Groups[2].Value);
                    //set position of the player
                    Random rand = new Random();
                    int x = rand.Next(Program.width);
                    int y = rand.Next(Program.height);
                    Program.playGround[x, y] = _users.Last().ID;
                    SendMessage("created " + _users.Count + ":" + x + ":" + y+":"+_users.Last().Turn);
                    Console.WriteLine("Accept create the game request");

                    Console.WriteLine("User " + _users.Last().Name + " created the game as " + match.Groups[5].Value + "in " + Program.width + "X" + Program.height);
                }
                else
                {
                    Console.WriteLine("Text is a create the game request");
                    SendMessage("This username is already created a game set new username or join the game");
                    Console.WriteLine("Not accept create the game request");
                }
            }
            else
            {
                SendMessage("Invalid parameters for create game command");
                Console.WriteLine("Not Accept create thee game request");
            }

        }

        // connect $username => connected $id
        private static void ConnectCommand(string text)
        {
            Match match = Regex.Match(text, @"^connect (\w+)$");
            if (match.Success)
            {
                if (!_users.Exists(user => user.Name == match.Groups[1].Value))
                {
                    Console.WriteLine("Text is a join to game request");
                    _users.Add(new User(match.Groups[1].Value, _users.Last().ID, true, false));
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
            _current.Send(data);
        }
    }
}