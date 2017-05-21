using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Messenger
{
    public class AcceptedMessage
    {
        public string DecodedData { get; set; }
        public string RestData { get; set; }

        public List<int> AsciiValues = new List<int>();

        private readonly TcpClient _client;

        public AcceptedMessage(TcpClient client)
        {
            _client = client;
        }


        public override string ToString()
        {
            var writeDecodedData = DecodedData.Replace("\n", "\\n");
            writeDecodedData= writeDecodedData.Replace("\r", "\\r");
            writeDecodedData= writeDecodedData.Replace("\t", "\\t");
            return $"DecodedData: {writeDecodedData};\t RestData: {RestData}";
        }

        public void Decode(string writeConsoleMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < AsciiValues.Count-2; i++)
            {
                stringBuilder.Append((char) AsciiValues[i]);
            }

            DecodedData = stringBuilder.ToString();
            WriteMessage(writeConsoleMessage);
        }

        private void WriteMessage(string writeConsoleMessage)
        {
            Console.WriteLine($"Client {_client.Client.RemoteEndPoint} \t{writeConsoleMessage}\t \"+{this}");
        }
      
    }
}
