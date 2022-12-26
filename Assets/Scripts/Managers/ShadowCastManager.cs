using System;
using System.Collections.Generic;
using Survivors.Base;
using Survivors.Base.Interfaces;
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
            public readonly IShadow Shadow;
            public Transform ShadowTransform;

            public ShadowData(IShadow shadow)
            {
                Shadow = shadow;
                ShadowTransform = null;
            }

            public void UpdatePosition()
            {
                var targetPosition = Shadow.transform.position;
                targetPosition.y += Shadow.ShadowOffset;
                ShadowTransform.position = targetPosition;
            }
            
            public void SetActive(in bool state)
            {
                ShadowTransform.gameObject.SetActive(state);
            }
        }
        //============================================================================================================//

        [SerializeField] private SpriteRenderer shadowPrefab;

        private List<ShadowData> _shadows;

        //============================================================================================================//

        private void OnEnable()
        {
            IShadow.OnAddShadow += AddShadow;
            IShadow.OnRemoveShadow += RemoveShadow;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            for (var i = 0; i < _shadows.Count; i++)
            {
                var shadowData = _shadows[i];

                shadowData.UpdatePosition();
            }
        }

        private void OnDisable()
        {
            IShadow.OnAddShadow -= AddShadow;
            IShadow.OnRemoveShadow -= RemoveShadow;
        }
        //============================================================================================================//

        private void AddShadow(IShadow shadow)
        {
            if (_shadows == null)
                _shadows = new List<ShadowData>();
            
            _shadows.Add(new ShadowData(shadow)
            {
                ShadowTransform = Instantiate(shadowPrefab, transform).transform,
            });
        }

        private void RemoveShadow(IShadow shadow)
        {
            if (_shadows == null)
                return;

            var index = _shadows.FindIndex(x => x.Shadow == shadow);

            if (index < 0)
                throw new Exception();
            
            //FIXME Add recycling
            Destroy(_shadows[index].ShadowTransform.gameObject);
            _shadows.RemoveAt(index);
        }

        //============================================================================================================//
    }
}
