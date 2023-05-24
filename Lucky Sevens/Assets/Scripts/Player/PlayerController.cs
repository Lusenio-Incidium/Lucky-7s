using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IPhysics, IStatusEffect
{

    static PlayerController pc;

    //Variables
    [Header("- - - Componets - - -")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

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
    [SerializeField] StatusEffectObj activeEffect;

    [Header("Audio")]
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] float footVol;
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField] float jumpVol;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] float hurtVol;

    [Header("GunSpawnables")]
    [SerializeField] GameObject pistolSpawn;
    [SerializeField] GameObject tommySpawn;

    //private variables
    Vector3 playerVelocity;
    Vector3 move;
    bool isGrounded;
    int jumpTimes;
    int HPOrig;
    float origSpeed;
    Vector3 pushBack;
    int selectedGunNum = 0;
    GunSystem gunSystem;
    float timePassed;
    bool stepPlaying;
    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        if(pc == null) 
        {
            pc = this;
            HPOrig = HP;
            origSpeed = playerSpeed;
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
        if (updates.addTommy) 
        {
            Instantiate(tommySpawn, transform.position, transform.rotation);
        }
    }


    void movement()
    {
        //Check if player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if(playerVelocity.y < 0) 
            {
                jumpTimes = 0;
                playerVelocity.y = 0;
            }
            if(!stepPlaying && move.normalized.magnitude > 0.5f) 
            {
                StartCoroutine(playStepAud());
            }
        }

        //Set move vector for player movement
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        //Jump Input
        if (Input.GetButtonDown("Jump") && jumpTimes < maxJumpAmmount)
        {
            aud.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)], jumpVol);
            jumpTimes++;
            playerVelocity.y = jumpHeight;
            
        }

        //gravity
        playerVelocity.y -= gravityScale * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
    }

    IEnumerator playStepAud() 
    {
        stepPlaying = true;
        aud.PlayOneShot(footsteps[Random.Range(0,footsteps.Length)], footVol);
        if (isSprinting) 
        {
            yield return new WaitForSeconds(0.3f);
        }
        else 
        {
            yield return new WaitForSeconds(0.5f);
        }
        stepPlaying = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)], hurtVol);
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
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;
        HP = HPOrig;
        updatePlayerUI();
    }

    public void spawnPlayerOnLoad() 
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;
        
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
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
    public void ApplyStatusEffect(StatusEffectObj data)
    {
        if (data != null && activeEffect == null)
        {
            activeEffect = data;
            StartCoroutine(BurnEffect());
        }
    }

    public IEnumerator BurnEffect()
    {
        int effectTimer = activeEffect.duration;
        if (activeEffect.duration != 0)
        {
            if (activeEffect.slowEffect != 0)
            {
                playerSpeed /= activeEffect.slowEffect;
            }
            timePassed = Time.time;
            while (Time.time - timePassed <= effectTimer && activeEffect != null)
            {
                    if (activeEffect.damage != 0)
                    {
                        yield return new WaitForSeconds(activeEffect.damagespeed);
                        takeDamage(activeEffect.damage);
                    }
                    else
                    {
                        yield return new WaitForSeconds(activeEffect.duration);
                        RemoveEffect();
                    }
            }
            if (activeEffect != null)
            {
                RemoveEffect();
            }
        }
    }

    public void RemoveEffect()
    {
        activeEffect = null;
        timePassed = 0;
        playerSpeed = origSpeed;
    }

}
