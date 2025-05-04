using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardType { TypeA, TypeB, TypeC, TypeD }

public class DeckManager : MonoBehaviour
{
    // 配置参数
    [Header("Settings")]
    public GameObject cardPrefab;
    public Transform deckPosition;
    public Transform playerHandPosition;
    public Transform opponentHandPosition;
    public Material[] typeMaterials;
    public float drawDuration = 0.5f;
    public float returnDuration = 0.3f;
    public int cardsPerRound = 1; // 每回合每方抽卡数

    // 状态变量
    private List<GameObject> allCards = new List<GameObject>(); // 所有卡牌
    private List<GameObject> deck = new List<GameObject>();    // 当前牌堆
    private int playerDrawCount = 0;
    private int aiDrawCount = 0;
    private bool isAnimating = false;

    void Start()
    {
        InitializeDeck();
        StartNewRound();
    }

    // 初始化所有卡牌（3A,3B,3C,1D）
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

    // 开始新回合
    public void StartNewRound()
    {
        // 重置计数
        playerDrawCount = 0;
        aiDrawCount = 0;

        // 回收所有卡牌
        ReturnAllCardsToDeck();
    }

    // 回收所有卡牌到牌堆并洗牌
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

    // Fisher-Yates洗牌算法
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

    // 玩家抽卡（鼠标点击触发）
    public void PlayerDraw()
    {
        if (CanDraw(true)) StartCoroutine(DrawCardRoutine(true));
    }

    // 机器人抽卡（自动触发）
    public void AIDraw()
    {
        if (CanDraw(false)) StartCoroutine(DrawCardRoutine(false));
    }

    // 抽卡协程
    IEnumerator DrawCardRoutine(bool isPlayer)
    {
        isAnimating = true;

        GameObject card = deck[0];
        deck.RemoveAt(0);
        card.SetActive(true);

        // 移动动画
        Transform target = isPlayer ? playerHandPosition : opponentHandPosition;
        card.transform.DOMove(target.position, drawDuration).SetEase(Ease.OutBack);

        // 翻转动画
        card.GetComponent<Card>().FlipCard(isPlayer ? 180 : 0, drawDuration);

        // 更新抽卡计数
        if (isPlayer) playerDrawCount++;
        else aiDrawCount++;

        // 检测回合是否结束
        if (playerDrawCount >= cardsPerRound && aiDrawCount >= cardsPerRound)
        {
            yield return new WaitForSeconds(drawDuration + 0.5f);
            StartNewRound(); // 开始新回合
        }

        isAnimating = false;
    }

    // 检测是否允许抽卡
    bool CanDraw(bool isPlayer)
    {
        return !isAnimating &&
               deck.Count > 0 &&
               (isPlayer ? playerDrawCount < cardsPerRound : aiDrawCount < cardsPerRound);
    }
}
