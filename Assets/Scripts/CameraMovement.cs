using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform targetPosition; // 目标位置（拖拽赋值）
    public float moveSpeed = 0.01f; // 移动速度
    public bool moveOnStart = true; // 是否在游戏开始时自动移动

    private Vector3 startPosition; // 初始位置
    private static Vector3 lastTargetPosition; // 记录上一次的目标位置

    void Start()
    {
        // 如果有上一次的目标位置，就使用它作为起始位置
        if (lastTargetPosition != Vector3.zero)
        {
            transform.position = lastTargetPosition;
        }
        startPosition = transform.position;
        if (moveOnStart)
        {
            StartCoroutine(MoveCamera());
        }
    }

    // 协程实现平滑移动
    private System.Collections.IEnumerator MoveCamera()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }
        // 移动完成后，更新上一次的目标位置
        lastTargetPosition = targetPosition.position;
    }

    // 外部调用方法（例如按钮触发）
    public void StartMoving()
    {
        StartCoroutine(MoveCamera());
    }

    // 重置摄像机位置的方法
    public static void ResetCameraPosition()
    {
        lastTargetPosition = Vector3.zero;
    }
}

