using Messenger;
using Model;

namespace TCPServer.Services
{
    static class RechargingService
    {
        public static void WaitForRecharging(StreamMessage streamMessage, ClientRobot robot)
        {
            robot.TcpClient.ReceiveTimeout = Constants.TIMEOUT_RECHARGING;
            streamMessage.ReadMessage("Accepted Full charged", MaxLenths.FullPower);
            if (!MessageValidator.ValidFullPower(streamMessage)) MessageService.SendLogicError(streamMessage, robot);
            robot.TcpClient.ReceiveTimeout = Constants.TIMEOUT;
        }
    }
}
