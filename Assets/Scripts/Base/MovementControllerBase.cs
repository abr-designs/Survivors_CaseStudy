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
        private bool _isMoving;
        private int _inputX, _inputY;

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
            currentPos.x += _inputX * speed * Time.deltaTime;
            currentPos.y += _inputY * speed * Time.deltaTime;

            _transform.position = currentPos;
        }

        //============================================================================================================//

        protected virtual void OnMovementChanged(float x, float y)
        {
            _inputX = Mathf.RoundToInt(x);
            _inputY = Mathf.RoundToInt(y);

            _isMoving = _inputX != 0 || _inputY != 0;
        }

        //============================================================================================================//
    }
}