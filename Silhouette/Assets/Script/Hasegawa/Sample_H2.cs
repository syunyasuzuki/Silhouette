using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_H2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 start = new Vector2(1 / 2f, 2 / 4f);

        for(int i = 0; i < 1000; ++i)
        {
            //どこまでずれるかのテスト
        }

        Vector2 end = new Vector2(1 / 2f, 2 / 4f);

        Debug.Log(start);
        Debug.Log(end);

        if(start == end)
        {
            Debug.Log("call");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
