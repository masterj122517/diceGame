using UnityEngine;

public class SceneDataManager
{
    public static SceneDataManager Instance { get; } = new SceneDataManager();

    private int[] diceValues = new int[3]; // 玩家骰子值
    private int[] computerDiceValues = new int[3]; // 电脑骰子值

    // 添加骰子数目变量
    public int playerDiceCount = 3; // 玩家骰子数目，默认为3
    public int computerDiceCount = 3; // 电脑骰子数目，默认为3
    public int speicalPlayerDiceIndex = 0;
    public int speicalComputerDiceIndex = 0;

    //
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

    // 设置和获取玩家骰子数目
    public void SetPlayerDiceCount(int count)
    {
        if (count > 0)
        {
            playerDiceCount = count;
            // 调整骰子值数组大小
            diceValues = new int[count];
        }
    }

    public int GetPlayerDiceCount()
    {
        return playerDiceCount;
    }

    // 设置和获取电脑骰子数目
    public void SetComputerDiceCount(int count)
    {
        if (count > 0)
        {
            computerDiceCount = count;
            // 调整骰子值数组大小
            computerDiceValues = new int[count];
        }
    }

    public int GetComputerDiceCount()
    {
        return computerDiceCount;
    }

    // 原有的方法保持不变，但需要根据新的数组大小调整
    public void SetDiceValues(int[] values)
    {
        diceValues = values;
    }

    public int[] GetDiceValues()
    {
        return diceValues;
    }

    public void SetComputerDiceValues(int[] values)
    {
        computerDiceValues = values;
    }

    public int[] GetComputerDiceValues()
    {
        return computerDiceValues;
    }

    public void resetPlayer()
    {
        playerDiceCount = 3; // 玩家骰子数目，默认为3
    }

    public void resetComputer()
    {
        computerDiceCount = 3; // 电脑骰子数目，默认为3
    }
}

// 确保SceneDataManager在同一个命名空间中
public class DiceValueDetector : MonoBehaviour
{
    public Transform[] diceArray; // 骰子数组
    public Transform ground; // 地面对象
    private int[] diceValues = new int[3]; // 存储三个骰子的点数
    private bool hasOutput = false; // 是否已经输出过点数

    private int PlayerDiceCount = SceneDataManager.Instance.GetPlayerDiceCount();

    private int diceMap(int dice)
    {
        if (dice == 1)
            return 6;
        if (dice == 2)
            return 4;
        if (dice == 3)
            return 5;
        if (dice == 4)
            return 2;
        if (dice == 5)
            return 3;
        if (dice == 6)
            return 1;
        return dice; // 如果不在映射范围内，直接返回原值
    }

    void Update()
    {
        // 检查所有骰子是否都已停止
        if (AreAllDiceStopped())
        {
            // 获取所有骰子的点数
            for (int i = 0; i < PlayerDiceCount; i++)
            {
                int value = GetDiceValue(diceArray[i]);
                if (value > 0 && value <= 6) // 确保值在有效范围内
                {
                    diceValues[i] = diceMap(value);
                }
                else
                {
                    // Debug.LogWarning($"骰子 {i + 1} 的值无效: {value}");
                }
            }
            // Debug.Log("Dice Values: " + string.Join(", ", diceValues));
        }
    }

    public bool AreAllDiceStopped()
    {
        foreach (Transform dice in diceArray)
        {
            if (!dice.gameObject.activeInHierarchy)
                continue; // 跳过被隐藏的骰子

            Rigidbody rb = dice.GetComponent<Rigidbody>();
            if (
                rb != null
                && (rb.velocity.magnitude > 0.01f || rb.angularVelocity.magnitude > 0.01f)
            )
            {
                return false;
            }
        }
        return true;
    }

    private int GetDiceValue(Transform dice)
    {
        // 获取骰子的六个面
        Transform[] faces = new Transform[6];
        for (int i = 0; i < 6; i++)
        {
            faces[i] = dice.GetChild(i); // 每个子对象代表一个面
        }

        // 检查每个面是否与地面接触
        for (int i = 0; i < faces.Length; i++)
        {
            // 获取面的碰撞器
            Collider faceCollider = faces[i].GetComponent<Collider>();
            if (faceCollider != null)
            {
                // 检查这个面是否与地面接触
                Collider[] colliders = Physics.OverlapBox(
                    faceCollider.bounds.center,
                    faceCollider.bounds.extents
                );
                foreach (Collider col in colliders)
                {
                    if (col.gameObject == ground.gameObject)
                    {
                        // 返回接触面的值
                        return i + 1;
                    }
                }
            }
        }

        // Debug.LogWarning($"骰子 {dice.name} 没有检测到与地面的接触");
        return 0; // 如果没有检测到接触，返回0而不是1000
    }

    // 获取所有骰子的点数
    public int[] GetDiceValues()
    {
        if (AreAllDiceStopped())
        {
            // 检查是否有无效值
            for (int i = 0; i < PlayerDiceCount; i++)
            {
                if (diceValues[i] <= 0 || diceValues[i] > 6)
                {
                    // Debug.LogError($"骰子 {i + 1} 的值无效: {diceValues[i]}");
                    return null;
                }
            }
            return diceValues;
        }
        return null;
    }

    public void TransferDiceValuesToScene()
    {
        int[] diceValues = GetDiceValues();
        if (diceValues != null)
        {
            SceneDataManager.Instance.SetDiceValues(diceValues);
        }
    }
}
