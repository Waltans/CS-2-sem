using System.Collections.Generic;
using System.Linq;

	namespace Clones;
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private List<Clone> _clones = new List<Clone> { new Clone { } };
		public string Execute(string request)
		{
			var activity = request.Split(' ')[0];
			var splitRequest = request.Split(' ');
			var number = int.Parse(request.Split(' ')[1]);
			switch (activity)
			{
				case "learn":
					_clones[number - 1].Learn(splitRequest[2]);
					break;
				case "rollback":
					_clones[number - 1].Rollback();
					break;
				case "relearn":
					_clones[number - 1].Relearn();
					break;
				case "clone":
					_clones.Add(_clones[number - 1].NewClone());
					break;
				case "check":
					return _clones[number - 1].CheckMemory();
				default:
					return null;
			}
			return null;
		}
	}

	public class Clone
	{
		private Stack<string> history = new Stack<string>();
		private Stack<string> memory = new Stack<string>();
		
		private bool subsidiary;
		private bool parental;
		private string item; 
		
		public void Learn(string item)
		{
			if (subsidiary || parental)
				StackReverse();
			memory.Push(item);
			history.Clear();
		}
		
		public void Rollback()
		{
			if (subsidiary || parental)
				StackReverse();
			item = memory.Pop();
			history.Push(item);
		}

		public void Relearn()
		{
			if (subsidiary || parental)
				StackReverse();
			item = history.Pop();
			memory.Push(item);
		}
		
		public Clone NewClone()
		{
			parental = true;
			return new Clone
			{
				history = history,
				memory = memory,
				subsidiary = true
			};
		}
		
		public string CheckMemory()
		{
			return memory.Count == 0 ? "basic" : memory.Peek();
		}
		
		private void StackReverse()
		{
			history = new Stack<string>(history.Reverse());
			memory = new Stack<string>(memory.Reverse());
		}
	}