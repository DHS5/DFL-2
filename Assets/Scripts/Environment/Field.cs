using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public struct FieldMaterials
{
    public Material grass;
    public Material endzone;
    public Material goalpostBase;
    public Material goalpostMetal;
    public Material sideline;
}


[ExecuteInEditMode]
public class Field : MonoBehaviour
{   
    [Tooltip("Struct containing the materials to use on the field")]
    public FieldMaterials fieldMaterials;


    [Header("Prefab objects of the field")]
    [SerializeField] GameObject grass;
    [SerializeField] GameObject endzone1;
    [SerializeField] GameObject endzone2;
    [SerializeField] GameObject sidelineL;
    [SerializeField] GameObject sidelineR;
    [Space]
    [SerializeField] GameObject goalpost1Base;
    [SerializeField] GameObject goalpost1Metal;
    [SerializeField] GameObject goalpost2Base;
    [SerializeField] GameObject goalpost2Metal;
    [Space]
    public GameObject entryGoalpost;
    [Space, Space]
    [SerializeField] private Volume ambianceGlobalVolume;
    [SerializeField] private VolumeProfile ambianceProfile;
    [SerializeField] private VolumeProfile zombieProfile;
    [Space, Space]
    [SerializeField] GameObject[] fieldLimits;


    [Header("Zones of the field")]
    public GameObject fieldZone;
    public GameObject centerZone;
    public GameObject leftZone;
    public GameObject rightZone;
    public GameObject frontZone;
    public GameObject middleZone;
    public GameObject endZone;
    public GameObject enterZone;
    public GameObject bonusZone;
    public GameObject obstacleZone;

    [Header("Enemy Spawn Position")]
    [SerializeField] private GameObject enemySpawnPos;
    public Vector3 OneVOneEnemyPos { get { return enemySpawnPos.transform.position; } }


    /// <summary>
    /// Sets all the materials of the new field
    /// </summary>
    public void CreateField(GameData? data)
    {
        // ### Ground
        // ## Grass
        grass.GetComponent<MeshRenderer>().material = fieldMaterials.grass;
        // ## Endzones
        endzone1.GetComponent<MeshRenderer>().material = fieldMaterials.endzone;
        endzone2.GetComponent<MeshRenderer>().material = fieldMaterials.endzone;
        // ## Sidelines
        sidelineL.GetComponent<MeshRenderer>().material = fieldMaterials.sideline;
        sidelineR.GetComponent<MeshRenderer>().material = fieldMaterials.sideline;

        // ### Goalposts
        // ## Goalpost 1
        if (goalpost1Base != null) goalpost1Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase;
        if (goalpost1Metal != null) goalpost1Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal;
        // ## Goalpost 2
        if (goalpost2Base != null) goalpost2Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase;
        if (goalpost2Metal != null) goalpost2Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal;

        if (entryGoalpost != null && DataManager.InstanceDataManager != null)
            entryGoalpost.SetActive(DataManager.InstanceDataManager.gameplayData.goalpost);

        if (ambianceProfile != null && ambianceGlobalVolume != null)
            ambianceGlobalVolume.profile = ambianceProfile;
        else if (ambianceGlobalVolume != null) ambianceGlobalVolume.profile = null;

        if (data.HasValue && data.GetValueOrDefault().gameMode == GameMode.ZOMBIE)
            ambianceGlobalVolume.profile = zombieProfile;
    }
    public void CreateField() { CreateField(null); }

    public void ActivateFieldLimits()
    {
        fieldLimits[0].SetActive(true);
        fieldLimits[1].SetActive(true);
    }

    private void OnValidate()
    {
        CreateField();
    }
}
