using System;
using Survivors.Base;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Factories;
using Survivors.Player;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Managers
{
    public class PlayerManager : ManagerBase, IUpdate, IEnable
    {
        public static event Action<Transform, PlayerHealth, SpriteRenderer> OnPlayerCreated; 

        private PlayerHealth playerHealth;
        private PlayerMovementController playerMovementController;
        private PlayerStateControllerBase playerStateController;
        private AnimationControllerBase playerAnimationController;

        private Transform _playerTransform;
        private SpriteRenderer _spriteRenderer;
        private PlayerProfileScriptableObject _playerProfile;
        
        public PlayerManager(in MonoBehaviour coroutineCaller, in PlayerProfileScriptableObject selectedPlayer)
        {
            var (spriteRenderer, profile) = FactoryManager
                .GetFactory<PlayerFactory>()
                .CreatePlayer(selectedPlayer.name, Vector2.zero);

            _spriteRenderer = spriteRenderer;
            _playerProfile = profile;
            
            _playerTransform = _spriteRenderer.transform;
            playerHealth = new PlayerHealth(coroutineCaller, _spriteRenderer);
            
            OnPlayerCreated?.Invoke(_playerTransform, playerHealth, _spriteRenderer);
        }
        public void Update()
        {
            playerHealth.Update();
            playerMovementController.Update();
            playerStateController.Update();
            playerAnimationController.Update();
        }

        public void OnEnable()
        {
            playerMovementController = new PlayerMovementController(_playerTransform);
            playerAnimationController = new AnimationControllerBase(_spriteRenderer);
            playerStateController = new PlayerStateControllerBase(
                playerHealth,
                _playerProfile,
                playerMovementController,
                playerAnimationController,
                _spriteRenderer,
                STATE.IDLE);
        }

        public void OnDisable()
        {

        }
    }
}