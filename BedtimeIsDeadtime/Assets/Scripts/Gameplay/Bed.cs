using System;
using AI;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class Bed : MonoBehaviour
    {
        #region Variables


        private int _sanity;

        private AudioSource _audioSource;
        public AudioClip scream;


        private void Awake()
        {
            Game.Bed = this;
        }

        private void Start()
        {
            _sanity = 100;
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion

        

        public void ReduceSanity(int amount)
        {
            _sanity -= amount;

            Game.Hud.sanity.value = _sanity;
            
            if (_sanity <= 0)
            {
                _audioSource.PlayOneShot(scream);
                Game.Controller.isGameOver = true;
                Game.Controller.GameOver();
            }
            
        }
        
        public void AddSanity(int value)
        {
            _sanity += value;

            if (_sanity > Game.Hud.sanity.maxValue) _sanity = (int)Game.Hud.sanity.maxValue;
            Game.Hud.sanity.value = _sanity;
        }
        
        public void AddSanity(Transform enemy)
        {
            AI.EnemyBehaviour enemyScript = enemy.GetComponent<EnemyBehaviour>();

            if (!enemyScript) return;
            _sanity += enemyScript.rewardedSanity;

            if (_sanity > Game.Hud.sanity.maxValue) _sanity = (int)Game.Hud.sanity.maxValue;
            Game.Hud.sanity.value = _sanity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }
            
            var enemy = other.GetComponent<EnemyBehaviour>();


            enemy.SetIsAttacking(true);

            if (Game.Controller.isGameOver)
            {
                return;
            }
            
            StartCoroutine(enemy.ApplySanity(this));
        }




        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }
            
            other.GetComponent<EnemyBehaviour>().SetIsAttacking(false);
        }
    }
}