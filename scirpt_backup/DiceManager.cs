// remember this script is meant to initialize the dice
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dice
{
    public int[] DiceValue = new int[3];
    public int chips = 5;
    public int defaultDiceNumber = 3;
    public int nextRoundDiceNumber = 3;
    public int specialDiceIndex = 0;
    public int opponentDiceNumber = 3;
    public int energy = 0;
    public long points = 0;
    public int round = 1; // 添加回合数
    public bool isAlive = true; // 添加存活状态
    public int consecutiveWins = 0; // 添加连续胜利次数
    public int totalRolls = 0; // 添加总掷骰次数

    void resetDiceNumber()
    {
        defaultDiceNumber = 3;
        nextRoundDiceNumber = 3;
        specialDiceIndex = 0;
        opponentDiceNumber = 3;
    }

    void TimeDice(Dice Player) // specialDiceIndex = 0
    {
        if (Player.DiceValue[Player.specialDiceIndex] >= 4)
        {
            Player.opponentDiceNumber = 2;
        }
    }

    void SplitDice(Dice Player) // specialDiceIndex = 1
    {
        if (Player.DiceValue[Player.specialDiceIndex] % 2 == 1)
        {
            Player.nextRoundDiceNumber = Player.defaultDiceNumber - 1;
            Player.DiceValue[Player.specialDiceIndex] =
                Player.DiceValue[Player.specialDiceIndex] * 2;
        }
    }

    void parasiteDice(Dice Player, Dice Computer) // specialDiceIndex = 2
    {
        if (
            Player.DiceValue[Player.specialDiceIndex]
                - Computer.DiceValue[Computer.specialDiceIndex]
            >= 3
        )
        {
            Player.DiceValue[Player.specialDiceIndex] = Computer.DiceValue.Min();
        }
    }

    public void applySpecialDice(Dice Player, Dice Computer)
    {
        Player.specialDiceIndex = UnityEngine.Random.Range(0, 2);
        if (Player.specialDiceIndex == 0)
        {
            TimeDice(Player);
        }
        else if (Player.specialDiceIndex == 1)
        {
            SplitDice(Player);
        }
        else if (Player.specialDiceIndex == 2)
        {
            parasiteDice(Player, Computer);
        }
    }
}

public class DiceManager : MonoBehaviour
{
    private DiceValueDetector diceValueDetector;

    public Dice Player { get; private set; } = new Dice();
    public Dice Computer { get; private set; } = new Dice();

    private bool hasProcessedThisRound = false; // 防止重复计算

    public void UpdateRoundDiceNumber()
    {
        Player.nextRoundDiceNumber = Computer.opponentDiceNumber;
        Player.defaultDiceNumber = Player.nextRoundDiceNumber;
        Computer.nextRoundDiceNumber = Player.opponentDiceNumber;
        Computer.defaultDiceNumber = Computer.nextRoundDiceNumber;
    }

    public void generateComputerDiceValue(int[] diceValue)
    {
        for (int i = 0; i < diceValue.Length; i++)
        {
            Computer.DiceValue[i] = UnityEngine.Random.Range(1, 6);
        }
        Computer.totalRolls++;
    }

    public bool AreDiceStopped()
    {
        diceValueDetector = GetComponent<DiceValueDetector>();
        return diceValueDetector != null && diceValueDetector.AreAllDiceStopped();
    }

    public void UpdateDiceValues()
    {
        if (AreDiceStopped())
        {
            Player.DiceValue = diceValueDetector.GetDiceValues();
            generateComputerDiceValue(Computer.DiceValue);
            Player.totalRolls++;
        }
    }

    public void ApplySpecialDiceEffect()
    {
        Player.applySpecialDice(Player, Computer);
        Computer.applySpecialDice(Computer, Player);
    }

    public void ResetGame()
    {
        Player = new Dice();
        Computer = new Dice();
        hasProcessedThisRound = false;
    }

    // void Start() { }

    // void Update() { }
}
