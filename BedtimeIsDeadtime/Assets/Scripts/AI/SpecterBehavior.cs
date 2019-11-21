using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class SpecterBehavior : EnemyBehaviour
{
    public float minTimeToNextTeleport = 3f;
    public float maxTimeToNextTeleport = 5f;
    public float teleportRange = 3f;
    public float lerpSpeed = 2f;
    public float lerpDuration = 2f;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine("Teleport");
    }

    IEnumerator Teleport()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minTimeToNextTeleport, maxTimeToNextTeleport));
            Vector2 newPos = GetRandomPositionInRange();
            yield return LerpPosition(transform.position, newPos, lerpDuration);
        }
    }

    IEnumerator LerpPosition(Vector2 startPos, Vector2 endPos, float time)
    {
        float i = 0f;
        float rate = (1f / time) * lerpSpeed;

        while (i < 1f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector2.Lerp(startPos, endPos, i);
            yield return null;
        }

        yield return null;
    }

    Vector2 GetRandomPositionInRange()
    {
        Vector2 randomPos = Random.insideUnitCircle * teleportRange;

        while (Vector2.Distance(randomPos, Game.Bed.transform.position) > Vector2.Distance(transform.position, Game.Bed.transform.position))
        {
            randomPos = Random.insideUnitCircle * teleportRange;
        }

        return randomPos;
    }
}
