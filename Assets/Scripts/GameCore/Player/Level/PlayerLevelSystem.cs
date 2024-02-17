using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Slider))]
public class PlayerLevelSystem : MonoBehaviour
{
    [SerializeField] private Slider levelBar;

    private int currentLevel = 1;

    private int nextLevelExperience = 75;
    private int experience = 0;

    private int expIncreaseFactor = 5;

    public event Action levelUp;

    public int AddExperience
    {
        get { return experience; }
        set
        {
            experience += value;

            CheckNextLevelRequirements();
            UpdateLevelBar();
        }
    }

    private void CheckNextLevelRequirements()
    {
        if (experience >= nextLevelExperience)
        {
            experience -= nextLevelExperience;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        nextLevelExperience = nextLevelExperience + (expIncreaseFactor * currentLevel);
        currentLevel++;

        levelUp?.Invoke();
    }

    private void UpdateLevelBar()
    {
        levelBar.maxValue = nextLevelExperience;
        levelBar.value = experience;

        //
        // Your turn Mr. Yura
        // Add smooth slider
        //
    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        AddExperience = 10;
    //    }

    //}
}
