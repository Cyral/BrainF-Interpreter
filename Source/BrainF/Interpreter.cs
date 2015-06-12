using System;
using System.Collections.Generic;

namespace Cyral.BrainF.Interpreter
{
    public class Interpreter
    {
        /// <summary>
        /// The number of cells of memory.
        /// </summary>
        public int Cells { get; }

        /// <summary>
        /// The current cell's value.
        /// </summary>
        public byte CurrentValue
        {
            get { return Memory[Pointer]; }
            set
            {
                Memory[Pointer] = value;
                memoryUpdated(Pointer, value);
            }
        }

        /// <summary>
        /// The program's input.
        /// </summary>
        public char[] Input { get; private set; }

        /// <summary>
        /// The current position in the input.
        /// </summary>
        public int InputPointer { get; private set; }

        /// <summary>
        /// The current position in the source code.
        /// </summary>
        public int InstructionPointer { get; private set; }

        /// <summary>
        /// A collection of loop jumps, which are pre-processed for speed.
        /// </summary>
        public Dictionary<int, int> Jumps { get; }

        /// <summary>
        /// The length of the source code.
        /// </summary>
        public int Length
        {
            get { return Source.Length; }
        }

        /// <summary>
        /// A representation of the program's memory.
        /// </summary>
        public byte[] Memory { get; private set; }

        /// <summary>
        /// The current cell's position.
        /// </summary>
        public int Pointer { get; private set; }

        /// <summary>
        /// The sourc code.
        /// </summary>
        public char[] Source { get; }

        /// <summary>
        /// The call stack. (Used for loops)
        /// </summary>
        public Stack<int> Stack { get; }

        /// <summary>
        /// Determines if execution should continue or stop.
        /// </summary>
        public bool Stopped { get; set; }

        private readonly Action<int, byte> memoryUpdated;

        public Interpreter(int cells, string source, Action<int, byte> memoryUpdated)
        {
            this.memoryUpdated = memoryUpdated;
            Cells = cells;
            Stack = new Stack<int>();
            Jumps = new Dictionary<int, int>();
            Source = source.Replace(" ", "").ToCharArray();
        }

        public IEnumerable<char> Parse(string input)
        {
            Input = input.ToCharArray();

            //On first run of this source code, or re-runs, set up variables and pre-process code.
            if (!Stopped || InstructionPointer == Length)
            {
                Memory = new byte[Cells];
                Stack.Clear();
                Jumps.Clear();
                InstructionPointer = InputPointer = Pointer = 0;
                //Pre-process loops
                while (InstructionPointer < Length)
                {
                    var instruction = Source[InstructionPointer];

                    if (instruction == '[')
                        Stack.Push(InstructionPointer);
                    if (instruction == ']')
                    {
                        if (Stack.Count == 0)
                            throw new InvalidOperationException(
                                string.Format(
                                    "Bracket mismatch. No opening bracket for closing bracket at instrction {0}.",
                                    InstructionPointer));
                        var open = Stack.Pop();
                        Jumps.Add(open, InstructionPointer);
                        Jumps.Add(InstructionPointer, open);
                    }

                    InstructionPointer++;
                }
                InstructionPointer = 0;
            }
            Stopped = false;

            //Parse and run code.
            while (InstructionPointer < Length && !Stopped)
            {
                var instruction = Source[InstructionPointer];

                //Run the operation for one of the 8 instructions.
                switch (instruction)
                {
                    case '>':
                        Pointer++;
                        if (Pointer > Memory.Length - 1)
                            throw new InvalidOperationException("Pointer incremented above memory bounds.");
                        break;
                    case '<':
                        Pointer--;
                        if (Pointer < 0)
                            throw new InvalidOperationException("Pointer decremented below 0.");
                        break;
                    case '+':
                        CurrentValue++;
                        break;
                    case '-':
                        CurrentValue--;
                        break;
                    case '[':
                        if (CurrentValue == 0)
                            InstructionPointer = Jumps[InstructionPointer]; //Select the coresponding close bracket.
                        break;
                    case ']':
                        if (CurrentValue != 0)
                            InstructionPointer = Jumps[InstructionPointer] - 1; //Subtract one, as it will be added back later.
                        break;
                    case '.':
                        yield return (char)CurrentValue;
                        break;
                    case ',':
                        if (InputPointer < Input.Length)
                            CurrentValue = (byte)Input[InputPointer++];
                        break;
                }

                InstructionPointer++;
            }
        }
    }
}