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
 
    bool death_switch;

    // Start is called before the first frame update
    void Start()
    {
        color_switch = false;
        animator = GetComponent<Animator>();

        death_switch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            death_switch = true;
        }

        if(death_switch)
        {
            Enemy_Eat();
        }
        else
        {
            Enemy_aura();
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(red, 0, 0);
        Debug.Log(death_switch);
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
    }
}