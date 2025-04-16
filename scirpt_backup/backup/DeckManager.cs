using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardType { TypeA, TypeB, TypeC, TypeD }

public class DeckManager : MonoBehaviour
{
    // ���ò���
    [Header("Settings")]
    public GameObject cardPrefab;
    public Transform deckPosition;
    public Transform playerHandPosition;
    public Transform opponentHandPosition;
    public Material[] typeMaterials;
    public float drawDuration = 0.5f;
    public float returnDuration = 0.3f;
    public int cardsPerRound = 1; // ÿ�غ�ÿ���鿨��

    // ״̬����
    private List<GameObject> allCards = new List<GameObject>(); // ���п���
    private List<GameObject> deck = new List<GameObject>();    // ��ǰ�ƶ�
    private int playerDrawCount = 0;
    private int aiDrawCount = 0;
    private bool isAnimating = false;

    void Start()
    {
        InitializeDeck();
        StartNewRound();
    }

    // ��ʼ�����п��ƣ�3A,3B,3C,1D��
    void InitializeDeck()
    {
        for (int i = 0; i < 3; i++) CreateCard(CardType.TypeA);
        for (int i = 0; i < 3; i++) CreateCard(CardType.TypeB);
        for (int i = 0; i < 3; i++) CreateCard(CardType.TypeC);
        CreateCard(CardType.TypeD);
    }

    void CreateCard(CardType type)
    {
        GameObject card = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);
        card.GetComponent<Card>().Initialize(type, typeMaterials[(int)type]);
        card.SetActive(false);
        allCards.Add(card);
    }

    // ��ʼ�»غ�
    public void StartNewRound()
    {
        // ���ü���
        playerDrawCount = 0;
        aiDrawCount = 0;

        // �������п���
        ReturnAllCardsToDeck();
    }

    // �������п��Ƶ��ƶѲ�ϴ��
    void ReturnAllCardsToDeck()
    {
        foreach (GameObject card in allCards)
        {
            card.SetActive(false);
            card.transform.position = deckPosition.position;
            card.transform.rotation = Quaternion.identity;
            card.GetComponent<Card>().ResetCard();
        }

        deck.Clear();
        deck.AddRange(allCards);
        ShuffleDeck();
    }

    // Fisher-Yatesϴ���㷨
    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            GameObject temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // ��ҳ鿨�������������
    public void PlayerDraw()
    {
        if (CanDraw(true)) StartCoroutine(DrawCardRoutine(true));
    }

    // �����˳鿨���Զ�������
    public void AIDraw()
    {
        if (CanDraw(false)) StartCoroutine(DrawCardRoutine(false));
    }

    // �鿨Э��
    IEnumerator DrawCardRoutine(bool isPlayer)
    {
        isAnimating = true;

        GameObject card = deck[0];
        deck.RemoveAt(0);
        card.SetActive(true);

        // �ƶ�����
        Transform target = isPlayer ? playerHandPosition : opponentHandPosition;
        card.transform.DOMove(target.position, drawDuration).SetEase(Ease.OutBack);

        // ��ת����
        card.GetComponent<Card>().FlipCard(isPlayer ? 180 : 0, drawDuration);

        // ���³鿨����
        if (isPlayer) playerDrawCount++;
        else aiDrawCount++;

        // ���غ��Ƿ����
        if (playerDrawCount >= cardsPerRound && aiDrawCount >= cardsPerRound)
        {
            yield return new WaitForSeconds(drawDuration + 0.5f);
            StartNewRound(); // ��ʼ�»غ�
        }

        isAnimating = false;
    }

    // ����Ƿ�����鿨
    bool CanDraw(bool isPlayer)
    {
        return !isAnimating &&
               deck.Count > 0 &&
               (isPlayer ? playerDrawCount < cardsPerRound : aiDrawCount < cardsPerRound);
    }
}
