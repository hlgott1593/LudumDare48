using Unity.Mathematics;
using UnityEngine;

namespace LD48 {
    public class CharacterDash : CharacterAbility {
        private bool _lastFramePressed = false;
        private bool _dashing = false;
        private bool _canDash = true;

        protected Vector2 _directionCache = Vector2.right;
        protected Vector2 _dashDirection = Vector2.right;


        [SerializeField] protected float dashDuration = 0.25f;
        [SerializeField] protected float dashVelocity = 100f;
        private double _dashStartAt;
        [SerializeField] private GameObject _dust;
        [SerializeField] private Transform _dustSpawnPos;


        public override void HandleInput() {
            base.HandleInput();
            if (!IsAuthorized()) return;

            if (PressedThisFrame()) {
                DashStart();
            }
        }

        public override void ProcessAbility() {
            if (!CheckIsDashing()) {
                _dashing = false;
                _animator.SetBool("Dashing", _dashing);
                return;
            }


            if (Time.time - _dashStartAt >= dashDuration || _controller.Ceiling || _controller.Grounded) {
                DashEnd();
            }
            else {
                var velocity = dashVelocity * _dashDirection;
                _controller.SetTargetVelocityX(velocity.x);
                _controller.SetTargetVelocityY(0);
            }
        }

        public void DashStart() {
            if (!CheckDashStartConditions()) return;
            _dashing = true;
            _canDash = false;
            _dashStartAt = Time.time;
            _dashDirection = _directionCache;
            PlayStartSfxRandomPitch(0.9f, 1.1f);
            _character.ChangeMovementState(CharacterStates.MovementStates.Dashing);
            MakeJuice();
        }

        private void MakeJuice() {
            if (_dust == null || _dustSpawnPos == null) return;
            var dust = Instantiate(_dust, _dustSpawnPos.position, Quaternion.identity);
            if (_character.Model.flipX) {
                for (int i = 0; i < dust.transform.childCount; i++) {
                    var c = dust.transform.GetChild(i);
                    c.transform.localScale = new Vector3(-1, 1, 1);
                }

                dust.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void DashEnd() {
            _dashing = false;
            _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
        }

        public override void Flip() {
            _directionCache = (_directionCache == Vector2.right) ? Vector2.left : Vector2.right;
        }

        public override void UpdateAnimator() {
            _animator.SetBool("Dashing", _dashing);
        }

        private bool CheckIsDashing() {
            return _character.MovementState == CharacterStates.MovementStates.Dashing;
        }

        private bool CheckDashStartConditions() {
            return !_dashing && _canDash && !_controller.Grounded;
        }

        public override void LateProcessAbility() {
            _lastFramePressed = _inputManager.Special;
            if (_controller.Grounded) {
                _dashing = false;
                _canDash = true;
            }
        }

        protected bool PressedThisFrame() {
            return !_lastFramePressed && _inputManager.Special;
        }

        protected bool ReleasedThisFrame() {
            return _lastFramePressed && !_inputManager.Special;
        }

        void OnGUI() {
            if (Application.isEditor && renderGUI) {
                GUILayout.Box($"Dashing {_dashing}");
                GUILayout.Box($"Rotation {Quaternion.identity * _dashDirection}");
            }
        }
    }
}