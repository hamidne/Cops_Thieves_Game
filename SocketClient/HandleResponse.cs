using System;
using System.Text.RegularExpressions;

namespace SocketClient
{
    public static class HandleResponse
    {
        private static string _text;

        public static void Handle(string text)
        {
            _text = text;
            
            if (_text == "invalid")
                InvalidResponse();
            else if (_text.StartsWith("connected"))
                ConnectResponse();
        }

        private static void ConnectResponse()
        {
            Match match = Regex.Match(_text, @"^connected (\w+):(\w+)$");
            if (match.Success)
            {
                int userId = Convert.ToInt32(match.Groups[1].Value);
                string userName = match.Groups[2].Value;
            }
        }

        private static void InvalidResponse()
        {
            throw new System.NotImplementedException();
        }
    }
}