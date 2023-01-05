using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int i=0;
    public int numberOfAsteroids;
    public int levelNumber = 1;
    public GameObject asteroid;
    public AlienScript alien;

    public void UpdateNumberOfAsteroids(int change)
    {
        numberOfAsteroids += change;

        if (numberOfAsteroids <= 0)
        {
            Invoke("StartNewLevel", 3f);
        }
    }


    void StartNewLevel()
    {
        levelNumber++;

        for(i=0; i < levelNumber * 2; i++)
        {
            Vector2 spawnposition = new Vector2(Random.Range(-14f, 14f), 10.8f);
            Instantiate(asteroid, spawnposition, Quaternion.identity);
            numberOfAsteroids++;
        }


        alien.NewLevel();
       
      
    }


    public bool checkhighscore(int score)
    {
        int HighScore = PlayerPrefs.GetInt("highscore");
        if(score > HighScore)
        {
            return true;
        }

        return false;
    }
}
