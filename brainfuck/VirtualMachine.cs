using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		private readonly Dictionary<char, Action<IVirtualMachine>> _getInstruction = new();


		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (!_getInstruction.ContainsKey(symbol)) _getInstruction.Add(symbol, execute);
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				Action<IVirtualMachine> value;

				if (_getInstruction.TryGetValue(Instructions[InstructionPointer], out value))
					value(this);
				    

				InstructionPointer++;
			}
		}
	}
}