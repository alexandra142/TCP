using System;
using Messenger;
using Model;

namespace TCPServer.Services
{
    public static class MoveService
    {
        public static void TryMoveToGoal(StreamMessage streamMessage, ClientRobot robot)
        {
            SetOrientationToRobot(streamMessage, robot);
            while (robot.Position.X != 0 || robot.Position.Y != 0)
            {
                TurnToXAxis(streamMessage, robot);
                GetToXAxis(streamMessage, robot);
                TurnToYAxis(streamMessage, robot);
                GetToYAxis(streamMessage, robot);
            }
        }

        private static void SetOrientationToRobot(StreamMessage streamMessage, ClientRobot robot)
        {
            Move(streamMessage, robot);
            var positionA = GetPositionSetToRobot(streamMessage, robot);

            Move(streamMessage, robot);
            var positionB = GetPositionSetToRobot(streamMessage, robot);

            robot.Orientation = OrientationRecogrition.Recognize(positionA, positionB);
        }

        private static Position GetPositionSetToRobot(StreamMessage streamMessage, ClientRobot robot)
        {
            var data = streamMessage.AcceptedMessage.DecodedData.Split(' ');
            if (!HasRightFormat(streamMessage)) return robot.Position;

            int x = GetPositionInt(data[1]);
            int y = GetPositionInt(data[2]);

            robot.Position = new Position(x, y);

            return robot.Position;
        }

        private static void TurnToXAxis(StreamMessage streamMessage, ClientRobot robot)
        {
            switch (robot.Orientation)
            {
                case Orientation.East:
                    if (robot.Position.Y > 0)
                        TurnRight(streamMessage, robot);
                    else
                        TurnLeft(streamMessage, robot);
                    break;
                case Orientation.South:
                    if (robot.Position.Y < 0)
                        TurnAbout(streamMessage, robot);
                    break;
                case Orientation.West:
                    if (robot.Position.Y > 0) TurnLeft(streamMessage, robot);
                    else
                        TurnRight(streamMessage, robot);
                    break;

                case Orientation.North:
                    if (robot.Position.Y > 0)
                        TurnAbout(streamMessage, robot);
                    break;
            }
        }

        private static void TurnToYAxis(StreamMessage streamMessage, ClientRobot robot)
        {
            switch (robot.Orientation)
            {
                case Orientation.East:
                    if (robot.Position.X > 0)
                        TurnAbout(streamMessage, robot);
                    break;
                case Orientation.South:
                    if (robot.Position.X < 0)
                        TurnLeft(streamMessage, robot);
                    else
                        TurnRight(streamMessage, robot);
                    break;
                case Orientation.West:
                    if (robot.Position.X > 0) TurnAbout(streamMessage, robot);
                    break;
                case Orientation.North:
                    if (robot.Position.X < 0)
                        TurnRight(streamMessage, robot);
                    else
                        TurnLeft(streamMessage, robot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void GetToXAxis(StreamMessage streamMessage, ClientRobot robot)
        {
            if (robot.Position.Y == 0) return;

            while (robot.Position.Y != 0)
            {
                var oldPosition = robot.Position;
                Move(streamMessage, robot);
                if (robot.IsCloserToXAxis(oldPosition)) continue;
                if (robot.HasntMoved(oldPosition)) continue;
                if (robot.Position.X != oldPosition.X)
                {
                    PositionXChanged(streamMessage, robot, oldPosition);
                    continue;
                }
                TurnAbout(streamMessage, robot);
            }
        }

        private static void GetToYAxis(StreamMessage streamMessage, ClientRobot robot)
        {
            if (robot.Position.X == 0) return;

            while (robot.Position.X != 0)
            {
                var oldPosition = robot.Position;
                Move(streamMessage, robot);
                if (robot.IsCloserToYAxis(oldPosition)) continue;
                if (robot.HasntMoved(oldPosition)) continue;
                if (robot.Position.Y != oldPosition.Y)
                {
                    PositionYChanged(streamMessage, robot);
                    continue;
                }
                TurnAbout(streamMessage, robot);
            }

            Console.WriteLine(robot.Position);
        }

        private static void PositionYChanged(StreamMessage streamMessage, ClientRobot robot)
        {
            TurnAbout(streamMessage, robot);
            Move(streamMessage, robot);
        }

        private static void PositionXChanged(StreamMessage streamMessage, ClientRobot robot, Position oldPosition)
        {
            if (robot.IsCloserToYAxis(oldPosition))
            {
                if (robot.Position.X >= 0)
                    TurnLeft(streamMessage, robot);
                else
                    TurnRight(streamMessage, robot);
            }
            else
            {
                if (robot.Position.X >= 0)
                    TurnRight(streamMessage, robot);
                else
                    TurnLeft(streamMessage, robot);
            }
        }

        private static int GetPositionInt(string position)
        {
            int positionInt;
            int.TryParse(position, out positionInt);

            return positionInt;
        }

        private static void Move(StreamMessage streamMessage, ClientRobot robot)
        {
            MessageService.SendMoveChallenge(streamMessage);
            GetConfirm(streamMessage, robot);
            var oldPosition = robot.Position;
            GetPositionSetToRobot(streamMessage, robot);
            if (robot.HasntMoved(oldPosition))
                Move(streamMessage, robot);
        }

        private static void TurnAbout(StreamMessage streamMessage, ClientRobot robot)
        {
            TurnRight(streamMessage, robot);
            TurnRight(streamMessage, robot);
        }

        private static void TurnRight(StreamMessage streamMessage, ClientRobot robot)
        {
            MessageService.SendTurnRightChallenge(streamMessage);
            GetConfirm(streamMessage, robot);
            robot.TurnRight();
        }

        private static void TurnLeft(StreamMessage streamMessage, ClientRobot robot)
        {
            MessageService.SendTurnLeftChallenge(streamMessage);
            GetConfirm(streamMessage, robot);
            robot.TurnLeft();
        }

        private static void GetConfirm(StreamMessage streamMessage, ClientRobot robot)
        {
            if (IsConfirmMessageValid(streamMessage)) return;

            MessageService.SendSyntaxError(streamMessage, robot);
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
