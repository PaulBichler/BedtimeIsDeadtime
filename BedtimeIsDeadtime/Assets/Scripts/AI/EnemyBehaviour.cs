using System;
using System.Collections;
using Gameplay;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public enum Target
    {
        Bed,
        Player
    }

    [RequireComponent(typeof(AudioSource), typeof(Seeker), typeof(Rigidbody2D))]
    public class EnemyBehaviour : MonoBehaviour
    {
        public Transform target;
        public Target targetType;
        public int weighting;
        public int speed;
        public float nextWayPointDistance;
        protected Path Path;
        protected int CurrentWayPoint = 0;
        protected bool ReachedEndOfPath = false;
        protected Seeker Seeker;
        protected Rigidbody2D Rb;
        public string enemyName;
        public int currentWaveIndex;
        public GameObject cloudPrefab;
        public GameObject sanityPickupPrefab;

        //Audio
        protected AudioSource AudioSource;
        public AudioClip[] dieSounds;
        public AudioClip[] attackSound;

        private bool _stop;

        private bool IsAttacking { get; set; }
        public float attackTime;
        public int damage;
        public int rewardedPower;
        public int rewardedSanity;
        public float scaleDecreaseFactor;
        public float deathScale;

        public int difficultyLevel;

        protected void UpdatePath()
        {
            if (Seeker.IsDone() && GetComponent<Seeker>() != null)
            {
                this.Seeker.StartPath(Rb.position, target.position, OnPathCompleted);
            }
        }

        protected void OnPathCompleted(Path p)
        {
            if (!p.error)
            {
                this.Path = p;
                CurrentWayPoint = 0;
            }
        }


        private IEnumerator _coroutine;


        public void SetIsAttacking(bool state)
        {
            IsAttacking = state;
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public virtual void Stop(bool gameOver = false)
        {
            _stop = true;
            
            Seeker.CancelCurrentPathRequest(true);
            CancelInvoke(nameof(UpdatePath));
            StopCoroutine(nameof(ApplySanity));
            int dice = Random.Range(1, 20);

            if (cloudPrefab && !gameOver)
            {
                var cloud = Instantiate(cloudPrefab.GetComponent<CloudBehaviour>(), transform.position, Quaternion.identity);
                
                if (dieSounds.Length != 0)
                {
                    cloud.PlaySound(dieSounds[Random.Range(0, dieSounds.Length)]);
                }
            }

            if (sanityPickupPrefab && dice == 1)
            {
                Instantiate(sanityPickupPrefab, transform.position, Quaternion.identity);
            }

            EnemyTracker.RemoveEnemy(currentWaveIndex);
        }


        protected virtual void Start()
        {
            this.tag = "Enemy";
            this.Seeker = GetComponent<Seeker>();
            this.Rb = GetComponent<Rigidbody2D>();

            this.Rb.gravityScale = 0;
            _stop = false;
            AudioSource = GetComponent<AudioSource>();

            switch (targetType)
            {
                case Target.Bed:
                    target = Game.Bed.transform;
                    break;
                case Target.Player:
                    target = Game.Player.transform;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            InvokeRepeating(nameof(UpdatePath), 0f, .5f);
            this.Seeker.StartPath(Rb.position, this.target.position, OnPathCompleted);
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Game.Player.GetComponent<BoxCollider2D>());
        }

        protected virtual void FixedUpdate()
        {
            if (this.Path == null)
                return;
            if (CurrentWayPoint >= Path.vectorPath.Count)
            {
                ReachedEndOfPath = true;
                return;
            }
            else
            {
                ReachedEndOfPath = false;
            }

            var direction = ((Vector2) this.Path.vectorPath[CurrentWayPoint] - Rb.position).normalized;

            GetComponent<SpriteRenderer>().flipX = (transform.position.x > target.transform.position.x);


            var force = speed * Time.deltaTime * direction;

            this.Rb.AddForce(force);

            var distance = Vector2.Distance(Rb.position, Path.vectorPath[CurrentWayPoint]);

            if (distance < nextWayPointDistance)
            {
                CurrentWayPoint++;
            }
        }
        
        public IEnumerator ApplySanity(Bed bed)
        {
            while (IsAttacking && !Game.Controller.isGameOver && !_stop)
            {
                bed.ReduceSanity(damage);

                if (!AudioSource.isPlaying && attackSound.Length != 0)
                {
                    AudioSource.PlayOneShot(attackSound[Random.Range(0, attackSound.Length)]);
                }

                yield return new WaitForSeconds(attackTime);
            }
        }
    }
}