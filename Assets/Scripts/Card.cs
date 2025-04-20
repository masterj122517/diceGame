using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardInfo cardInfo; // 卡牌类型信息
    private SceneDataManager dataManager; // 场景数据管理器

    void Start()
    {
        cardInfo = GetComponent<CardInfo>();
        dataManager = SceneDataManager.Instance;
    }

    // 执行卡牌效果
    public void ExecuteCard()
    {
        switch (cardInfo.GetCardType())
        {
            case CardInfo.CardType.Plunder:
                // 掠夺：夺取对手1枚筹码
                int computerChips = dataManager.getComputerChips();
                if (computerChips > 0)
                {
                    dataManager.setComputerChips(computerChips - 1);
                    dataManager.setPlayerChips(dataManager.getPlayerChips() + 1);
                }
                break;

            case CardInfo.CardType.Luck:
                // 运势：双方积分±随机值（范围1-6）
                int randomValue = Random.Range(1, 7);
                if (Random.value > 0.5f)
                {
                    // 玩家加分，电脑减分
                    dataManager.AddPlayerScore(randomValue);
                    dataManager.AddComputerScore(-randomValue);
                }
                else
                {
                    // 玩家减分，电脑加分
                    dataManager.AddPlayerScore(-randomValue);
                    dataManager.AddComputerScore(randomValue);
                }
                break;

            case CardInfo.CardType.Reverse:
                // 逆转：下一回合所有骰子显示（7-原点数）
                // 这个效果需要在GameManager中处理骰子点数时实现
                // 这里只是标记状态
                dataManager.NextRound();
                break;

            case CardInfo.CardType.Death:
                // 死亡：接下来3回合，每次投掷自动损失1筹码
                // 这个效果需要在GameManager中每回合开始时检查
                // 这里只是标记状态
                for (int i = 0; i < 3; i++)
                {
                    if (dataManager.getPlayerChips() > 0)
                    {
                        dataManager.setPlayerChips(dataManager.getPlayerChips() - 1);
                    }
                }
                break;
        }
    }

    // 不执行卡牌效果（可以添加一些取消效果的逻辑）
    public void CancelCard()
    {
        // 这里可以添加一些取消效果的逻辑
        // 比如显示取消动画，播放音效等
        Debug.Log($"取消使用{cardInfo.GetCardType()}卡");
    }

    // 获取卡牌类型
    public CardInfo.CardType GetCardType()
    {
        return cardInfo.GetCardType();
    }
}
