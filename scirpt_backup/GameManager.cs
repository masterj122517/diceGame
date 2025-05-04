using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DiceManager diceManager;
    public DiceThrower diceThrower;

    private bool isGameActive = true;
    private bool isPlayerTurn = true;
    private bool canThrowDice = true;
    private bool isProcessingResults = false;
    private int currentGame = 1; // 当前游戏局数
    private bool isInHeaven = false; // 是否进入天堂
    private bool isInHell = false; // 是否进入地狱

    void Start()
    {
        // 检查并获取必要的组件
        if (diceManager == null)
        {
            diceManager = GetComponent<DiceManager>();
            if (diceManager == null)
            {
                Debug.LogError(
                    "DiceManager 组件未找到！请确保 GameManager 对象上有 DiceManager 脚本。"
                );
                enabled = false;
                return;
            }
        }

        if (diceThrower == null)
        {
            diceThrower = GetComponent<DiceThrower>();
            if (diceThrower == null)
            {
                Debug.LogError(
                    "DiceThrower 组件未找到！请确保 GameManager 对象上有 DiceThrower 脚本。"
                );
                enabled = false;
                return;
            }
        }

        // 检查 DiceThrower 的设置
        if (diceThrower.playerDiceArray == null || diceThrower.playerDiceArray.Length == 0)
        {
            Debug.LogError(
                "DiceThrower 的 playerDiceArray 未设置！请在 Inspector 中设置骰子对象。"
            );
            enabled = false;
            return;
        }

        InitializeGame();
    }

    void Update()
    {
        if (!isGameActive)
            return;

        // 检查组件是否仍然有效
        if (diceThrower == null)
        {
            Debug.LogError("DiceThrower 引用丢失！");
            return;
        }

        // 检查是否可以投掷骰子
        if (canThrowDice && Input.GetKeyDown(KeyCode.Space))
        {
            ThrowDice();
        }

        // 检查骰子是否停止
        if (diceThrower.AreDiceStopped() && !canThrowDice && !isProcessingResults)
        {
            ProcessDiceResults();
        }
    }

    void ThrowDice()
    {
        if (diceThrower != null)
        {
            diceThrower.ThrowPlayerDice();
            canThrowDice = false;
            isProcessingResults = false;
        }
        else
        {
            Debug.LogError("无法投掷骰子：DiceThrower 为空！");
        }
    }

    void InitializeGame()
    {
        if (diceManager == null || diceThrower == null)
        {
            Debug.LogError("无法初始化游戏：缺少必要的组件！");
            return;
        }

        // 重置游戏状态
        diceManager.Player.points = 0;
        diceManager.Computer.points = 0;
        diceManager.Player.chips = 5;
        diceManager.Computer.chips = 5;
        isGameActive = true;
        isPlayerTurn = true;
        canThrowDice = true;
        isProcessingResults = false;
        diceManager.Player.round = 0;
        isInHeaven = false;
        isInHell = false;

        Debug.Log($"\n=== 第 {currentGame} 局游戏开始！===");
        Debug.Log("按空格键投掷骰子");
        StartNewRound();
    }

    void UpdateUI()
    {
        if (diceManager == null)
            return;
        Debug.Log($"当前局数: {currentGame}/3");
        Debug.Log($"当前回合: {diceManager.Player.round}");
        Debug.Log($"玩家筹码: {diceManager.Player.chips} | 电脑筹码: {diceManager.Computer.chips}");
    }

    void StartNewRound()
    {
        if (diceManager == null)
            return;

        diceManager.Player.round++;
        isPlayerTurn = true;
        canThrowDice = true;
        isProcessingResults = false;
        diceManager.UpdateRoundDiceNumber();
        Debug.Log($"\n=== 第 {diceManager.Player.round} 回合开始 ===");
        Debug.Log("按空格键投掷骰子");
        UpdateUI();
    }

    void ProcessDiceResults()
    {
        if (isPlayerTurn && !canThrowDice && !isProcessingResults)
        {
            if (diceManager == null || diceThrower == null)
                return;

            isProcessingResults = true;
            StartCoroutine(ProcessResultsAfterDelay());
        }
    }

    System.Collections.IEnumerator ProcessResultsAfterDelay()
    {
        yield return new WaitForEndOfFrame();

        if (diceManager == null || diceThrower == null)
            yield break;

        diceManager.UpdateDiceValues();

        // 生成电脑的骰子值
        diceManager.Computer.DiceValue = diceThrower.GenerateComputerDiceValues(
            diceManager.Computer.defaultDiceNumber
        );

        // 记录特殊骰子效果前的分数
        long playerPointsBefore = diceManager.Player.points;
        long computerPointsBefore = diceManager.Computer.points;

        // 计算基础分数
        diceManager.Player.points += diceManager.Player.DiceValue.Sum();
        diceManager.Computer.points += diceManager.Computer.DiceValue.Sum();

        // 输出骰子结果
        Debug.Log($"\n=== 第 {diceManager.Player.round} 回合结果 ===");
        Debug.Log($"玩家骰子: [{string.Join(", ", diceManager.Player.DiceValue)}]");
        Debug.Log($"电脑骰子: [{string.Join(", ", diceManager.Computer.DiceValue)}]");

        // 应用特殊骰子效果
        diceManager.ApplySpecialDiceEffect();

        // 输出特殊骰子效果
        string playerSpecialDiceType = GetSpecialDiceType(diceManager.Player.specialDiceIndex);
        string computerSpecialDiceType = GetSpecialDiceType(diceManager.Computer.specialDiceIndex);

        Debug.Log($"\n特殊骰子效果：");
        Debug.Log(
            $"玩家特殊骰子: 第{diceManager.Player.specialDiceIndex + 1}个骰子 - {playerSpecialDiceType}"
        );
        Debug.Log(
            $"电脑特殊骰子: 第{diceManager.Computer.specialDiceIndex + 1}个骰子 - {computerSpecialDiceType}"
        );

        // 输出分数变化
        Debug.Log($"\n分数变化：");
        Debug.Log(
            $"玩家: {playerPointsBefore} + {diceManager.Player.DiceValue.Sum()} = {diceManager.Player.points}"
        );
        Debug.Log(
            $"电脑: {computerPointsBefore} + {diceManager.Computer.DiceValue.Sum()} = {diceManager.Computer.points}"
        );

        isPlayerTurn = false;

        // 等待1秒后结束回合
        yield return new WaitForSeconds(1f);
        EndRound();
    }

    string GetSpecialDiceType(int specialDiceIndex)
    {
        switch (specialDiceIndex)
        {
            case 0:
                return "时间骰子";
            case 1:
                return "分裂骰子";
            case 2:
                return "寄生骰子";
            default:
                return "未知";
        }
    }

    void EndRound()
    {
        UpdateUI();
        if (diceManager == null)
            return;

        // 检查当前局是否结束
        if (diceManager.Player.chips <= 0 || diceManager.Computer.chips <= 0)
        {
            if (diceManager.Player.chips > diceManager.Computer.chips)
            {
                // 玩家赢得当前局
                if (currentGame == 3)
                {
                    isInHeaven = true;
                    EndGame();
                }
                else
                {
                    currentGame++;
                    Debug.Log($"\n恭喜你赢得第 {currentGame - 1} 局！");
                    InitializeGame();
                }
            }
            else
            {
                // 玩家输掉当前局
                if (currentGame == 3)
                {
                    isInHell = true;
                    EndGame();
                }
                else
                {
                    Debug.Log($"\n很遗憾，你输掉了第 {currentGame} 局！");
                    Debug.Log("重新开始游戏...");
                    currentGame = 1;
                    InitializeGame();
                }
            }
        }
        else
        {
            StartNewRound();
        }
    }

    void EndGame()
    {
        if (diceManager == null)
            return;

        isGameActive = false;

        if (isInHeaven)
        {
            Debug.Log("\n=== 恭喜你进入天堂！游戏胜利！===");
        }
        else if (isInHell)
        {
            Debug.Log("\n=== 很遗憾，你进入了地狱！游戏结束！===");
        }

        Debug.Log("\n按R键重新开始游戏");
    }

    public void RestartGame()
    {
        currentGame = 1;
        InitializeGame();
    }
}
