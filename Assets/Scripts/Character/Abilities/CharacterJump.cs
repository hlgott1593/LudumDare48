using UnityEngine;

namespace LD48
{
    public class CharacterJump : CharacterAbility
    {
        [SerializeField] private float jumpDuration = 4f;
        [SerializeField] private float jumpVelocity = 4f;
        
        private bool _jumpedLastFrame = false;
        private float _jumpStartedAt = 0f;    
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (!IsAuthorized() || (_character.Condition != CharacterStates.CharacterConditions.Normal))
            {
                return;
            }

            if (_inputManager.JumpPerformed && !_jumpedLastFrame)
            {
                JumpStart();
            }
            
            if (_inputManager.JumpEnd)
            {
                JumpEnd();
            }
            
        }
        
        protected virtual bool CheckJumpStartConditions()
        {
            return _controller.Grounded;
        }
        

        private void JumpStart()
        {
            if (!CheckJumpStartConditions()) return;
            _jumpStartedAt = Time.time;
            _jumpedLastFrame = true;
            _character.ChangeMovementState(CharacterStates.MovementStates.Jumping);
        }

        private void JumpEnd()
        {
            if (_character.MovementState == CharacterStates.MovementStates.Jumping)
            {
                _controller.AddTargetVelocityY((Physics2D.gravity * (Time.deltaTime * 2f)).y);
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
            }
            _jumpedLastFrame = false;
        }

        public override void ProcessAbility()
        {
            if (_character.MovementState == CharacterStates.MovementStates.Jumping)
            {
                if (Time.time - _jumpStartedAt >= jumpDuration)
                {
                    JumpEnd();                       
                }
                else
                {
                    _controller.SetTargetVelocityY(jumpVelocity);
                }
                
            }
        }
    }
}