using UnityEngine;

namespace Survivors.Utilities
{
    public static class SMath
    {
        //Based on: https://kyndinfo.notion.site/Detecting-Collision-7c8c74926f824b2bb310b08988a384a7
        public static bool CirclesIntersect(in float radius0, in float radius1, in Vector2 pos0, in Vector2 pos1)
        {
            var distance = Vector2.Distance(pos0, pos1);
            var rPlus = radius0 + radius1;
            
            if (distance <= rPlus)
                return true;
            
            var rMinus = radius0 - radius1;
            return distance - rMinus <= 0;
        }
        
        //Based on: http://www.jeffreythompson.org/collision-detection/circle-rect.php
        public static bool CircleOverlapsRect(in Vector2 circlePosition, in float radius, in Bounds bounds)
        {
            var rectMin = (Vector2)bounds.min;
            var rectMax = (Vector2)bounds.max;

            // temporary variables to set edges for testing
            float testX = circlePosition.x;
            float testY = circlePosition.y;

            // which edge is closest?
            if (circlePosition.x < rectMin.x) testX = rectMin.x; // test left edge
            else if (circlePosition.x > rectMax.x) testX = rectMax.x; // right edge
            if (circlePosition.y < rectMin.y) testY = rectMin.y; // top edge
            else if (circlePosition.y > rectMax.y) testY = rectMax.y; // bottom edge

            // get distance from closest edges
            float distX = circlePosition.x - testX;
            float distY = circlePosition.y - testY;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            var hasCollision = distance <= radius;

            return hasCollision;
        }
        
        public static bool CirlceIsCollidingFast(in float px1, in float py1, in float r1, in float px2, in float py2, in float r2)
        {
            var a = r1 + r2;
            var dx = px1 - px2;
            var dy = py1 - py2;
            return a * a > (dx * dx + dy * dy);
        }

        //https://stackoverflow.com/a/2556688
        public static float FastSquareMagnitude(in float px1, in float py1, in float px2, in float py2)
        {
            var x = (px1 - px2);
            var y = (py1 - py2);
            return x * x + y * y;
        }
    }
}