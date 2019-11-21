using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UIElements;

public class RandomLightning : MonoBehaviour
{
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;
    public float minLightningDuration = 0.2f;
    public float maxLightningDuration = 0.8f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.1f;
    public SpriteRenderer backgroundRef;
    public Sprite normalBg;
    public Sprite lightningBg;
    public List<AudioClip> thunderClips = new List<AudioClip>();

    private AudioSource audioComp;
    private Light2D lightComp;
    private float oldIntensity;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        audioComp = GetComponent<AudioSource>();
        lightComp = GetComponent<Light2D>();
        oldIntensity = lightComp.intensity;
        
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minWaitTime, maxWaitTime));
            yield return Lightning(Random.Range(minLightningDuration, maxLightningDuration));
        }
    }

    IEnumerator Lightning(float duration)
    {
        while (duration > 0)
        {
            lightComp.intensity = Random.Range(minIntensity, maxIntensity);
            float durationToNext = Random.Range(0.2f, 0.4f);
            duration -= durationToNext;
            if (backgroundRef) backgroundRef.sprite = lightningBg;
            if(audioComp) audioComp.PlayOneShot(thunderClips[Random.Range(0, thunderClips.Count)]);
            yield return new WaitForSecondsRealtime(durationToNext);
            lightComp.intensity = oldIntensity;
            if (backgroundRef) backgroundRef.sprite = normalBg;
        }

        yield return null;
    }
}
