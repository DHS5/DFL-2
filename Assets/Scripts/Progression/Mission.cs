using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Mission
{
    [SerializeField] private List<GameMode> gameModes;
    [SerializeField] private List<GameDifficulty> gameDifficulties;
    [SerializeField] private List<GameWheather> gameWheathers;
    [SerializeField] private List<GameOption> gameOptions;
    [SerializeField] private GameDrill gameDrill;

    [SerializeField] private int waveToReach;


    public bool CompleteMission(GameData data, int wave)
    {
        if (wave >= waveToReach)
        {
            Debug.Log(waveToReach);
            if (gameModes.Count > 0 && !gameModes.Contains(data.gameMode))
                return false;
            if (gameDifficulties.Count > 0 && !gameDifficulties.Contains(data.gameDifficulty))
                return false;
            if (gameWheathers.Count > 0 && !gameWheathers.Contains(data.gameWheather))
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
