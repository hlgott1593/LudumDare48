using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PatrolAndChaseAI : MonoBehaviour {
        enum PatrolStates {
            Patrolling,
            Chasing,
            ReturningToPatrol
        }

        PatrolStates PatrolState;

        [SerializeField] protected AnimationMap animations;
        public Vector2 MovementDirection => _direction;
        public float MovementSpeed => movementSpeed;

        private float reachedWaypointDelayLeftToWait = 0;
        [SerializeField] protected float reachedWaypointDelay = 2f;
        [SerializeField] protected float movementSpeed = 1f;
        [SerializeField] protected float slowRegion = 2f;
        [SerializeField] protected float reachedWaypointThreshold = 0.1f;
        [SerializeField] protected List<Transform> waypoints = new List<Transform>();

        public Transform currentTarget;
        public int currentTargetIndex = 0;
        protected Rigidbody2D _rb;


        private Vector2 _towards;
        private Vector2 _direction;
        private CircleCollider2D _col;
        private SpriteRenderer _renderer;
        public bool _yMovement;
        private Animator _ani;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CircleCollider2D>();
            _ani = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            PatrolState = PatrolStates.Patrolling;
        }

        private void Start() {
            _rb.isKinematic = true;
            _rb.freezeRotation = true;
            _ani.Play(animations.IdleName);
        }
        protected void SetMovement() {
            if (currentTarget == null) {
                _towards = Vector2.zero;
                _direction = Vector2.zero;
                return;
            }

            _towards = currentTarget.position - transform.position;
            _direction = _towards.normalized;
        }
        protected Transform GetNextTarget() {
            if (PatrolState == PatrolStates.Chasing && currentTarget == null)
                PatrolState = PatrolStates.ReturningToPatrol;
            
            switch (PatrolState) {
                case PatrolStates.Chasing:
                    return currentTarget;
                default:
                    if (waypoints.Count == 0) return null;
                    currentTargetIndex += 1;
                    if (currentTargetIndex >= waypoints.Count) {
                        currentTargetIndex = 0;
                    }
                    return waypoints[currentTargetIndex];
            }
        }

        private void FixedUpdate() {
            if (currentTarget == null) {
                if (IdleAtDestination()) return;

                currentTarget = GetNextTarget();
                return;
            }

            SetMovement();
            if (_towards.sqrMagnitude <= reachedWaypointThreshold) {
                if (IdleAtDestination()) return;
                
                currentTarget = GetNextTarget();
            }

            

            var speed = movementSpeed;
            if (_towards.sqrMagnitude <= slowRegion) {
                speed *= _towards.sqrMagnitude / slowRegion;
            }


            var movement = (speed * Time.fixedDeltaTime * _direction);

            if (movement.sqrMagnitude > _towards.sqrMagnitude) {
                movement = _towards;
            }
            

            var motion = (Vector2) transform.position + new Vector2(movement.x, _yMovement ? movement.y : 0);
            _renderer.flipX = motion.x < transform.position.x;

            _rb.MovePosition(motion);
        }

        private bool IdleAtDestination() {
            if (reachedWaypointDelayLeftToWait > 0 && PatrolState!= PatrolStates.Chasing) {
                _ani.Play(animations.IdleName);
                reachedWaypointDelayLeftToWait -= Time.fixedDeltaTime;
                return true;
            }

            _ani.Play(animations.WalkName);
            reachedWaypointDelayLeftToWait = reachedWaypointDelay;
            return false;
        }

        private static bool IsInvalidTarget(Collider2D other) =>
            other == null ||
            other.gameObject == null ||
            !other.CompareTag("Player");

        private void OnTriggerEnter2D(Collider2D other) {
            if (IsInvalidTarget(other)) return;
            TargetAcquired(other.gameObject);
        }

        private void TargetAcquired(GameObject target) {
            PatrolState = PatrolStates.Chasing;
            currentTarget = target.transform;
        }

        public void TargetLost(Collider2D other) {
            if (IsInvalidTarget(other)) return;
            currentTarget = null;
            PatrolState = PatrolStates.ReturningToPatrol;
        }
    }
}