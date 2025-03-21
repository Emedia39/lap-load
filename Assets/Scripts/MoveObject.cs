using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    Rigidbody rb;
    MoveObjectManager manager;
    bool isFalling = false;
    bool isStop = false;
    bool hasRegistered = false;
    bool hasStoppedReported = false;

    float stopTimer = 0f;
    float stopThreshold = 0.05f;     // 静止とみなす速度
    float requiredStopDuration = 1f; // 静止状態が続く秒数

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        manager = GameObject.FindObjectOfType<MoveObjectManager>();
    }

    void Update()
    {
        if (rb != null && !isStop)
        {
            float speed = rb.velocity.magnitude;

            // 動き始めたら登録
            if (speed > 0.5f && !hasRegistered)
            {
                isFalling = true;
                hasRegistered = true;
                manager.Register(this);
                Debug.Log($"{gameObject.name} が落下開始として登録されました");
            }

            if (isFalling && !hasStoppedReported)
            {
                if (speed < stopThreshold)
                {
                    // 停止状態ならタイマーを加算
                    stopTimer += Time.deltaTime;

                    if (stopTimer >= requiredStopDuration)
                    {
                        hasStoppedReported = true;
                        manager.NotifyStopped(this);
                        Debug.Log($"{gameObject.name} が静止を0.5秒確認して報告しました");
                        isStop = true;
                    }
                }
                else
                {
                    // 動いたらタイマーリセット
                    stopTimer = 0f;
                }
            }
        }
    }
}
