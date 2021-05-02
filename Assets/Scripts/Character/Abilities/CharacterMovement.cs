using UnityEngine;

namespace LD48 {
    public class CharacterMovement : CharacterAbility {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float airMovementSpeed = 4f;
        [SerializeField] private float idleThreshold = 0.1f;
        [SerializeField] private float fallingGravityMultiplier = 2f;

        private Vector2 _movement = Vector2.zero;
        [SerializeField] private GameObject _stopDust;
        [SerializeField] private GameObject _startDust;
        [SerializeField] private GameObject _runDust;
        [SerializeField] private Transform _dustSpawnPos;

        public override void HandleInput() {
            base.HandleInput();

            _movement = _inputManager.Movement;

            if (!IsAuthorized() || (_character.Condition != CharacterStates.CharacterConditions.Normal)) {
                _movement = Vector2.zero;
            }

            //_controller.SetTargetVelocityX(0);
            //_controller.SetTargetVelocityY(0);
        }

        public override void ProcessAbility() {
            base.ProcessAbility();
            HandleMovementState();
            SetMovement();
        }

        private void SetMovement() {
            // Horizontal
            var xVelocity = 0f;
            if (!_controller.Grounded) {
                xVelocity = _movement.x * airMovementSpeed;
            }
            else {
                xVelocity = _movement.x * movementSpeed;
            }

            _controller.SetTargetVelocityX(xVelocity);

            // Falling
            var fallVelocityBoost = 0f;
            if (_character.MovementState == CharacterStates.MovementStates.Falling) {
                fallVelocityBoost = (Physics2D.gravity * (Time.deltaTime * fallingGravityMultiplier)).y;
            }

            _controller.AddTargetVelocityY(fallVelocityBoost);

            // Bump into ceiling
            if (!_controller.Grounded && _controller.Ceiling) {
                _controller.SetTargetVelocityY((Physics2D.gravity * Time.fixedDeltaTime).y);
                _controller.SetVelocityY((Physics2D.gravity * Time.fixedDeltaTime).y);
            }

            // Grounded
            if (_controller.Grounded) {
                _controller.SetTargetVelocityY(0);
            }
        }

        private void HandleMovementState() {
            if (_character.MovementState == CharacterStates.MovementStates.Dashing) {
                if (_runDustInstance) Destroy(_runDustInstance);
                return;
            }

            if (!_controller.Grounded && (_character.Condition == CharacterStates.CharacterConditions.Normal)
                                      && (_character.MovementState == CharacterStates.MovementStates.Idle ||
                                          IsRunning())) {
                _character.ChangeMovementState(CharacterStates.MovementStates.Falling);
                if (_runDustInstance) Destroy(_runDustInstance);
            }

            //
            if (_controller.Grounded && (_character.MovementState == CharacterStates.MovementStates.Falling)) {
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
                if (_runDustInstance) Destroy(_runDustInstance);
            }

            //
            if (_controller.Grounded && (Mathf.Abs(_movement.x) > idleThreshold) &&
                (_character.MovementState == CharacterStates.MovementStates.Idle)) {
                _character.ChangeMovementState(CharacterStates.MovementStates.Running);
                MakeJuice(_startDust);
                if (!_runDustInstance) MakeContinuousRunDust();
            }

            if (_controller.Grounded && (Mathf.Abs(_movement.x) <= idleThreshold) &&
                (IsRunning())) {
                _character.ChangeMovementState(CharacterStates.MovementStates.Idle);
                MakeJuice(_stopDust, true);
                if (_runDustInstance) Destroy(_runDustInstance);
            }
        }

        private bool IsRunning() {
            return _character.MovementState == CharacterStates.MovementStates.Running;
        }

        private GameObject _runDustInstance;

        private void MakeContinuousRunDust() {
            _runDustInstance = Instantiate(_runDust, _dustSpawnPos.position, Quaternion.identity);
            if (_movement.x < 0) {
                _runDustInstance.transform.localScale = new Vector3(_runDustInstance.transform.localScale.x * -1, 1, 1);
            }

            _runDustInstance.transform.SetParent(gameObject.transform);
        }

        [SerializeField] private SortingLayer layer;
        private void MakeJuice(GameObject _dust, bool stop = false) {
            if (_dust == null || _dustSpawnPos == null) return;

            var dust = Instantiate(_dust, _dustSpawnPos.position, Quaternion.identity);

            // dust.layer = transpa
            var stopPos = !_character.Model.flipX;
            if (stop ? stopPos : _movement.x < 0) {
                for (int i = 0; i < dust.transform.childCount; i++) {
                    var c = dust.transform.GetChild(i);
                    c.transform.localScale = new Vector3(c.transform.localScale.x * -1, 1, 1);
                }

                dust.transform.localScale = new Vector3(dust.transform.localScale.x * -1, 1, 1);
            }
        }

        public override void UpdateAnimator() {
            _animator?.SetBool("Grounded", _controller.Grounded);
        }
    }
}