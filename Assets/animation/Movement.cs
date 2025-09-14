using System.Collections;
using UnityEngine;

public class RandomMove2D : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 0.7f;               // 移動速度
    public float minChangeTime = 1f;           // 方向最短持續時間
    public float maxChangeTime = 1.5f;           // 方向最長持續時間

    [Header("移動範圍限制")]
    public float minX = -5f; // 左邊界
    public float maxX = -2f;  // 右邊界

    [Header("停頓相關")]
    [Range(0f, 1f)] public float idleProbability = 0.2f;  // 停頓機率 (預設 60%)
    public float idleTimeMultiplier = 1f;                 // 停頓時間加長倍率 (預設 2 倍)

    private float direction = 0f;  // -1 左 / 1 右 / 0 停止
    private float timer = 0f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ChangeDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
        }

        // 移動角色
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // 範圍檢查：超過邊界就強制往反方向移動
        if (transform.position.x <= minX)
        {
            direction = 1f;
            ResetTimer();
        }
        else if (transform.position.x >= maxX)
        {
            direction = -1f;
            ResetTimer();
        }

        // 翻轉角色
        if (spriteRenderer != null)
        {
            if (direction > 0)
                spriteRenderer.flipX = false; // 面向右
            else if (direction < 0)
                spriteRenderer.flipX = true;  // 面向左
        }

        // 動畫控制
        if (animator != null)
        {
            animator.SetBool("walk", direction != 0); // 有移動才播放 walk
        }
    }

    void ChangeDirection()
    {
        float rand = Random.value; // 0~1 隨機數

        if (rand < idleProbability)
        {
            direction = 0f; // 停頓
        }
        else
        {
            direction = Random.value < 0.5f ? -1f : 1f; // 左或右
        }

        ResetTimer();
    }

    void ResetTimer()
    {
        if (direction == 0)
        {
            // 停頓更久
            timer = Random.Range(minChangeTime, maxChangeTime) * idleTimeMultiplier;
        }
        else
        {
            // 移動時間正常
            timer = Random.Range(minChangeTime, maxChangeTime);
        }
    }
}
