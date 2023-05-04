using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Variables
    [Header("- - - Componets - - -")]
    [SerializeField] CharacterController controller;

    [Header("- - - Atributes - - -")]
    [SerializeField][Range(1.0f, 10.0f)] float playerSpeed;
    [SerializeField][Range(1.5f, 5.0f)] float sprintMod;
    [SerializeField][Range(1.0f, 20.0f)] float jumpHeight;
    [SerializeField][Range(5.0f, 30.0f)] float gravityScale;
    [SerializeField][Range(1, 4)] int maxJumpAmmount;

    //private variables
    Vector3 playerVelocity;
    Vector3 move;
    bool isGrounded;
    int jumpTimes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }

    void movement()
    {
        //Check if player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            jumpTimes = 0;
            playerVelocity.y = 0;
        }

        //Set move vector for player movement
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        //Jump Input
        if (Input.GetButtonDown("Jump") && jumpTimes < maxJumpAmmount)
        {
            jumpTimes++;
            playerVelocity.y = jumpHeight;
        }

        //gravity
        playerVelocity.y -= gravityScale * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
        }
    }
}
