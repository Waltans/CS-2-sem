using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{
    public class LevelsTask
    {
        private static readonly Physics StandardPhysics = new Physics();
        private static readonly Vector Aim = new Vector(700, 500);
        private static readonly Rocket Rocket = new Rocket(new Vector(200, 500), 
            Vector.Zero, -0.5 * Math.PI);

        private static readonly Gravity WhiteGravity = delegate(Vector size, Vector v)
        {
            var vectorLength = (Aim - v).Length;
            return (Aim - v).Normalize() * -140 * vectorLength / (Math.Pow(vectorLength,2)+1);
        };

        private static readonly Gravity BlackGravity = delegate(Vector size, Vector v)
        {
            var vectorLength = ((Aim + Rocket.Location) * 0.5 - v).Length;
            return ((Aim + Rocket.Location) * 0.5 - v).Normalize() * 300 * vectorLength 
                   / (Math.Pow(vectorLength,2) + 1);
        };
        
        private static readonly Gravity MixedGravity = (size, v) =>
            (WhiteGravity(size, v) + BlackGravity(size, v)) / 2;
        
        public static IEnumerable<Level> CreateLevels()
        {
            yield return new Level("Zero", Rocket, new Vector(600, 200),
                (size, v) => Vector.Zero, StandardPhysics);
            yield return new Level("Heavy", Rocket, new Vector(600, 200),
                (size, v) => new Vector(0, 0.9), StandardPhysics);
            yield return new Level("Up", Rocket, Aim,
                (size, v) => new Vector(0, -300 / (900 - v.Y)), StandardPhysics);
            yield return new Level("WhiteHole", Rocket, Aim,
                (size, v) => WhiteGravity(size,v),StandardPhysics);
            yield return new Level("BlackHole", Rocket, Aim,
                (size, v) => BlackGravity(size,v), StandardPhysics);
            yield return new Level("BlackAndWhite", Rocket, Aim,
                (size, v) => MixedGravity(size,v),StandardPhysics);
        }
    }
}