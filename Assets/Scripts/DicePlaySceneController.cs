using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DicePlayManager : MonoBehaviour
{
    public DiceValueDetector diceDetector;
    private bool isChecking = false;

    void Start()
    {
        StartCoroutine(CheckDiceValues());
    }

    private IEnumerator CheckDiceValues()
    {
        if (!isChecking)
        {
            isChecking = true;

            // 先等待一段时间，让骰子开始运动
            yield return new WaitForSeconds(2f);

            // 等待骰子停止
            while (!diceDetector.AreAllDiceStopped())
            {
                yield return new WaitForSeconds(0.2f);
            }

            // 等待更长时间确保检测稳定
            yield return new WaitForSeconds(1.5f);

            // 传递骰子值
            diceDetector.TransferDiceValuesToScene();

            // 打印值以验证
            int[] values = SceneDataManager.Instance.GetDiceValues();

            // 延长等待时间，让玩家能看清结果
            yield return new WaitForSeconds(1.5f);

            // 返回主场景
            SceneManager.LoadScene("MainScence");
        }
    }
}
