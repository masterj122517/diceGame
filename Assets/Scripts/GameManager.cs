using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // int round = 1;

    void Start()
    {
        Debug.Log("Welcome To The DiceGame");
    }

    void Update()
    {
        // 按空格键切换到骰子场景
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // if (
            //     !(
            //         SceneDataManager.Instance.speicalPlayerDiceIndex == 1
            //         || SceneDataManager.Instance.speicalPlayerDiceIndex == 2
            //     )
            // )
            // {
            //     SceneDataManager.Instance.resetPlayer();
            // }
            // if (
            //     !(
            //         SceneDataManager.Instance.speicalComputerDiceIndex == 1
            //         || SceneDataManager.Instance.speicalComputerDiceIndex == 2
            //     )
            // )
            // {
            //     SceneDataManager.Instance.resetComputer();
            // }
            SceneManager.LoadScene("DicePlay");
        }
    }
}
