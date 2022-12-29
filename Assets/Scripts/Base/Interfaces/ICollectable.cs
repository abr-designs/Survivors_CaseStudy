using System;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Base.Interfaces
{
    public interface ICollectable
    {
        const float RADIUS = 0.1f;

        static Action<ICollectable> OnAddItem;
        static Action<ICollectable> OnRemoveItem;
        
        Transform transform { get; }
        CollectableProfileScriptableObject Profile { get; }
    }
}