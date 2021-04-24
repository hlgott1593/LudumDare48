using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float xMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump");
    }

    public void XMove(InputAction.CallbackContext ctx)
    {
        xMove = ctx.ReadValue<float>();
    }
}
