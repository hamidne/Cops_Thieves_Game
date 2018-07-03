namespace SocketServer
{
    enum UserStatus : byte
    {
        Created
    }
    
    public struct User
    {
        public byte Status { get; private set; }
        public string Name { get; private set; }
        public int ID { get; private set; }

        public User(string name) : this()
        {
            Name = name;
            Status = (byte) UserStatus.Created;
        }
    }
}