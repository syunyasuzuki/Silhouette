using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCamera : MonoBehaviour
{
    [SerializeField] GameObject Target = null;

    [SerializeField] bool Freeze_X = false;
    [SerializeField] bool Freeze_Y = false;

    private Vector2Int Freeze;

    [SerializeField] Vector2 Clamp_X = new Vector2(-10, 10);

    private void Start()
    {
        Freeze = new Vector2Int(Freeze_X ? 0 : 1, Freeze_Y ? 0 : 1);
    }


    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        transform.position = new Vector3(Mathf.Clamp(Target.transform.position.x * Freeze.x, Clamp_X.x, Clamp_X.y), Target.transform.position.y * Freeze.y, -10);
    }
}
