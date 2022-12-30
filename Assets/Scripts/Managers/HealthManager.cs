using System;
using System.Collections.Generic;
using Survivors.Base;
using Survivors.Base.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Survivors.Managers
{
    public class HealthManager : ManagerBase, IEnable, ILateUpdate
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
                var ratio = _iHealth.CurrentHealth / _iHealth.MaxHealth;
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
        private readonly SpriteRenderer _healthBarPrefab;
        private readonly float _healthBarYOffset;
        
        private List<IHealth> _healthObjectsWithDisplay;
        private List<HealthBar> _trackedHealthBars;

        public HealthManager(in SpriteRenderer healthBarPrefab, in float healthBarYOffset)
        {
            _healthBarPrefab = healthBarPrefab;
            _healthBarYOffset = healthBarYOffset;
        }

        //============================================================================================================//
        public void OnEnable()
        {
            HealthBase.OnNewHealth += TryAddTrackedHealth;
            HealthBase.OnHealthRemoved += TryRemoveTrackedHealth;
        }

        public void LateUpdate()
        {
            for (var i = 0; i < _trackedHealthBars.Count; i++)
            {
                var currentPos = _trackedHealthBars[i]._iHealth.transform.position;
                currentPos.y += _healthBarYOffset;
                _trackedHealthBars[i].Transform.position = currentPos;
                
                _trackedHealthBars[i].UpdateDisplay();

            }
        }

        public void OnDisable()
        {
            HealthBase.OnNewHealth -= TryAddTrackedHealth;
            HealthBase.OnHealthRemoved -= TryRemoveTrackedHealth; 
        }

        //============================================================================================================//
        
        private void TryAddTrackedHealth(IHealth health)
        {
            if (_healthObjectsWithDisplay == null)
                _healthObjectsWithDisplay = new List<IHealth>();

            _healthObjectsWithDisplay.Add(health);

            if (health.ShowHealthBar == false)
                return;

            if (_trackedHealthBars == null)
                _trackedHealthBars = new List<HealthBar>();

            var temp = Object.Instantiate(_healthBarPrefab);
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