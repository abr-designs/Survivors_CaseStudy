using Survivors.Base.Interfaces;
using UnityEngine;

namespace Survivors.Base
{
    public abstract class MovementControllerBase : MonoBehaviour, IMovementController
    {
        private Transform _transform;

        public float Speed => speed;
        [SerializeField, Min(0f)]
        private float speed;

        public bool IsMoving => _isMoving;
        public int MoveDirection => (int)XDirection;


        private bool _isMoving;
        protected float XDirection, YDirection;

        //============================================================================================================//
        private void Start()
        {
            _transform = gameObject.transform;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isMoving == false)
                return;
            
            var currentPos = (Vector2)_transform.position;
            var dir = new Vector2(XDirection, YDirection).normalized;
            currentPos += dir * (speed * Time.deltaTime);

            _transform.position = currentPos;
        }

        public void SetSpeed(in float speed)
        {
            this.speed = speed;
        }

        public void SetActive(bool state)
        {
            enabled = state;
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