using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public enum GameScreen { GAME = 0, RESTART = 1, TUTO = 2 }


public class GameUIManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [Tooltip("Game UI screens\n" +
        "0 --> game screen\n" +
        "1 --> restart screen")]
    [SerializeField] private GameObject[] screens;

    [Tooltip("Wave number UI texts")]
    [SerializeField] private TextMeshProUGUI[] waveNumberTexts;

    [Tooltip("Score UI texts")]
    [SerializeField] private TextMeshProUGUI[] scoreTexts;

    [Tooltip("Resume game (3 2 1) text")]
    [SerializeField] private TextMeshProUGUI resumeGameText;



    [Tooltip("UI components of the acceleration bar")]
    [SerializeField] private GameObject[] accelerationBars;
    [Tooltip("UI components of the bonus bar")]
    [SerializeField] private GameObject[] bonusBars;
    [Tooltip("UI components of the life bonuses")]
    [SerializeField] private GameObject[] lifeBonuses;


    [SerializeField] private Animation accBarAnim;
    [SerializeField] private Animation bonusBarAnim;

    private float bonusBarSize;


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
    /// Called when the game is over
    /// Activates the restart screen and deactivates the game screen
    /// </summary>
    public void GameOver()
    {
        SetScreen(GameScreen.GAME, false);
        SetScreen(GameScreen.RESTART, true);
    }



    /// <summary>
    /// Constructs and plays the acceleration bar animation
    /// </summary>
    /// <param name="dechargeTime"></param>
    /// <param name="rechargeTime"></param>
    public void AccBarAnim(float dechargeTime, float rechargeTime)
    {
        accelerationBars[0].SetActive(false);

        accBarAnim.Stop();

        Keyframe[] keys = new Keyframe[3];
        keys[0] = new Keyframe(0.0f, 0.0f);
        keys[1] = new Keyframe(dechargeTime, -accelerationBars[0].GetComponent<RectTransform>().rect.height / 2);
        keys[2] = new Keyframe(dechargeTime + rechargeTime, 0.0f);
        keys[0].outTangent = -accelerationBars[0].GetComponent<RectTransform>().rect.height / (2 * dechargeTime);
        keys[1].inTangent = -accelerationBars[0].GetComponent<RectTransform>().rect.height / (2 * dechargeTime);
        keys[1].outTangent = accelerationBars[0].GetComponent<RectTransform>().rect.height / (2 * rechargeTime);
        keys[2].inTangent = accelerationBars[0].GetComponent<RectTransform>().rect.height / (2 * rechargeTime);
        AnimationCurve curve = new AnimationCurve(keys);
        accBarAnim.clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);
        
        keys[1].value = -accelerationBars[0].GetComponent<RectTransform>().rect.height;
        keys[0].outTangent = -accelerationBars[0].GetComponent<RectTransform>().rect.height / dechargeTime;
        keys[1].inTangent = -accelerationBars[0].GetComponent<RectTransform>().rect.height / dechargeTime;
        keys[1].outTangent = accelerationBars[0].GetComponent<RectTransform>().rect.height / rechargeTime;
        keys[2].inTangent = accelerationBars[0].GetComponent<RectTransform>().rect.height / rechargeTime;
        curve = new AnimationCurve(keys);
        accBarAnim.clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve);

        accBarAnim.Play();

        Invoke(nameof(FullAccBar), dechargeTime + rechargeTime);
    }
    /// <summary>
    /// Makes the acceleration bar full
    /// </summary>
    private void FullAccBar() { accelerationBars[0].SetActive(true); }



    public void BonusBarAnim(float bonusTime, Color color)
    {
        bonusBarAnim.Stop();

        bonusBars[1].GetComponent<Image>().color = color;
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
}
