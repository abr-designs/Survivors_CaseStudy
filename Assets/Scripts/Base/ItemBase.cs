using System;
using Survivors.Base.Interfaces;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Base
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemBase : MonoBehaviour, IItem
    {
        public ItemProfileScriptableObject Profile => itemProfile;
        [SerializeField]
        private ItemProfileScriptableObject itemProfile;

        private SpriteRenderer _spriteRenderer;

        private void OnDisable()
        {
            IItem.OnRemoveItem?.Invoke(this);
        }
        //============================================================================================================//

        public void Init(in ItemProfileScriptableObject itemProfile)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            this.itemProfile = itemProfile;

            _spriteRenderer.sprite = itemProfile.sprite;
            IItem.OnAddItem?.Invoke(this);
        }

        //============================================================================================================//
        
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, IItem.RADIUS);
        }

#endif
        //============================================================================================================//
        
    }
}
