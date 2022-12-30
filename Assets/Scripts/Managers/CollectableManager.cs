using System;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Player;
using Survivors.ScriptableObjets;
using Survivors.Utilities;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Survivors.Managers
{
    public class CollectableManager : ManagerBase, IEnable, IUpdate
    {
        private struct CollectableData
        {
            public bool IsBeingPickedUp;
            public float CurrentSpeed;
            public ICollectable Collectable;
        }

        //============================================================================================================//
        
        private readonly float pickupThreshold;
        private readonly float pickupRange;
        private readonly float pickupSpeed;
        private readonly float initialPickupPush;
        
        private readonly PlayerHealth _playerHealth;
        private readonly Transform _playerTransform;
        
        private List<CollectableData> _itemsToPickup;

        //============================================================================================================//
        
        public CollectableManager(float pickupThreshold,
            float pickupRange,
            float pickupSpeed,
            float initialPickupPush)
        {
            _playerHealth = Object.FindObjectOfType<PlayerHealth>();
            _playerTransform = _playerHealth.transform;
            
            //------------------------------------------------//
            
            this.pickupThreshold = pickupThreshold;
            this.pickupRange = pickupRange;
            this.pickupSpeed = pickupSpeed;
            this.initialPickupPush = initialPickupPush;
        }

        //Unity Functions
        //============================================================================================================//
        
        public void OnEnable()
        {
            ICollectable.OnAddItem += OnAddItem;
            ICollectable.OnRemoveItem += OnRemoveItem;
        }

        // Update is called once per frame
        public void Update()
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

        

        public void OnDisable()
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
            Object.Destroy(collectable.transform.gameObject);
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
        //============================================================================================================//
    }
}
