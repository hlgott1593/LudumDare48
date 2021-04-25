using System;
using LD48.Health;
using UnityEngine;

namespace LD48
{
    public class Character : MonoBehaviour, IDamagable {

        public HealthSystem HealthSystem { get; private set; }
        public GameObject Behaviour => gameObject;
        public CharacterStates.MovementStates MovementState { get; protected set; }
        public CharacterStates.CharacterConditions Condition { get; protected set; }
        public CharacterStates.Form Form { get; protected set;  }
        public InputManager InputManager => inputManager;
        [SerializeField] private InputManager inputManager;

        public Animator Animator => animator;
        [SerializeField] private Animator animator;
        
        public SpriteRenderer Model => model;

        [SerializeField] protected SpriteRenderer model;
        protected CharacterAbility[] _abilities;

        protected void Awake() => Initialize();

        protected void Initialize() {
            HealthSystem = new HealthSystem(this, new HealthConfig(3, false));
            Condition = CharacterStates.CharacterConditions.Normal;
            MovementState = CharacterStates.MovementStates.Idle;
            _abilities = GetComponents<CharacterAbility>();
        }

        public void OnHealthChanged(float prevAmount) { }

        public void OnDeath() {
            Destroy(gameObject.transform.root.gameObject);
        }
        protected void Update()
        {
            HandleInput();
            ProcessAbilities();
            LateProcessAbilities();
            UpdateAnimators();
        }
        
        protected virtual void HandleInput()
        {
            foreach (var ability in _abilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.HandleInput();
                }
            }
        }
        
        protected virtual void ProcessAbilities()
        {
            foreach (var ability in _abilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.ProcessAbility();
                }
            }
        }
        
        protected virtual void LateProcessAbilities()
        {
            foreach (var ability in _abilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.LateProcessAbility();
                }
            }
        }

        
        protected virtual void UpdateAnimators()
        {
            foreach (var ability in _abilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {	
                    ability.UpdateAnimator();
                }
            }
        }
        
        public void FlipAllAbilities()
        {
            foreach (var ability in _abilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {	
                    ability.Flip();
                }
            }
        }

        public void ChangeMovementState(CharacterStates.MovementStates newState)
        {
            MovementState = newState;
            UpdateAnimator();
        }

        public void ChangeForm(CharacterStates.Form newForm)
        {
            Form = newForm;
        }

        protected void UpdateAnimator()
        {
            
            if (MovementState == CharacterStates.MovementStates.Idle)
            {
                animator?.SetBool("Idle", true);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", false);
            }
            if (MovementState == CharacterStates.MovementStates.Falling)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", true);
            }
            
            if (MovementState == CharacterStates.MovementStates.Jumping)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", true);
                animator?.SetBool("Falling", false);
            }
            
            if (MovementState == CharacterStates.MovementStates.DoubleJumping)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", true);
                animator?.SetBool("Falling", false);
            }
            
            if (MovementState == CharacterStates.MovementStates.Running)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", true);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", false);
            }
            
        }
        
        
        
        void OnGUI()
        {
            if (Application.isEditor)
            {
                GUI.Label(new Rect(50, 200, 150, 100), $"Form {Form}");
                // GUI.Label(new Rect(50, 140, 150, 100), $"MovementState {MovementState}");
            }
        }


    }
}