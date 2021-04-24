using UnityEngine;
using UnityEngine.Serialization;

namespace LD48
{
    public class CharacterDoubleJump : CharacterJump
    {

        [SerializeField] protected int extraJumpCount = 1;
        [SerializeField] protected bool ignoreYVelocity = true;
        [SerializeField] protected float yVelocityGreaterThan = -1f;
        protected int currentJumpCount;

        protected override void Initialize()
        {
            base.Initialize();
            currentJumpCount = extraJumpCount;
        }

        protected bool HasJumpRemaining()
        {
            return currentJumpCount > 0;
        }
        
        protected override bool CheckJumpStartConditions()
        {
            return !_controller.Grounded && _character.MovementState != CharacterStates.MovementStates.Jumping && HasJumpRemaining() && (ignoreYVelocity || _controller.Velocity.y >= yVelocityGreaterThan);
        }

        protected override bool CheckIsJumping()
        {
            return _character.MovementState == CharacterStates.MovementStates.DoubleJumping;
        }
        protected override void SetJumpState()
        {
            _character.ChangeMovementState(CharacterStates.MovementStates.DoubleJumping);
        }

        protected override void JumpStart()
        {
            base.JumpStart();
            
            if (!CheckIsJumping()) return;
            currentJumpCount -= 1;
        }
        
        public override void ProcessAbility()
        {
            if (_controller.Grounded) currentJumpCount = extraJumpCount;
            base.ProcessAbility();
        }
        
        void OnGUI()
        {
            if (Application.isEditor)
            {
                GUI.Label(new Rect(100, 180, 100, 100), $"ExtraJumpsLeft {currentJumpCount}");
            }
        }
        
    }
}