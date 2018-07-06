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
        public bool Type { get; private set; }
        public bool Turn { get; private set; }
        public User(string name, int id, bool type, bool turn) : this()
          
        {
            Name = name;
            ID = id;
            Type = type;
            Turn = turn;
            Status = (byte) UserStatus.Created;
        }
    }
}