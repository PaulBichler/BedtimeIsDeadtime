using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.9f;
    public float maxIntensity = 1.1f;
    public float minAddedValue = -0.1f;
    public float maxAddedValue = 0.1f;
    public float minFlickerRate = 0.1f;
    public float maxFlickerRate = 0.5f;

    private Light2D lightSource;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        lightSource = GetComponent<Light2D>();
        
        while (true)
        {
            if (lightSource.intensity < minIntensity)
            {
                lightSource.intensity = minIntensity;
            } 
            else if (lightSource.intensity > maxIntensity)
            {
                lightSource.intensity = maxIntensity;
            }
            else
            {
                lightSource.intensity += Random.Range(minAddedValue, maxAddedValue);
            }
            yield return new WaitForSecondsRealtime(Random.Range(minFlickerRate, maxFlickerRate));
        }
    }

    
}
