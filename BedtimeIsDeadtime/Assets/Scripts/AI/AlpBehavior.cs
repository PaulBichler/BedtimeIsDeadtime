using AI;
using UnityEngine;

public class AlpBehavior : EnemyBehaviour
{
    public int minNmbrBats = 2;
    public int maxNmbrBats = 4;
    public GameObject batPrefab;
    
    public override void Stop(bool gameOver = false)
    {
        base.Stop();
        
        if (batPrefab)
        {
            for (int i = 0; i < Random.Range(minNmbrBats, maxNmbrBats); i++)
            {
                Instantiate(batPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}