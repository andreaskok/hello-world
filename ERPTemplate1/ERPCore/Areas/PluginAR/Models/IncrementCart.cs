namespace PluginAR.Models
{
    public class IncrementCart
    {
        private long? LastNumber = 0;

        public void InitializeNumber(long? initNo)
        {
            LastNumber = initNo;
        }

        public long? GetNewNo()
        {
            LastNumber += 1;
            return LastNumber;
        }

        public long? GetCurrentNo()
        {
            return LastNumber;
        }

        public void Clear()
        {
            LastNumber = 0;
        }
    }
}