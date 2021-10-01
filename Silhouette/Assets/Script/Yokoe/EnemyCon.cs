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

    bool gameover_switch;

    float gameover_scale = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        color_switch = false;
        animator = GetComponent<Animator>();
        gameover_switch=false;

        //白目＆目玉取得
        eye_back = GameObject.FindGameObjectWithTag("EnemyeyeBack");
        eye = GameObject.FindGameObjectWithTag("Enemyeye");

        Enemy_alphazero();

        //メインカメラ取得
        cam = Camera.main;

        //ターゲット取得
        target = GameObject.FindGameObjectWithTag("Player");

        flashCtrl = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが動けるとき
       if(Player_test.game_check||Player_test.move_check)
        {
            Enemy_move();
        }
        Enemy_alphaChange();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Enemy_move()
    {
        //ゲームオーバーではないとき
        if (!gameover_switch)
        {
            transform.position = new Vector3(target.transform.position.x, cam.transform.position.y + 3.0f, 0);
        }
        else
        {
            transform.localScale = new Vector3(gameover_scale, gameover_scale, 0);
            transform.position = new Vector3(transform.position.x, cam.transform.position.y - 1.0f, 0);
        }
    }

    /// <summary>
    /// アルファ値を0にする
    /// </summary>
    void Enemy_alphazero()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
            eye_back.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            eye.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
            //光っているとき
            enemy_alpha = 1.0f;
            Enemy_aura();
            gameObject.GetComponent<SpriteRenderer>().color = new Color(red, 0.0f, 0.0f, enemy_alpha);
            eye_back.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemy_alpha);
            eye.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemy_alpha);
        }
    }

    /// <summary>
    /// エネミーのオーラ調整
    /// </summary>
    void Enemy_aura()
    {
        if(color_switch)
        {
            red += aura_speed * Time.deltaTime;
            if (red >= 0.5f)
            {
                color_switch = false;
            }
        }
        if(!color_switch)
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
        red +=eat_red_speed*Time.deltaTime;
       
        if (red >= 1.0f) 
        {
            animator.SetFloat("EatFloat", 0.5f);
        }

        Invoke(nameof(GameOver),3.0f);
    }

    /// <summary>
    /// ゲームオーバーシーンのフェード
    /// </summary>
    void GameOver()
    {
        gameover_switch = true;
        Fade_ctr.main_fade = true;
        Fade_ctr.main_fade_out = true;
    }
}