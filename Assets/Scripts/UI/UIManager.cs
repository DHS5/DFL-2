using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected Slider sensitivitySlider;
    [SerializeField] protected Slider smoothRotationSlider;
                     
    [SerializeField] protected TextMeshProUGUI[] highNames;
    [SerializeField] protected TextMeshProUGUI[] highWaves;

    [SerializeField] protected TextMeshProUGUI[] highNamesL;
    [SerializeField] protected TextMeshProUGUI[] highWavesL;

    protected Vector3Int gameType;
    public int GTMode { set { gameType.x = value; ActuLeaderboards(); ActuScoreList(); } }
    public int GTDifficulty { set { gameType.y = value; ActuLeaderboards(); ActuScoreList(); } }
    protected List<GameOption> gTOptions = new List<GameOption>();
    public int GTOptions { set { gameType.z = value; ActuLeaderboards(); ActuScoreList(); } }


    public void CloseAllTexts(GameObject g)
    {
        foreach (TextMeshProUGUI t in g.GetComponentsInChildren<TextMeshProUGUI>())
        {
            t.gameObject.SetActive(false);
        }
    }

    public void ActuScoreList()
    {
        //Vector3Int gt = gameType + new Vector3Int(1, 0, 0);
        //int i = 0;
        //while (DataManager.InstanceDataManager.highscores[i].gameType != gt && i < 96) i++;
        //
        //if (DataManager.InstanceDataManager.highscores[i].gameType == gt)
        //{
        //    for (int j = 0; j < 5; j++)
        //    {
        //        highNames[j].text = DataManager.InstanceDataManager.highscores[i].names[j];
        //        highWaves[j].text = "" + DataManager.InstanceDataManager.highscores[i].waves[j];
        //    }
        //}
        //else Debug.Log("Invalid game type");
    }

    public void ActuLeaderboards()
    {
        //int limit = DataManager.InstanceDataManager.leaderboards[gameType.x, gameType.y, gameType.z].names.Count <= DataManager.InstanceDataManager.leaderboardLimit ? DataManager.InstanceDataManager.leaderboards[gameType.x, gameType.y, gameType.z].names.Count : DataManager.InstanceDataManager.leaderboardLimit;
        //for (int i = 0; i < limit; i++)
        //{
        //    highNamesL[i].text = DataManager.InstanceDataManager.leaderboards[gameType.x, gameType.y, gameType.z].names[i];
        //    highWavesL[i].text = DataManager.InstanceDataManager.leaderboards[gameType.x, gameType.y, gameType.z].scores[i].ToString();
        //}
        //for (int j = limit; j < highNamesL.Length; j++)
        //{
        //    highNamesL[j].text = "None";
        //    highWavesL[j].text = "0";
        //}
    }



    public void GTChooseOption(int option)
    {
        if (!gTOptions.Contains((GameOption)option)) { gTOptions.Add((GameOption)option); }
        else { gTOptions.Remove((GameOption)option); }
        GTOptions = DataManager.InstanceDataManager.OptionsToInt(gTOptions);
    }
}
