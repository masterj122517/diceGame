using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> sceneCards;
    public Transform deckPosition;

    void Start()
    {
        ShuffleAndArrange();
    }

    public void ShuffleAndArrange()
    {
        if (sceneCards.Count != 10)
        {
            Debug.LogError("请在 sceneCards 里放入 10 张场景中已有的卡牌对象");
            return;
        }

        List<GameObject> shuffled = new List<GameObject>(sceneCards);
        Shuffle(shuffled);
        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject card = shuffled[i];

            card.transform.SetParent(deckPosition);
            card.transform.localPosition = new Vector3(0, i * 0.1f, 0);
            card.transform.localRotation = Quaternion.Euler(0, 90f, 0);
        }
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // [0, i] 闭区间
            GameObject temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
