using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayer : MonoBehaviour
{
    GameObject eye;


    // Start is called before the first frame update
    void Start()
    {
        eye = GameObject.Find("eye");
    }

    // Update is called once per frame
    void Update()
    {
        //playerposs();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * 2.0f * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * 2.0f * Time.deltaTime;
        }
    }

    void playerposs()
    {
        eye.GetComponent<Sample_Y>().SetTargetPos(transform.position.x, transform.position.y);
    }
}
