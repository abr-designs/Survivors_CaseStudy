using System;
using System.Collections.Generic;
using Survivors.Animation;
using Survivors.Base.Interfaces;
using Survivors.Base.Managers.Interfaces;
using Survivors.ScriptableObjets.Animation;
using UnityEngine;

namespace Survivors.Base
{
    public class AnimationControllerBase : IAnimationController
    {
        //============================================================================================================//

        private float _frameTime;
        private float _frameTimer;

        private IReadOnlyList<AnimationStateData> _animationStateDatas;

        private STATE _currentState;
        private int _currentStateIndex;

        private int _currentAnimationIndex;

        private Dictionary<STATE, int> _currentStateIndicies;

        private SpriteRenderer _spriteRenderer;

        //Unity Functions
        //============================================================================================================//
        public AnimationControllerBase(in SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public void Update()
        {
            if (_currentState == STATE.NONE)
                return;

            if (_frameTimer < _frameTime)
            {
                _frameTimer += Time.deltaTime;
                return;
            }

            var animationData = _animationStateDatas[_currentStateIndex];

            if (_currentAnimationIndex >= animationData.Count)
            {
                if (animationData.looping)
                    _currentAnimationIndex = 0;
                else
                    return;
            }

            _spriteRenderer.sprite = animationData.sprites[_currentAnimationIndex++];
            _frameTimer = 0f;
        }

        //============================================================================================================//

        public void SetAnimationProfile(in AnimationProfileScriptableObject animationProfileScriptableObject)
        {
            _frameTime = 1f / animationProfileScriptableObject.framesPerSecond;
            _frameTimer = 0f;
            _currentState = STATE.NONE;
            
            _animationStateDatas = animationProfileScriptableObject.AnimationStateDatas;
            _currentStateIndicies = new Dictionary<STATE, int>(_animationStateDatas.Count);

            for (var i = 0; i < _animationStateDatas.Count; i++)
            {
                var index = i;
                var state = _animationStateDatas[i].state;

                _animationStateDatas[i].Count = _animationStateDatas[i].sprites.Length;

                if (_currentStateIndicies.ContainsKey(state))
                    throw new Exception();
                
                _currentStateIndicies.Add(state, index);
            }
        }
        
        public void SetCurrentState(in STATE state)
        {
            if (_currentStateIndicies == null)
                return;
            if (state == STATE.NONE)
                return;
            
            if (_currentStateIndicies.ContainsKey(state) == false)
                throw new NotImplementedException($"{_spriteRenderer.gameObject.name} does not contain state: {state}");
            
            _currentState = state;
            _currentStateIndex = _currentStateIndicies[state];
            _currentAnimationIndex = 0;
        }
        
        //============================================================================================================//
    }
}