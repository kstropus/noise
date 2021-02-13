using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Noise
{
    public static class BuildingBlocks
    {
        public static void Main(Stream outStream)
        {
            List<ISteppable> steppables = new List<ISteppable>();

            SumDouble block1 = new SumDouble();
            SumDouble block2 = new SumDouble();
            WrapInt block3 = new WrapInt(5);

            Connector<double> connector1 = 
                new Connector<double>(
                    () => block2.Output, 
                    i => block1.InputB = i);
            
            Connector<double> connector2 = 
                new Connector<double>(
                    () => block1.Output, 
                    i => block2.InputB = i);

            Connector<double> connector3 = 
                new Connector<double>(
                    () => block2.Output, 
                    i => block3.Input = (int)i);
            
            steppables.AddRange(
                new ISteppable[] 
                {
                    connector1,
                    connector2,
                    connector3,
                    block1, 
                    block2,
                    block3
                });

            block1.InputA = 1;
            block1.InputB = 0;

            block2.InputA = 1;
            block2.InputB = 0;
            
            bool done = false;

            while(!done)
            {
                foreach(var steppable in steppables)
                    steppable.Step();
                
                Console.WriteLine(block3.Output);

                for(int i = 0 ; i < 10 ; i++)
                {
                    if(Console.KeyAvailable)
                    {
                        done = true;
                        break;
                    }
                    
                    Thread.Sleep(50);
                }
            }
        }
    }

    public class WrapInt : ISteppable
    {
        public WrapInt(int bigNPowerOfTwo)
        {
            if(bigNPowerOfTwo <= 0)
                throw new ArgumentException($"{nameof(bigNPowerOfTwo)} must be greater than zero.");

            _shiftAmount = 32 - bigNPowerOfTwo;
        }

        public int Input { get; set; }
        public int Output { get; private set; }

        public void Step()
        {
            Output = (Input << _shiftAmount) >> _shiftAmount;
        }

        private readonly int _shiftAmount;
    }

    public class WrapDouble : ISteppable
    {
        public WrapDouble(int bigN)
        {
            if(bigN <= 0)
                throw new ArgumentException($"{nameof(bigN)} must be greater than zero.");

            _bigN = bigN;
        }

        public double Input { get; set; }
        public double Output { get; private set; }

        public void Step()
        {
            double output = Input;

            while(output >= _bigN)
                output -= 2 * _bigN;
            
            while(output < -_bigN)
                output += 2 * _bigN;
            
            Output = output;
        }

        private readonly int _bigN;
    }
    
    public interface ISteppable
    {
        void Step();
    }

    public class SumDouble : ISteppable
    {
        public double InputA { get; set; }
        public double InputB { get; set; }

        public double Output { get; private set; }

        public void Step()
        {
            Output = InputA + InputB;
        }
    }

    public class SumInt32 : ISteppable
    {
        public Int32 InputA { get; set; }
        public Int32 InputB { get; set; }

        public Int32 Output { get; private set; }

        public void Step()
        {
            Output = InputA + InputB;
        }
    }

    public class SumUInt32 : ISteppable
    {
        public UInt32 InputA { get; set; }
        public UInt32 InputB { get; set; }

        public UInt32 Output { get; private set; }

        public void Step()
        {
            Output = InputA + InputB;
        }
    }

    public class Connector<T> : ISteppable
    {
        public Connector(Func<T> getInput, Action<T> setOutput)
        {
            _getInput = getInput;
            _setOutput = setOutput;
        }

        public void Step()
        {
            _setOutput(_getInput());
        }

        private readonly Func<T> _getInput;
        private readonly Action<T> _setOutput;
    }
}