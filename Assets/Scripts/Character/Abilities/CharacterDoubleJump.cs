using UnityEngine;

namespace LD48
{
    public class CharacterDoubleJump : CharacterJump
    {

        [SerializeField] protected int ExtraJumpCount = 1;
        protected int currentJumpCount;

        protected override void Initialize()
        {
            base.Initialize();
            currentJumpCount = ExtraJumpCount;
        }

        protected bool HasJumpRemaining()
        {
            return currentJumpCount > 0;
        }
        
        protected override bool CheckJumpStartConditions()
        {
            return !_controller.Grounded && _character.MovementState != CharacterStates.MovementStates.Jumping && HasJumpRemaining();
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
            if (_controller.Grounded) currentJumpCount = ExtraJumpCount;
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