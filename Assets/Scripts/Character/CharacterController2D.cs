using System;
using UnityEngine;

namespace LD48
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        public bool Grounded { get; private set; }
        public Vector3 Velocity => _velocity;
        public Vector2 TargetVelocity => _targetVelocity;
        
        [Header("Movement")]
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        
        [Header("Ground")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Transform groundCheckPosition;
        
        
        private Rigidbody2D _rb;
        private Collider2D _groundTest;
        private Vector2 _targetVelocity = Vector2.zero;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            GroundCheck();
        }
        
        private void GroundCheck()
        {
            _groundTest = Physics2D.OverlapPoint((Vector2)groundCheckPosition.position, groundLayerMask);
            Grounded = (_groundTest != null);
        }

        public void SetTargetVelocityX(float newXVelocity)
        {
            _targetVelocity.x = newXVelocity;
        }

        public void AddTargetVelocityX(float velocityChange)
        {
            _targetVelocity.x += velocityChange;
        }

        public void AddForce(Vector2 force)
        {
            _rb.AddForce(force);
        }

        private void FixedUpdate()
        {
            
            _targetVelocity = new Vector2(_targetVelocity.x, _rb.velocity.y); 
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, _targetVelocity, ref _velocity, movementSmoothing);
        }
    }
}