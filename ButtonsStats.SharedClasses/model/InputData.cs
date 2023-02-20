namespace ButtonsStats.Shared.Model
{
    [Serializable]
    public class InputData
    {
        public char KeyPressed { get; }
        public DateTime InputTime { get; }

        public InputData(char keyPressed, DateTime inputTime)
        {
            KeyPressed = keyPressed;
            InputTime = inputTime;
        }

        public override string ToString()
        {
            return $"Char: {KeyPressed}, Time: {InputTime}";
        }
    }
}