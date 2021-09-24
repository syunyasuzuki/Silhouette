using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    FlashCtrl flashCtrl;

    [SerializeField] GameObject eye_back;
    [SerializeField] GameObject eye;

    float enemy_alpha = 0.0f;

    float alpha_speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        color_switch = false;
        animator = GetComponent<Animator>();

        flashCtrl = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();

        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
        eye_back.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
        eye.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_alphaChange();
    }

    /// <summary>
    /// エネミーのa値変更
    /// </summary>
    void Enemy_alphaChange()
    {
        if (!flashCtrl.GetFlash())
        {
            if (enemy_alpha != 0.0f)
            {
                enemy_alpha -= alpha_speed * Time.deltaTime;
            }
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
            eye_back.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            eye.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, enemy_alpha);
        }
        else
        {
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

        Invoke(nameof(GameOver),2.0f);
    }

    void GameOver()
    {
        Fade_ctr.main_fade = true;
        Fade_ctr.main_fade_out = true;
    }
}