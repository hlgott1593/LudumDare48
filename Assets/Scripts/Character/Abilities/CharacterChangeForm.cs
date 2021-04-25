using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class CharacterChangeForm : CharacterAbility
    {
        private bool _lastFramePressed = false;
        private bool _change = false;

        public override void HandleInput()
        {
            base.HandleInput();
            if (PressedThisFrame())
            {
                _change = true;
            }
        }
        
        public override void LateProcessAbility()
        {
            ChangeForm();
            _lastFramePressed = _inputManager.ChangeForm;
        }

        protected void ChangeForm()
        {
            if (_change)
            {
                _change = false;
                _character.ChangeForm((_character.Form == CharacterStates.Form.Ghost) ? CharacterStates.Form.Shaman : CharacterStates.Form.Ghost);
            } 
        }
        
        protected bool PressedThisFrame()
        {
            return !_lastFramePressed && _inputManager.ChangeForm;
        }
        
        protected bool ReleasedThisFrame()
        {
            return _lastFramePressed && !_inputManager.ChangeForm;
        }
    }
}
