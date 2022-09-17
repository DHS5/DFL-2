using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public enum GameScreen { GAME = 0, RESTART = 1, TUTO = 2, WIN = 3, LOSE = 4 }


public class GameUIManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [Tooltip("Game UI screens\n" +
        "0 --> game screen\n" +
        "1 --> restart screen\n" +
        "2 --> tutorial screen\n" +
        "3 --> win screen")]
    [SerializeField] private GameObject[] screens;

    [Header("Count texts")]
    [Tooltip("Wave number UI texts")]
    [SerializeField] private TextMeshProUGUI[] waveNumberTexts;

    [Tooltip("Score UI texts")]
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private CoinsInfos coinsInfos;
    
    [Tooltip("Coins UI texts")]
    [SerializeField] private TextMeshProUGUI coinsText;
    
    [Tooltip("Kills UI texts")]
    [SerializeField] private TextMeshProUGUI killsText;

    [Space]
    [Header("Game Screen")]
    [Tooltip("Resume game (3 2 1) text")]
    [SerializeField] private TextMeshProUGUI resumeGameText;


    [Header("Bonus & sprint & jump")]
    [Tooltip("UI components of the acceleration bar")]
    [SerializeField] private GameObject[] accelerationBars;
    [Tooltip("UI components of the bonus bar")]
    [SerializeField] private GameObject[] bonusBars;
    [Tooltip("UI components of the life bonuses")]
    [SerializeField] private GameObject[] lifeBonuses;

    [SerializeField] private Animation bonusBarAnim;

    [Space]
    public JumpBar jumpBar;

    private float bonusBarSize;


    [Header("Weapons")]
    [Tooltip("")]
    [SerializeField] private GameObject weaponsObject;

    [Tooltip("")]
    [SerializeField] private GameObject weaponsBackground;

    [Tooltip("Text indicating the number of shoot left")]
    [SerializeField] private TextMeshProUGUI shootLeftText;

    [SerializeField] private Image weaponIcon;


    [Header("Restart Screen")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;



    /// <summary>
    /// Gets the Main Manager
    /// </summary>
    private void Awake()
    {
        bonusBarSize = bonusBars[1].GetComponent<RectTransform>().rect.height;
        main = GetComponent<MainManager>();
    }




    // ### Tools ###

    public void SetScreen(GameScreen GS, bool state) { screens[(int)GS].SetActive(state); }


    // ### Functions ###

    /// <summary>
    /// Actualize all wave texts
    /// </summary>
    /// <param name="wave"></param>
    public void ActuWaveNumber(int wave)
    {
        foreach (TextMeshProUGUI t in waveNumberTexts)
        {
            if (wave < 10) t.text = "0";
            else t.text = "";
            t.text += wave.ToString();
        }
    }

    /// <summary>
    /// Actualize all score texts
    /// </summary>
    /// <param name="score"></param>
    public void ActuScore(int score)
    {
        foreach (TextMeshProUGUI t in scoreTexts)
        {
            t.text = score.ToString();
        }
    }
    
    /// <summary>
    /// Actualize the coins text
    /// </summary>
    /// <param name="score"></param>
    public void ActuCoins(GameData data, int score, int wave, int kills, float percentage, int total)
    {
        coinsText.text = total.ToString();
        coinsInfos.ApplyCoinsResult(data, score, wave, kills, percentage, total);
    }
    
    /// <summary>
    /// Actualize the coins text
    /// </summary>
    /// <param name="score"></param>
    public void ActuKills(int kills)
    {
        killsText.gameObject.SetActive(true);
        killsText.text = kills.ToString();

        Vector2 pos = coinsText.rectTransform.anchoredPosition;
        coinsText.rectTransform.anchoredPosition = new Vector2(-600, pos.y);
    }





    /// <summary>
    /// Called when the game is over
    /// Activates the restart screen and deactivates the game screen
    /// </summary>
    public void GameOver()
    {
        SetScreen(GameScreen.GAME, false);
        SetScreen(GameScreen.RESTART, true);
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE)
        {
            homeButton.image.color = Color.white;
            restartButton.image.color = Color.white;
        }
    }
    
    /// <summary>
    /// Called when the player win
    /// Activates the win screen and deactivates the game screen
    /// </summary>
    public void Win()
    {
        SetScreen(GameScreen.GAME, false);
        SetScreen(GameScreen.WIN, true);
    }
    
    /// <summary>
    /// Called when the player win
    /// Activates the win screen and deactivates the game screen
    /// </summary>
    public void Lose()
    {
        SetScreen(GameScreen.GAME, false);
        SetScreen(GameScreen.LOSE, true);
    }



    /// <summary>
    /// Constructs and plays the acceleration bar animation
    /// </summary>
    /// <param name="dechargeTime"></param>
    /// <param name="rechargeTime"></param>
    public IEnumerator AccBarAnim(float dechargeTime, float rechargeTime)
    {
        float timeOffset = 0.000001f;
        
        Animation fullBarAnim = accelerationBars[0].GetComponent<Animation>();
        Animation chargingBarAnim = accelerationBars[1].GetComponent<Animation>();

        fullBarAnim.Stop();
        chargingBarAnim.Stop();

        // Decharge

        accelerationBars[1].SetActive(false);

        Keyframe[] keys = new Keyframe[3];
        keys[0] = new Keyframe(0.0f, 0.0f);
        keys[1] = new Keyframe(dechargeTime, -accelerationBars[2].GetComponent<RectTransform>().rect.height / 2);
        keys[2] = new Keyframe(dechargeTime + timeOffset, 0.0f);
        keys[0].outTangent = -accelerationBars[2].GetComponent<RectTransform>().rect.height / (2 * dechargeTime);
        keys[1].inTangent = -accelerationBars[2].GetComponent<RectTransform>().rect.height / (2 * dechargeTime);
        AnimationCurve curve = new(keys);
        fullBarAnim.clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);

        keys[1].value = -accelerationBars[2].GetComponent<RectTransform>().rect.height;
        keys[0].outTangent = -accelerationBars[2].GetComponent<RectTransform>().rect.height / dechargeTime;
        keys[1].inTangent = -accelerationBars[2].GetComponent<RectTransform>().rect.height / dechargeTime;
        curve = new AnimationCurve(keys);
        fullBarAnim.clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve);

        fullBarAnim.Play();

        yield return new WaitForSeconds(dechargeTime);


        // Recharge

        keys[0].value = -accelerationBars[2].GetComponent<RectTransform>().rect.height / 2;
        keys[1] = new Keyframe(rechargeTime, 0.0f);
        keys[2].time = rechargeTime + timeOffset;
        keys[0].outTangent = accelerationBars[2].GetComponent<RectTransform>().rect.height / (2 * rechargeTime);
        keys[1].inTangent = accelerationBars[2].GetComponent<RectTransform>().rect.height / (2 * rechargeTime);

        curve = new AnimationCurve(keys);
        chargingBarAnim.clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);

        keys[0].value = -accelerationBars[2].GetComponent<RectTransform>().rect.height;
        keys[0].outTangent = accelerationBars[2].GetComponent<RectTransform>().rect.height / rechargeTime;
        keys[1].inTangent = accelerationBars[2].GetComponent<RectTransform>().rect.height / rechargeTime;

        curve = new AnimationCurve(keys);
        chargingBarAnim.clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve);

        accelerationBars[1].SetActive(true);
        chargingBarAnim.Play();

        yield return new WaitForSeconds(timeOffset);

        accelerationBars[0].SetActive(false);

        yield return new WaitForSeconds(rechargeTime - timeOffset);

        accelerationBars[0].SetActive(true);
    }



    public void BonusBarAnim(float bonusTime, Color color, Sprite sprite)
    {
        bonusBarAnim.Stop();

        bonusBars[1].GetComponent<Image>().color = color;
        bonusBars[2].GetComponent<Image>().sprite = sprite;

        bonusBars[0].SetActive(true);
        bonusBars[1].SetActive(true);
        bonusBars[2].SetActive(true);

        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0.0f, 0.0f);
        keys[1] = new Keyframe(bonusTime, -bonusBarSize / 2);
        keys[0].outTangent = -bonusBarSize / (2 * bonusTime);
        keys[1].inTangent = -bonusBarSize / (2 * bonusTime);
        AnimationCurve curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);

        keys[1].value = -bonusBarSize;
        keys[0].outTangent = -bonusBarSize / bonusTime;
        keys[1].inTangent = -bonusBarSize / bonusTime;
        curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve);
        
        bonusBarAnim.AddClip(clip, "clip");
        bonusBarAnim.Play("clip");

        Invoke(nameof(EndBonus), bonusTime);
    }
    /// <summary>
    /// Makes the bonus bar disappear
    /// </summary>
    private void EndBonus()
    { 
        bonusBars[0].SetActive(false);
        bonusBars[1].SetActive(false);
        bonusBars[2].SetActive(false);
    }


    public void ModifyLife(bool plus, int lifeNumber)
    {
        lifeBonuses[lifeNumber].SetActive(plus);
    }


    public void ResumeGameText(int number, bool state)
    {
        resumeGameText.gameObject.SetActive(state);
        resumeGameText.text = number.ToString();
    }


    // # Weapons #

    public void DisplayWeapon(int ammunition, bool canShoot, bool state)
    {
        weaponsObject.SetActive(state);
        weaponsBackground.SetActive(canShoot);

        shootLeftText.text = ammunition.ToString();

        weaponIcon.enabled = state;
    }

    public void DisplayWeapon(Sprite weaponSprite, int ammunition, bool canShoot, bool state)
    {
        weaponIcon.sprite = weaponSprite;
        DisplayWeapon(ammunition, canShoot, state);
    }
}
