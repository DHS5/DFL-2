using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Mission
{
    [SerializeField] private List<GameMode> gameModes;
    [SerializeField] private List<GameDifficulty> gameDifficulties;
    [SerializeField] private List<GameWeather> gameWeathers;
    [SerializeField] private List<GameOption> gameOptions;
    [SerializeField] private GameDrill gameDrill;
    [Space]
    [SerializeField] private int waveToReach;
    [Space]
    [SerializeField] private bool locked;


    public bool CompleteMission(GameData data, int wave)
    {
        if (locked) return false;
        if (wave >= waveToReach)
        {
            Debug.Log(waveToReach);
            if (gameModes.Count > 0 && !gameModes.Contains(data.gameMode))
                return false;
            if (gameDifficulties.Count > 0 && !gameDifficulties.Contains(data.gameDifficulty))
                return false;
            if (gameWeathers.Count > 0 && !gameWeathers.Contains(data.gameWeather))
                return false;
            if (gameOptions.Count > 0)
            {
                foreach (GameOption option in gameOptions)
                {
                    if (!data.gameOptions.Contains(option))
                        return false;
                }
            }
            if (gameModes.Contains(GameMode.DRILL) && gameDrill != data.gameDrill)
                return false;

            return true;
        }

        return false;
    }
}
