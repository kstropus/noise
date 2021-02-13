namespace Noise
{
    public class Accumulator
    {
        public Accumulator(uint increment)
        {
            _increment = increment;
        }

        public uint Next()
        {
            uint returnValue = _accumulator;
            _accumulator += _increment;
            return returnValue;
        }

        private uint _accumulator;
        private uint _increment;
    }
}