namespace SocketServer
{
    public struct User
    {
        public string Name { get; private set; }

        public User(string name) : this()
        {
            Name = name;
        }
    }
}