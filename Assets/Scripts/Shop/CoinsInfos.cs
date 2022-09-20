using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsInfos : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI optionsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI weatherText;
    [SerializeField] private TextMeshProUGUI stadiumText;
    [SerializeField] private TextMeshProUGUI totalText;


    public void ApplyCoinsResult(GameData data, int score, int wave, int kills, float percentage, int total)
    {
        if (data.gameMode == GameMode.DRILL && (data.gameDrill == GameDrill.ONEVONE || data.gameDrill == GameDrill.OBJECTIF))
            ApplyCoinsResultDrill(data, score, total);
        else if (data.gameMode == GameMode.DRILL || data.gameMode == GameMode.TUTORIAL)
            ApplyCoinsResultTuto();

        scoreText.text = "Score : " + score;
        waveText.text = "Wave : " + 100 * (wave * (wave - 1)) / 2;
        optionsText.text = "Options multiplier : " + OptionsMultiplier(data);
        killsText.text = kills != 0 ? "Kills : " + kills + " * " + 10 * ((int)data.gameDifficulty + 1) * ((int)data.gameWeather + 1) : "Kills : 0";
        difficultyText.text = "Difficulty multiplier : " + ((int)data.gameDifficulty * 2 + 1);
        weatherText.text = "Weather multiplier : " + ((int)data.gameWeather + 1);
        stadiumText.text = "Stadium multiplier : " + percentage;

        totalText.text = "Total : " + total;
    }

    private void ApplyCoinsResultDrill(GameData data, int score, int total)
    {
        scoreText.text = "Score : " + score;
        waveText.text = "Wave : " + 0;
        optionsText.text = "Options multiplier : " + total / (score * (int)data.gameDifficulty + 1 * (int)data.gameWeather + 1);
        killsText.text = "Kills : " + 0;
        difficultyText.text = "Difficulty multiplier : " + (int)data.gameDifficulty + 1;
        weatherText.text = "Weather multiplier : " + (int)data.gameWeather + 1;
        stadiumText.text = "Stadium multiplier : " + 1;

        totalText.text = "Total : " + total;
    }
    private void ApplyCoinsResultTuto()
    {
        scoreText.text = "Score : " + 0;
        waveText.text = "Wave : " + 0;
        optionsText.text = "Options multiplier : " + 0;
        killsText.text = "Kills : " + 0;
        difficultyText.text = "Difficulty multiplier : " + 0;
        weatherText.text = "Weather multiplier : " + 0;
        stadiumText.text = "Stadium multiplier : " + 0;

        totalText.text = "Total : " + 0;
    }
    


    private float OptionsMultiplier(GameData data)
    {
        float mult = 1f;
        if (data.gameOptions.Contains(GameOption.BONUS))
            mult *= 0.33f;
        if (data.gameOptions.Contains(GameOption.OBSTACLE))
            mult *= 1.5f;
        if (data.gameOptions.Contains(GameOption.OBJECTIF))
            mult *= 1.5f;
        if (data.gameOptions.Contains(GameOption.WEAPONS))
            mult /= 3;

        return mult;
    }
}
