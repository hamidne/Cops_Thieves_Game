namespace SocketServer
{
    public struct User
    {
        private int id;
        private string name;

        public User(int id, string name) : this()
        {
            this.id = id;
            this.name = name;
        }
    }
}