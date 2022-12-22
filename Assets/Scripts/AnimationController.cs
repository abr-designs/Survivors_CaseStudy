﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimationController : MonoBehaviour
    {
        //============================================================================================================//
        
        [Serializable]
        public class AnimationStateData
        {
            public string name;
            
            public bool looping = true;
            public STATE state;
            [NonSerialized]
            public int Count;
            [NonReorderable]
            public Sprite[] sprites;
        }
        
        //============================================================================================================//

        [SerializeField, Min(0)]
        private int framesPerSecond;

        private float _frameTime;
        private float _frameTimer;
        
        
        [SerializeField][NonReorderable]
        private AnimationStateData[] animationStateDatas;

        private STATE _currentState;
        private int _currentStateIndex;

        private int _currentAnimationIndex;

        private Dictionary<STATE, int> _currentStateIndicies;

        private SpriteRenderer _spriteRenderer;

        //Unity Functions
        //============================================================================================================//

        private void Start()
        {
            _frameTime = 1f / framesPerSecond;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _currentStateIndicies = new Dictionary<STATE, int>();

            for (var i = 0; i < animationStateDatas.Length; i++)
            {
                var index = i;
                var state = animationStateDatas[i].state;

                animationStateDatas[i].Count = animationStateDatas[i].sprites.Length;

                if (_currentStateIndicies.ContainsKey(state))
                    throw new Exception();
                
                _currentStateIndicies.Add(state, index);
            }

            SetCurrentState(STATE.IDLE);
        }

        private void Update()
        {
            if (_currentState == STATE.NONE)
                return;

            if (_frameTimer < _frameTime)
            {
                _frameTimer += Time.deltaTime;
                return;
            }

            var animationData = animationStateDatas[_currentStateIndex];

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
        
        public void SetCurrentState(in STATE state)
        {
            if (_currentStateIndicies.ContainsKey(state) == false)
                throw new NotImplementedException();
            
            _currentState = state;
            _currentStateIndex = _currentStateIndicies[state];
            _currentAnimationIndex = 0;
        }
        
        //============================================================================================================//
    }
}