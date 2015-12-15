using System;
using System.Collections.Generic;

namespace Cyral.BrainF.Interpreter
{
    public class Optimizer
    {
        // Enabled optimization.
        public readonly bool
            Contraction = true,
            Clear = true;

        public Dictionary<int, int> Jumps { get; private set; }

        private List<Instruction> ops;
        private List<Instruction> newOps;  

        /// <summary>
        /// Optimize instruction list to be faster and more consise.
        /// </summary>
        public List<Instruction> Optimize(List<Instruction> ops)
        {
            var ip = 0;
            this.ops = ops;
            newOps = new List<Instruction>();
            Jumps = new Dictionary<int, int>();
            var stack = new Stack<int>();

            while (ip < ops.Count)
            {
                var instruction = ops[ip];
                var op = instruction.Operator;
                switch (op)
                {
                    // Optimize add, sub, left, and right to include the number to move by.
                    case Op.Add:
                    case Op.Sub:
                    case Op.Left:
                    case Op.Right:
                        ip = OptimizeRow(ip, instruction);
                        break;
                    case Op.Open:
                        // Optimize clear loops ([-])
                        if (Clear && ops[ip + 1] == Op.Sub && ops[ip + 2] == Op.Close)
                        {
                            newOps.Add(Op.Clear);
                            ip += 2;
                        }
                        else
                        {
                            newOps.Add(instruction);
                        }
                        break;
                    default:
                        newOps.Add(instruction);
                        break;
                }

                ip++;
            }

            ip = 0;

            // Lastly, create a lookup table of ending and closing brackets.
            while (ip < newOps.Count)
            {
                var instruction = newOps[ip];
                var op = instruction.Operator;
                switch (op)
                {
                    case Op.Open:
                        stack.Push(ip);
                        break;
                    case Op.Close:
                        if (stack.Count == 0)
                            throw new InvalidOperationException(
                                $"Bracket mismatch. No opening bracket for closing bracket at instruction {ip}.");
                        var open = stack.Pop();
                        Jumps.Add(open, ip);
                        Jumps.Add(ip, open);
                        break;
                }

                ip++;
            }
            return newOps;
        }

        private int OptimizeRow(int ip, Instruction instruction)
        {
            if (Contraction)
            {
                var loop = 0;
                while (ops[ip + loop] == instruction.Operator)
                    loop++;
                ip += loop - 1;
                instruction.Data[0] = (byte) loop;
            }
            newOps.Add(instruction);
            return ip;
        }
    }
}