using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGameController : MonoBehaviour
{
    [System.Serializable]
    public class TargetGame 
    {
        public int ID;
        public List<GameObject> targets;
        public int ammoReward;
        public bool gameWon;
    }

    [SerializeField] TargetGame[] games;
    [SerializeField] GameObject[] affectedObjects;
    public void onTargetDeath(GameObject target, int game) 
    {
        for(int i = 0; i < games[game-1].targets.Count; i++) 
        {
            if(games[game - 1].targets[i] == target) 
            {
                games[game - 1].targets.Remove(target);
            }
        }
        if(games[game - 1].targets.Count <= 0) 
        {
            games[game - 1].gameWon = true;
            //GameManager.instance.gunSystem.AddBullets(games[game - 1].ammoReward);
            checkGameStatus();
        }

    }

    void checkGameStatus() 
    {
        int gamesWon = 0;
        for(int i = 0; i < games.Length; i++) 
        {
            if (games[i].gameWon)
                gamesWon++;
        }
        if(gamesWon >= 6) 
        {
            foreach(GameObject triggered in affectedObjects) 
            {
                IMinigame minigame = triggered.GetComponent<IMinigame>();
                if(minigame != null) 
                {
                    minigame.onWin();
                }
            }
        }
    }
    
}
