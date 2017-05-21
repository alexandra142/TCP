using System;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public class AcceptedMessage
    {
        private const string Splitter = "\r\n";
        private const string Delimiter = "|";
        public string DecodedData { get; set; }
        public string RestData { get; set; }
        private readonly TcpClient _client;

        public AcceptedMessage(TcpClient client)
        {
            _client = client;
        }

        public override string ToString()
        {
            return $"DecodedData: {DecodedData};\t RestData: {RestData}";
        }

        public void Decode()
        {
            string[] messages = GetMessages();

            SetDecodedData(messages);
            SetNewRestData();
            WriteMessage();
        }

        private void WriteMessage()
        {
            Console.WriteLine($"Client {_client.Client.RemoteEndPoint} {this}");
        }

        private string[] GetMessages()
        {
            return RestData.Split(new[] { Splitter }, StringSplitOptions.None);
        }

        private void SetNewRestData()
        {
            int index = RestData.LastIndexOf(Splitter, StringComparison.Ordinal);

            RestData = index <= 0 ? string.Empty : RestData.Substring(index);
        }

        private void SetDecodedData(string[] messages)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var message in messages)
            {
                stringBuilder.Append(message);
                stringBuilder.Append(Delimiter);
            }

            DecodedData = stringBuilder.ToString();
        }
    }
}
