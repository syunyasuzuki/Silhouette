using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Y : MonoBehaviour
{
    /// <summary>
    /// ターゲットの座標
    /// </summary>
    private Vector3 target_pos;

    /// <summary>
    /// 追跡速度の減衰
    /// </summary>
    private float attenuation = 0.5f;

    /// <summary>
    /// 目の移動範囲の半径
    /// </summary>
    float radius = 1.0f;

    /// <summary>
    /// 目玉初期位置
    /// </summary>
    private Vector3 start_pos;

    /// <summary>
    /// 敵胴体
    /// </summary>
    GameObject Enemybody;

    // Start is called before the first frame update
    void Start()
    {
        Enemybody = GameObject.Find("Enemybody");

        //初期位置を取得
        start_pos = Enemybody.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Eye_move();
    }

    /// <summary>
    /// 追跡
    /// </summary>
    void Eye_move()
    {
        //ベクトル
        Vector3 direction = (target_pos - start_pos);

        //円の範囲内にプレイヤーがいるとき
        if (direction.magnitude < radius) 
        {
            transform.position = target_pos;
        }
        else
        {
            direction = direction.normalized;
            transform.position = new Vector2(radius * direction.x, radius * direction.y);
        }
    }

    /// <summary>
    /// ターゲットの座標取得
    /// </summary>
    /// <param name="x">プレイヤーのx座標</param>
    /// <param name="y">プレイヤーのy座標</param>
    public void SetTargetPos(float x,float y)
    {
        target_pos = new Vector3(x, y, 0);
    }
}
