using UnityEngine;

namespace LD48
{
    public class CharacterMovement : CharacterAbility
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float airMovementSpeed = 4f;
        [SerializeField] private float idleThreshold = 0.1f;
        [SerializeField] private float fallingGravityMultiplier = 2f;
        
        private Vector2 _movement = Vector2.zero;

        public override void HandleInput()
        {
            base.HandleInput();

            _movement = _inputManager.Movement;

            if (!IsAuthorized() || (_character.Condition != CharacterStates.CharacterConditions.Normal))
            {
                _movement = Vector2.zero;
            }
            
            //_controller.SetTargetVelocityX(0);
            //_controller.SetTargetVelocityY(0);
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            HandleMovementState();
            SetMovement();
        }

        private void SetMovement()
        {
            // Horizontal
            var xVelocity = 0f;
            if (!_controller.Grounded)
            {
                xVelocity = _movement.x * airMovementSpeed;
            }
            else
            {
                xVelocity = _movement.x * movementSpeed;
            }
            _controller.SetTargetVelocityX(xVelocity);
            
            // Falling
            var fallVelocityBoost = 0f;
            if (_character.MovementState == CharacterStates.MovementStates.Falling)
            {
                fallVelocityBoost = (Physics2D.gravity * (Time.deltaTime * fallingGravityMultiplier)).y;
            }
            _controller.AddTargetVelocityY(fallVelocityBoost);

            // Bump into ceiling
            if (!_controller.Grounded && _controller.Ceiling)
            { 
                _controller.SetTargetVelocityY((Physics2D.gravity * Time.fixedDeltaTime).y);
                _controller.SetVelocityY((Physics2D.gravity * Time.fixedDeltaTime).y);
            }
            
            // Grounded
            if (_controller.Grounded)
            {
                _controller.SetTargetVelocityY(0);
            }
        }

        private void HandleMovementState()
        {
            //
            if (!_controller.Grounded && (_character.Condition == CharacterStates.CharacterConditions.Normal)
                && (
                    (_character.MovementState == CharacterStates.MovementStates.Idle) ||
                    (_character.MovementState == CharacterStates.MovementStates.Running)
                ))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Falling);
            }
            
            //
            if (_controller.Grounded && (_character.MovementState == CharacterStates.MovementStates.Falling))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
            }
            
            //
            if (_controller.Grounded && (Mathf.Abs(_inputManager.Movement.x) > idleThreshold) &&
                (_character.MovementState == CharacterStates.MovementStates.Idle))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Running);	
            }
            
            if (_controller.Grounded && (Mathf.Abs(_inputManager.Movement.x) <= idleThreshold) &&
                (_character.MovementState == CharacterStates.MovementStates.Running))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);	
            }
        }

        public override void UpdateAnimator()
        {
            _animator?.SetBool("Grounded", _controller.Grounded);
        }
    }
}