using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Survivors.Utilities
{
    public static class CollisionSolver
    {
        private readonly struct CollisionCheckData
        {
            public readonly int index;
            public readonly float size;
            public readonly float xPos;
            public readonly float yPos;

            public CollisionCheckData(int index, float size, float xPos, float yPos)
            {
                this.index = index;
                this.size = size;
                this.xPos = xPos;
                this.yPos = yPos;
            }
            public CollisionCheckData(in int index, in float size, in Transform transform)
            {
                this.index = index;
                this.size = size;
                
                var pos = transform.position;
                xPos = pos.x;
                yPos = pos.y;
            }

        }

        private class CollisionSolverDelta
        {
            public int index;
            public Vector2 delta;
        }
        
        public static void SolveCollisions(in float radius, in List<Transform> toSolve)
        {
            if (toSolve == null || toSolve.Count == 0)
                return;
            
            var enemyCount = toSolve.Count;

            //Convert Transforms into usable Collision Data
            //------------------------------------------------//
            var collisionChecks = new CollisionCheckData[enemyCount];
            for (var i = 0; i < enemyCount; i++)
            {
                var index = i;
                collisionChecks[i] = new CollisionCheckData(index, radius, toSolve[i]);
            }

            //Do all Calculations Async
            //------------------------------------------------//
            var task = GetAllData(enemyCount, collisionChecks);
            task.Wait();

            var results = task.Result;

            //Traverse all Results
            //------------------------------------------------//

            var deltaTime = Time.deltaTime;
            var collisionPairs = new HashSet<(int, int)>();
            var collisionSolvers = new Dictionary<int, CollisionSolverDelta>();

            for (int i = 0; i < results.Length; i++)
            {
                var (index, collisionIndices) = results[i];
                if(collisionIndices == null)
                    continue;
                //Debug.Log($"[{index}] colliding with: [{string.Join(", ",collisionIndices)}]");

                for (int ii = 0; ii < collisionIndices.Count; ii++)
                {
                    var otherIndex = collisionIndices[ii];

                    var collisionPair = (index < otherIndex ? index : otherIndex,
                        otherIndex > index ? otherIndex : index);
                    
                    if(collisionPairs.Contains(collisionPair))
                        continue;

                    var dif = new Vector2(collisionChecks[index].xPos, collisionChecks[index].yPos) -
                              new Vector2(collisionChecks[otherIndex].xPos, collisionChecks[otherIndex].yPos);
                    var dir = dif.normalized * deltaTime;

                    //Debug.Log($"[{index}] Solved colliding with: [{otherIndex}]");
                    
                    collisionPairs.Add(collisionPair);
                    TryAddSolverDelta(index, dir, ref collisionSolvers);
                    TryAddSolverDelta(otherIndex, -dir, ref collisionSolvers);
                }
            }

            //Apply any repels
            //------------------------------------------------//
            var values = collisionSolvers.Values;
            foreach (var value in values)
            {
                var transform= toSolve[value.index];
                transform.Translate(value.delta);
            }

        }
        
        private static void TryAddSolverDelta(in int index, in Vector2 dir,
            ref Dictionary<int, CollisionSolverDelta> dictionary)
        {
            if (dictionary.TryGetValue(index, out var collisionSolverDelta))
            {
                collisionSolverDelta.delta += dir;
                return;
            }

            collisionSolverDelta = new CollisionSolverDelta
            {
                index = index,
                delta = dir
            };
            
            dictionary.Add(index, collisionSolverDelta);
        }

        //Async Tasks
        //============================================================================================================//
        
        private static async Task<(int, List<int>)[]> GetAllData(int count, IReadOnlyList<CollisionCheckData> collisionChecks)
        {
            var tasks = new Task<(int, List<int>)>[count];

            for (var i = 0; i < count; i++)
            {
                tasks[i] = CheckForCollisions(collisionChecks[i], collisionChecks);
            }

            var results = await Task.WhenAll(tasks);

            return results;
        }

        private static async Task<(int, List<int>)> CheckForCollisions(CollisionCheckData toCheck, IReadOnlyList<CollisionCheckData> others)
        {
            var listLength = others.Count;
            var outList = new List<int>(listLength - 1);
            var count = 0;
            for (int i = 0; i < listLength; i++)
            {
                var other = others[i];
                if(toCheck.index == other.index)
                    continue;

                var sizeSqr = other.size * other.size;
                
                if (SMath.FastSquareMagnitude(toCheck.xPos, toCheck.yPos, other.xPos, other.yPos) > sizeSqr)
                    continue;
                
                if (SMath.CirlceIsCollidingFast(toCheck.xPos, toCheck.yPos, toCheck.size,
                        other.xPos, other.yPos, other.size) == false)
                    continue;

                
                outList.Add(other.index);
                count++;
            }

            return count == 0 ? 
                (toCheck.index, default) : 
                (toCheck.index, outList);
        }
        
        //============================================================================================================//
    }
}