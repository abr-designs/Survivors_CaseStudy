using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Utilities
{
    public static class CollectionExtensions
    {
        //FIXME This can be made into a non-alloc version
        public static T[] GetRandomElements<T>(this IEnumerable<T> collection, in int count)
        {
            var outList = new T[count];
            var listCopy = new List<T>(collection);

            for (var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, listCopy.Count);
                outList[i] = listCopy[randomIndex];
                
                listCopy.RemoveAt(randomIndex);
            }

            return outList;
        }
        
        public static T GetRandomElement<T>(this IEnumerable<T> collection)
        {
            var array = collection.ToArray();
            var randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }
    }
}