using System.Collections.Generic;

namespace Cyral.BrainF.Interpreter
{
    internal class Parser
    {
        /// <summary>
        /// Parse source into operator list.
        /// </summary>
        public List<Instruction> Parse(char[] source)
        {
            var ip = 0; // Instruction pointer.
            var length = source.Length;
            var ops = new List<Instruction>(length);
            while (ip < length)
            {
                var instruction = source[ip];

                switch (instruction)
                {
                    case '>':
                        ops.Add(new Instruction(Op.Right, 1));
                        break;
                    case '<':
                        ops.Add(new Instruction(Op.Left, 1));
                        break;
                    case '+':
                        ops.Add(new Instruction(Op.Add, 1));
                        break;
                    case '-':
                        ops.Add(new Instruction(Op.Sub, 1));
                        break;
                    case '[':
                        ops.Add(Op.Open);
                        break;
                    case ']':
                        ops.Add(Op.Close);
                        break;
                    case '.':
                        ops.Add(Op.Output);
                        break;
                    case ',':
                        ops.Add(Op.Input);
                        break;
                }

                ip++;
            }
            return ops;
        }
    }
}