using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float speed;
    public float distance;

    public LayerMask groundLayer;
    public Collider2D bodyCollider;

    [HideInInspector]
    public bool touchingWall = false;

    private bool movingRight = true;

    public Transform groundDetection;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        //raycasts to find the ground
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        //if the raycast does not find ground or if enemy is touching a wall they will change directions
        if (groundInfo.collider == false || touchingWall == true)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                touchingWall = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
                touchingWall = false;
            }
        }
    }

    //If the enemy enters into the trigger boxes on the walls it will change the touchingwall variable to true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Debug.Log("you have hit a wall");
            touchingWall = true;
        }
    }
}
