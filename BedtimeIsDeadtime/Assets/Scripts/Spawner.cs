using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AI;
using Gameplay;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Transform[] _spawnPositions;

    private readonly Dictionary<EnemyBehaviour,int> _enemies = new Dictionary<EnemyBehaviour, int>();

    public Transform enemyContainer;

    public float spawnRate;

    private void Awake()
    {
        _spawnPositions = new Transform[transform.childCount];
        
        for (var i = 0; i < transform.childCount; i++)
        {
            _spawnPositions[i] = transform.GetChild(i);
        }

        var enemies = Resources.LoadAll<EnemyBehaviour>("Prefabs/Enemies");
        
        Parallel.ForEach(enemies, (enemy) =>
        {
            _enemies.Add(enemy,enemy.weighting);
        });
    }
    
    public IEnumerator Spawn(int amount)
    {
        EnemyTracker.InitializeNewWave(amount);
//        Game.Hud.remainingEnemies.text = "Remaining enemies: " + amount; 
        
        
        for (var i = 0; i < amount; i++)
        {
            var spawnIndex = Random.Range(0, _spawnPositions.Length);
            
            yield return new WaitForSeconds(1 / Game.Controller.currentWave);
            
            var enemy = Instantiate
            (
                WeightedRandomizer.From(_enemies).TakeOne(), 
                _spawnPositions[spawnIndex].position, 
                Quaternion.identity,
                enemyContainer
            );
            
            enemy.name = enemy.enemyName;
            enemy.currentWaveIndex = i;
            
            EnemyTracker.Enemies[i] = enemy;
            
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
