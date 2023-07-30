using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPhysics, IStatusEffect
{
    #region Variables
    static PlayerController pc;

    [Header("- - - Componets - - -")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioSource musicAud;
    [SerializeField] Animator animator;
    [SerializeField] GameObject mainCam;
    [SerializeField] Transform playerCenterPos;

    [Header("- - - Atributes - - -")]
    [Range(1, 100)][SerializeField] float HP;
    [SerializeField][Range(1.0f, 10.0f)] float playerSpeed;
    [SerializeField][Range(1.5f, 5.0f)] float sprintMod;
    [SerializeField][Range(0.1f, 0.5f)] float crawlMod;
    [SerializeField][Range(1.0f, 20.0f)] float jumpHeight;
    [SerializeField][Range(5.0f, 30.0f)] float gravityScale;
    [SerializeField][Range(1, 4)] int maxJumpAmmount;
    [SerializeField] int interactDist;
    [SerializeField] float pushBackResolve;
    [SerializeField] int throwPower;
    [SerializeField] StatusEffectObj activeEffect;

    [Header("Audio")]
    [SerializeField] AudioClip[] footsteps;
    [SerializeField][Range(0f, 1f)] float footVol;
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField][Range(0f, 1f)] float jumpVol;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField][Range(0f, 1f)] float hurtVol;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField][Range(0f, 1f)] float deathVol;
    [SerializeField][Range(0f, 1f)] float musicVol;

    [Header("DamageOverlay")]
    public float duration;
    public float fadeSpeed;
    public bool respawnWithSameHealth;
    bool fadeout;

    [Header("Head Bonk Variables")]
    [SerializeField] LayerMask headIneraction;
    [SerializeField] float headBonkRayLengthJumping;
    [SerializeField] float headBonkRayLengthCrouching;

    [Header("Inventory and Equipment")]
    [SerializeField] GunStats[] equipedGuns = new GunStats[4];
    [SerializeField] InventoryItem[] inventory = new InventoryItem[20];

    //private variables
    Vector3 playerVelocity;
    Vector3 move;
    Vector3 pushBack;
    Color backHpOrig;
    bool isGrounded;
    bool isDead;
    bool stepPlaying;
    bool isSprinting;
    bool isCrawl;
    bool bonked;
    bool dontDamage;
    bool canJump;
    bool canMove;
    bool hasGun;
    int jumpTimes;
    int speedHash;
    int currWeaponsEquiped;
    int currItemCount;
    float HPOrig;
    float origSpeed;
    float timePassed;
    float lerpTimer;
    float playerSpeedOrig;
    float durationTimer;
    float speed;
    #endregion
    void Start()
    {
        if (MainMenuManager.instance != null)
        {
            musicVol = MainMenuManager.instance.musicVolume / 10f;
            footVol = MainMenuManager.instance.SFXVolume / 10f;
            jumpVol = MainMenuManager.instance.SFXVolume / 10f;
            hurtVol = MainMenuManager.instance.SFXVolume / 10f;
            deathVol = MainMenuManager.instance.SFXVolume / 10f;
        }
        if (pc == null)
        {
            pc = this;
            HPOrig = HP;
            origSpeed = playerSpeed;
            backHpOrig = GameManager.instance.backPlayerHPBar.color;
            playerSpeedOrig = playerSpeed;
            respawnWithSameHealth = false;
            spawnPlayer();
            animator = GetComponent<Animator>();
            speedHash = Animator.StringToHash("speed");
            canJump = true;
            canMove = true;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            if (canMove)
                movement();

            interact();

            if (!isCrawl)
                sprint();

            crawl();

            checks();

            DamageFlash();

            if (GameManager.instance.backPlayerHPBar.fillAmount != (float)HP / HPOrig || GameManager.instance.playerHPBar.fillAmount != (float)HP / HPOrig)
            {
                updatePlayerUI();
            }
        }
    }

    #region playerMovement
    void movement()
    {
        //Check if player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if (playerVelocity.y < 0)
            {
                jumpTimes = 0;
                playerVelocity.y = 0;
            }
            if (!stepPlaying && move.normalized.magnitude > 0.5f)
            {
                StartCoroutine(playStepAud());
            }
            bonked = false;
        }

        if (!isGrounded && HeadBonkDetection(headBonkRayLengthJumping) && !bonked)
        {
            playerVelocity.y = 0;
            bonked = true;
        }


        //Set move vector for player movement
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
        speed = Mathf.Lerp(speed, controller.velocity.normalized.magnitude, Time.deltaTime * playerSpeed);
        animator.SetFloat(speedHash, speed);

        //Jump Input
        if (Input.GetButtonDown("Jump") && canJump)
        {
            if (isGrounded)
            {
                jumpTimes = 0;
               
                aud.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)], jumpVol);
                jumpTimes++;
                playerVelocity.y = jumpHeight;
                bonked = false;
            }
            else if (jumpTimes < maxJumpAmmount)
            {
                aud.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)], jumpVol);
                jumpTimes++;
                playerVelocity.y = jumpHeight;
                bonked = false;
            }
        }

        //gravity
        playerVelocity.y -= gravityScale * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
    }

    bool HeadBonkDetection(float headBonkLength)
    {
        RaycastHit headBonk;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.up, out headBonk, headBonkLength, headIneraction))
        {
            return true;
        }
        return false;

    }
    void crawl()
    {
        if (Input.GetButtonDown("Crawl"))
        {
            isSprinting = false;
            isCrawl = true;
            playerSpeed = playerSpeedOrig;
            playerSpeed = playerSpeedOrig * crawlMod;
            GetComponent<CapsuleCollider>().height = 1;
            controller.height = 1;
        }
        else if (isCrawl && !Input.GetButton("Crawl") && !HeadBonkDetection(headBonkRayLengthCrouching))
        {
            isCrawl = false;
            playerSpeed /= crawlMod;
            GetComponent<CapsuleCollider>().height = 2;
            controller.height = 2;
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && !isCrawl)
        {
            playerSpeed = playerSpeedOrig * sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }
    #endregion

    #region Music stuff
    public void SetMusic(AudioClip song)
    {
        musicAud.clip = song;
        musicAud.volume = 0;
        StartCoroutine(musicFadeIn());
        musicAud.Play();
        musicAud.loop = true;
    }

    public void fadeOut()
    {
        StartCoroutine(musicFade());
    }

    IEnumerator musicFade()
    {
        while (musicAud.volume > 0)
        {
            musicAud.volume -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator musicFadeIn()
    {
        while (musicAud.volume < musicVol)
        {
            musicAud.volume += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator playStepAud()
    {
        stepPlaying = true;
        aud.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], footVol);
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

    public void UpdateSFX(float newVol)
    {
        footVol = newVol / 10f;
        jumpVol = newVol / 10f;
        hurtVol = newVol / 10f;
        deathVol = newVol / 10f;
    }

    public void UpdateMusic(float newVol)
    {
        musicVol = newVol / 10f;
        musicAud.volume = musicVol;
    }
    #endregion

    #region damage
    public void takeDamage(float amount, Transform pos = null)
    {
        if (isDead || dontDamage)
        {
            return;
        }
        HP -= amount;
        aud.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)], hurtVol);
        lerpTimer = 0f;
        durationTimer = 0f;
        updatePlayerUI();
        GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, 1);
        GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, 1);

        StartCoroutine(Invincibility());
        if (HP <= 0)
        {
            aud.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)], deathVol);
            pushBack = Vector3.zero;
            StartCoroutine(GameManager.instance.DeathSequence());
            isDead = true;
        }
 
    }
    public void instaKill()
    {
        takeDamage(HP);
    }

    #endregion

    #region invinciblity
    public void Invincible(bool isInvincible)
    {
        controller.detectCollisions = !isInvincible;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = !isInvincible;
    }
    IEnumerator Invincibility()
    {
        dontDamage = true;
        yield return new WaitForSeconds(1f);
        dontDamage = false;
    }
    #endregion

    #region spawning player
    public void spawnPlayer()
    {
        Invincible(true);
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;
        isDead = false;
        if (!respawnWithSameHealth)
        {
            HP = HPOrig;
        }
        pushBack = Vector3.zero;
        updatePlayerUI();
        GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, 0);
        GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, 0);
        Invincible(false);
    }


    public void spawnPlayerOnLoad()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;
        isDead = false;
    }
    #endregion

    #region gameSystems

    public bool addGunToEquiped(GunStats gunToAdd) 
    {
        if (currWeaponsEquiped < equipedGuns.Length) 
        {
            equipedGuns[currWeaponsEquiped] = gunToAdd;
            currWeaponsEquiped++;
            return true;
        }
        return false;
    }

    /* turnGunIntoInventory
     * Takes 1 argument (GunStats) of the gun that needs to be turned into an InventoryItem class
     * If the player has room in their inventory, then the gunStats will be setup as a new InventoryItem and
     * added to the inventory and the function will return true, otherwise the function will return false.
     */
    public bool turnGunIntoInventory(GunStats gunToConvert) 
    {
        if (currItemCount < inventory.Length) 
        {
            InventoryItem item = new();
            item.type = InventoryItem.itemType.Weapon;
            item.toolTip = gunToConvert.gunToolTip;
            item.name = gunToConvert.gunName;
            item.icon = gunToConvert.gunSprite;
            inventory[currItemCount] = item;
            currItemCount++;
            return true;
        }
        return false;
    }

    /* turnGunIntoInventory
    * Takes 1 argument (ConsumableType) of the consumable that needs to be turned into an InventoryItem class
    * If the player has room in their inventory, then the consimable will be setup as a new InventoryItem and
    * added to the inventory and the function will return true, otherwise the function will return false.
    */
    public bool turnConsumableIntoInventory() 
    {
        if (currItemCount < inventory.Length)
        {

            return true;
        }
        return false;
    }
    #region Setters
    public void speedChange(float amount)
    {
        playerSpeed += amount;
    }
    public void TakePush(Vector3 dir)
    {
        pushBack += dir;
    }

    public void setJumpMode(bool canjump) 
    {
        canJump = canjump;
    }

    public void setControl(bool controlEnabled) 
    {
        canMove = controlEnabled;
    }
    #endregion

    void interact()
    {
        RaycastHit hit;
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

    public bool RemoveGun(GunStats gtr)
    {
        return false;
    }


    #endregion

    #region getters

    public int GetPlayerHP()
    {
        return (int)HP;
    }
    public float GetMaxHP()
    {
        return HPOrig;
    }
    public bool playerGrounded()
    {
        return isGrounded;
    }
    public int GetThrowPower()
    {
        return throwPower * 10;
    }
    public float GetMusicVol()
    {
        return musicVol;
    }
    public float GetJumpVol()
    {
        return jumpVol;
    }
    public CharacterController GetCharacterController()
    {
        return controller;
    }
    public Transform GetPlayerCenter()
    {
        return playerCenterPos;
    }
    public void SetisDead(bool boolean)
    {
        isDead = boolean;
    }

    #endregion

    #region health
    public void playerHeal(int amount)
    {
        HP += amount;
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        lerpTimer = 0f;
        updatePlayerUI();
    }
    public void fullHeal()
    {
        HP = 100;
        HPOrig = 100;
        updatePlayerUI();
    }
    #endregion

    #region effects
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
        }
    }

    public void RemoveEffect()
    {
        StopCoroutine(BurnEffect());
        activeEffect = null;
        timePassed = 0;
        if (isSprinting)
        {
            playerSpeed = playerSpeedOrig * sprintMod;
        }
        else
        {
            playerSpeed = origSpeed;
        }
    }
    #endregion

    public void updatePlayerUI()
    {
        float backfill = GameManager.instance.backPlayerHPBar.fillAmount;
        float frontfill = GameManager.instance.playerHPBar.fillAmount;
        float currHealth = (float)HP / HPOrig;
        GameManager.instance.HPDisplay.text = HP.ToString();

        lerpTimer += Time.deltaTime;
        float delayBarSpeed = lerpTimer / 2f;
        delayBarSpeed = delayBarSpeed * delayBarSpeed;
        if (backfill > currHealth)
        {
            GameManager.instance.playerHPBar.fillAmount = currHealth;
            GameManager.instance.backPlayerHPBar.color = backHpOrig;
            GameManager.instance.backPlayerHPBar.fillAmount = Mathf.Lerp(backfill, currHealth, delayBarSpeed);
        }
        else if (frontfill < currHealth && HPOrig != 0)
        {
            GameManager.instance.backPlayerHPBar.color = Color.green;
            GameManager.instance.backPlayerHPBar.fillAmount = currHealth;
            GameManager.instance.playerHPBar.fillAmount = Mathf.Lerp(frontfill, currHealth, delayBarSpeed);
        }
    }
    public void shopRegister(ShopPickup updates)
    {
        if (updates.ar)
        {
           
        }
        if (updates.shotgun)
        {
            
        }
        if (updates.fullheal)
        {
            fullHeal();
        }
        if (updates.speed)
        {
            speedChange(1.5f);
        }
    }
    void DamageFlash()
    {
        if (GameManager.instance.damagePanel.color.a > 0 || HP <= 10)
        {
            float panelAlpha = GameManager.instance.damagePanel.color.a;
            float bloodAlpha = GameManager.instance.damageBlood.color.a;
            durationTimer += Time.deltaTime;
            if (HP <= 10)
            {
                if (panelAlpha >= 2)
                {
                    fadeout = true;
                    panelAlpha = 2;
                }
                if (fadeout == true)
                {
                    fadeSpeed = 2f;
                    panelAlpha -= Time.deltaTime * fadeSpeed;
                    bloodAlpha -= Time.deltaTime * fadeSpeed;
                    GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, panelAlpha);
                    GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, panelAlpha);
                    if (panelAlpha <= 0)
                    {
                        fadeout = false;
                    }
                }
                if (fadeout == false)
                {
                    fadeSpeed = 2f;
                    panelAlpha += Time.deltaTime * fadeSpeed;
                    bloodAlpha += Time.deltaTime * fadeSpeed;
                    GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, panelAlpha);
                    GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, panelAlpha);
                }
                return;
            }
            else if (durationTimer > duration)
            {
                fadeSpeed = 7f;
                panelAlpha -= Time.deltaTime * fadeSpeed;
                bloodAlpha -= Time.deltaTime * fadeSpeed;
                GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, panelAlpha);
                GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, panelAlpha);
            }
        }
    }
    void checks()
    {
        if (!isSprinting && !isCrawl && activeEffect == null)
            playerSpeed = playerSpeedOrig;

        if (transform.position.y < -10 || (GameManager.instance.playerAmmo <= 0 && hasGun))
            instaKill();

        DamageFlash();
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyAI agent = collision.GetComponent<EnemyAI>();
            float dis = Vector3.Distance(agent.GetEnemyHeadHeight(), transform.position);
            if (dis < 1.2f && isGrounded && agent.GetAgent().isActiveAndEnabled)
            {
                TakePush(new Vector3(.5f, 0, .5f));
            }
        }
    }

}
