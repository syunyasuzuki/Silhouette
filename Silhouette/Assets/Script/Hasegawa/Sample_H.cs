using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_H : MonoBehaviour
{
    //すり抜けテスト

    [SerializeField] GameObject obj = null;

    [SerializeField] int N = 20;

    [SerializeField] float ResetTime = 5f;

    private GameObject[] objs = null;

    private float time = 0;

    private void ResetObjs()
    {
        for(int i = 0; i < N; ++i)
        {
            objs[i].transform.position = new Vector3(Random.Range(-7f, 7f), 10, 0);
            objs[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        objs = new GameObject[N];

        for(int i = 0; i < N; ++i)
        {
            objs[i] = Instantiate(obj);
        }
        ResetObjs();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= ResetTime)
        {
            ResetObjs();
            time = 0;
        }
    }
}
