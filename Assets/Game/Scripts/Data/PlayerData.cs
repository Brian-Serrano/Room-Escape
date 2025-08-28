using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // login info
    public string playerToken;
    public int playerId;
    public string playerName;
    public bool hasLoggedIn;

    // primary data
    public int level;
    public int highScore;
    public int coins;
    public int totalCoins;
    public int totalQuestsCompleted;
    public int totalQuestsCompletedOneGame;
    public int totalJumps;
    public float totalTime;
    public int totalAttempts;
    public int levelAttempts;
    public float levelProgress;
    public string materialsOwned;
    public List<int> materialsSelected;

    // quest data
    public int levelsCompletedQuestTotal;
    public int levelsCompletedQuestProgress;
    public int attemptsQuestTotal;
    public int attemptsQuestProgress;
    public int coinsCollectedQuestTotal;
    public int coinsCollectedQuestProgress;
    public string lastQuestLoadTime;
    
    // settings data
    public float musicVolume;
    public float sfxVolume;
    public float sensitivity;

    public PlayerData()
    {
        playerToken = "";
        playerId = 0;
        playerName = "";

        level = 1;
        highScore = 0;
        coins = 100;
        totalCoins = 100;
        totalQuestsCompleted = 0;
        totalQuestsCompletedOneGame = 0;
        totalJumps = 0;
        totalTime = 0f;
        totalAttempts = 0;
        levelAttempts = 0;
        levelProgress = 0f;
        materialsOwned = "10000100000000010000000101000000000000001000000000";
        materialsSelected = new List<int>() { 0, 23, 5, 15, 25, 40 }; // Block, Door, Floor, Spike, Wall, Wood

        levelsCompletedQuestTotal = 0;
        levelsCompletedQuestProgress = -1;
        attemptsQuestTotal = 0;
        attemptsQuestProgress = -1;
        coinsCollectedQuestTotal = 0;
        coinsCollectedQuestProgress = -1;
        lastQuestLoadTime = "";

        musicVolume = 1f;
        sfxVolume = 1f;
        sensitivity = 0.5f;
    }

    public static PlayerData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "player_data.re");

        return PersistentDataController.LoadData<PlayerData>(path);
    }

    public bool SaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, "player_data.re");

        return PersistentDataController.SaveData(this, path);
    }
}
