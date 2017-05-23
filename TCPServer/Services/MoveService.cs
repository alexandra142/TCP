using System;
using Messenger;
using Model;

namespace TCPServer.Services
{
    public static class MoveService
    {
        public static void TryMoveToGoal(StreamMessage streamMessage)
        {
            streamMessage.Robot.Orientation = GetOrientation(streamMessage);
            Position position = GetPosition(streamMessage);
            TurnToXAxis(streamMessage, position);
            GetToXAxis(streamMessage, position);
            TurnToYAxis(streamMessage, position);
            GetToGoal(streamMessage, position);
        }

        private static Orientation GetOrientation(StreamMessage streamMessage)
        {
            Move(streamMessage);
            var positionA = GetPosition(streamMessage);

            Move(streamMessage);
            var positionB = GetPosition(streamMessage);

            return OrientationRecogrition.Recognize(positionA, positionB);
        }

        private static Position GetPosition(StreamMessage streamMessage)
        {
            var data = streamMessage.AcceptedMessage.DecodedData.Split(' ');

            int x = GetPositionInt(data[1]);
            int y = GetPositionInt(data[2]);

            var position = new Position(x, y);

            return position;
        }

        private static void TurnToXAxis(StreamMessage streamMessage, Position position)
        {
            switch (streamMessage.Robot.Orientation)
            {
                case Orientation.East:
                    if (position.Y > 0)
                        TurnRight(streamMessage);
                    else
                        TurnLeft(streamMessage);
                    break;
                case Orientation.South:
                    if (position.Y < 0)
                        TurnAbout(streamMessage);
                    break;
                case Orientation.West:
                    if (position.Y > 0) TurnLeft(streamMessage);
                    else
                        TurnRight(streamMessage);
                    break;
               
                case Orientation.North:
                    if (position.Y > 0)
                        TurnAbout(streamMessage);
                    break;
            }
        }

        private static void TurnToYAxis(StreamMessage streamMessage, Position position)
        {
            switch (streamMessage.Robot.Orientation)
            {
                case Orientation.East:
                    if (position.X > 0)
                        TurnAbout(streamMessage);
                    break;
                case Orientation.South:
                    if (position.X < 0)
                        TurnLeft(streamMessage);
                    else
                        TurnRight(streamMessage);
                    break;
                case Orientation.West:
                    if (position.X > 0) TurnAbout(streamMessage);
                    break;
                case Orientation.North:
                    if (position.X < 0)
                        TurnRight(streamMessage);
                    else
                        TurnLeft(streamMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void GetToXAxis(StreamMessage streamMessage, Position position)
        {
            if (position.Y == 0) return;

            for (int i = 0; i < Math.Abs(position.Y); i++)
                Move(streamMessage);
        }

        private static void GetToGoal(StreamMessage streamMessage, Position position)
        {
            if (position.X == 0) return;

            for (int i = 0; i < Math.Abs(position.X); i++)
                Move(streamMessage);

            position = GetPosition(streamMessage);
            Console.WriteLine(position);
        }

        private static int GetPositionInt(string position)
        {
            int positionInt;
            int.TryParse(position, out positionInt);

            return positionInt;
        }

        private static void Move(StreamMessage streamMessage)
        {
            MessageService.SendMoveChallenge(streamMessage);
            GetConfirm(streamMessage);
            streamMessage.Robot.MoveCoun++;
        }

        private static void TurnAbout(StreamMessage streamMessage)
        {
            TurnRight(streamMessage);
            TurnRight(streamMessage);
        }

        private static void TurnRight(StreamMessage streamMessage)
        {
            MessageService.SendTurnRightChallenge(streamMessage);
            GetConfirm(streamMessage);
            streamMessage.Robot.TurnRight();
        }

        private static void TurnLeft(StreamMessage streamMessage)
        {
            MessageService.SendTurnLeftChallenge(streamMessage);
            GetConfirm(streamMessage);
            streamMessage.Robot.TurnLeft();
        }

        private static void GetConfirm(StreamMessage streamMessage)
        {
            if (IsConfirmMessageValid(streamMessage)) return;

            MessageService.SendSyntaxError(streamMessage);
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
