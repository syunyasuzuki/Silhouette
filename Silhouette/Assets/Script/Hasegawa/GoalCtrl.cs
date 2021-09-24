using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCtrl : MonoBehaviour
{

    [SerializeField] SpriteRenderer goal_sr = null;

    [SerializeField] float interval = 2f;

    bool mode = false;

    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (mode)
        {
            goal_sr.color = new Color(1, 1, 1, time / interval);
            if (time >= interval)
            {
                time = 0;
                mode = false;
            }
        }
        else
        {
            goal_sr.color = new Color(1, 1, 1, 1f - time / interval);
            if (time >= interval)
            {
                time = 0;
                mode = true;
            }
        }
    }
}
