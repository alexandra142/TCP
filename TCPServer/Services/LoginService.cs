using System;
using System.Collections.Generic;
using System.Linq;
using Messenger;
using Model;

namespace TCPServer.Services
{
    public static class LoginService
    {
        public static bool TryLogIn(StreamMessage streamMessage, ClientRobot robot)
        {
            GetUserName(streamMessage, robot);

            if (streamMessage.ClientClosed) return false;

            string expectedPassword = GetExpectedPassword(streamMessage);

            GetPassword(streamMessage, robot);

            if (streamMessage.ClientClosed) return false;

            if (LoginIsValid(expectedPassword, streamMessage))
            {
                MessageService.SendOk(streamMessage);
                return true;
            }

            MessageService.SendLoginFailed(streamMessage);

            return false;
        }

        private static void GetUserName(StreamMessage streamMessage, ClientRobot robot)
        {
            MessageService.SendLoginChallenge(streamMessage);
            ValidUserName(streamMessage, robot);
        }

        private static void ValidUserName(StreamMessage streamMessage, ClientRobot robot)
        {
            if (UserNameIsValid(streamMessage)) return;

            MessageService.SendSyntaxError(streamMessage, robot);
        }

        private static bool UserNameIsValid(StreamMessage streamMessage)
        {
            var userName = streamMessage.AcceptedMessage.DecodedData;
            if (NoMessageDecoded(userName)) return false;
            if (MessageValidator.IsTooLong(streamMessage, MaxLenths.Login)) return false;
            if (BeginWithKeyWord(userName)) return false;

            return true;
        }

        private static string GetExpectedPassword(StreamMessage streamMessage)
        {
            var asciiSum = streamMessage.AcceptedMessage.AsciiValues.Sum() - Constants.SplitterAscii.Sum();
            return asciiSum.ToString();
        }

        private static void GetPassword(StreamMessage streamMessage, ClientRobot robot)
        {
            MessageService.SendPasswordChallenge(streamMessage);
            ValidPassword(streamMessage, robot);
        }

        private static void ValidPassword(StreamMessage streamMessage, ClientRobot robot)
        {
            if (PasswordIsValid(streamMessage)) return;

            MessageService.SendSyntaxError(streamMessage, robot);
        }

       private static bool LoginIsValid(string expectedPassword, StreamMessage streamMessage)
        {
            return expectedPassword == streamMessage.AcceptedMessage.DecodedData;
        }

        private static bool PasswordIsValid(StreamMessage streamMessage)
        {
            var password = streamMessage.AcceptedMessage.DecodedData;
            if (NoMessageDecoded(password)) return false;
            if (MessageValidator.IsTooLong(streamMessage, MaxLenths.Password)) return false;
            if (!ContainsOnlyDigits(password)) return false;

            return true;
        }

        private static bool ContainsOnlyDigits(string password)
        {
            return password.All(char.IsDigit);
        }

        private static bool NoMessageDecoded(string decodedData)
        {
            return decodedData == string.Empty;
        }

        private static bool BeginWithKeyWord(string decodedData)
        {
            var keyWords = GetKeyWords();

            return keyWords.Contains(decodedData);
        }

        private static List<string> GetKeyWords()
        {
            var enumValues = Enum.GetValues(typeof(ClientMessageCodes));
            List<string> keyWords = new List<string>();
            foreach (var enumValue in enumValues)
                keyWords.Add(((ClientMessageCodes)enumValue).GetEnumDescription());

            return keyWords;
        }
    }
}
