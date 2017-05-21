using System.Linq;

namespace Messenger
{
    public class MessageSplitter
    {
        public bool Splitted;
        private bool[] _splitterRecognized;
        private static readonly int[] SplitterAscii = { 13, 10 };

        public MessageSplitter()
        {
            CreateSplitterRecognizedSymtpoms();
        }

        public void RecognizeSpliter(int value)
        {
            int splitterIndex = GetSplitterIndex(value);

            if (splitterIndex == -1)
            {
                NoSplitterRecognized();
                return;
            }

            SplitterRecognized(splitterIndex);

            CheckAllSplittersRecognized();
        }

        private void CheckAllSplittersRecognized()
        {
            if (_splitterRecognized.Any(splitter => splitter == false))
                return;

            Splitted = true;
        }

        private void SplitterRecognized(int splitterIndex)
        {
            if (!IsSplittersBeforeRecognized(splitterIndex)) return;

            _splitterRecognized[splitterIndex] = true;
        }

        private bool IsSplittersBeforeRecognized(int splitterIndex)
        {
            for (int i = 0; i < splitterIndex; i++)
                if (_splitterRecognized[i] != true)
                    return false;

            return true;
        }

        private void NoSplitterRecognized()
        {
            for (int i = 0; i < _splitterRecognized.Length; i++)
                _splitterRecognized[i] = false;
        }

        private int GetSplitterIndex(int value)
        {
            for (int i = 0; i < SplitterAscii.Length; i++)
                if (value == SplitterAscii[i])
                    return i;

            return -1;
        }

        private void CreateSplitterRecognizedSymtpoms()
        {
            _splitterRecognized = new bool[SplitterAscii.Length];

            for (int i = 0; i < SplitterAscii.Length; i++)
                _splitterRecognized[i] = false;
        }
    }
}
