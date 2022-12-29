using System;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using Survivors.Player;
using Survivors.ScriptableObjets;
using Survivors.Utilities;
using UnityEngine;

namespace Survivors.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class CollectableManager : MonoBehaviour
    {
        private struct CollectableData
        {
            public bool IsBeingPickedUp;
            public float CurrentSpeed;
            public ICollectable Collectable;
        }

        //============================================================================================================//
        
        [SerializeField, Min(0f), Header("Pickup Info")]
        private float pickupThreshold;
        [SerializeField, Min(0f)]
        private float pickupRange;
        [SerializeField, Min(0f)]
        private float pickupSpeed;
        [SerializeField, Min(0f)]
        private float initialPickupPush;
        
        private List<CollectableData> _itemsToPickup;
        private PlayerHealth _playerHealth;
        private Transform _playerTransform;

        //Unity Functions
        //============================================================================================================//
        
        private void OnEnable()
        {
            ICollectable.OnAddItem += OnAddItem;
            ICollectable.OnRemoveItem += OnRemoveItem;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _playerTransform = _playerHealth.transform;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_itemsToPickup == null)
                return;
            
            var playerPosition = (Vector2)_playerTransform.position;
            var deltaTime = Time.deltaTime;
            
            for (var i = _itemsToPickup.Count - 1; i >= 0; i--)
            {
                var itemData = _itemsToPickup[i];
                var itemPosition = (Vector2)itemData.Collectable.transform.position;
                
                if (itemData.IsBeingPickedUp)
                {
                    if (Vector2.Distance(itemPosition, playerPosition) <= pickupThreshold)
                    {
                        ClaimCollectable(itemData.Collectable);
                        continue;
                    }
                    //Move Towards Player
                    //------------------------------------------------//
                    itemPosition = Vector2.MoveTowards(itemPosition, playerPosition, itemData.CurrentSpeed * deltaTime);

                    itemData.CurrentSpeed += pickupSpeed;
                    itemData.Collectable.transform.position = itemPosition;

                    _itemsToPickup[i] = itemData; 
                    continue;
                }
                
                //Check should start pickup
                //------------------------------------------------//
                if (SMath.CirclesIntersect(ICollectable.RADIUS, pickupRange, playerPosition, itemPosition) == false)
                    continue;

                itemData.IsBeingPickedUp = true;
                itemData.CurrentSpeed = -initialPickupPush;
                _itemsToPickup[i] = itemData;
            }
        }

        

        private void OnDisable()
        {
            ICollectable.OnAddItem -= OnAddItem;
            ICollectable.OnRemoveItem -= OnRemoveItem;
        }
        //============================================================================================================//

        private void ClaimCollectable(ICollectable collectable)
        {
            //TODO Claim Item shit
            switch (collectable.Profile.type)
            {
                case PICKUP.NONE:
                    break;
                case PICKUP.EXP:
                    XpManager.AddXp(collectable.Profile.value);
                    break;
                case PICKUP.HEAL:
                    _playerHealth.ChangeHealth(collectable.Profile.value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroy(collectable.transform.gameObject);
        }

        //IItem Callbacks
        //============================================================================================================//
        private void OnAddItem(ICollectable collectable)
        {
            if (_itemsToPickup == null)
                _itemsToPickup = new List<CollectableData>();
            
            _itemsToPickup.Add(new CollectableData
            {
                Collectable = collectable
            });
        }
        private void OnRemoveItem(ICollectable collectable)
        {
            if (_itemsToPickup == null)
                return;

            var index = _itemsToPickup.FindIndex(x => x.Collectable == collectable);

            if (index < 0)
                throw new Exception();
            
            //FIXME Add recycling
            //Destroy(_itemsToPickup[index].Item.transform.gameObject);
            _itemsToPickup.RemoveAt(index);
        }
        
        //Unity Editor Functions
        //============================================================================================================//

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying == false)
                return;

            var playerPosition = _playerTransform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerPosition, pickupThreshold);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerPosition, pickupRange);
        }
#endif
        //============================================================================================================//
    }
}
