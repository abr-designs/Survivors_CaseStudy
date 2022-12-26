using System;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using Survivors.Player;
using Survivors.Utilities;
using UnityEngine;

namespace Survivors.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class ItemManager : MonoBehaviour
    {
        private struct ItemPickupData
        {
            public bool IsBeingPickedUp;
            public float CurrentSpeed;
            public IItem Item;
        }

        //============================================================================================================//
        
        [SerializeField, Min(0f), Header("Pickup Info")]
        private float pickupThreshold;
        [SerializeField, Min(0f)]
        private float pickupRange;
        [SerializeField, Min(0f)]
        private float pickupSpeed;
        
        private List<ItemPickupData> _itemsToPickup;
        private Transform _playerTransform;

        //Unity Functions
        //============================================================================================================//
        
        private void OnEnable()
        {
            IItem.OnAddItem += OnAddItem;
            IItem.OnRemoveItem += OnRemoveItem;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _playerTransform = FindObjectOfType<PlayerHealth>().transform;
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
                var itemPosition = (Vector2)itemData.Item.transform.position;
                
                if (itemData.IsBeingPickedUp)
                {
                    if (Vector2.Distance(itemPosition, playerPosition) <= pickupThreshold)
                    {
                        ClaimItem(itemData.Item);
                        continue;
                    }
                    //Move Towards Player
                    //------------------------------------------------//
                    itemPosition = Vector2.MoveTowards(itemPosition, playerPosition, itemData.CurrentSpeed * deltaTime);

                    itemData.CurrentSpeed += pickupSpeed;
                    itemData.Item.transform.position = itemPosition;

                    _itemsToPickup[i] = itemData; 
                    continue;
                }
                
                //Check should start pickup
                //------------------------------------------------//
                if (SMath.CirclesIntersect(IItem.RADIUS, pickupRange, playerPosition, itemPosition) == false)
                    continue;

                itemData.IsBeingPickedUp = true;
                _itemsToPickup[i] = itemData;
            }
        }

        

        private void OnDisable()
        {
            IItem.OnAddItem -= OnAddItem;
            IItem.OnRemoveItem -= OnRemoveItem;
        }
        //============================================================================================================//

        private void ClaimItem(IItem item)
        {
            //TODO Claim Item shit
            Destroy(item.transform.gameObject);
        }

        //IItem Callbacks
        //============================================================================================================//
        private void OnAddItem(IItem item)
        {
            if (_itemsToPickup == null)
                _itemsToPickup = new List<ItemPickupData>();
            
            _itemsToPickup.Add(new ItemPickupData
            {
                Item = item
            });
        }
        private void OnRemoveItem(IItem item)
        {
            if (_itemsToPickup == null)
                return;

            var index = _itemsToPickup.FindIndex(x => x.Item == item);

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
