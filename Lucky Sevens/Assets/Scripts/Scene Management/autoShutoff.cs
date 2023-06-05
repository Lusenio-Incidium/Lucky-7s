using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoShutoff : MonoBehaviour
{
    [SerializeField] bool refresh;

    private void Update()
    {
       if(refresh == true) 
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.unPauseState();
                refresh = false;
                this.enabled = false;
            }
        }
        
    }
}
