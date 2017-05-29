using System;

namespace Messenger
{
    public static class MessageValidator
    {
        public static bool IsTooLong(StreamMessage streamMessage, int maxLength)
        {
            var x = streamMessage.AcceptedMessage.AsciiValues.Count > maxLength;
            return x;
        }

        public static bool IsRecharging(StreamMessage streamMessage)
        {
            if (streamMessage.AcceptedMessage.DecodedData == ClientMessageCodes.CLIENT_RECHARGING.GetEnumDescription())
                return true;
            return false;
        }

        public static bool ValidFullPower(StreamMessage streamMessage)
        {
            Console.WriteLine("RechargingFull accepted");

            if (IsTooLong(streamMessage, MaxLenths.FullPower))
                return false;

            if (streamMessage.AcceptedMessage.DecodedData == ClientMessageCodes.CLIENT_FULL_POWER.GetEnumDescription())
                return true;

            return false;
        }
    }
}
