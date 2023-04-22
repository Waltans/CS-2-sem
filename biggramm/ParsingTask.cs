using System;
using System.Collections.Generic;
using System.Linq;
using static System.Int32;

namespace linq_slideviews
{
	public class ParsingTask
	{
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			var types = new[] { "theory", "exercise", "quiz" };
			return lines.Select(x => x.Split(';'))
				.Where(x => x.Length == 3 && types.Any(s => s == x[1]) && TryParse(x[0], out int id))
				.Select(x =>
					x[1] == "theory" ? new SlideRecord(Parse(x[0]), SlideType.Theory, x[2]) :
					x[1] == "exercise" ? new SlideRecord(Parse(x[0]), SlideType.Exercise, x[2]) :
					new(Parse(x[0]), SlideType.Quiz, x[2]))
				.ToDictionary(x => x.SlideId, y => y);
		}
		
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			VisitRecord Selector(string x)
			{
				var line = x.Split(';');
				return line.Length == 4 && TryParse(line[0], out var userId) && TryParse(line[1], out var slideId) &&
				       DateTime.TryParse($"{line[2]} {line[3]}", out var date) && slides.ContainsKey(slideId)
					? new VisitRecord(userId, slideId, date, slides[slideId].SlideType)
					: throw new FormatException("Wrong line [" + x + "]");
			}
			return lines
					
				.Where(x => x != "UserId;SlideId;Date;Time")
				.Select(Selector);
		}
	}
}