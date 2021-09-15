using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPoint_ctr : MonoBehaviour
{
    bool found_check;
    // Start is called before the first frame update
    void Start()
    {
        found_check = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
