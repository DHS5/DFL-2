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

    [SerializeField] private Vector3 oneVOneEnemyPos;
    public Vector3 OneVOneEnemyPos { get { return oneVOneEnemyPos + transform.position; } }


    /// <summary>
    /// Sets all the materials of the new field
    /// </summary>
    public void CreateField()
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
        goalpost1Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase;
        goalpost1Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal;
        // ## Goalpost 2
        goalpost2Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase;
        goalpost2Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal;

        if (DataManager.InstanceDataManager != null)
            entryGoalpost.SetActive(DataManager.InstanceDataManager.gameplayData.goalpost);

        if (ambianceProfile != null)
            ambianceGlobalVolume.profile = ambianceProfile;
        else ambianceGlobalVolume.profile = null;
    }


    private void OnValidate()
    {
        CreateField();
    }
}
