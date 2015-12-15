namespace Cyral.BrainF.Interpreter
{
    /// <summary>
    ///  Represents an instruction operator and associated data.
    /// </summary>
    public sealed class Instruction
    {
        public Op Operator { get; private set; }

        public byte[] Data { get; private set; } = new byte[3];

        public Instruction(Op op)
        {
            Operator = op;
        }

        public Instruction(Op op, byte data1) : this(op)
        {
            Data[0] = data1;
        }

        public Instruction(Op op, byte data1, byte data2) : this(op, data1)
        {
            Operator = op;
            Data[1] = data2;
        }

        public Instruction(Op op, byte data1, byte data2, byte data3) : this(op, data1, data2)
        {
            Operator = op;
            Data[2] = data3;
        }

        public static implicit operator Instruction(Op op) => new Instruction(op);

        public static bool operator ==(Instruction i, Op op) => i != null && i.Operator == op;

        public static bool operator !=(Instruction i, Op op) => !(i == op);
    }
}