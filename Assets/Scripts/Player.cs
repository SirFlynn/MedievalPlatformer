using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;

    public GameObject platform;
    //public GameObject clonePlatform;
    // List<GameObject> unityGameObjects = new List<GameObjects>();

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    //Creating the platform
    public void Spawn(Vector3 position)
    {
        //creates the platform at the position and destroys old platform
        Instantiate(platform).transform.position = position;
    }

    void Update()
    {
        //if A or left arrow pressed this moves the player
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //If jump button pressed then jump becomes true
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        //Move the player, allows player to jump
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

        //if F key pressed spawn object at mousepointer
        if (Input.GetKeyDown(KeyCode.F))
        {
            //destroy clone
            Destroy(platform);

            //Takes the mouse position (on the canvas) and converts them into the transform co-ordinates in game
            Vector3 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

            //Make Z position the Z position of the platform prefab not the camera
            Vector3 adjustZ = new Vector3(worldpoint.x, worldpoint.y, platform.transform.position.z);
            Spawn(adjustZ);

            //adds platform to list
            //unityGameObjects.Add(platform);
        }
    }
}
