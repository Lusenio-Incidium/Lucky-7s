using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IPhysics
{

    static PlayerController pc;

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
    [SerializeField] float pushBackResolve;
    [SerializeField] int throwPower;

    [Header("GunSpawnables")]
    [SerializeField] GameObject pistolSpawn;
    [SerializeField] GameObject tommySpawn;

    //private variables
    Vector3 playerVelocity;
    Vector3 move;
    bool isGrounded;
    int jumpTimes;
    private int HPOrig;
    Vector3 pushBack;
    int selectedGunNum = 0;
    GunSystem gunSystem;

    // Start is called before the first frame update
    void Start()
    {
        if(pc == null) 
        {
            pc = this;
            HPOrig = HP;
            spawnPlayer();

            gunSystem = GetComponent<GunSystem>();
        }
        else 
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        interact();
        sprint();

        if(gunList.Count > 0)
            switchGun();
    }

    public void shopRegister(ShopPickup updates) 
    {
        //Add players health from shop
        playerHeal(updates.healthAmount);


        if (updates.addPistol) 
        {
            Instantiate(pistolSpawn, transform.position, transform.rotation);
        }

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
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
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
        updatePlayerUI();
    }

    public void spawnPlayerOnLoad() 
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        
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
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward);
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, interactDist))
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                GameManager.instance.interactTxt.gameObject.SetActive(true);

                if (Input.GetButtonDown("Interact"))
                {
                    hit.collider.GetComponent<IInteractable>().onInteract();
                }
            }
            else 
            {
                GameManager.instance.interactTxt.gameObject.SetActive(false);
            }
        }
        else 
        {
            GameManager.instance.interactTxt.gameObject.SetActive(false);
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
        updatePlayerUI();
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float) HP / HPOrig;
        GameManager.instance.HPDisplay.text = HP.ToString();    
    }
    public void speedChange(float amount)
    {
        playerSpeed += amount;
    }

    public void TakePush(Vector3 dir)
    {
        pushBack += dir;
    }
    public int GetThrowPower()
    {
        return throwPower * 10;
    }
}
