using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private int[] finalDiceValues; // 玩家骰子值
    private int[] computerDiceValues; // 电脑骰子值
    private int speicalComputerDiceIndex;
    private int speicalPlayerDiceIndex;

    public void PlayerTimeDice()
    {
        SceneDataManager.Instance.SetComputerDiceCount(2);
    }

    public void PlayerSplitDice()
    {
        finalDiceValues[0] *= 2;
        SceneDataManager.Instance.SetPlayerDiceCount(2);
    }

    public void PlayerparaDice()
    {
        finalDiceValues[0] = computerDiceValues.Where(x => x != 0).Min();
    }

    public void applyPlayerSpecialDice()
    {
        if (speicalPlayerDiceIndex == 1)
        {
            PlayerTimeDice();
        }
        if (speicalPlayerDiceIndex == 2)
            PlayerSplitDice();
        if (speicalPlayerDiceIndex == 3)
            PlayerparaDice();
    }

    public void ComputerTimeDice()
    {
        SceneDataManager.Instance.SetPlayerDiceCount(2);
    }

    public void ComputerSplitDice()
    {
        computerDiceValues[0] *= 2;
        SceneDataManager.Instance.SetComputerDiceCount(2);
    }

    public void ComputerparaDice()
    {
        computerDiceValues[0] = finalDiceValues.Where(x => x != 0).Min();
    }

    public void applyComputerSpecialDice()
    {
        if (speicalComputerDiceIndex == 1)
            ComputerTimeDice();
        if (speicalComputerDiceIndex == 2)
            ComputerSplitDice();
        if (speicalComputerDiceIndex == 3)
            ComputerparaDice();
    }

    private void ResetDiceCounts()
    {
        // 如果这轮抽到 Time 或 Split，就影响下一轮的骰子数量
        if (speicalPlayerDiceIndex == 1 || speicalPlayerDiceIndex == 2)
        {
            // Time 和 Split 的效果已经在 applyPlayerSpecialDice 中处理了
            // 这里不需要额外处理
        }
        else
        {
            // 如果这轮没抽到 Time 或 Split，就重置骰子数量
            SceneDataManager.Instance.resetPlayer();
        }

        if (speicalComputerDiceIndex == 1 || speicalComputerDiceIndex == 2)
        {
            // Time 和 Split 的效果已经在 applyComputerSpecialDice 中处理了
            // 这里不需要额外处理
        }
        else
        {
            // 如果这轮没抽到 Time 或 Split，就重置骰子数量
            SceneDataManager.Instance.resetComputer();
        }
    }

    void Start()
    {
        // 获取玩家骰子值
        int[] diceValues = SceneDataManager.Instance.GetDiceValues();
        // speicalPlayerDiceIndex = SceneDataManager.Instance.getSpeicalPlayerDiceIndex();
        // speicalComputerDiceIndex = SceneDataManager.Instance.getSpeicalComputerDiceIndex();
        speicalPlayerDiceIndex = Random.Range(1, 4);
        speicalComputerDiceIndex = Random.Range(1, 4);
        // 输出特殊骰子索引

        // 判断玩家骰子值是否有效（不为0）
        if (diceValues != null && diceValues[0] != 0)
        {
            finalDiceValues = diceValues;
            computerDiceValues = SceneDataManager.Instance.GetComputerDiceValues();
            ResetDiceCounts();
            // Debug.Log($"玩家骰子值: {string.Join(", ", finalDiceValues)}");
            // Debug.Log($"电脑骰子值: {string.Join(", ", computerDiceValues)}");
            applyPlayerSpecialDice();
            applyComputerSpecialDice();
            Debug.Log($"改变后玩家骰子值: {string.Join(", ", finalDiceValues)}");
            Debug.Log($"改变后电脑骰子值: {string.Join(", ", computerDiceValues)}");
            Debug.Log($"PlayerSpecialDiceIndex {speicalPlayerDiceIndex}");
            Debug.Log($"computerSpecialDiceIndex {speicalComputerDiceIndex}");
        }
    }
}
