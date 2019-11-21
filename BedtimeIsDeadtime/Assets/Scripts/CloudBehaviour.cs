using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour
{
    private float lifespan = 0.6f;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.pitch = Random.Range(1f, 2f);
        _audioSource.PlayOneShot(clip);
    }
    
    private void Update()
    {
        if ((lifespan -= Time.deltaTime) < 0)
        {
            Destroy(gameObject);
        }
    }
}