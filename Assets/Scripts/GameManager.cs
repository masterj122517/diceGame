using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private bool canUseCard = false; // 是否可以使用卡牌
    private Card currentCard = null; // 当前发出的卡牌
    private bool shouldDealCard = false; // 是否应该发牌

    void Awake()
    {
        // 确保只保留一个 GameManager 实例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 不在场景切换时销毁
            SceneManager.sceneLoaded += OnSceneLoaded; // 注册场景加载事件
        }
        else
        {
            Destroy(gameObject); // 防止重复实例
        }
    }

    void Start()
    {
        // Debug.Log("Welcome To The DiceGame");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 开始新一轮游戏
            SceneManager.LoadScene("DicePlay");
            shouldDealCard = true; // 设置标志，表示这一轮需要发牌
            canUseCard = false;
            currentCard = null;

            // 洗牌
            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null)
            {
                deckManager.ShuffleAndArrange();
                deckManager.StartNewTurn();
            }
        }

        // 在骰子游戏结束时自动发牌
        if (
            SceneManager.GetActiveScene().name == "MainScence"
            && shouldDealCard
            && !canUseCard
            && currentCard == null
        )
        {
            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null)
            {
                currentCard = deckManager.MoveTopCardToPlayerHand();
                if (currentCard != null)
                {
                    canUseCard = true;
                    shouldDealCard = false;
                    Debug.Log($"抽到了一张{currentCard.GetCardType()}卡，按Y使用，按N取消");
                }
            }
        }

        // 按Y使用卡牌，按N取消
        if (canUseCard && currentCard != null)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                currentCard.ExecuteCard();
                Debug.Log($"使用了{currentCard.GetCardType()}卡");
                canUseCard = false;
                // 不清空currentCard，让卡牌保持在玩家位置
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                currentCard.CancelCard();
                canUseCard = false;
                // 不清空currentCard，让卡牌保持在玩家位置
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 移除了在场景加载时的洗牌逻辑，因为我们只在按空格开始新一轮时洗牌
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
