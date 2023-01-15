﻿using Survivors.Base.Interfaces;
using Survivors.Base.Managers.Interfaces;
using UnityEngine;

namespace Survivors.Base
{
    public abstract class MovementControllerBase : IMovementController, IUpdate
    {
        private Transform _transform;

        public virtual float Speed => speed;
        [SerializeField, Min(0f)]
        protected float speed;

        public bool IsMoving => _isMoving;
        public int MoveDirection => (int)XDirection;


        private bool _isMoving;
        protected float XDirection, YDirection;

        //============================================================================================================//

        public MovementControllerBase(in Transform transform)
        {
            _transform = transform;
        }

        // Update is called once per frame
        public void Update()
        {
            if (_isMoving == false)
                return;
            
            var currentPos = (Vector2)_transform.position;
            var dir = new Vector2(XDirection, YDirection).normalized;
            currentPos += dir * (Speed * Time.deltaTime);

            _transform.position = currentPos;
        }

        public void SetSpeed(in float speed)
        {
            this.speed = speed;
        }

        //============================================================================================================//

        protected virtual void OnMovementChanged(float x, float y)
        {
            XDirection = Mathf.RoundToInt(x);
            YDirection = Mathf.RoundToInt(y);

            _isMoving = XDirection != 0 || YDirection != 0;
        }

        //============================================================================================================//
    }
}