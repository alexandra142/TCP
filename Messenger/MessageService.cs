namespace Messenger
{
    public static class MessageService
    {
        public static void SendOk(StreamMessage streamMessage)
        {
            WriteMessage(streamMessage, ServerMessagesCodes.SERVER_OK);
        }

        public static void SendLoginFailed(StreamMessage streamMessage)
        {
            WriteMessage(streamMessage, ServerMessagesCodes.SERVER_LOGIN_FAILED);
        }

        public static void SendSyntaxError(StreamMessage streamMessage)
        {
            WriteMessage(streamMessage, ServerMessagesCodes.SERVER_SYNTAX_ERROR);

            streamMessage.CloseClient();
        }

        public static void SendLoginChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_USER, "Accepted userName");
        }

        public static void SendPasswordChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_PASSWORD, "Accepted password");
        }

        public static void SendMoveChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_MOVE, "Move");
        }

        public static void SendTurnRightChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_TURN_RIGHT, "Turn right");
        }

        public static void SendTurnLeftChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_TURN_LEFT, "Turn left");
        }

        public static void SendPickUpChallenge(StreamMessage streamMessage)
        {
            SendMessage(streamMessage, ServerMessagesCodes.SERVER_PICK_UP, "Pick up");
        }

        private static void SendMessage(StreamMessage streamMessage, ServerMessagesCodes messageCode, string consoleMessage)
        {
            WriteMessage(streamMessage, messageCode);
            streamMessage.ReadMessage(consoleMessage);
        }

        private static void WriteMessage(StreamMessage streamMessage, ServerMessagesCodes messageCode)
        {
            string message = ServerMessageFactory.Create(messageCode);
            streamMessage.WriteMessage(message);
        }
    }
}
