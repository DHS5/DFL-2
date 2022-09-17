using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinsManager
{
    public static int GameCoins(GameData data, int score, int wave, int kills, float percentage)
    {
        int coins = 0;

        if (data.gameMode != GameMode.DRILL && data.gameMode != GameMode.TUTORIAL)
        {
            coins = score + 100 * (wave * (wave - 1)) / 2;

            if (data.gameOptions.Contains(GameOption.BONUS))
                coins /= 3;
            if (data.gameOptions.Contains(GameOption.OBSTACLE))
                coins = (int)(coins * 1.5f);
            if (data.gameOptions.Contains(GameOption.OBJECTIF))
                coins = (int)(coins * 1.5f);
            if (data.gameOptions.Contains(GameOption.WEAPONS))
            {
                coins /= 3;
                coins += kills * 10 * ((int)data.gameDifficulty + 1) * ((int)data.gameWeather + 1);
            }

            coins *= ((int)data.gameDifficulty * 2 + 1) * ((int)data.gameWeather + 1);

            coins = (int) (coins * percentage);
        }
        else if (data.gameMode == GameMode.DRILL)
        {
            if (data.gameDrill == GameDrill.OBJECTIF || data.gameDrill == GameDrill.ONEVONE)
                coins = score / (10 - (int)data.gameDifficulty - (int)data.gameWeather);
        }

        return coins;
    }
}
