namespace Messenger
{
    public static class MessageService
    {
        public static void SendOk(StreamMessage streamMessage)
        {
            string okMessage = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_OK);
            streamMessage.WriteMessage(okMessage);
        }

        public static void SendLoginFailed(StreamMessage streamMessage)
        {
            string message = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_LOGIN_FAILED);
            streamMessage.WriteMessage(message);
        }

        public static void SendSyntaxError(StreamMessage streamMessage)
        {
            string message = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_SYNTAX_ERROR);
            streamMessage.WriteMessage(message);
            streamMessage.CloseClient();
        }

        public static void SendLoginChallenge(StreamMessage streamMessage)
        {
            string logInChallenge = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_USER);
            streamMessage.WriteMessage(logInChallenge);
            streamMessage.ReadMessage("Accepted UserName");
        }

        public static void SendPasswordChallenge(StreamMessage streamMessage)
        {
            string passwordChallenge = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_PASSWORD);
            streamMessage.WriteMessage(passwordChallenge);
            streamMessage.ReadMessage("Accepted Password");
        }

        public static void SendMoveChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_MOVE);
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Moving");
        }

        public static void SendTurnRightChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_TURN_RIGHT);
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Turn right");
        }

        public static void SendTurnLeftChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_TURN_LEFT);
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Turn left");
        }
    }
}
