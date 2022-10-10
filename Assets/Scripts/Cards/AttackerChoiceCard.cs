using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackerChoiceCard : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private CardManager cardManager;

    [Header("Attacker number")]
    [SerializeField] private int attackerNumber;

    [Header("UI Components")]
    [SerializeField] private AttackerCard card;
    [SerializeField] private TextMeshProUGUI numberText;

    private void Awake()
    {
        numberText.text = (attackerNumber + 1).ToString();
    }

    private void Start()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
        {
            AttackerCardSO cardSO = dataManager.cardsContainer.teamCards.GetAttacker(dataManager.playerPrefs.teamIndex[attackerNumber]);

            card.gameObject.SetActive(true);
            ApplyInfos(cardSO);
            DataManager.InstanceDataManager.gameData.team[attackerNumber] = cardSO.attribute;
        }
    }

    public void Add()
    {
        ApplyInfos(cardManager.CurrentAttackerCard);
        DataManager.InstanceDataManager.gameData.team[attackerNumber] = card.attackerCardSO.attribute;
        DataManager.InstanceDataManager.playerPrefs.teamIndex[attackerNumber] = (int) card.attackerCardSO.attacker;
    }

    private void ApplyInfos(AttackerCardSO cardSO)
    {
        card.ApplyCardSOInfo(cardSO);
        cardManager.teamLockerRoom.ApplyAttackerInfo(attackerNumber, cardSO);
    }
}
