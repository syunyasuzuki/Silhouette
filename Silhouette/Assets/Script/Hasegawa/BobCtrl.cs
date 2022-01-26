using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobCtrl : MonoBehaviour
{
    //普段の移動速度
    private readonly float MoveSpeed = 5f;

    private Rigidbody2D rid2 = null;

    [SerializeField] Sprite bob = null;

    GameObject[] bobzanzou = null;

    private void UnActivZanzou()
    {
        if (!bobzanzou[0].activeSelf) return;

        for(int i = 0; i < bobzanzou.Length; ++i)
        {
            bobzanzou[i].SetActive(false);
        }
    }

    //最終的に向いてた方向
    private float nowvectorx = 1;

    //
    private float AvoidCooltime = 0.5f;

    //
    private float AvoidTime = 0.2f;

    //回避時の速度
    private float AvoidPower = 20f;

    //カウントするほう
    private float cooltime = 0;

    private float time = 0;

    private void Start()
    {
        rid2 = GetComponent<Rigidbody2D>();
        PhysicsMaterial2D pm2 = new PhysicsMaterial2D();
        pm2.bounciness = 0;
        pm2.friction = 0;
        GetComponent<BoxCollider2D>().sharedMaterial = pm2;

        bobzanzou = new GameObject[5];
        for(int i = 0; i < bobzanzou.Length; ++i)
        {
            bobzanzou[i] = new GameObject("zanzou" + i);
            bobzanzou[i].AddComponent<SpriteRenderer>().sprite = bob;
            bobzanzou[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f / bobzanzou.Length * (i + 1));
            bobzanzou[i].GetComponent<SpriteRenderer>().sortingOrder = 9;
        }
        UnActivZanzou();
    }

    // Update is called once per frame
    void Update()
    {
        cooltime -= cooltime > 0 ? Time.deltaTime : 0;
        time -= time > 0 ? Time.deltaTime : 0;

        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            nowvectorx = x;
            transform.localScale = new Vector3(x, 1, 1);
        }

        if (rid2.velocity.y == 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            rid2.velocity = new Vector2(rid2.velocity.x, 15);
        }

        if (time <= 0)
        {
            UnActivZanzou();
            rid2.velocity = new Vector2(MoveSpeed * x, rid2.velocity.y);

            if (cooltime <= 0 && Input.GetKeyDown(KeyCode.Space))
            {
                cooltime = AvoidCooltime;
                time = AvoidTime;
            }
        }
        else
        {
            rid2.velocity = new Vector2(AvoidPower * nowvectorx, rid2.velocity.y);

            int n = bobzanzou.Length - 1 - (int)(time / (AvoidTime / bobzanzou.Length));
            if (!bobzanzou[n].activeSelf)
            {
                bobzanzou[n].SetActive(true);
                bobzanzou[n].transform.position = gameObject.transform.position;
                bobzanzou[n].transform.localScale = new Vector3(nowvectorx, 1, 1);
            }
        }
    }
}
