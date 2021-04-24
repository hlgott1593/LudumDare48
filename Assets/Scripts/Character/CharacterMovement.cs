using UnityEngine;

namespace LD48
{
    public class CharacterMovement : CharacterAbility
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float airMovementSpeed = 4f;
        [SerializeField] private float idleThreshold = 0.01f;
        
        private Vector2 _movement = Vector2.zero;

        public override void HandleInput()
        {
            base.HandleInput();

            _movement = _inputManager.Movement;

            if (!IsAuthorized() || (_character.Condition != CharacterStates.CharacterConditions.Normal))
            {
                _movement = Vector2.zero;
            }
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            HandleMovementState();
            SetMovement();
        }

        private void SetMovement()
        {
            if (!_controller.Grounded)
            {
                _controller.SetTargetVelocityX(_movement.x * airMovementSpeed);
                return;
            }

            _controller.SetTargetVelocityX(_movement.x * movementSpeed);
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
            if (_controller.Grounded && (_controller.Velocity.magnitude > idleThreshold) &&
                (_character.MovementState == CharacterStates.MovementStates.Idle))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Running);	
            }
            
            if (_controller.Grounded && (_controller.Velocity.magnitude <= idleThreshold) &&
                (_character.MovementState == CharacterStates.MovementStates.Running))
            {
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);	
            }
        }
    }
}