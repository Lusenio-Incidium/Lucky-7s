using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{

    //Variables
    [Header("- - - Componets - - -")]
    [SerializeField] CharacterController controller;

    [Header("- - - Atributes - - -")]
    public List<GunStats> gunList = new List<GunStats>();
    [Range(1, 100)][SerializeField] int HP;
    [SerializeField][Range(1.0f, 10.0f)] float playerSpeed;
    [SerializeField][Range(1.5f, 5.0f)] float sprintMod;
    [SerializeField][Range(1.0f, 20.0f)] float jumpHeight;
    [SerializeField][Range(5.0f, 30.0f)] float gravityScale;
    [SerializeField][Range(1, 4)] int maxJumpAmmount;
    [SerializeField] int interactDist;

    //private variables
    Vector3 playerVelocity;
    Vector3 move;
    bool isGrounded;
    int jumpTimes;
    private int HPOrig;

    int selectedGunNum = 0;
    GunSystem gunSystem;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        GetPlayerHP();
        GameManager.instance.UpdatePlayerHP();
        spawnPlayer();

        gunSystem = GetComponent<GunSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        interact();
        sprint();
        switchGun();
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        GetPlayerHP();
        GameManager.instance.UpdatePlayerHP();
        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = HPOrig;
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

    void interact()
    {
        Debug.Log("This works");
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward);
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, interactDist))
        {
            Debug.Log("This works too");
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                Debug.Log("Press E to interact");

                if (Input.GetButtonDown("Interact"))
                {
                    hit.collider.GetComponent<IInteractable>().onInteract();
                }
            }
        }
    }
    void switchGun()
    {
        int previousGunNum = selectedGunNum;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedGunNum = (selectedGunNum + 1) % gunList.Count;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedGunNum--;
            if (selectedGunNum < 0)
            {
                selectedGunNum = gunList.Count - 1;
            }
        }
        if (previousGunNum != selectedGunNum)
        {
            gunSystem.EquipWeapon(selectedGunNum);
        }
    }


    public int GetPlayerHP()
    {
        return HP;
    }

    public void playerHeal(int amount)
    {
        HP += amount;
        if (HP > 100)
        {
            HP = 100;
        }
    }

    public void speedChange(float amount)
    {
        playerSpeed += amount;
    }
}
