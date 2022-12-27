using UnityEngine;

namespace Survivors.Utilities
{
    public static class SMath
    {
        public static bool CirclesIntersect(in float radius0, in float radius1, in Vector2 pos0, in Vector2 pos1)
        {
            //(R0 - R1)^2 <= (x0 - x1)^2 + (y0 - y1)^2 <= (R0 + R1)^2

            /*var rMinus = radius0 - radius1;
            var rPlus = radius0 + radius1;

            var xMinus = pos0.x - pos1.x;
            var yMinus = pos0.y - pos1.y;

            var a = rMinus * rMinus;
            var b = (xMinus * xMinus) + (yMinus * yMinus);
            var c = rPlus * rPlus;

            return a <= b && b <= c;*/

            var distance = Vector2.Distance(pos0, pos1);
            var rPlus = radius0 + radius1;
            
            if (distance <= rPlus)
                return true;
            
            var rMinus = radius0 - radius1;
            return distance - rMinus <= 0;
        }
    }
}