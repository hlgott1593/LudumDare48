using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public Vector2 Movement { get; protected set; }
        public bool Jump { get; protected set; }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            Jump = ctx.performed;
        }

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            Movement = ctx.ReadValue<Vector2>();
        }
    }
}
