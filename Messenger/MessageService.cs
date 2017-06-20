using Model;

namespace Messenger
{
    public static class MessageService
    {
        public static void SendOk(StreamMessage streamMessage)
        {
            string okMessage = ServerMessagesCodes.SERVER_OK.GetEnumDescription();
            streamMessage.WriteMessage(okMessage);
        }

        public static void SendLoginFailed(StreamMessage streamMessage)
        {
            string message = ServerMessagesCodes.SERVER_LOGIN_FAILED.GetEnumDescription();
            streamMessage.WriteMessage(message);
        }

        public static void SendSyntaxError(StreamMessage streamMessage, ClientRobot robot)
        {
            string message = ServerMessagesCodes.SERVER_SYNTAX_ERROR.GetEnumDescription();
            streamMessage.WriteMessage(message);
            streamMessage.CloseClient(robot);
        }

        public static void SendLoginChallenge(StreamMessage streamMessage)
        {
            string logInChallenge = ServerMessagesCodes.SERVER_USER.GetEnumDescription();
            streamMessage.WriteMessage(logInChallenge);
            streamMessage.ReadMessage("Accepted UserName", MaxLenths.Login);
        }

        public static void SendPasswordChallenge(StreamMessage streamMessage)
        {
            string passwordChallenge = ServerMessagesCodes.SERVER_PASSWORD.GetEnumDescription();
            streamMessage.WriteMessage(passwordChallenge);
            streamMessage.ReadMessage("Accepted Password");
        }

        public static void SendMoveChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessagesCodes.SERVER_MOVE.GetEnumDescription();
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Moving");
        }

        public static void SendTurnRightChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessagesCodes.SERVER_TURN_RIGHT.GetEnumDescription();
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Turn right");
        }

        public static void SendTurnLeftChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessagesCodes.SERVER_TURN_LEFT.GetEnumDescription();
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Turn left");
        }

        public static void SendPickUpChallenge(StreamMessage streamMessage)
        {
            string message = ServerMessagesCodes.SERVER_PICK_UP.GetEnumDescription();
            streamMessage.WriteMessage(message);
            streamMessage.ReadMessage("Pick up", MaxLenths.Message);
        }

        public static void SendLogicError(StreamMessage streamMessage, ClientRobot robot)
        {
            string message = ServerMessagesCodes.SERVER_LOGIC_ERROR.GetEnumDescription();
            streamMessage.WriteMessage(message);
            robot.Close();
        }
    }
}
