using System.IO;
using System.Net.Sockets;
using System.Text;
using Model;

namespace Messenger
{
    public class StreamMessage
    {
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public AcceptedMessage AcceptedMessage { get; set; }
        public bool ClientClosed;
        public StreamMessage(TcpClient client)
        {
            StreamReader = new StreamReader(client.GetStream(), Encoding.UTF8);

            StreamWriter = new StreamWriter(client.GetStream());
            AcceptedMessage = new AcceptedMessage(client);
        }

        public void WriteMessage(string message)
        {
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();
        }

        public void ReadMessage(string writeConsoleMessage, int maxLength)
        {
            ClearOldValues();
            MessageSplitter splitter = new MessageSplitter();
            int charCount = 0;
            while (!splitter.Splitted && charCount< maxLength)
            {
                charCount++;
                int value = StreamReader.Read();
                splitter.RecognizeSpliter(value);
                AcceptedMessage.AsciiValues.Add(value);
            }
            if (!splitter.Splitted && charCount == maxLength)
                AcceptedMessage.AsciiValues.Add(0);

            AcceptedMessage.Decode(writeConsoleMessage);
        }

        private void ClearOldValues()
        {
            AcceptedMessage.AsciiValues.Clear();
            AcceptedMessage.DecodedData = string.Empty;
        }

        public void CloseClient(ClientRobot robot)
        {
            if (robot.IsClosed) return;

            robot.Close();
            StreamReader.Dispose();
            StreamWriter.Dispose();
            ClientClosed = true;
        }

        public string GetData()
        {
            return AcceptedMessage.DecodedData;
        }
    }
}
