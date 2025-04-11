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
        if (sceneCards.Count != 4)
        {
            Debug.LogError("请在 sceneCards 里放入 10 张场景中已有的卡牌对象");
            return;
        }

        List<GameObject> shuffled = new List<GameObject>(sceneCards);
        Shuffle(shuffled);

        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject card = shuffled[i];
            card.transform.position = deckPosition.position + new Vector3(0, i * 0.1f, 0);
            card.transform.rotation = Quaternion.Euler(0, 90f, 0);
            // card.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
