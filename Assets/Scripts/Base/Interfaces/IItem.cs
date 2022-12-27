using System;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Base.Interfaces
{
    public interface IItem
    {
        const float RADIUS = 0.1f;

        static Action<IItem> OnAddItem;
        static Action<IItem> OnRemoveItem;
        
        Transform transform { get; }
        ItemProfileScriptableObject Profile { get; }
    }
}