using UnityEngine;

public class CardInfo : MonoBehaviour
{
    // 卡牌类型枚举
    public enum CardType
    {
        Plunder, // 掠夺：夺取对手1枚筹码
        Luck, // 运势：双方积分±随机值（范围1-6）
        Reverse, // 逆转：下一回合所有骰子显示（7-原点数）
        Death, // 死亡：接下来3回合，每次投掷自动损失1筹码
    }

    [SerializeField]
    private CardType cardType; // 在Inspector中可以设置的卡牌类型

    // 获取卡牌类型
    public CardType GetCardType()
    {
        return cardType;
    }
}

