using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geneartePlayeyDice : MonoBehaviour
{
    public Transform[] playerDice;
    private int PlayerDiceCount;

    private Dictionary<int, Vector3> diceRotations;
    private Vector3[] dicePositions = new Vector3[]
    {
        new Vector3(1.91f, 0.442f, -1.72f),
        new Vector3(2.52f, 0.44f, -0.87f),
        new Vector3(1.68f, 0.48f, -0.22f),
    };

    void Start()
    {
        PlayerDiceCount = SceneDataManager.Instance.GetPlayerDiceCount();
        InitializeDiceRotations();

        int[] diceValues = SceneDataManager.Instance.GetDiceValues();

        for (int i = 0; i < playerDice.Length; i++)
        {
            if (i < PlayerDiceCount)
            {
                playerDice[i].gameObject.SetActive(true);
                UpdateDiceDisplay(playerDice[i], diceValues[i], dicePositions[i]);
            }
            else
            {
                playerDice[i].gameObject.SetActive(false);
            }
        }
    }

    private void InitializeDiceRotations()
    {
        diceRotations = new Dictionary<int, Vector3>
        {
            { 1, new Vector3(180, -90, 0) },
            { 2, new Vector3(90, -90, 0) },
            { 3, new Vector3(0, 0, -90) },
            { 4, new Vector3(-90, 0, 0) },
            { 5, new Vector3(0, 90, 90) },
            { 6, new Vector3(0, 0, 0) },
        };
    }

    private void UpdateDiceDisplay(Transform dice, int value, Vector3 position)
    {
        if (dice != null && diceRotations.ContainsKey(value))
        {
            dice.rotation = Quaternion.Euler(diceRotations[value]);
            dice.position = position;
        }
    }

    void Update() { }
}
