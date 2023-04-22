using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
            vm.RegisterCommand('+', b => { unchecked { b.Memory[b.MemoryPointer]++; } });
            vm.RegisterCommand('-', b => { unchecked { b.Memory[b.MemoryPointer]--; } });
            vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
            DoShift(vm);
            var alphabet = AlphabetCreate().ToArray();
            foreach (var symbol in alphabet)
            {
                vm.RegisterCommand(symbol, b => { b.Memory[b.MemoryPointer] = (byte)symbol; });
            }
        }

        private static void DoShift(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                if (b.Memory.Length - 1 == b.MemoryPointer)
                    b.MemoryPointer = 0;
                else
                    b.MemoryPointer++;
            });
            vm.RegisterCommand('<', b =>
            {
                if (b.MemoryPointer == 0)
                    b.MemoryPointer = b.Memory.Length - 1;
                else
                    b.MemoryPointer--;
            });
        }

        private static IEnumerable<char> AlphabetCreate()
        {
            for (var letter = 'A'; letter <= 'Z'; letter++)
            {
                yield return letter;
            }
            for (var letter = 'a'; letter <= 'z'; letter++)
            {
                yield return letter;
            }
            for (var letter = '0'; letter <= '9'; letter++)
            {
                yield return letter;
            }
        }
    }
}