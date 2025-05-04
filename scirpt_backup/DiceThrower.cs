using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    public Transform[] playerDiceArray; // 玩家的骰子数组
    public float spawnHeight = 10f; // 生成高度
    private bool isThrowing = false;

    // 投掷玩家的骰子
    public void ThrowPlayerDice()
    {
        if (isThrowing) return;
        isThrowing = true;

        foreach (Transform dice in playerDiceArray)
        {
            // 在矩形范围内随机生成 X 和 Z 坐标
            float x = Random.Range(-5f, 2f);
            float z = Random.Range(-8f, 1f);

            Vector3 randomPosition = new Vector3(x, spawnHeight, z);
            Quaternion randomRotation = Random.rotation;

            // 设置位置和旋转
            dice.position = randomPosition;
            dice.rotation = randomRotation;

            // 给予初始力和旋转力
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(Random.insideUnitSphere * 1f, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
            }
        }
    }

    // 生成电脑的骰子值
    public int[] GenerateComputerDiceValues(int diceCount)
    {
        int[] values = new int[diceCount];
        for (int i = 0; i < diceCount; i++)
        {
            values[i] = Random.Range(1, 7); // 生成1到6的随机数
        }
        return values;
    }

    // 检查骰子是否停止
    public bool AreDiceStopped()
    {
        foreach (Transform dice in playerDiceArray)
        {
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            if (rb != null && (rb.velocity.magnitude > 0.01f || rb.angularVelocity.magnitude > 0.01f))
            {
                return false;
            }
        }
        isThrowing = false;
        return true;
    }
} 