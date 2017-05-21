namespace Messenger
{
    public class ServerMessageFactory
    {
        public static string Create(ServerMessagesCodes code)
        {
            return code.GetEnumDescription();
        }
    }
}
