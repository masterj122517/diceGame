using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateComputerDice : MonoBehaviour
{
    public Transform[] computerDice; // 电脑骰子数组
    private Dictionary<int, Vector3> diceRotations; // 存储每个值对应的旋转角度

    private int ComputerDiceCount;

    private Vector3[] dicePositions = new Vector3[] // 固定的骰子位置
    {
        new Vector3(-5.21f, 0.48f, 0.24f),
        new Vector3(-4.32f, 0.44f, -0.33f),
        new Vector3(-4.66f, 0.44f, 1.09f),
    };

    // 添加一个变量来存储电脑骰子的值
    private int[] computerDiceValues = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        ComputerDiceCount = SceneDataManager.Instance.GetComputerDiceCount();
        // 初始化骰子旋转字典
        InitializeDiceRotations();

        // 随机生成三个骰子值
        for (int i = 0; i < ComputerDiceCount; i++)
        {
            computerDiceValues[i] = Random.Range(1, 7); // 生成1-6的随机数
        }

        // 更新每个骰子的显示
        for (int i = 0; i < computerDice.Length; i++)
        {
            if (i < computerDiceValues.Length)
            {
                computerDice[i].gameObject.SetActive(true);
                UpdateDiceDisplay(computerDice[i], computerDiceValues[i], dicePositions[i]);
            }
            else
            {
                computerDice[i].gameObject.SetActive(false);
            }
        }

        // 将电脑骰子值保存到 SceneDataManager
        SaveComputerDiceValues();
    }

    private void InitializeDiceRotations()
    {
        diceRotations = new Dictionary<int, Vector3>
        {
            { 1, new Vector3(180, -90, 0) }, // 1点朝上
            { 2, new Vector3(90, -90, 0) }, // 2点朝上
            { 3, new Vector3(0, 0, -90) }, // 3点朝上
            { 4, new Vector3(-90, 0, 0) }, // 4点朝上
            { 5, new Vector3(0, 90, 90) }, // 5点朝上
            { 6, new Vector3(0, 0, 0) }, // 6点朝上
        };
    }

    private void UpdateDiceDisplay(Transform dice, int value, Vector3 position)
    {
        if (dice != null && diceRotations.ContainsKey(value))
        {
            // 设置骰子的旋转
            dice.rotation = Quaternion.Euler(diceRotations[value]);

            // 设置骰子的位置
            dice.position = position;
        }
    }

    // 添加新方法来保存电脑骰子值
    private void SaveComputerDiceValues()
    {
        // 这里我们可以添加一个新方法到 SceneDataManager 来存储电脑的骰子值
        SceneDataManager.Instance.SetComputerDiceValues(computerDiceValues);
    }

    // Update is called once per frame
    void Update() { }
}
