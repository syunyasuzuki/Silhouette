using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    /// <summary>
    /// 白目
    /// </summary>
    GameObject eye_back;
    /// <summary>
    /// 目玉
    /// </summary>
    GameObject eye;

    /// <summary>
    /// エネミーオーラの色
    /// </summary>
    float red;

    /// <summary>
    /// オーラの色を切り替えるタイミング
    /// </summary>
    bool color_switch;

    /// <summary>
    /// オーラの点滅速度
    /// </summary>
    float aura_speed = 0.3f;

    /// <summary>
    /// アニメーター
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ゲームオーバーシーンのオーラの速度
    /// </summary>
    float eat_red_speed = 1.0f;

    /// <summary>
    /// 光るタイミング
    /// </summary>
    FlashCtrl flashCtrl;

    /// <summary>
    /// アルファ値
    /// </summary>
    float enemy_alpha = 0.0f;

    /// <summary>
    /// アルファ値の変更スピード
    /// </summary>
    float alpha_speed = 3.0f;

    /// <summary>
    /// ターゲット
    /// </summary>
    [HideInInspector] public GameObject target;

    /// <summary>
    /// メインカメラ
    /// </summary>
    Camera cam;

    /// <summary>
    /// ゲームオーバー時
    /// </summary>
    bool gameover_switch;

    /// <summary>
    /// ゲームオーバーの時の敵の大きさ
    /// </summary>
    float gameover_scale = 5.0f;

    /// <summary>
    /// カメラを揺らすScript
    /// </summary>
    CameraShake cam_shake; 

    // Start is called before the first frame update
    void Start()
    {
        color_switch = false;
        gameover_switch=false;

        //アニメーター取得
        animator = GetComponent<Animator>();

        //白目＆目玉取得
        eye_back = GameObject.FindGameObjectWithTag("EnemyeyeBack");
        eye = GameObject.FindGameObjectWithTag("Enemyeye");

        Enemy_alphazero();

        //メインカメラ取得
        cam = Camera.main;

        //カメラを揺らすScript取得
        cam_shake = cam.GetComponent<CameraShake>();

        //ターゲット取得
        target = GameObject.FindGameObjectWithTag("Player");

        //光るタイミングの判定を取得
        flashCtrl = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームがスタートしたら
       if(Player_test.game_check)
        {
            Enemy_move();
            Enemy_alphaChange();
            Debug.Log("DeadPoint_ctr.hide_check:" + DeadPoint_ctr.hide_check);
        }
        else
        {
            Enemy_alphazero();
        }
    }

    /// <summary>
    /// プレイヤー追従
    /// </summary>
    void Enemy_move()
    {
        //ゲームオーバーではないとき
        if (!gameover_switch)
        {
            if (enemy_alpha <= 0.0f)
            {
                transform.position = new Vector3(target.transform.position.x, cam.transform.position.y + 3.0f, 0);
            }
        }
        else
        {
            //敵　拡大
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
            transform.localScale = new Vector3(gameover_scale, gameover_scale, 0);
            transform.position = new Vector3(target.transform.position.x, 0, transform.position.z);
        }
    }

    /// <summary>
    /// アルファ値を0にする
    /// </summary>
    void Enemy_alphazero()
    {
        enemy_alpha = 0.0f;                                                                                                                                                             
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
        eye_back.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        eye.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// エネミーのa値変更
    /// </summary>
    void Enemy_alphaChange()
    {
        if (!flashCtrl.GetFlash())
        {
            //光っていないとき
            if (enemy_alpha != 0.0f)
            {
                enemy_alpha -= alpha_speed * Time.deltaTime;
            }
            GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
            eye_back.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            eye.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
            //光っているとき
            enemy_alpha = 1.0f;
            GetComponent<SpriteRenderer>().color = new Color(red, 0.0f, 0.0f, enemy_alpha);
            eye_back.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemy_alpha);
            eye.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemy_alpha);

            if (DeadPoint_ctr.hide_check)
            {
                Enemy_aura();
            }
        }
    }

    /// <summary>
    /// エネミーのオーラ調整
    /// </summary>
    void Enemy_aura()
    {
        Debug.Log("起動");
        if (color_switch)
        {
            red += aura_speed * Time.deltaTime;
            if (red >= 0.5f)
            {
                color_switch = false;
            }
        }
        if (!color_switch)
        {
            red -= aura_speed * Time.deltaTime;
            if (red <= 0.2f)
            {
                color_switch = true;
            }
        }
    }

    /// <summary>
    /// ゲームオーバーシーンの捕食
    /// </summary>
    public void Enemy_Eat()
    {
        Debug.Log("color_switch:" + color_switch);
        red +=eat_red_speed*Time.deltaTime;
        if (red >= 1.0f) 
        {
            animator.SetFloat("EatFloat", 0.5f);
        }

        Invoke(nameof(GameOver),2.5f);
    }

    /// <summary>
    /// ゲームオーバーシーンのフェード
    /// </summary>
    void GameOver()
    {
        cam_shake.gameovershake_switch = true;
        gameover_switch = true;
        Fade_ctr.main_fade = true;
        Fade_ctr.main_fade_out = true;
    }
}