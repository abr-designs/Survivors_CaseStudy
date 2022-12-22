using System;
using UnityEngine;

namespace Survivors.Animation
{
    [Serializable]
    public class AnimationStateData
    {
        public string name;
            
        public bool looping = true;
        public STATE state;
        [NonSerialized]
        public int Count;
        [NonReorderable]
        public Sprite[] sprites;
    }
}