using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> sceneCards;
    public Transform deckPosition;
    public Transform playerHandPosition;

    private List<GameObject> currentDeck = new List<GameObject>();

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

        currentDeck = new List<GameObject>(sceneCards);
        Shuffle(currentDeck);

        for (int i = 0; i < currentDeck.Count; i++)
        {
            GameObject card = currentDeck[i];
            // card.transform.position = deckPosition.position + new Vector3(0, i * 0.1f, 0);
            // card.transform.rotation = Quaternion.Euler(0, 90f, 0);
            card.transform.SetParent(deckPosition);
            card.transform.localPosition = new Vector3(0, i * 0.1f, 0);
            card.transform.localRotation = Quaternion.Euler(0, 90f, 0);
        }
    }

    public void MoveTopCardToPlayerHand()
    {
        if (currentDeck.Count == 0)
        {
            Debug.Log("牌堆已空");
            return;
        }

        GameObject topCard = currentDeck[currentDeck.Count - 1];
        currentDeck.RemoveAt(currentDeck.Count - 1);

        topCard.transform.position = playerHandPosition.position;
        topCard.transform.rotation = Quaternion.Euler(0, 90f, 180f);

        Debug.Log("已将一张牌移动到手牌");
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
