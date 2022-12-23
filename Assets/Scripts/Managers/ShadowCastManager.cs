using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Managers
{
    //TODO Make this a non-monobehaviour
    [DefaultExecutionOrder(-1000)]
    public class ShadowCastManager : MonoBehaviour
    {
        //============================================================================================================//
        private struct ShadowData
        {
            public bool Tracking;
            public Transform ShadowTransform;
            public Transform TargetTransform;
            public float Offset;

            public void UpdatePosition()
            {
                var targetPosition = TargetTransform.position;
                targetPosition.y += Offset;
                ShadowTransform.position = targetPosition;
            }
            
            public void SetActive(in bool state)
            {
                ShadowTransform.gameObject.SetActive(state);
            }
        }
        //============================================================================================================//

        private static ShadowCastManager _instance;

        [SerializeField] private SpriteRenderer shadowPrefab;

        private List<ShadowData> _shadows;

        //============================================================================================================//

        private void Awake()
        {
            _instance = this;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            for (var i = 0; i < _shadows.Count; i++)
            {
                var shadowData = _shadows[i];
                if (shadowData.Tracking == false)
                    continue;

                shadowData.UpdatePosition();
            }
        }
        //============================================================================================================//

        public static void AddShadow(in Transform transform, in float offset)
        {
            if (_instance._shadows == null)
                _instance._shadows = new List<ShadowData>();
            
            _instance._shadows.Add(new ShadowData
            {
                Tracking = true,
                TargetTransform = transform,
                ShadowTransform = Instantiate(_instance.shadowPrefab, _instance.transform).transform,
                Offset = offset
            });
        }

        public static void RemoveShadow(in Transform transform)
        {
            throw new NotImplementedException();
        }

        //============================================================================================================//
    }
}
