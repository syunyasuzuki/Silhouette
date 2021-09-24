﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Came : MonoBehaviour
{
    GameObject playerObj;
    Player_test player;
    Transform playerTransform;
    Transform cameTransform;
    Vector3 camepos;
    Vector3 playerpos;
    public static bool came_check;
    [SerializeField] Camera came;
    [SerializeField] float came_move_max = 10;
    [SerializeField] float came_move_min = -10;
    [SerializeField] Image RightArrow;
    [SerializeField] Image LeftArrow;
    [SerializeField] Image CameImage;
    


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");                
        playerTransform = playerObj.transform;
        cameTransform = gameObject.GetComponent<Transform>();
        came =gameObject.GetComponent<Camera>();
        came_check = false;
        RightArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        CameImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        playerpos = GameObject.Find("Player").transform.position;
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {       
        Move_Camera();
    }        

    void Move_Camera()
    {       
        //カメラ移動ON/OFF
        if (Input.GetKeyDown(KeyCode.Z))
        {
            came_check =! came_check;
        }
        switch (came_check)
        {
            case true:              
                    Player_test.move_check = false;
                    Vector3 camepos = cameTransform.position;
                RightArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                CameImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    camepos.x += 0.1f;                   
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        camepos.x += -0.1f;                
                    }
                if(camepos.x < 0 || camepos.x == 0)
                {
                    camepos.x = 0.0f;
                    LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                float camedistance = Vector2.Distance(playerpos, camepos);
                if (camedistance >= came_move_max || camedistance == came_move_max)
                {
                    camepos = cameTransform.position;
                   RightArrow .GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                else if(camedistance <= came_move_min || camedistance == came_move_min)
                {
                    camepos = cameTransform.position;
                    LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                cameTransform.position = camepos;
                //if (Input.GetKeyDown(KeyCode.Z))
                //{                    
                //    Player_test.move_check = false;
                //    came_check = false;
                //}
                break;
        case false:
                Player_test.move_check = true;              
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                RightArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                LeftArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                CameImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);

                break;
        }
    }
}
