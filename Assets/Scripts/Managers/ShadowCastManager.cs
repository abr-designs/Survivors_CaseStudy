using System;
using System.Collections.Generic;
using Survivors.Base;
using Survivors.Base.Interfaces;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Survivors.Managers
{

    public class ShadowCastManager : ManagerBase, IEnable, ILateUpdate
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

        private readonly SpriteRenderer _shadowPrefab;
        private readonly Transform _parentTransform;

        private List<ShadowData> _shadows;

        //============================================================================================================//

        public ShadowCastManager(Transform parentTransform, SpriteRenderer shadowPrefab)
        {
            _parentTransform = parentTransform;
            _shadowPrefab = shadowPrefab;
        }
        
        //============================================================================================================//

        public void OnEnable()
        {
            IShadow.OnAddShadow += AddShadow;
            IShadow.OnRemoveShadow += RemoveShadow;
        }

        // Update is called once per frame
        public void LateUpdate()
        {
            for (var i = 0; i < _shadows.Count; i++)
            {
                var shadowData = _shadows[i];

                shadowData.UpdatePosition();
            }
        }

        public void OnDisable()
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
                ShadowTransform = Object.Instantiate(_shadowPrefab, _parentTransform).transform,
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
            Object.Destroy(_shadows[index].ShadowTransform.gameObject);
            _shadows.RemoveAt(index);
        }

        //============================================================================================================//
    }
}
