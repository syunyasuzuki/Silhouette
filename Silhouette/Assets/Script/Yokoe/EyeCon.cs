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
    private Vector3 target_pos;

    ///// <summary>
    ///// 追跡速度の減衰
    ///// </summary>
    //private float attenuation = 0.5f;

    /// <summary>
    /// 目の移動範囲の半径
    /// </summary>
    float radius = 1.0f;

    //敵胴体スクリプト
    EnemyCon enemycon;

    Vector3 enemy_pos;


    /// <summary>
    /// 目標地点
    /// </summary>
    Vector3 dest_pos;

    // Start is called before the first frame update
    void Start()
    {
        //敵胴体&座標取得
        Enemybody = GameObject.FindGameObjectWithTag("Enemybody");

        //スクリプト取得
        enemycon = Enemybody.GetComponent<EnemyCon>();

        //Random_pos();
        //StartCoroutine(nameof(Move_Delay));

    }

    // Update is called once per frame
    void Update()
    {
        //敵の胴体とターゲットの座標を取得
        enemy_pos = Enemybody.transform.position;
        target_pos = enemycon.target.transform.position;

        Eye_move();
        //Move_Ramdom();

        //transform.position = Vector3.MoveTowards(transform.position, dest_pos, Time.deltaTime);
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
            transform.position = new Vector2(enemy_pos.x + radius * direction.x, enemy_pos.y + radius * direction.y);
        }
    }

    //void Move_Ramdom()
    //{
    //    //ベクトル
    //    Vector3 direction = (target_pos - dest_pos);
    //    direction = direction.normalized;

    //    transform.position = Vector3.MoveTowards(transform.position, dest_pos, 20.0f * Time.deltaTime);

    //    if (transform.position==dest_pos)
    //    {
    //        Debug.Log("Random");
    //        Debug.Log(dest_pos);
    //        Random_pos();
    //    }
    //}

    void Random_pos()
    {
        dest_pos = new Vector3(Random.Range((float)System.Math.Truncate(enemy_pos.x - 1),
                                            (float)System.Math.Truncate(enemy_pos.x + 1)),
                               Random.Range((float)System.Math.Truncate(enemy_pos.y - 1),
                                            (float)System.Math.Truncate(enemy_pos.y)), 0.0f);
    }

    private IEnumerator Move_Delay()
    {
        while(true)
        {
            //ベクトル
            Vector3 direction = (dest_pos - enemy_pos);
            direction = direction.normalized;

            if (transform.position == dest_pos)
            {
                yield return new WaitForSeconds(1);
                //Debug.Log(dest_pos);
                Random_pos();
                //Debug.Log("距離"+(dest_pos - enemy_pos).magnitude);
                if ((dest_pos - enemy_pos).magnitude <= 1.0f)
                {
                    Debug.Log("再");
                    Random_pos();
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, dest_pos, Time.deltaTime);
            }
        }
      
    }
}
