using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// 経過時間
    /// </summary>
    float elapsed;

    /// <summary>
    /// 揺れの起動スイッチ
    /// </summary>
    bool shake_switch;

    /// <summary>
    /// 揺れる前のカメラのポジション
    /// </summary>
    Vector3 cam_startpos;

    /// <summary>
    /// ゲームオーバ時の揺れの起動スイッチ
    /// </summary>
    [HideInInspector]public bool gameovershake_switch;

    void Start()
    {
        shake_switch = false;
        gameovershake_switch = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!shake_switch)
            {
                shake_switch = true;
                cam_startpos = transform.position;
            }
            //elapsed = 1.0f;
        }

        if(shake_switch)
        {
            Shake(1.0f,1.0f);
        }

        if(gameovershake_switch)
        {
            cam_startpos = transform.position;
            Shake(0.5f, 0.5f);
        }
    }

    /// <summary>
    /// 揺らす
    /// </summary>
    /// <param name="duration">揺れる時間</param>
    /// <param name="magnitude">揺らす強さ</param>
    void Shake(float duration,float magnitude)
    {
        if(elapsed<=0.0f)
        {
            elapsed = duration;
        }
        
        elapsed -= Time.deltaTime;

        //正規化
        float n = 1f / duration * elapsed;
        float x = cam_startpos.x + Random.Range(-n, n) * magnitude;
        float y = cam_startpos.y + Random.Range(-n, n) * magnitude;

        transform.localPosition = new Vector3(x, y, cam_startpos.z);


        if (elapsed <= 0f) {
            shake_switch = false;
            transform.localPosition = cam_startpos;
        }
    }
}
