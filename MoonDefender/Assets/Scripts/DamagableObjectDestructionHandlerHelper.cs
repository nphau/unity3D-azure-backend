using System;
using UnityEngine;

public class DamagableObjectDestructionHandlerHelper
{
    public static void HandleDestruction(DamagableObject enemy, bool collectScore, LevelStatsSnapshot levelStats, LevelDefinition currentLevelDefinition, DefendorGame gameController, MoonDefenseGameSoundManager soundManager, DefendorGameUIManager uiManager)
    {
        if (collectScore)
        {
            float scoreCalculated = 0;
            float bonusCalculated = 0;


            if (!enemy.isBonus)
            {
                levelStats.comboLevel += 1;
                levelStats.maxComboThisLevel = Mathf.Max(levelStats.comboLevel - 1, levelStats.maxComboThisLevel);
                scoreCalculated = enemy.baseScore * Mathf.Min(levelStats.comboLevel, 10);
                levelStats.scoreThisLevel += scoreCalculated;

                if (UnityEngine.Random.Range(0.0f, currentLevelDefinition.oddsOfBonusASpawnOnEnemyDeath + currentLevelDefinition.oddsOfNothingOnEnemyDeath) < currentLevelDefinition.oddsOfBonusASpawnOnEnemyDeath)
                {
                    gameController.SpawnBonusAt(enemy.transform.position);
                }
            }
            else
            {
                bonusCalculated = enemy.bonusScore * Mathf.Min(levelStats.comboLevel, 10);
                levelStats.bonusBankedThisRound += bonusCalculated;
                soundManager.getBonusSound.Play();
            }

            Vector3 position = enemy.transform.position;
            uiManager.DisplayPoints(enemy.isBonus ? bonusCalculated : scoreCalculated, position);

            // Azure
            AzureConfig.Score = levelStats.scoreThisLevel;
            Debug.LogError("Game: " + levelStats.scoreThisLevel);
            Debug.LogError("Azure: " + AzureConfig.Score.ToString());

            uiManager.totalScoreDisplay.text = levelStats.scoreThisLevel.ToString();
            levelStats.enemiesDestroyed += 1;

        }
        else
        {
            levelStats.enemiesMissed += 1;
            //HandleComboEnd();
            levelStats.maxComboThisLevel = Mathf.Max(levelStats.comboLevel - 1, levelStats.maxComboThisLevel);
            levelStats.comboLevel = 1;
        }

        uiManager.multiplierDisplay.text = "X" + levelStats.comboLevel.ToString();
    }
}

