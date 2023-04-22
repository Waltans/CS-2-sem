using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            double angle;
            var distance = target - rocket.Location;

            if (Math.Abs(distance.Angle - rocket.Direction) > 0.6 &&
                Math.Abs(distance.Angle - rocket.Velocity.Angle) > 0.6)
                angle = distance.Angle - rocket.Direction;
            else
                angle = distance.Angle * 2 - rocket.Direction - rocket.Velocity.Angle;
            
            if (angle < 0)
                return Turn.Left;
            return angle > 0 ? Turn.Right : Turn.None;
        }
    }
}