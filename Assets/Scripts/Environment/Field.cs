using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public struct FieldMaterials
{
    public Material grass;
    public Material endzone1;
    public Material endzone2;
    public Material barriers;
    public Material goalpostBase1;
    public Material goalpostMetal1;
    public Material goalpostBase2;
    public Material goalpostMetal2;
    public Material stadiumMaterial;
}


public class Field : MonoBehaviour
{   
    [Tooltip("Struct containing the materials to use on the field")]
    [HideInInspector] public FieldMaterials fieldMaterials;


    [Header("Prefab objects of the field")]
    [SerializeField] GameObject grass;
    [SerializeField] GameObject endzone1;
    [SerializeField] GameObject endzone2;

    [SerializeField] GameObject barrierL;
    [SerializeField] GameObject barrierR;

    [SerializeField] GameObject goalpost1Base;
    [SerializeField] GameObject goalpost1Metal;
    [SerializeField] GameObject goalpost2Base;
    [SerializeField] GameObject goalpost2Metal;


    [Header("Zones of the field")]
    public GameObject fieldZone;
    public GameObject centerZone;
    public GameObject leftZone;
    public GameObject rightZone;
    public GameObject frontZone;
    public GameObject middleZone;
    public GameObject endZone;
    public GameObject stadium;


    [Header("Stadium's audio sources")]
    public AudioSource[] entryAS;
    public AudioSource[] exitAS;
    public AudioSource[] bleachersAS;
    public AudioSource[] ouuhAS;
    public AudioSource[] boohAS;

    [Tooltip("List of the enemies on the field")]
    [HideInInspector] public List<GameObject> enemies;


    /// <summary>
    /// Sets all the materials of the new field
    /// </summary>
    public void CreateField()
    {
        // ### Ground
        // ## Grass
        grass.GetComponent<MeshRenderer>().material = fieldMaterials.grass;
        // ## Endzones
        endzone1.GetComponent<MeshRenderer>().material = fieldMaterials.endzone1;
        endzone2.GetComponent<MeshRenderer>().material = fieldMaterials.endzone2;

        // ### Barriers
        // ## Barrier L
        barrierL.GetComponent<MeshRenderer>().material = fieldMaterials.barriers;
        // ## Barrier R
        barrierR.GetComponent<MeshRenderer>().material = fieldMaterials.barriers;

        // ### Goalposts
        // ## Goalpost 1
        goalpost1Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase1;
        goalpost1Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal1;
        // ## Goalpost 2
        goalpost2Base.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostBase2;
        goalpost2Metal.GetComponent<MeshRenderer>().material = fieldMaterials.goalpostMetal2;

        // ### Stadium
        stadium.GetComponent<MeshRenderer>().material = fieldMaterials.stadiumMaterial;
    }

    /// <summary>
    /// Destroys all the enemies on the field
    /// </summary>
    public void SuppEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i]);
    }


    public void BoohAudio()
    {
        foreach (AudioSource a in boohAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void OuuhAudio()
    {
        foreach (AudioSource a in ouuhAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void StopAmbianceAudios()
    {
        foreach (AudioSource a in entryAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in exitAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in bleachersAS)
            a.gameObject.SetActive(false);
    }
}
