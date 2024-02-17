using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
            LevelUp();
        }
    }

    private void LevelUp()
    {
        int nextExpRequirement = nextLevelExperience + (expIncreaseFactor * (currentLevel + 1));

        currentLevel++;
        nextLevelExperience = nextExpRequirement;

        levelUp?.Invoke();
    }

    private void UpdateLevelBar()
    {
        // Your turn Mr. Yura
    }


}
