using System;
using UnityEngine;

namespace Survivors.Base.Interfaces
{
    public interface IShadow
    {
        static Action<IShadow> OnAddShadow;
        static Action<IShadow> OnRemoveShadow;
        
        Transform transform { get; }
        float ShadowOffset { get; }
    }
}