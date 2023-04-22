using System;

namespace func_rocket;

public class ForcesTask
{
    public static RocketForce GetThrustForce(double forceValue) => r =>
    {
        return new Vector(Math.Cos(r.Direction) * forceValue, Math.Sin(r.Direction) * forceValue);
    };

    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize)
        => r => gravity(spaceSize,r.Location);

    public static RocketForce Sum(params RocketForce[] forces)
    {
        return r =>
        {
            var vectors = new Vector(0, 0);
            foreach (var vector in forces)
            {
                vectors += vector(r);
            }
            return vectors;
        };
    }
}