using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        public Vector2 MovementDirection => _direction;
        public float MovementSpeed => movementSpeed;
        
        [SerializeField] protected float movementSpeed = 1f;
        [SerializeField] protected float slowRegion = 2f;
        [SerializeField] protected float reachedWaypointThreshold = 0.1f;
        [SerializeField] protected List<Transform> waypoints = new List<Transform>();
        
        public Transform currentTarget;
        public int currentTargetIndex = 0;
        protected Rigidbody2D _rb;

        
        
        private Vector2 _towards;
        private Vector2 _direction;

        protected void SetMovement()
        {
            if (currentTarget == null)
            {
                _towards = Vector2.zero;
                _direction = Vector2.zero;
                return;
            }
            
            _towards = currentTarget.position - transform.position;
            _direction = _towards.normalized;
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rb.isKinematic = true;
            _rb.freezeRotation = true;
        }

        protected Transform GetNextTarget()
        {
            if (waypoints.Count == 0) return null;
            currentTargetIndex += 1;
            if (currentTargetIndex >= waypoints.Count)
            {
                currentTargetIndex = 0;
            }
            return waypoints[currentTargetIndex];
        }
        
        private void FixedUpdate()
        {
            if (currentTarget == null)
            {
                currentTarget = GetNextTarget();
                return;
            }
            
            SetMovement();
            if (_towards.sqrMagnitude <= reachedWaypointThreshold)
            {
                currentTarget = GetNextTarget();
            }


            var speed = movementSpeed;
            if (_towards.sqrMagnitude <= slowRegion)
            {
                speed *= _towards.sqrMagnitude / slowRegion;
            }
            
    
            var movement = (speed * Time.fixedDeltaTime * _direction);

            if (movement.sqrMagnitude > _towards.sqrMagnitude)
            {
                movement = _towards;
            }
            
            _rb.MovePosition((Vector2)transform.position + movement);
        }
    }
}
