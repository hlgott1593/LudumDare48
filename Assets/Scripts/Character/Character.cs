using System;
using System.Collections;
using DG.Tweening;
using LD48.Health;
using SuperTiled2Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace LD48
{
    public class Character : MonoBehaviour, IDamagable, ICheckpointUpdater {

        public static Action GhostFormEntered = delegate { };
        public static Action CorporealFormEntered = delegate { };
        
        [SerializeField] public LayerMask ghostCollisionLayer;
        [SerializeField] public LayerMask corporealCollisionlayer;
        
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
        protected CharacterController2D _controller;
        [SerializeField] private bool _renderGUI;
        [SerializeField] private int _spiritLayer;
        [SerializeField] private int _corporealLayer;

        [SerializeField] private float RespawnDelay = 0.5f;
        
        private float checkpointHp = 3;
        
        protected void Awake() => Initialize();

        private void Start() {
            Application.quitting += CorporealFormEntered;
            CorporealFormEntered();
            _controller.groundLayerMask = corporealCollisionlayer;

            CheckpointManager.Instance.OnCheckpointLoaded += OnCheckpointLoaded;
            CheckpointManager.Instance.OnCheckpointChanged += OnCheckpointChanged;
        }

        public void OnCheckpointChanged(Checkpoint checkpoint)
        {
            checkpointHp = HealthSystem.CurrentHp;
        }

        public void OnCheckpointLoaded(Checkpoint checkpoint)
        {
            StartCoroutine(Respawn(checkpoint.SpawnPoint.position));
        }

        IEnumerator Respawn(Vector3 position)
        {
            MovementState = CharacterStates.MovementStates.Idle;
            
            // Not sure why it is not resetting on its own???
            animator?.SetBool("Idle", true);
            animator?.SetBool("Running", false);
            animator?.SetBool("Jumping", false);
            animator?.SetBool("Falling", false);
            animator?.SetBool("Dashing", false);
            
            Condition = CharacterStates.CharacterConditions.Dead;
            DOTween.Sequence().Append(DOTweenModuleSprite.DOFade(Model, 0, 0));
            transform.position = position;
            _controller.ZeroVelocities();
            DOTween.Sequence().Append(DOTweenModuleSprite.DOFade(Model, 1, RespawnDelay));
            yield return new WaitForSeconds(RespawnDelay);
            Condition = CharacterStates.CharacterConditions.Normal;            
        }
        

        protected void Initialize() {
            HealthSystem = new HealthSystem(this, new HealthConfig(3, false));
            Condition = CharacterStates.CharacterConditions.Normal;
            MovementState = CharacterStates.MovementStates.Idle;
            _abilities = GetComponents<CharacterAbility>();
            _controller = GetComponent<CharacterController2D>();
        }
       
        public void OnHealthChanged(float prevAmount) { }

        public void OnDeath() {
            CheckpointManager.Instance.LoadLastCheckpoint();
            Condition = CharacterStates.CharacterConditions.Dead;
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
            if (Form == CharacterStates.Form.Ghost) {
                GhostFormEntered();
                gameObject.layer = _spiritLayer;
                gameObject.AssignChildLayers();
                _controller.groundLayerMask = ghostCollisionLayer;
            }
            else {
                CorporealFormEntered();
                gameObject.layer = _corporealLayer;  
                _controller.groundLayerMask = corporealCollisionlayer;
                gameObject.AssignChildLayers();
            }
        }

        protected void UpdateAnimator()
        {
            if (MovementState == CharacterStates.MovementStates.Dashing)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", false);
                animator?.SetBool("Dashing", true);
            }
            if (MovementState == CharacterStates.MovementStates.Idle)
            {
                animator?.SetBool("Idle", true);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", false);
                animator?.SetBool("Dashing", false);
            }
            if (MovementState == CharacterStates.MovementStates.Falling)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", true);
                animator?.SetBool("Dashing", false);
            }
            
            if (MovementState == CharacterStates.MovementStates.Jumping)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", true);
                animator?.SetBool("Falling", false);
                animator?.SetBool("Dashing", false);
            }
            
            if (MovementState == CharacterStates.MovementStates.DoubleJumping)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", false);
                animator?.SetBool("Jumping", true);
                animator?.SetBool("Falling", false);
                animator?.SetBool("Dashing", false);
            }
            
            if (MovementState == CharacterStates.MovementStates.Running)
            {
                animator?.SetBool("Idle", false);
                animator?.SetBool("Running", true);
                animator?.SetBool("Jumping", false);
                animator?.SetBool("Falling", false);
                animator?.SetBool("Dashing", false);
            }
            
            
        }

        void OnGUI()
        {
            if (Application.isEditor && _renderGUI)
            {
                GUI.Label(new Rect(50, 200, 150, 100), $"Form {Form}");
                GUI.Label(new Rect(50, 300, 150, 100), $"Condition {Condition}");
                GUI.Label(new Rect(50, 140, 150, 100), $"MovementState {MovementState}");
            }
        }


    }
}