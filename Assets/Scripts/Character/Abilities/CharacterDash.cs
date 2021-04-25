using UnityEngine;

namespace LD48
{
    public class CharacterDash : CharacterAbility
    {
        private bool _lastFramePressed = false;
        private bool _dashing = false;
        private bool _canDash = true;
        
        protected Vector2 _directionCache = Vector2.right;
        protected Vector2 _dashDirection = Vector2.right;
        
        
        [SerializeField] protected float dashDuration = 0.25f;
        [SerializeField] protected float dashVelocity = 100f;
        private double _dashStartAt;


        public override void HandleInput()
        {
            base.HandleInput();
            if (!IsAuthorized()) return;
            
            if (PressedThisFrame())
            {
                DashStart();
            }
        }

        public override void ProcessAbility()
        {
            if (!CheckIsDashing()) return;
            
            
            if (Time.time - _dashStartAt >= dashDuration || _controller.Ceiling || _controller.Grounded)
            {
                DashEnd();                       
            }
            else
            {
                var velocity = dashVelocity * _dashDirection; 
                _controller.SetTargetVelocityX(velocity.x);
                _controller.SetTargetVelocityY(0);
            }
        }
        
        public void DashStart()
        {
            if (!CheckDashStartConditions()) return;
            _dashing = true;
            _canDash = false;
            _dashStartAt = Time.time;
            _dashDirection = _directionCache;
            _character.ChangeMovementState(CharacterStates.MovementStates.Dashing);
        }

        public void DashEnd()
        {
            _dashing = false;
            _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
        }

        public override void Flip()
        {
            _directionCache = (_directionCache == Vector2.right) ? Vector2.left : Vector2.right;
        }

        public override void UpdateAnimator()
        {
            _animator.SetBool("Dashing", _dashing);
        }

        private bool CheckIsDashing()
        {
            return _character.MovementState == CharacterStates.MovementStates.Dashing;
        }

        private bool CheckDashStartConditions()
        {
            return !_dashing  && _canDash && !_controller.Grounded;
        }

        public override void LateProcessAbility()
        {
            _lastFramePressed = _inputManager.Special;
            if (_controller.Grounded) _canDash = true;
        }
        
        protected bool PressedThisFrame()
        {
            return !_lastFramePressed && _inputManager.Special;
        }
        
        protected bool ReleasedThisFrame()
        {
            return _lastFramePressed && !_inputManager.Special;
        }
        
        void OnGUI()
        {
            if (Application.isEditor && renderGUI)
            {
                GUI.Label(new Rect(50, 20, 150, 100), $"Dashing {_dashing}");
            }
        }
    }
}