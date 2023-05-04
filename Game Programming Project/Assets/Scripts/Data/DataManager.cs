using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static void SaveData(TemporaryData d)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.re";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, new Data(d.level, d.highscore, d.coins, d.totalCoins, d.completedQuests, d.completedQuestsOneGame, d.completedAchievements,
            d.totalJumps, d.totalTime, d.totalAttempts, d.levelAttempts, d.levelProgress, d.currentLevel, d.ownedMaterials, d.selectedMaterials,
            d.selectedMaterialPreview, d.musicVolume, d.SFXVolume, d.sensitivity, d.simulationDistance, d.questCompleted, d.currentQuest, d.date,
            d.questVariables, d.username, d.email, d.password, d.isLoggedIn));
        stream.Close();
    }
    public static Data LoadData()
    {
        string path = Application.persistentDataPath + "/data.re";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }

        return null;
    }
}
