using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool start;
    [SerializeField] float dur;
    [SerializeField] float strength;
    private void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            if (start)
            {
                start = false;
                StartCoroutine(Shake(dur));
            }
        }
    }
    public IEnumerator Shake(float duration)
    {
        Vector3 originalVec = transform.localEulerAngles;
        float timePassed = duration;
        while (timePassed > 0)
        {
            timePassed -= Time.deltaTime;
            float yawToAdd = ReturnNumber() * (timePassed/duration) * strength;
            float pitchToAdd = ReturnNumber() * (timePassed / duration) * strength;
            float rollToAdd = ReturnNumber() * (timePassed / duration) * strength;
            transform.localEulerAngles = new Vector3(originalVec.x + pitchToAdd, originalVec.y + yawToAdd, originalVec.z + rollToAdd);
            yield return null;
        }
    }
    float ReturnNumber()
    {
        return Random.Range(-1f, 1f);
    }
    public void SetDuration(float duration)
    {
        dur = duration;
    }
    public void SetStrengthAmount(float _strength)
    {
        strength = _strength;
    }
}
