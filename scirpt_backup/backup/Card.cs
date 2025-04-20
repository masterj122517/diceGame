using UnityEngine;
using System.Collections;
public class Card : MonoBehaviour
{
    [Header("References")]
    public GameObject cardFront;

    public void Initialize(CardType type, Material material)
    {
        cardFront.GetComponent<Renderer>().material = material;
        cardFront.SetActive(false);
    }

    // 翻转动画（targetYRotation: 0或180）
    public void FlipCard(float targetYRotation, float duration)
    {
        StartCoroutine(FlipRoutine(targetYRotation, duration));
    }

    IEnumerator FlipRoutine(float targetYRot, float duration)
    {
        float startYRot = transform.eulerAngles.y;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float yRot = Mathf.Lerp(startYRot, targetYRot, elapsed / duration);
            transform.rotation = Quaternion.Euler(0, yRot, 0);

            // 在90度时切换牌面显示
            if (Mathf.Abs(yRot - 90) < 10 && !cardFront.activeSelf)
            {
                cardFront.SetActive(true);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, targetYRot, 0);
    }

    public void ResetCard()
    {
        cardFront.SetActive(false);
        transform.rotation = Quaternion.identity;
    }
}