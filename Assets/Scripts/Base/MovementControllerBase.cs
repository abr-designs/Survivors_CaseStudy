using Survivors.Base.Interfaces;
using UnityEngine;

namespace Survivors.Base
{
    public abstract class MovementControllerBase : MonoBehaviour, IMovementController
    {
        private Transform _transform;

        [SerializeField, Min(0f)]
        private float speed;

        public bool IsMoving => _isMoving;
        public int MoveDirection => _xDirection;


        private bool _isMoving;
        private int _xDirection, _yDirection;

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
            
            var currentPos = _transform.position;
            currentPos.x += _xDirection * speed * Time.deltaTime;
            currentPos.y += _yDirection * speed * Time.deltaTime;

            _transform.position = currentPos;
        }
        
        public void SetActive(bool state)
        {
            enabled = state;
        }

        //============================================================================================================//

        protected virtual void OnMovementChanged(float x, float y)
        {
            _xDirection = Mathf.RoundToInt(x);
            _yDirection = Mathf.RoundToInt(y);

            _isMoving = _xDirection != 0 || _yDirection != 0;
        }

        //============================================================================================================//
    }
}