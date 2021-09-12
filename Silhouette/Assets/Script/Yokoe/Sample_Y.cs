using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Y : MonoBehaviour
{
    GameObject eye;

    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        eye = GameObject.Find("eye");
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        eye.transform.position = new Vector3(Mathf.Clamp(eye.transform.position.x, 5, 15), 
                                                Mathf.Clamp(eye.transform.position.y,1,1));
    }
}
