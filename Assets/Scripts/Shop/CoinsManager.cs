using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinsManager
{
    public static int GameCoins(GameData data, int score, int wave, int kills)
    {
        int coins = 0;

        if (data.gameMode != GameMode.DRILL)
        {
            coins = score * ((int)data.gameDifficulty + 1) * ((int)data.gameWeather + 1) + 100 * (wave * (wave - 1)) / 2;

            if (data.gameOptions.Contains(GameOption.BONUS))
                coins /= 3;
            if (data.gameOptions.Contains(GameOption.OBSTACLE))
                coins = (int)(coins * 1.5f);
            if (data.gameOptions.Contains(GameOption.OBJECTIF))
                coins = (int)(coins * 1.5f);
            if (data.gameOptions.Contains(GameOption.WEAPONS))
            {
                coins /= 3;
                coins += kills * 10;
            }
            else if (data.gameDrill == GameDrill.OBJECTIF)
                coins = score / (10 - (int)data.gameDifficulty - (int)data.gameWeather);
            else if (data.gameDrill == GameDrill.ONEVONE)
                coins = score / (10 - (int)data.gameWeather);
        }

        return coins;
    }
}
