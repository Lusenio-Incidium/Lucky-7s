using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IPhysics, IStatusEffect
{
    static PlayerController pc;

    [Header("- - - Componets - - -")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioSource musicAud;
    [SerializeField] Animator animator;
    [SerializeField] GameObject mainCam;

    [Header("- - - Atributes - - -")]
    public List<GunStats> gunList = new List<GunStats>();

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
    [SerializeField][Range(0f,1f)] float footVol;
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField][Range(0f, 1f)] float jumpVol;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField][Range(0f, 1f)] float hurtVol;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField][Range(0f, 1f)] float deathVol;
    [SerializeField][Range(0f, 1f)] float musicVol;

    [Header("GunSpawnables")]
    [SerializeField] GameObject ShotgunSpawn;
    [SerializeField] GameObject tommySpawn;

    [Header("DamageOverlay")]
    public float duration;
    public float fadeSpeed;
    bool fadeout;

    //private variables
    GunSystem gunSystem;
    Vector3 playerVelocity;
    Vector3 move;
    Vector3 pushBack;
    Color backHpOrig;
    bool isGrounded;
    bool isDead;
    bool stepPlaying;
    bool isSprinting;
    bool isCrawl;
    int jumpTimes;
    int selectedGunNum;
    int speedHash;
    float HPOrig;
    float origSpeed;
    float timePassed;
    float lerpTimer;
    float playerSpeedOrig;
    float durationTimer;
    float speed;
    bool ceilingCheck;

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
            selectedGunNum = 0;
            origSpeed = playerSpeed;
            backHpOrig = GameManager.instance.backPlayerHPBar.color;
            playerSpeedOrig = playerSpeed;
            spawnPlayer();
            animator = GetComponent<Animator>();
            speedHash = Animator.StringToHash("speed");
            gunSystem = GetComponent<GunSystem>();
        }
        else 
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if(GameManager.instance.didRestart == true)
        {
            HP = 50;
            GameManager.instance.didRestart = false;
        }
        
        
    }

    void Update()
    {
        movement();
        if(!GameManager.instance.isPaused)
            interact();
        if(!isCrawl)
            sprint();

        crawl();

        checks();

        if(GameManager.instance.backPlayerHPBar.fillAmount != (float) HP / HPOrig || GameManager.instance.playerHPBar.fillAmount != (float)HP / HPOrig)
        {
            updatePlayerUI();
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
        }

        if(ceilingCheck && !isGrounded) 
        {
            TakePush(new Vector3(0, -10, 0));
        }


        //Set move vector for player movement
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
        speed = Mathf.Lerp(speed, controller.velocity.normalized.magnitude, Time.deltaTime * playerSpeed);
        animator.SetFloat(speedHash, speed);

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
    void crawl()
    {
        if (Input.GetButtonDown("Crawl"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
            playerSpeed = playerSpeedOrig * crawlMod;
            GetComponent<CapsuleCollider>().height = 1;
            controller.height = 1;
        }
        else if (Input.GetButtonUp("Crawl") && !ceilingCheck)
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
        HP -= amount;
        aud.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)], hurtVol);
        lerpTimer = 0f;
        durationTimer = 0f;
        updatePlayerUI();
        GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, 1);
        GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, 1);
        
        StartCoroutine(Invincibility());
        if (HP <= 0 && !isDead)
        {
            aud.PlayOneShot(deathSounds[Random.Range(0,deathSounds.Length)], deathVol);
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
        controller.detectCollisions = false;
        yield return new WaitForSeconds(1f);
        controller.detectCollisions = true;
    }
    #endregion

    #region spawning player
    public void spawnPlayer()
    {
        isDead = false;
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;
        HP = HPOrig;
        pushBack = Vector3.zero;
        updatePlayerUI();
        GameManager.instance.damagePanel.color = new Color(GameManager.instance.damagePanel.color.r, GameManager.instance.damagePanel.color.g, GameManager.instance.damagePanel.color.b, 0);
        GameManager.instance.damageBlood.color = new Color(GameManager.instance.damageBlood.color.r, GameManager.instance.damageBlood.color.g, GameManager.instance.damageBlood.color.b, 0);
    }


    public void spawnPlayerOnLoad() 
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        controller.enabled = true;

    }
    #endregion

    #region gameSystems
    public void speedChange(float amount)
    {
        playerSpeed += amount;
    }
    public void TakePush(Vector3 dir)
    {
        pushBack += dir;
    }
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
                    StopAllCoroutines();
                    RemoveEffect();
                }
            }
            /*    if (activeEffect != null)
                {
                    RemoveEffect();
                }*/
        }
    }

    public void RemoveEffect()
    {
        activeEffect = null;
        timePassed = 0;
        playerSpeed = origSpeed;
    }
    #endregion

    public void updatePlayerUI()
    {
        float backfill = GameManager.instance.backPlayerHPBar.fillAmount;
        float frontfill = GameManager.instance.playerHPBar.fillAmount;
        float currHealth = (float) HP / HPOrig;
        GameManager.instance.HPDisplay.text = HP.ToString();
         
        lerpTimer += Time.deltaTime;
        float delayBarSpeed = lerpTimer / 2f;
        delayBarSpeed = delayBarSpeed * delayBarSpeed;
        if(backfill > currHealth)
        {
            GameManager.instance.playerHPBar.fillAmount = currHealth;
            GameManager.instance.backPlayerHPBar.color = backHpOrig;
            GameManager.instance.backPlayerHPBar.fillAmount = Mathf.Lerp(backfill,currHealth,delayBarSpeed);
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
            Instantiate(tommySpawn, transform.position, transform.rotation);
        }
        if (updates.shotgun)
        {
            Instantiate(ShotgunSpawn, transform.position, transform.rotation);
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
        //damage flash
        if (GameManager.instance.damagePanel.color.a > 0)
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
        if (!isSprinting && !isCrawl)
            playerSpeed = playerSpeedOrig;

        if (transform.position.y < -10)
            instaKill();

        if (gunList.Count > 0)
            switchGun();
        DamageFlash();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CelingColidable"))
            ceilingCheck = true;
    }

    private void OnTriggerExit(Collider other)
    {
            ceilingCheck = false;
    }

}
