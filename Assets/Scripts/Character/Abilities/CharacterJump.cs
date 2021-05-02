using UnityEngine;

namespace LD48
{
    public class CharacterJump : CharacterAbility
    {
        [SerializeField] protected float jumpDuration = 0.2f;
        [SerializeField] protected float jumpVelocity = 20f;

        protected bool _jumping = false;
        protected bool _lastFrameJump = false;
        protected float _jumpStartedAt = 0f;
        [SerializeField] private GameObject _dust;
        [SerializeField] private Transform _dustSpawnPos;

        public override void HandleInput()
        {
            base.HandleInput();

            if (!IsAuthorized() || (_character.Condition != CharacterStates.CharacterConditions.Normal))
            {
                return;
            }
            
            if (JumpPressedThisFrame())
            {
                JumpStart();
            }
            
            if (JumpReleasedThisFrame())
            {
                JumpEnd();
            }

        }

        protected bool JumpPressedThisFrame()
        {
            return !_lastFrameJump && _inputManager.Jump;
        }
        
        protected bool JumpReleasedThisFrame()
        {
            return _lastFrameJump && !_inputManager.Jump;
        }
        
        public override void LateProcessAbility()
        {
            _lastFrameJump = _inputManager.Jump;
            if (_controller.Grounded) _jumping = false;
        }

        protected virtual bool CheckJumpStartConditions()
        {
            return _controller.Grounded && !_jumping;
        }

        protected virtual bool CheckIsJumping()
        {
            return _character.MovementState == CharacterStates.MovementStates.Jumping;
        }
        protected virtual void SetJumpState()
        {
            _character.ChangeMovementState(CharacterStates.MovementStates.Jumping);
        }

        protected virtual void JumpStart()
        {
            if (!CheckJumpStartConditions()) return;
            _jumpStartedAt = Time.time;
            _jumping = true;
            PlayStartSfxRandomPitch(0.9f, 1.1f);
            SetJumpState();
            if (_dust == null || _dustSpawnPos == null) return;
            var dust = Instantiate(_dust, _dustSpawnPos.position, Quaternion.identity);
            if (_character.Model.flipX) dust.transform.localScale = new Vector3(-1, 1, 1);
        }

        private void JumpEnd()
        {
            if (!CheckIsJumping()) return;
            _jumping = false;
            _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
        }

        public override void ProcessAbility()
        {
            if (!CheckIsJumping()) return;
            if (Time.time - _jumpStartedAt >= jumpDuration || _controller.Ceiling)
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