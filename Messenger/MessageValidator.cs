namespace Messenger
{
    public static class MessageValidator
    {
        public static bool IsTooLong(StreamMessage streamMessage, int maxLength)
        {
            return streamMessage.AcceptedMessage.AsciiValues.Count > maxLength;
        }
    }
}
