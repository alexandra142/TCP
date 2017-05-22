using Messenger;

namespace TCPServer.Services
{
    public static class MoveService
    {
        public static void TryMove(StreamMessage streamMessage)
        {
            Move(streamMessage);
            if (!GetConfirm(streamMessage)) return;

            Move(streamMessage);
            if (!GetConfirm(streamMessage)) return;

            Move(streamMessage);
        }

        private static void Move(StreamMessage streamMessage)
        {
            MessageService.SendMoveChallenge(streamMessage);
        }

        private static bool GetConfirm(StreamMessage streamMessage)
        {
            if (IsConfirmMessageValid(streamMessage)) return true;

            MessageService.SendSyntaxError(streamMessage);
            return false;
        }

        private static bool IsConfirmMessageValid(StreamMessage streamMessage)
        {
            if (MessageValidator.IsTooLong(streamMessage, MaxLenths.Confirm))
                return false;

            return HasRightFormat(streamMessage);
        }

        private static bool HasRightFormat(StreamMessage streamMessage)
        {
            var message = streamMessage.AcceptedMessage.DecodedData;
            string[] data = message.Split(' ');
            if (data.Length != 3) return false;
            if (data[0] != ClientMessageCodes.CLIENT_CONFIRM.GetEnumDescription()) return false;

            return ValidatePosition(data);
        }

        private static bool ValidatePosition(string[] data)
        {
            int value;
            if (!int.TryParse(data[1], out value)) return false;
            if (!int.TryParse(data[2], out value)) return false;

            return true;
        }
    }
}
