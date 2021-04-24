﻿using System;
using UnityEngine;

namespace LD48
{
    public class Character : MonoBehaviour
    {
        public CharacterStates.MovementStates MovementState { get; protected set; }
        public CharacterStates.CharacterConditions Condition { get; protected set; }
        public InputManager InputManager => inputManager;
        [SerializeField] private InputManager inputManager;
        
        public SpriteRenderer Model => model;

        [SerializeField] protected SpriteRenderer model;
        protected CharacterAbility[] _abilities;

        protected void Awake() => Initialize();

        protected void Initialize()
        {
            Condition = CharacterStates.CharacterConditions.Normal;
            MovementState = CharacterStates.MovementStates.Idle;
            _abilities = GetComponents<CharacterAbility>();
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
        }
        
        
        
        void OnGUI()
        {
            if (Application.isEditor)
            {
                GUI.Label(new Rect(100, 140, 100, 100), $"MovementState {MovementState}");
            }
        }


    }
}