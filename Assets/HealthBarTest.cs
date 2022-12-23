using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Debugging
{
    public class HealthBarTest : MonoBehaviour
    {
        [SerializeField, Min(1f)]
        private float startHealth;
        [SerializeField, Min(0f)]
        private float currentHealth;
        [SerializeField]
        private SpriteRenderer backgroundRenderer;

        private float _startSize;

        [SerializeField]
        private SpriteRenderer healthRenderer;
        private Transform _healthTransform;

        //============================================================================================================//
        // Start is called before the first frame update
        private void Start()
        {
            currentHealth = startHealth;
            _startSize = backgroundRenderer.size.x;


            _healthTransform = healthRenderer.transform;
            SetSize(_startSize);
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            var ratio = currentHealth / startHealth;
            SetSize(ratio * _startSize);
        }

        //============================================================================================================//
        
        private void SetSize(in float size)
        {
            var healthBarSize = healthRenderer.size;
            healthBarSize.x = size;

            healthRenderer.size = healthBarSize;

            _healthTransform.localPosition = Vector3.right * (_startSize - size) / -2f;
        }
    }
}
