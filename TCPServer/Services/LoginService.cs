using System;
using System.Collections.Generic;
using System.Linq;
using Messenger;

namespace TCPServer.Services
{
    public static class LoginService
    {
        public static void TryLogIn(StreamMessage streamMessage)
        {
            GetUserName(streamMessage);
            var validUsername = UserNameIsValid(streamMessage);
            string expectedPassword = GetExpectedPassword(streamMessage); 

            GetPassword(streamMessage);
           
            SendRespond(validUsername, expectedPassword,streamMessage);
        }

        private static string GetExpectedPassword(StreamMessage streamMessage)
        {
            return (streamMessage.AcceptedMessage.AsciiValues.Sum() - 23).ToString();
        }

        private static void GetUserName(StreamMessage streamMessage)
        {
            MessageService.SendLoginChallenge(streamMessage);
        }

        private static void GetPassword(StreamMessage streamMessage)
        {
            MessageService.SendPasswordChallenge(streamMessage);
        }

        private static void SendRespond(bool validUserName, string expectedPassword, StreamMessage streamMessage)
        {
            if(!validUserName) MessageService.SendLoginFailed(streamMessage);
            if (PasswordIsValid(expectedPassword, streamMessage)) MessageService.SendOk(streamMessage);
            else MessageService.SendLoginFailed(streamMessage);
        }
     
        private static bool UserNameIsValid(StreamMessage streamMessage)
        {
            var userName = streamMessage.AcceptedMessage.DecodedData;
            if (NoMessageDecoded(userName)) return false;
            if (IsTooLong(streamMessage, MaxLenths.Login)) return false;
            if (BeginWithKeyWord(userName)) return false;

            return true;
        }

        private static bool PasswordIsValid(string expectedPassword, StreamMessage streamMessage)
        {
            var password = streamMessage.AcceptedMessage.DecodedData;
            if (NoMessageDecoded(password)) return false;
            if (IsTooLong(streamMessage, MaxLenths.Password)) return false;
            if (!ContainsOnlyDigits(password)) return false;
            if (expectedPassword == password) return true;

            return false;
        }

        private static bool ContainsOnlyDigits(string password)
        {
            return password.All(char.IsDigit);
        }

        private static bool IsTooLong(StreamMessage streamMessage, int maxLength)
        {
            return streamMessage.AcceptedMessage.AsciiValues.Count > maxLength;
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
