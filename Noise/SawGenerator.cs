namespace Noise
{
    public class SawGenerator
    {
        public double Get(double phase)
        {
            return (phase * 2) - 1;
        }
    }
}