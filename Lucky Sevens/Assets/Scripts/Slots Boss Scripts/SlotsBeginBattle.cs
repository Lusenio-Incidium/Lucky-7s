using UnityEngine;

public class SlotsBeginBattle : MonoBehaviour
{
    bool hit = false;
    [SerializeField] GameObject DestroyOnBegin;
    [SerializeField] GameObject ArenaLight;
    [SerializeField] Animator startAnimation;
    [SerializeField] Vector3 newSpawnPos;
    [SerializeField] AudioClip bossMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        Destroy(DestroyOnBegin);
        ArenaLight.SetActive(true);
        startAnimation.SetTrigger("CloseArena");
        BossManager.instance.onBossStart();
        GameManager.instance.playerScript.SetMusic(bossMusic,MainMenuManager.instance.musicVolume / 10f);
    }
}
