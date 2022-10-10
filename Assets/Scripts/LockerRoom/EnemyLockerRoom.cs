using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyLockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject enemyObject;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] SkinnedMeshRenderer enemyRenderer;

    [Header("Content")]
    [SerializeField] private EnemyCardSO enemyCard;


    private void Awake()
    {
        if (enemyCard != null)
            ApplyEnemyInfo(enemyCard);
    }


    public void ApplyEnemyInfo(Mesh _mesh, Avatar _avatar, Vector3 size)
    {
        enemyRenderer.sharedMesh = _mesh;
        enemyAnimator.avatar = _avatar;

        enemyObject.transform.localScale = new Vector3(size.x, size.y, size.z);
    }

    public void ApplyEnemyInfo(EnemyCardSO enemyCard)
    {
        ApplyEnemyInfo(enemyCard.mesh, enemyCard.avatar, enemyCard.attribute.size);
    }

    public void ApplyEnemyMaterial(Material mat)
    {
        enemyRenderer.material = mat;
    }
}
