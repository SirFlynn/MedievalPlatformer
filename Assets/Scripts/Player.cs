using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    void Update ()
    {
        //if A or left arrow pressed this moves the player
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //If jump button pressed then jump becomes true
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate ()
    {
        //Move the player, allows player to jump
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
