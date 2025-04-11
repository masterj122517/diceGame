using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

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
            Debug.Log("切换到 DicePlay 场景");
            SceneManager.LoadScene("DicePlay");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("加载完成场景：" + scene.name);

        if (scene.name == "MainScence")
        {
            Debug.Log("回到主场景，尝试洗牌");

            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null)
            {
                Debug.Log("找到 DeckManager，开始洗牌");
                deckManager.ShuffleAndArrange();
            }
            else
            {
                Debug.LogWarning("找不到 DeckManager！");
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
