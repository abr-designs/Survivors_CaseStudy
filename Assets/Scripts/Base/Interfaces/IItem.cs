using System;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Base.Interfaces
{
    public interface IItem
    {
        static Action<IItem> OnAddItem;
        static Action<IItem> OnRemoveItem;
        
        const float RADIUS = 0.1f;
        
        Transform transform { get; }
        ItemProfileScriptableObject Profile { get; }
    }
}