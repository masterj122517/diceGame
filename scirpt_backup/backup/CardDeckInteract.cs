using UnityEngine;

public class CardDeckInteract : MonoBehaviour
{
    public DeckManager deckManager;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // Êó±ê×ó¼ü
        {
            deckManager.PlayerDraw();
            deckManager.AIDraw(); // »úÆ÷ÈË¸úËæ³é¿¨
        }
    }
}