using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementSystem : MonoBehaviour
{
    [SerializeField] int trophyID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameJolt.API.Trophies.TryUnlock(trophyID, (GameJolt.API.TryUnlockResult result) => {
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
}
