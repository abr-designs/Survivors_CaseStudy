using System;
using System.Collections.Generic;
using Survivors.Base.Interfaces;
using UnityEngine;

namespace Survivors.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class HealthManager : MonoBehaviour
    {
        private class HealthBar
        {
            public readonly IHealth _iHealth;
            public readonly Transform Transform;
            private readonly SpriteRenderer _healthRenderer;
            private readonly Transform _healthRendererTransform;
            private readonly float _startSize;

            public HealthBar(in IHealth health, in SpriteRenderer parentRenderer)
            {
                _iHealth = health;

                Transform = parentRenderer.transform;
                _healthRenderer = Transform.GetChild(0).GetComponent<SpriteRenderer>();
                _healthRendererTransform = _healthRenderer.transform;
                _startSize = parentRenderer.size.x;
                SetSize(_startSize);
            }

            //============================================================================================================//

            // Update is called once per frame
            public void UpdateDisplay()
            {
                var ratio = _iHealth.CurrentHealth / _iHealth.StartingHealth;
                SetSize(ratio * _startSize);
            }

            //============================================================================================================//
        
            private void SetSize(in float size)
            {
                var healthBarSize = _healthRenderer.size;
                healthBarSize.x = size;

                _healthRenderer.size = healthBarSize;

                _healthRendererTransform.localPosition = Vector3.right * (_startSize - size) / -2f;
            }
        }
        
        //============================================================================================================//
        [SerializeField]
        private SpriteRenderer healthBarPrefab;

        [SerializeField]
        private float healthBarYOffset;
        
        private static HealthManager _instance;
        private List<IHealth> _healthObjectsWithDisplay;
        private List<HealthBar> _trackedHealthBars;

        //============================================================================================================//
        private void Awake()
        {
            _instance = this;
        }

        private void LateUpdate()
        {
            for (var i = 0; i < _trackedHealthBars.Count; i++)
            {
                var currentPos = _trackedHealthBars[i]._iHealth.transform.position;
                currentPos.y += healthBarYOffset;
                _trackedHealthBars[i].Transform.position = currentPos;
                
                _trackedHealthBars[i].UpdateDisplay();

            }
        }
        
        //============================================================================================================//

        public static void AddTrackedHealth(in IHealth health)
        {
            _instance.TryAddTrackedHealth(health);
        }
        
        public static void RemoveTrackedHealth(in IHealth health)
        {
            _instance.TryRemoveTrackedHealth(health);
        }
        
        //============================================================================================================//
        
        private void TryAddTrackedHealth(in IHealth health)
        {
            if (_healthObjectsWithDisplay == null)
                _healthObjectsWithDisplay = new List<IHealth>();

            _healthObjectsWithDisplay.Add(health);

            if (health.ShowHealthBar == false)
                return;

            if (_trackedHealthBars == null)
                _trackedHealthBars = new List<HealthBar>();

            var temp = Instantiate(healthBarPrefab);
            _trackedHealthBars.Add(new HealthBar(health, temp));
        }
        
        private void TryRemoveTrackedHealth(IHealth health)
        {
            _healthObjectsWithDisplay?.Remove(health);
            
            if (health.ShowHealthBar == false)
                return;

            if (_trackedHealthBars == null)
                return;

            var index = _trackedHealthBars
                .FindIndex(x => x._iHealth == health);
            
            if (index < 0)
                throw new Exception();
            
            _trackedHealthBars.RemoveAt(index);
        }
        
        //============================================================================================================//
    }
}