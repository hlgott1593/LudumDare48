using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public Vector2 Movement { get; protected set; }
        public bool JumpPerformed { get; protected set; }
        public bool JumpEnd { get; protected set; }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            JumpPerformed = ctx.performed;
            JumpEnd = ctx.canceled;
        }

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            Movement = ctx.ReadValue<Vector2>();
        }
    }
}
