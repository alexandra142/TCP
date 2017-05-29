using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger;
using Model;

namespace TCPServer.Services
{
    public static class PickUpService
    {
        public static void TryPickUp(StreamMessage streamMessage, ClientRobot robot)
        {
            GetKey(streamMessage);

            if (IsKeyValid(streamMessage))
                MessageService.SendOk(streamMessage);
            else
                MessageService.SendSyntaxError(streamMessage, robot);
        }

        private static bool IsKeyValid(StreamMessage streamMessage)
        {
            if (MessageValidator.IsTooLong(streamMessage, MaxLenths.Login)) return false;
            return true;
        }

        private static void GetKey(StreamMessage streamMessage)
        {
            MessageService.SendPickUpChallenge(streamMessage);
        }
    }
}
