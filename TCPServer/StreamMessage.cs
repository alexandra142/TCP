using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    class StreamMessage
    {
        public TcpClient Client { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public AcceptedMessage AcceptedMessage { get; set; }

        public StreamMessage(TcpClient client)
        {
            Client = client;
            StreamReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            StreamWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            AcceptedMessage = new AcceptedMessage(client);
        }


        public void ReadMessage()
        {
            AcceptedMessage.RestData += StreamReader.ReadLine();
            AcceptedMessage.Decode();
        }
    }
}
