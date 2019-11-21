using System.Collections;
using UnityEngine;
using Pathfinding;

namespace AI
{
    public class BatibatBehavior : EnemyBehaviour
    {
        public int jumpDistance = 2;
        public float paralyzeCooldown = 0f;
        public float paralyzeTime = 0.3f;

        private Vector2 _previousPosition;

        private void JumpPlayer()
        {
            if (!AudioSource.isPlaying && attackSound.Length != 0)
            {
                AudioSource.PlayOneShot(attackSound[Random.Range(0, attackSound.Length)]);
            }
            
            _previousPosition = transform.position;
            GetComponent<BoxCollider2D>().isTrigger = true;
            
            Rb.position = target.position;
            target.GetComponentInParent<PlayerBehavior>().Paralyze(paralyzeTime);
            paralyzeCooldown = 3.5f;

            StartCoroutine(ResetBack(1f));
        }

        protected override void FixedUpdate()
        {
            if (transform.localScale.x <= 0.4f)
            {
                return;
            }
            
            if (paralyzeCooldown <= 0)
            {
                base.FixedUpdate();

                if (Vector2.Distance(Rb.position, target.position) < jumpDistance)
                {
                    JumpPlayer();
                }
            }
            else
            {
                if ((paralyzeCooldown -= Time.deltaTime) < 0)
                {
                    paralyzeCooldown = 0;
                }
            }
        }

        private IEnumerator ResetBack(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            Rb.position = _previousPosition;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}