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
    }
}
