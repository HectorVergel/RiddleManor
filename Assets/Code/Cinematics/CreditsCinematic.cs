using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsCinematic : MonoBehaviour
{
    public string menuSceneName;
    public Animator anim;
    public float speedDown = 2f;
    public float speedUp;
    private void OnEnable() {
        InputManager.GetAction("Jump").action += FastForward;
        InputManager.GetAction("ExitDialogue").action += StopCinematic;
        anim.speed = speedDown;
        Cursor.visible = false;
    }
    private void OnDisable() {
        InputManager.GetAction("Jump").action -= FastForward;
        InputManager.GetAction("ExitDialogue").action -= StopCinematic;
    }
    public void EndCinematic()
    {
        Loader.instance.LoadScene(menuSceneName);
    }
    public void StopCinematic(InputAction.CallbackContext context)
    {
        if(context.started) EndCinematic();
    }
    public void FastForward(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() <= 0)
        {
            anim.speed = speedDown;
        }
        else
        {
            anim.speed = speedUp;
        }
    }
}
