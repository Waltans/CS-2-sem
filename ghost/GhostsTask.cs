using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
	private static readonly Vector _vector = new Vector(12, 12);
	private readonly Segment _segment = new Segment(new Vector(0, 0), _vector);
	private readonly Cat _cat = new Cat("Snowman", "cute", new DateTime().ToLocalTime());
	private readonly Robot _robot = new Robot("Robot's ID", 0);
	private readonly byte[] formfactorDoc = new byte[] { 50, 50, 50, 50, 50, 50, 50, 50 };	
	public void DoMagic()
	{
		_vector?.Add(_vector);
		_segment?.End.Add(_vector);
		_cat?.Rename("Snow");
		Robot.BatteryCapacity += 1;
		formfactorDoc[5] = 8;
	}
	
	Document IFactory<Document>.Create()
	{
		return new Document("DOC", Encoding.UTF8, formfactorDoc);
	}
	Vector IFactory<Vector>.Create() => _vector;

	Segment IFactory<Segment>.Create() => _segment;

	Cat IFactory<Cat>.Create() => _cat;

	Robot IFactory<Robot>.Create() => _robot;
}