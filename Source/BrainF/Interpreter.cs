using System;
using System.Collections.Generic;

namespace Cyral.BrainF.Interpreter
{
    public unsafe class Interpreter
    {
        private readonly int cells;

        private readonly Optimizer optimizer;

        private readonly Parser parser;
        private readonly Func<char> read;

        private readonly Action<char> write;

        public Interpreter(int cells, Action<char> write, Func<char> read)
        {
            this.write = write;
            this.read = read;
            this.cells = cells;

            parser = new Parser();
            optimizer = new Optimizer();
        }

        public void Run(string sourcecode)
        {
            var source = sourcecode.Replace(" ", "").ToCharArray();
            byte *mp = stackalloc byte[cells];
            int ip = 0;


            // Parse source intro operators.
            var opList = parser.Parse(source);

            // Optimize the operators
            var ops = optimizer.Optimize(opList).ToArray();

            // Run the final code.
            while (ip < ops.Length)
            {
                var instruction = ops[ip];
                var op = instruction.Operator;
                //Run the operation for one of the 8 instructions.
                switch (op)
                {
                    case Op.Left: // <
                        mp -= instruction.Data[0];
                        break;
                    case Op.Right: // >
                        mp += instruction.Data[0];
                        break;
                    case Op.Add: // +
                        (*mp) += instruction.Data[0];
                        break;
                    case Op.Sub: // -
                        (*mp) -= instruction.Data[0];
                        break;
                    case Op.Open: // [
                        if (*mp == 0)
                            ip = optimizer.Jumps[ip]; //Select the coresponding close bracket.
                        break;
                    case Op.Close: // ]
                        if (*mp != 0)
                            ip = optimizer.Jumps[ip];
                        //Subtract one, as it will be added back later.
                        break;
                    case Op.Output: // .
                        write.Invoke((char)*mp);
                        break;
                    case Op.Clear:
                        (*mp) = 0;
                        break;
                    case Op.Input: // ,
                        (*mp) = (byte)read.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ip++;
            }
        }
    }
}