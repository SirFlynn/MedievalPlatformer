using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;

    void Update ()
    {
        //if A or left arrow pressed this moves the player
        Debug.Log(Input.GetAxisRaw("Horizontal"));
    }
}
