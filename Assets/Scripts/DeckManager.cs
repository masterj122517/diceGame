using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> sceneCards;
    public Transform deckPosition;
    public Transform playerHandPosition;

    private List<GameObject> currentDeck = new List<GameObject>();
    private bool hasDrawnThisTurn = false;  // 追踪本回合是否已经抽过牌

    void Start()
    {
        ShuffleAndArrange();
    }

    public void ShuffleAndArrange()
    {
        if (sceneCards.Count != 10)
        {
            Debug.LogError("请在 sceneCards 里放入 10 张卡牌");
            return;
        }

        // 清空当前牌堆并重新添加所有卡牌
        currentDeck.Clear();
        currentDeck.AddRange(sceneCards);
        Shuffle(currentDeck);

        // 将所有卡牌移回牌堆位置
        for (int i = 0; i < currentDeck.Count; i++)
        {
            GameObject card = currentDeck[i];
            card.transform.SetParent(deckPosition);
            card.transform.localPosition = new Vector3(0, i * 0.1f, 0);
            card.transform.localRotation = Quaternion.Euler(0, 90f, 0);
        }
        Debug.Log($"洗牌完成，当前牌堆有 {currentDeck.Count} 张牌");
    }

    public Card MoveTopCardToPlayerHand()
    {
        if (hasDrawnThisTurn)
        {
            Debug.Log("本回合已经抽过牌了");
            return null;
        }

        Debug.Log($"尝试抽牌，当前牌堆还有 {currentDeck.Count} 张牌");
        
        if (currentDeck.Count == 0)
        {
            Debug.Log("牌堆已空");
            return null;
        }

        GameObject topCard = currentDeck[currentDeck.Count - 1];
        currentDeck.RemoveAt(currentDeck.Count - 1);

        topCard.transform.SetParent(playerHandPosition);
        topCard.transform.localPosition = Vector3.zero;
        topCard.transform.localRotation = Quaternion.Euler(0, 90f, 180f);

        hasDrawnThisTurn = true;  // 标记本回合已经抽过牌
        Debug.Log($"成功抽出一张牌，牌堆还剩 {currentDeck.Count} 张牌");
        return topCard.GetComponent<Card>();
    }

    public void StartNewTurn()
    {
        hasDrawnThisTurn = false;
        Debug.Log("新回合开始，可以抽牌");
    }

    private void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
