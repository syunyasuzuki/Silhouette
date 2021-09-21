using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Came : MonoBehaviour
{
    GameObject playerObj;
    Player_test player;
    Transform playerTransform;
    Transform cameTransform;
    public static bool came_check;
    [SerializeField] Camera came;


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player_test>();
        playerTransform = playerObj.transform;
        cameTransform = gameObject.GetComponent<Transform>();
        came =gameObject.GetComponent<Camera>();
        came_check = false;        
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
            came_check = !came_check;
        }
        switch (came_check)
        {
            case true:
                Player_test.move_check = false;
                Vector3 camepos = cameTransform.position;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    camepos.x += 0.1f;
                }
                if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    camepos.x += -0.1f;
                }
                cameTransform.position = camepos;
            break;
        case false:
                Player_test.move_check = true;
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                break;
        }
    }
}

