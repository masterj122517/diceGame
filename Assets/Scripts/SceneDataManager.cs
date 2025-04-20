using UnityEngine;

public class SceneDataManager
{
    private static SceneDataManager instance;
    public static SceneDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SceneDataManager();
            }
            return instance;
        }
    }

    // 游戏状态数据
    private int[] diceValues = new int[3]; // 玩家骰子值
    private int[] computerDiceValues = new int[3]; // 电脑骰子值
    public int playerDiceCount = 3; // 玩家骰子数目，默认为3
    public int computerDiceCount = 3; // 电脑骰子数目，默认为3
    public int speicalPlayerDiceIndex = 0;
    public int speicalComputerDiceIndex = 0;

    // 游戏分数相关
    public int playerScore = 0;
    public int computerScore = 0;
    public int currentRound = 1;
    private const int MAX_ROUNDS = 3;

    // chips

    public int playerChips = 5;
    public int computerChips = 5;

    public int getPlayerChips() => playerChips;

    public int getComputerChips() => computerChips;

    public void setComputerChips(int chips) => playerChips = chips;

    public void setPlayerChips(int chips) => playerChips = chips;

    private SceneDataManager()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        playerScore = 0;
        computerScore = 0;
        currentRound = 1;
        resetPlayer();
        resetComputer();
    }

    // 回合管理
    public int GetCurrentRound() => currentRound;

    public void NextRound() => currentRound = Mathf.Min(currentRound + 1, MAX_ROUNDS);

    public bool IsGameOver() => currentRound > MAX_ROUNDS;

    // 分数管理
    public void AddPlayerScore(int score) => playerScore += score;

    public void AddComputerScore(int score) => computerScore += score;

    public int GetPlayerScore() => playerScore;

    public int GetComputerScore() => computerScore;

    // 特殊骰子索引
    public int getSpeicalPlayerDiceIndex()
    {
        speicalPlayerDiceIndex = Random.Range(1, 4);
        return speicalPlayerDiceIndex;
    }

    public int getSpeicalComputerDiceIndex()
    {
        speicalComputerDiceIndex = Random.Range(1, 4);
        return speicalComputerDiceIndex;
    }

    // 玩家骰子管理
    public void SetPlayerDiceCount(int count)
    {
        if (count > 0)
        {
            playerDiceCount = count;
            diceValues = new int[count];
        }
    }

    public int GetPlayerDiceCount() => playerDiceCount;

    // 电脑骰子管理
    public void SetComputerDiceCount(int count)
    {
        if (count > 0)
        {
            computerDiceCount = count;
            computerDiceValues = new int[count];
        }
    }

    public int GetComputerDiceCount() => computerDiceCount;

    // 骰子值管理
    public void SetDiceValues(int[] values) => diceValues = values;

    public int[] GetDiceValues() => diceValues;

    public void SetComputerDiceValues(int[] values) => computerDiceValues = values;

    public int[] GetComputerDiceValues() => computerDiceValues;

    // 重置功能
    public void resetPlayer() => playerDiceCount = 3;

    public void resetComputer() => computerDiceCount = 3;
}

