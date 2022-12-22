using System.Collections.Generic;
using System.Linq;
using Survivors.Animation;
using UnityEngine;

namespace Survivors.ScriptableObjets.Animation
{
    [CreateAssetMenu(fileName = "Animation Profile", menuName = "ScriptableObjects/Animation Profile")]
    public class AnimationProfileScriptableObject : ScriptableObject
    {
        [Min(0)]
        public int framesPerSecond;
        public IReadOnlyList<AnimationStateData> AnimationStateDatas => animationStateDatas.ToList();
        
        [SerializeField][NonReorderable]
        private AnimationStateData[] animationStateDatas;
    }
}
