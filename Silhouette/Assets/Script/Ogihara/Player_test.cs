using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_test : MonoBehaviour
{
    private Rigidbody2D rig2D;
    BoxCollider2D box2D;
    Vector2 move;
    [SerializeField] float speed;//移動速度
    float speedx;
    float walkcount;
    float Max_speed = 4.0f;//最大速度    
    [SerializeField] float jumpForce;//ジャンプの強さ
    float jump;
    Animator animator;
    bool now_jump;
    public static bool move_check;

    // Start is called before the first frame update
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box2D = GetComponent<BoxCollider2D>();
        now_jump = false;
        move_check = true;
    }

    void PlayerCon()
    {
        //移動処理
        float x = Input.GetAxisRaw("Horizontal");
        //プレイヤーの移動処理
        move = new Vector2(x * speed * Time.deltaTime, 0.0f);
        speedx = Mathf.Abs(rig2D.velocity.x);
        //速度制限
        if (speedx < Max_speed)
        {
            rig2D.AddForce(move);
        }
        //移動アニメーション
        if (speedx > 0.1f)
        {
            animator.SetFloat("WalkFloat", speedx);
        }       
        else
        {
            animator.SetFloat("WalkFloat", 0.0f);
        }
        //反転処理
        if (x !=　0)
        {
            transform.localScale = new Vector3(x,1,1);
        }

        //ジャンプ処理

        if (now_jump ==false &&Input.GetKeyDown(KeyCode.Space))
        {
            rig2D.AddForce(transform.up * jumpForce);            
        }
        jump = Mathf.Abs(rig2D.velocity.y);

        if (jump >= 0.05)
        {
            now_jump = true;
            animator.SetFloat("JumpFloat", jump);
        }
        else
        {
            now_jump = false;
            animator.SetFloat("JumpFloat", 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move_check == true)
        {
            PlayerCon();
        }
        else
        {
            animator.SetFloat("JumpFloat", 0.0f);
            animator.SetFloat("WalkFloat", 0.0f);
        }
    }
}
