using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Challanges 
{
    PistolOnly,
    NoHit,
    NoDeath
}
public class AchivementSystem : MonoBehaviour
{
    [SerializeField] int normalID;
    [SerializeField] int hardID;
    [SerializeField] int challangeID1;
    [SerializeField] int challangeID2;
    [SerializeField] int challangeID3;
    [SerializeField] bool compleateLevel;
    [SerializeField] bool hasChalange;
    [SerializeField] bool hardMode;
    [SerializeField] Challanges chalange1;
    [SerializeField] Challanges chalange2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if(GameJolt.API.GameJoltAPI.Instance.CurrentUser != null) 
            {
                if (compleateLevel)
                    giveAchivement(normalID);
                if (hardMode && compleateLevel && GameManager.instance.hardMode)
                    giveAchivement(hardID);

            }
        }
    }

    void giveAchivement(int id) 
    {
        GameJolt.API.Trophies.TryUnlock(id, (GameJolt.API.TryUnlockResult result) => {
            switch (result)
            {
                case GameJolt.API.TryUnlockResult.Unlocked:
                    break;
                case GameJolt.API.TryUnlockResult.AlreadyUnlocked:
                    break;
                case GameJolt.API.TryUnlockResult.Failure:
                    break;
            }
        });
    }
}
