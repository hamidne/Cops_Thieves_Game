using System;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

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
            else if (_text.StartsWith("create"))
                CreateGameCommand(_text);
            else if (_text.StartsWith("want_to_join"))
                WantToJoin();
            else if (_text.StartsWith("join"))
                JoinTheGameCommand(_text);
            else if (_text.StartsWith("move"))
                MovePlayerCommand(_text);
            else
                UnknownCommand();
        }

        private static void MovePlayerCommand(string text)
        {
            Match match = Regex.Match(text, @"^move (\w+):(\w+):(\w+)$");
            if (match.Success)
            {
                Console.WriteLine("Text is a move the player request");
                int x = Convert.ToInt32(match.Groups[2].Value);
                int y = Convert.ToInt32(match.Groups[3].Value);
                int id = Convert.ToInt32(match.Groups[1].Value);
                for (int i = 0; i < Program.width; i++)
                {
                    for (int j = 0; j < Program.height; j++)
                    {
                        if (Program.playGround[i, j] ==id)
                            Program.playGround[i, j] = 0;
                    }
                }
                Program.playGround[x, y] = id;
                //change the player turn
                Program.ChangeTurn(id);
                //change the player turn

                byte[] data ;
                for (int i = 0; i < Program.ClientSockets.Count; i++)
                {
                    data = Encoding.ASCII.GetBytes("moved " + id + ":" + x + ":" + y+":"+_users[i].Turn);
                    Program.ClientSockets[i].Send(data);
                }
                Console.WriteLine("User " + _users[id].Name + " moved to " + x + "," + y);
            }
        }

        private static void JoinTheGameCommand(string text)
        {
            Match match = Regex.Match(text, @"^join (\w+):(\w+)$");
            if (match.Success)
            {
                if (!_users.Exists(user => user.Name == match.Groups[1].Value))
                {
                    Console.WriteLine("Text is a join the game request");
                    if (match.Groups[2].Value == "cop")
                        _users.Add(new User(match.Groups[1].Value, _users.Count + 1, true, false));
                    else
                    {
                        int thiefNum = 0;
                        for (int i = 0; i < _users.Count; i++)
                        {
                            if (_users[i].Type == false)
                                thiefNum++;
                        }
                        if (thiefNum == 0)
                            _users.Add(new User(match.Groups[1].Value, _users.Count + 1, false, true));
                        else
                            _users.Add(new User(match.Groups[1].Value, _users.Count + 1, false, false));
                    }
                    //set position of the player
                    Random rand = new Random();
                    int x, y;
                    do
                    {
                        x = rand.Next(Program.width);
                        y = rand.Next(Program.height);
                    }
                    while (Program.playGround[x, y] != 0);
                    Program.playGround[x, y] = _users.Last().ID;
                    SendMessage("joined " + _users.Count + ":" + x + ":" + y + ":" + _users.Last().Turn + ":" + Program.width + ":" + Program.height);
                    Console.WriteLine("Accept join the game request");
                    Console.WriteLine("User " + _users.Last().Name + " join the game as " + match.Groups[2].Value + "in " + x + "," + y);
                    if (_users.Count == Program.numberOfPlayers)
                    {
                        SendAllPlayerPositionInResponse();
                    }

                }
                else
                {
                    Console.WriteLine("Text is a create the game request");
                    SendMessage("This username is already created a game set new username or join the game");
                    Console.WriteLine("Not accept create the game request");
                }
            }
        }

        private static void SendAllPlayerPositionInResponse()
        {
            Thread.Sleep(500);
            String str = "allplayerpos " + Program.numberOfPlayers ;
            for (int i = 0; i < Program.numberOfPlayers; i++)
            {
                for (int j = 0; j < Program.width; j++)
                {
                    for (int k = 0; k < Program.height; k++)
                    {
                        if (Program.playGround[j,k] == _users[i].ID) {
                            str += ":" + _users[i].ID + ":" + j + ":" + k;

                        }
                    }
                }

            }
            SendMessageBroadCast(str);
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
                    SendMessage("created " + _users.Count + ":" + x + ":" + y + ":" + _users.Last().Turn);
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
                Console.WriteLine("Not Accept create the game request");
            }

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

        private static void WantToJoin()
        {
            Console.WriteLine("Text is a want to join the game request");

            int copNum = 0, thiefNum = 0;
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].Type == true)
                    copNum++;
                else
                    thiefNum++;
            }
            if (copNum + thiefNum == Program.numberOfPlayers)
            {
                SendMessage("nothing");
            }
            else if (copNum == Program.numberOfPlayers / 2)
            {
                SendMessage("onlythief");
            }
            else if (thiefNum == Program.numberOfPlayers / 2)
            {
                SendMessage("onlycop");
            }
            else
                SendMessage("both");
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

        private static void SendMessageBroadCast(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            for (int i = 0; i < Program.ClientSockets.Count; i++)
            {
                Program.ClientSockets[i].Send(data);
            }
        }
    }
}