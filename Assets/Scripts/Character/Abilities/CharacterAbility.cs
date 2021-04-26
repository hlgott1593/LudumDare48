using System.Collections.Generic;
using LD48.Audio;
using UnityEngine;


namespace LD48
{
    [RequireComponent(typeof(Character), typeof(CharacterController2D))]
    public class CharacterAbility : MonoBehaviour
    {
        
        [SerializeField] protected List<CharacterStates.CharacterConditions> blockedConditionStates;
        [SerializeField] protected List<CharacterStates.MovementStates> blockedMovementStates;
        [SerializeField] protected bool renderGUI = false;

        [SerializeField] protected List<AudioClip> SfxStart = new List<AudioClip>();

        protected void PlayStartSfx()
        {
            if (SfxStart.Count == 0) return;
            var chosen = Mathf.FloorToInt(Random.Range(0, SfxStart.Count - 1));
            var clip = SfxStart[chosen];
            if (clip != null)
            {
                AudioManager.Instance.PlaySfx(clip);   
            }
        }
        
        protected void PlayStartSfxRandomPitch(float min, float max)
        {
            if (SfxStart.Count == 0) return;
            var chosen = Mathf.FloorToInt(Random.Range(0, SfxStart.Count - 1));
            var pitch = Random.Range(min, max);
            var clip = SfxStart[chosen];
            if (clip != null)
            {
                AudioManager.Instance.PlaySfx(clip, pitch);   
            }
        }
        
        public bool IsUnlocked = true;
        public bool IsInitialized => _isInitialized;
        protected bool _isInitialized = false;
        
        protected Character _character;
        protected InputManager _inputManager;
        protected CharacterController2D _controller;
        protected Animator _animator;
        
        protected void Start() => Initialize();
        
        protected virtual void Initialize()
        {
            _character = GetComponent<Character>();
            _controller = GetComponent<CharacterController2D>();
            _inputManager = _character.InputManager;
            _animator = _character.Animator;
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

        public virtual void Flip()
        {
            
        }
    }
}