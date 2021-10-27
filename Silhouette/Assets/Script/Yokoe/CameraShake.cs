using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// 経過時間
    /// </summary>
    float elapsed;

    bool shake_switch;

    Vector3 pos;

    public bool gameover_switch;

    void Start()
    {
        shake_switch = false;
        gameover_switch = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!shake_switch)
            {
                shake_switch = true;
                pos = transform.position;
            }
            elapsed = 1.0f;
        }

        if(shake_switch)
        {
            Shake(1.0f,1.0f);
        }

        if(gameover_switch)
        {
            Shake(1.0f, 0.5f);
        }
    }

    /// <summary>
    /// 揺らす
    /// </summary>
    /// <param name="duration">揺れる時間</param>
    /// <param name="magnitude">揺らす強さ</param>
    public void Shake(float duration,float magnitude)
    {
        elapsed = duration;
        elapsed -= Time.deltaTime;

        //正規化
        float n = 1f / duration * elapsed;
        float x = pos.x + Random.Range(-n, n) * magnitude;
        float y = pos.y + Random.Range(-n, n) * magnitude;

        transform.localPosition = new Vector3(x, y, pos.z);


        if (elapsed <= 0f) {
            shake_switch = false;
            transform.localPosition = pos;
        }

    }
}
