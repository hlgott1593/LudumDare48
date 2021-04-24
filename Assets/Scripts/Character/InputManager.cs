using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public Vector2 Movement { get; protected set; }
        public bool JumpStart { get; protected set; }
        public bool JumpEnd { get; protected set; }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            JumpStart = ctx.started;
            JumpEnd = ctx.canceled;
        }

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            Movement = ctx.ReadValue<Vector2>();
        }
    }
}