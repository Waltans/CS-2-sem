using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var open = new Dictionary<int, int>();
            var close = new Dictionary<int, int>();
            var stack = new Stack<int>();
            var loopCommands = new BrainfuckLoopCommands();

            loopCommands.BracketsCondition(vm, open, close, stack);

            vm.RegisterCommand('[', delegate(IVirtualMachine b)
            {
                b.InstructionPointer = b.Memory[b.MemoryPointer] == 0
                    ? open[b.InstructionPointer]
                    : b.InstructionPointer;
            });

            vm.RegisterCommand(']', delegate(IVirtualMachine b)
            {
                b.InstructionPointer = b.Memory[b.MemoryPointer] == 0
                    ? b.InstructionPointer
                    : close[b.InstructionPointer];
            });
        }

        private void BracketsCondition(IVirtualMachine vm, Dictionary<int, int> open, Dictionary<int, int> close,
            Stack<int> stack)
        {
            for (var i = 0; i < vm.Instructions.Length; i++)
            {
                if (vm.Instructions[i] == '[')
                {
                    stack.Push(i);
                }

                if (vm.Instructions[i] != ']') continue;
                close[i] = stack.Pop();
                open[close[i]] = i;
            }
        }
    }
}