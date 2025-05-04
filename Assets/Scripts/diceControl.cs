using UnityEngine;

public class diceControl : MonoBehaviour
{
    public Transform[] diceArray; // diceArray[0] 特殊骰子 diceArray[1] 普通骰子1 diceArray[2] 普通骰子2
    public float spawnHeight = 8f;

    int PlayerDiceCount = SceneDataManager.Instance.GetPlayerDiceCount(); // 3 or 2

    void Start()
    {
        // 生成普通骰子（最多两个）
        if (PlayerDiceCount == 2)
        {
            Rigidbody rb = diceArray[2].GetComponent<Rigidbody>();
            rb.useGravity = false; // 关闭重力
        }
        for (int i = 0; i < PlayerDiceCount; i++)
        {
            SpawnDice(diceArray[i]);
        }
    }

    void SpawnDice(Transform dice)
    {
        float x = Random.Range(-4f, 4f);
        float z = Random.Range(-4f, 4f);
        Vector3 randomPosition = new Vector3(x, spawnHeight, z);
        Quaternion randomRotation = Random.rotation;

        dice.position = randomPosition;
        dice.rotation = randomRotation;

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
