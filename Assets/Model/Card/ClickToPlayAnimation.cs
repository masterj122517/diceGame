using UnityEngine;

public class ClickToPlayAnimation : MonoBehaviour
{
    [Header("动画配置")]
    public Animator CardAnima;  // 需要控制动画的Animator组件
    public string AnimationTrigger = "Play"; // Animator中触发的参数名

    [Header("点击检测")]
    public LayerMask Clickable; // 可点击物体的层级（如UI或3D物体）
    public float maxDistance = 100f; // 射线检测最大距离

    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 创建射线（从摄像机到鼠标位置）
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 检测射线是否命中物体
            if (Physics.Raycast(ray, out hit, maxDistance, Clickable))
            {
                // 如果命中的物体是当前脚本挂载的物体
                if (hit.collider.gameObject == gameObject)
                {
                    // 触发动画
                    CardAnima.SetTrigger(AnimationTrigger);
                }
            }
        }
    }
}