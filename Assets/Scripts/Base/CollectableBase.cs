using Survivors.Base.Interfaces;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Base
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CollectableBase : MonoBehaviour, ICollectable
    {
        public CollectableProfileScriptableObject Profile => collectableProfile;
        [SerializeField]
        private CollectableProfileScriptableObject collectableProfile;

        public int Value => value;
        private int value;

        private SpriteRenderer _spriteRenderer;

        private void OnDisable()
        {
            ICollectable.OnRemoveItem?.Invoke(this);
        }
        //============================================================================================================//

        public void Init(in CollectableProfileScriptableObject collectableProfile, int value)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            this.collectableProfile = collectableProfile;

            this.value = value == 0 ? collectableProfile.value : value;

            _spriteRenderer.sprite = collectableProfile.sprite;
            ICollectable.OnAddItem?.Invoke(this);
        }

        //============================================================================================================//
        
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ICollectable.RADIUS);
        }

#endif
        //============================================================================================================//
        
    }
}
