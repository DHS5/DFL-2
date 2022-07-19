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
            if (gameModes != null && !gameModes.Contains(data.gameMode))
                return false;
            if (gameDifficulties != null && !gameDifficulties.Contains(data.gameDifficulty))
                return false;
            if (gameWheathers != null && !gameWheathers.Contains(data.gameWheather))
                return false;
            if (gameOptions != null)
            {
                foreach (GameOption option in gameOptions)
                {
                    if (!data.gameOptions.Contains(option))
                        return false;
                }
            }
            if (gameModes.Contains(GameMode.DRILL) && gameDrill != data.gameDrill)
                return false;

            Debug.Log(this + " complete !");
            return true;
        }

        return false;
    }
}
