using AI;
using UnityEngine;

public static class EnemyTracker
{
    public static int RemainingEnemies;
    
    public static EnemyBehaviour[] Enemies;
    
    public static void InitializeNewWave(int amount)
    {
        RemainingEnemies = amount;
        Enemies = new EnemyBehaviour[amount];
    }

    public static void RemoveEnemy(int index)
    {
        if (Enemies[index] == null)
        {
            return;
        }
        
        RemainingEnemies--;

        if (RemainingEnemies == 0 && !Game.Controller.isGameOver)
        {
            Game.Controller.NextWave();
        }
    }
}