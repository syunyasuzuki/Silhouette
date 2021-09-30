using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCon : MonoBehaviour
{
    /// <summary>
    /// 敵胴体
    /// </summary>
    GameObject Enemybody;

    /// <summary>
    /// ターゲットの座標
    /// </summary>
    public Vector3 target_pos;

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

    //敵胴体スクリプト
    EnemyCon enemycon;

    Vector3 enemy_pos;

    // Start is called before the first frame update
    void Start()
    {
        //敵胴体取得
        Enemybody = GameObject.FindGameObjectWithTag("Enemybody");

        enemy_pos = Enemybody.transform.position;

        //スクリプト取得
        enemycon = Enemybody.GetComponent<EnemyCon>();

        //初期位置を取得
        start_pos = enemy_pos;
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの座標を取得
        target_pos = enemycon.target.transform.position;
        Eye_move();
    }

    /// <summary>
    /// 追跡
    /// </summary>
    void Eye_move()
    {
        //ベクトル
        Vector3 direction = (target_pos - enemy_pos);

        //円の範囲内にプレイヤーがいるとき
        if (direction.magnitude < radius) 
        {
            transform.position = target_pos;
        }
        else
        {
            direction = direction.normalized;
            transform.position = new Vector2(enemy_pos.x + radius * direction.x, enemy_pos.y+radius * direction.y); 
        }
    }
}
