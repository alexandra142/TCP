using Messenger;
using Model;

namespace TCPServer.Services
{
    public static class PickUpService
    {
        public static void TryPickUp(StreamMessage streamMessage, ClientRobot robot)
        {
            GetKey(streamMessage);

            if (IsKeyValid(streamMessage, robot))
                MessageService.SendOk(streamMessage);
            else
                MessageService.SendSyntaxError(streamMessage, robot);
        }

        private static bool IsKeyValid(StreamMessage streamMessage, ClientRobot robot)
        {
            if (MessageValidator.IsRecharging(streamMessage))
            {
                RechargingService.WaitForRecharging(streamMessage, robot);
                streamMessage.ReadMessage("Accepted key", MaxLenths.Password);
                IsKeyValid(streamMessage, robot);
            }
            if (MessageValidator.IsTooLong(streamMessage, MaxLenths.Login)) return false;
            return true;
        }

        private static void GetKey(StreamMessage streamMessage)
        {
            MessageService.SendPickUpChallenge(streamMessage);
        }
    }
}
