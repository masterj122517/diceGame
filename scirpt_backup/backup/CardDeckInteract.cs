using UnityEngine;

public class CardDeckInteract : MonoBehaviour
{
    public DeckManager deckManager;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // ������
        {
            deckManager.PlayerDraw();
            deckManager.AIDraw(); // �����˸���鿨
        }
    }
}