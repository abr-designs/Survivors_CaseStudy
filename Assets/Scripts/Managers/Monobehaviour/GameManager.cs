using System;
using System.Linq;
using Cinemachine;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Factories;
using Survivors.ScriptableObjets;
using Survivors.ScriptableObjets.Weapons.Items;
using Survivors.ScriptableObjets.Items;
using TMPro;
using UnityEngine;

namespace Survivors.Managers.MonoBehaviours
{
    [DefaultExecutionOrder(-1000)]
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Header("Player")]
        private PlayerProfileScriptableObject selectedPlayer;
        
        [SerializeField, Header("Item Manager")]
        private WeaponProfileScriptableObject[] weaponProfiles;
        
        [SerializeField]
        private PassiveItemProfileScriptableObject[] passiveProfiles;

        //============================================================================================================//
        
        [SerializeField, Min(0f), Header("Collectable Manager")]
        private float pickupThreshold;
        [SerializeField, Min(0f)]
        private float pickupRange;
        [SerializeField, Min(0f)]
        private float pickupSpeed;
        [SerializeField, Min(0f)]
        private float initialPickupPush;

        //============================================================================================================//
        
        [SerializeField, Header("Damage Text Manager")]
        private TMP_Text textPrefab;

        [SerializeField] private float yOffset;
        [SerializeField, Min(0f)]
        private float fadeTime;
        [SerializeField, Header("Damage Text Manager - Scale")]
        private AnimationCurve scaleCurve;
        [SerializeField, Min(0f)]
        private float scaleMultiplier;
        [SerializeField, Header("Damage Text Manager - Color")]
        private AnimationCurve colorCurve;
        [SerializeField]
        private Color startColor = Color.white;
        [SerializeField]
        private Color endColor = Color.white;
        
        [SerializeField, Header("Health Manager")]
        private SpriteRenderer healthBarPrefab;

        [SerializeField]
        private float healthBarYOffset;
        
        [SerializeField, Header("Shadow cast Manager")]
        private SpriteRenderer shadowPrefab;
        
        private ManagerBase[] _managers;
        private IEnable[] _enables;
        private IUpdate[] _updates;
        private ILateUpdate[] _lateUpdates;

        //Unity Functions
        //============================================================================================================//
        
        // Start is called before the first frame update
        private void Awake()
        {

            CreateManagers();
        }

        private void CreateManagers()
        {
            _managers = new ManagerBase[]
            {
                new InputDelegator(),
                new EnemyManager(),
                new ItemManager(this, weaponProfiles, passiveProfiles),
                new CollectableManager(pickupThreshold, pickupRange, pickupSpeed, initialPickupPush),
                new DamageTextManager(textPrefab, yOffset, fadeTime, scaleCurve, scaleMultiplier, colorCurve, startColor, endColor),
                new HealthManager(healthBarPrefab, healthBarYOffset),
                new ShadowCastManager(transform, shadowPrefab),
                new XpManager()
            };

            _enables = _managers
                .OfType<IEnable>()
                .ToArray();
            
            _updates = _managers
                .OfType<IUpdate>()
                .ToArray();
            
            _lateUpdates = _managers
                .OfType<ILateUpdate>()
                .ToArray();
        }
        
        //============================================================================================================//

        private void OnEnable()
        {
            for (var i = 0; i < _enables.Length; i++)
            {
                _enables[i].OnEnable();
            }
        }

        private void Start()
        {
            var playerStateControllerBase = FactoryManager
                .GetFactory<PlayerFactory>()
                .CreatePlayer(selectedPlayer.name, Vector2.zero);

            var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.Follow = playerStateControllerBase.transform;
        }

        private void Update()
        {
            for (var i = 0; i < _updates.Length; i++)
            {
                _updates[i].Update();
            }
        }

        private void LateUpdate()
        {
            for (var i = 0; i < _lateUpdates.Length; i++)
            {
                _lateUpdates[i].LateUpdate();
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < _enables.Length; i++)
            {
                _enables[i].OnDisable();
            }
            
        }

        //============================================================================================================//
    }
}
