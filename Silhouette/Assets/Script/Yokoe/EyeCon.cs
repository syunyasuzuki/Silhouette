using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCon : MonoBehaviour
{
    [SerializeField] GameObject target;

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

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Enemybody = GameObject.Find("Enemybody");

        //初期位置を取得
        start_pos = Enemybody.transform.position;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの座標を取得
        target_pos = target.transform.position;
        Eye_move();
    }

    /// <summary>
    /// 追跡
    /// </summary>
    void Eye_move()
    {
        //ベクトル
        Vector3 direction = (target_pos - cam.transform.position);

        //円の範囲内にプレイヤーがいるとき
        if (direction.magnitude < radius) 
        {
            transform.position = target_pos;
        }
        else
        {
            direction = direction.normalized;
            transform.position = new Vector2(cam.transform.position.x+radius * direction.x, radius * direction.y);
           
        }
    }
}
