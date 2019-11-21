using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI;

public class FieldOfView : MonoBehaviour
{
    private PlayerBehavior playerScript;
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public float dps;

    public LayerMask targetMask;

    [HideInInspector]
    public List<Transform> hitTargets = new List<Transform>();
    private Camera _camera1;

    void Start()
    {
        _camera1 = Camera.main;
        playerScript = GetComponentInParent<PlayerBehavior>();
        StartCoroutine(nameof(FindTargetsWithDelay), dps);
    }

    private void Update()
    {
        if (playerScript._paralyzed <= 0)
        {
            FaceMouse();
        }
    }
    
    private void FaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        if (_camera1 != null) mousePos = _camera1.ScreenToWorldPoint(mousePos);

        var transform1 = transform;
        var position = transform1.position;
        Vector2 direction = new Vector2(mousePos.x - position.x, mousePos.y - position.y);
        transform1.up = direction;
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets ();
        }
    }

    void FindVisibleTargets() {
        hitTargets.Clear ();
        Collider2D[] targetsInHitRadius = Physics2D.OverlapCircleAll(transform.position, -1 * viewRadius, targetMask);

        for (int i = 0; i < targetsInHitRadius.Length; i++) 
        {
            if (!targetsInHitRadius[i].CompareTag("Enemy"))
            {
                continue;
            }
			
            Transform target = targetsInHitRadius[i].transform;
            EnemyBehaviour targetScript = target.GetComponent<EnemyBehaviour>();
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2) 
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, targetMask)) {
                    hitTargets.Add(target);
                    if (target.localScale.x > targetScript.deathScale)
                    {
                        target.localScale -= new Vector3(targetScript.scaleDecreaseFactor, targetScript.scaleDecreaseFactor, 0f);
                    } 
                    else
                    {
                        if(!playerScript.isUltimateInUse) playerScript.AddPower(target);
                        target.GetComponent<EnemyBehaviour>().Stop();
                        
                        Destroy(target.gameObject);
                    }
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}