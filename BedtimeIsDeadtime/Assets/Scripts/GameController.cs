using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Interface;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int currentWave;

    public float speedMultiplier;
    public int spawnMultiplier;
    public GameObject gameOverScreen;
    public HUD hud;
    
    [SerializeField] public Spawner _spawner;

    public bool isGameOver;

    private void Awake()
    {
        Game.Controller = this;
    }

    private void Start()
    {
        isGameOver = false;
        currentWave = 0;
        NextWave();
    }
    


    public void NextWave()
    {
        currentWave++;
        hud.WaveCount();
        StartCoroutine(_spawner.Spawn(currentWave * spawnMultiplier));
    }
    

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        GameObject.Destroy(Game.Player);
        foreach (var enemy in EnemyTracker.Enemies)
        {
            enemy.Stop(true);
        }
    }
}