namespace Messenger
{
    public static class MessageValidator
    {
        public static bool IsTooLong(StreamMessage streamMessage, int maxLength)
        {
            var x = streamMessage.AcceptedMessage.AsciiValues.Count > maxLength;
            return x;
        }
    }
}
