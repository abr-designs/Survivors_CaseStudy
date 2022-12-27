using Survivors.Attacks;
using Survivors.Player;
using UnityEngine;

namespace Survivors.Managers
{
    public class AttackManager : MonoBehaviour
    {
        //TODO Get all AttackBases on this Object
        //TODO Update all active attacks
        private static Transform _playerTransform;
        

        private AttackBase[] _attacks;

        //============================================================================================================//
        private void OnEnable()
        {
            _playerTransform = FindObjectOfType<PlayerHealth>().transform;
            _attacks = GetComponentsInChildren<AttackBase>();

            foreach (var attackBase in _attacks)
            {
                attackBase.Setup();
                attackBase.SetActive(false);
            }
        }

        private void Update()
        {
            var playerPosition = (Vector2)_playerTransform.position;
            var deltaTime = Time.deltaTime;
            
            for (var i = 0; i < _attacks.Length; i++)
            {
                var attackBase = _attacks[i];
                
                if(attackBase.isActive == false)
                    continue;

                attackBase.ManualUpdate(playerPosition, deltaTime);
                attackBase.PostUpdate();
            }
        }
        //============================================================================================================//
    }
}