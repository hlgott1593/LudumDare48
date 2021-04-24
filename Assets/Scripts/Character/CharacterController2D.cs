using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD48
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        public bool Grounded { get; private set; }
        public Vector2 Velocity => _rb.velocity;
        public Vector2 TargetVelocity => _targetVelocity;
        
        [Header("Movement")]
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        
        [Header("Ground")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private List<Transform> groundCheckPositions = new List<Transform>();
        
        
        private Rigidbody2D _rb;
        private Collider2D _groundTest;
        private Vector2 _targetVelocity = Vector2.zero;
        private Vector3 _velocity = Vector3.zero;
        private float _x = 0f;
        private float _y = 0f;

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
            var isGrounded = groundCheckPositions.Any(groundCheckPosition =>
            {
                _groundTest = Physics2D.OverlapPoint((Vector2) groundCheckPosition.position, groundLayerMask);
                return _groundTest != null;
            });
            Grounded = isGrounded;
        }

        public void SetTargetVelocityX(float newXVelocity)
        {
            _targetVelocity.x = newXVelocity;
        }

        public void AddTargetVelocityX(float velocityChange)
        {
            _targetVelocity.x += velocityChange;
        }
        
        public void SetTargetVelocityY(float newXVelocity)
        {
            _targetVelocity.y = newXVelocity;
        }

        public void AddTargetVelocityY(float velocityChange)
        {
            _targetVelocity.y += velocityChange;
        }

        public void AddForce(Vector2 force)
        {
            _rb.AddForce(force);
        }

        private void FixedUpdate()
        {
            _x = Mathf.Clamp(_targetVelocity.x, -20, +20);
            _y =  Mathf.Clamp(_targetVelocity.y, -50, +50);
            _targetVelocity = new Vector2(_x, _y); 
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, _targetVelocity, ref _velocity, movementSmoothing);
        }

        void OnGUI()
        {
            if (Application.isEditor)
            {
                GUI.Label(new Rect(100, 100, 100, 100), $"Grounded {Grounded}");
            }
        }
    }
}