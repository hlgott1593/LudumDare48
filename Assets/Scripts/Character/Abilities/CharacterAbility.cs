using System.Collections.Generic;
using UnityEngine;


namespace LD48
{
    [RequireComponent(typeof(Character), typeof(CharacterController2D))]
    public class CharacterAbility : MonoBehaviour
    {
        
        [SerializeField] protected List<CharacterStates.CharacterConditions> blockedConditionStates;
        [SerializeField] protected List<CharacterStates.MovementStates> blockedMovementStates;

        public bool IsUnlocked = true;
        public bool IsInitialized => _isInitialized;
        protected bool _isInitialized = false;
        
        protected Character _character;
        protected InputManager _inputManager;
        protected CharacterController2D _controller;
        
        protected void Start() => Initialize();
        
        protected virtual void Initialize()
        {
            _character = GetComponent<Character>();
            _controller = GetComponent<CharacterController2D>();
            _inputManager = _character.InputManager;
            _isInitialized = true;
        }

        public virtual bool IsAuthorized()
        {
            if (_character != null)
            {
                if ((blockedConditionStates != null) && (blockedConditionStates.Count > 0))
                {
                    if (blockedConditionStates.Contains(_character.Condition))
                    {
                        return false;
                    }
                }
                        
                if ((blockedMovementStates != null) && (blockedMovementStates.Count > 0))
                {
                    if (blockedMovementStates.Contains(_character.MovementState))
                    {
                        return false;
                    }
                }

              
            }
            return IsUnlocked;
        }
        
        public virtual void HandleInput()
        {

        }
        
        public virtual void ProcessAbility()
        {
			
        }
        
        public virtual void LateProcessAbility()
        {
			
        }

        public virtual void UpdateAnimator()
        {
            
        }
    }
}