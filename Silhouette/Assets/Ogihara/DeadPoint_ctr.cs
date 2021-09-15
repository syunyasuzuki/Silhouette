using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPoint_ctr : MonoBehaviour
{
    bool found_check;
    [SerializeField] GameObject exc_mark;
    float alpha;
    // Start is called before the first frame update
    void Start()
    {
        found_check = false;
        //GameObject.Find("") = GetComponent<FlashCtrl>();
        alpha = 0;
        exc_mark.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        //if(GetFlash() == true && found_check==true)
        //{
        //alpha = 1;
        //Flashctrl.StopFlash();
        //exc_mark.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
        //}
        if(found_check == false)
        {
            alpha = 1;
            exc_mark.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.tag == "Wall")
        {
            found_check = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (gameObject.tag == "Wall")
        {          
            found_check = true;
        }
    }
}
