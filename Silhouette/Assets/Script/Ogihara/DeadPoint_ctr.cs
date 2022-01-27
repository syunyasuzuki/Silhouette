using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPoint_ctr : MonoBehaviour
{
    /// <summary>
    /// 隠れているとき　true
    /// </summary>
    public static bool hide_check;
    [SerializeField] GameObject exc_mark;
    float alpha;
    FlashCtrl flashcon;
    public static bool exc_mark_check;
    EnemyCon enemycon;
    public GameObject Came;
    // Start is called before the first frame update
    void Start()
    {
        exc_mark_check = false;
        hide_check = false;
        flashcon = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();
        alpha = 0;
        exc_mark.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        enemycon = GameObject.Find("Enemybody").GetComponent<EnemyCon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashcon.GetFlash() == true && hide_check == false)
        {
            GetComponent<Collider2D>().enabled = false;
            alpha = 1;
            flashcon.StopFlash();
            exc_mark.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            Player_test.move_check = false;
            exc_mark_check = true;
            enemycon.Enemy_Eat();
            Came.GetComponent<Player_Came>().enabled = false;
        }       
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            hide_check = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {          
            hide_check = false;
        }
    }
}
