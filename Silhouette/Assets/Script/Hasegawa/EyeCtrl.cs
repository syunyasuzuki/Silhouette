using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCtrl : MonoBehaviour
{
    private GameObject BlackEye = null;
    private GameObject WhiteEye = null;
    private int Number = 0;
    
    private bool RandomMove = false;
    private float BaseMoveTime = 0.2f;
    private float BaseStayTime = 0.4f;

    public void SetEyes(GameObject b, GameObject w, int n, bool r)
    {
        BlackEye = b;
        WhiteEye = w;
        Number = n;
        RandomMove = r;
    }

    public void SetEyeState(float m,float s)
    {
        BaseMoveTime = m;
        BaseStayTime = s;
    }

    [SerializeField] GameObject Uemabuta = null;

    [SerializeField] Sprite[] uesprite = new Sprite[2];

    private SpriteRenderer uesr = null;

    [SerializeField] GameObject Shitamabuta = null;

    [SerializeField] Sprite[] shitasprite = new Sprite[2];

    private SpriteRenderer shitasr = null;

    [SerializeField] SpriteRenderer EyeBack = null;
    private enum MoveMode
    {
        Target = 0,
        Stay = 1,
        Move = 2,
        Stop = 3,
        Danger = 4
    }

    private MoveMode NowMoveMode = MoveMode.Stay;

    // Start is called before the first frame update
    private void Start()
    {
        //使用する白目と黒目の大きさを本体の大きさに合わせる
        BlackEye.transform.localScale = transform.localScale;
        WhiteEye.transform.localScale = transform.localScale;

        //上瞼と下瞼のスプライトレンダラーを取得する
        uesr = Uemabuta.GetComponent<SpriteRenderer>();
        shitasr = Shitamabuta.GetComponent<SpriteRenderer>();

        //指定された番号に合わせて各描画順を変更する
        BlackEye.GetComponent<SpriteRenderer>().sortingOrder += Number;
        WhiteEye.GetComponent<SpriteRenderer>().sortingOrder += Number;
        uesr.sortingOrder += Number;
        shitasr.sortingOrder += Number;
        EyeBack.sortingOrder += Number;
        GetComponent<SpriteRenderer>().sortingOrder += Number;

        //モードを確定
        if (RandomMove)
        {
            NowMoveMode = MoveMode.Stay;
        }
        else
        {
            NowMoveMode = MoveMode.Target;
        }
    }

    private float time = 0;
    private float mtime = 0;

    private Vector3 movepos = Vector3.zero;
    private Vector3 startpos = Vector3.zero;

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        switch (NowMoveMode)
        {
            //目はターゲットかマウスカーソルの位置を追いかける
            case MoveMode.Target:

                Vector3 targetpos;
                //もしターゲットが設定されている場合
                if (EyeDirector.TargetTransform != null)
                {
                    targetpos = EyeDirector.TargetTransform.position;
                }
                else
                {
                    //マウスのワールド座標を取得
                    targetpos = OtherData.GetWorldMousePosition();
                }

                //ターゲットの位置と目の中心位置の差を計算
                Vector3 targetdec = targetpos - transform.position;

                //黒目の処理
                if (targetdec.magnitude < 1 * transform.localScale.x)
                {
                    BlackEye.transform.position = targetpos;
                }
                else
                {
                    BlackEye.transform.position = new Vector3(transform.position.x + 1 * transform.localScale.x * targetdec.normalized.x, transform.position.y + 1 * transform.localScale.x * targetdec.normalized.y, 0);
                }

                //白目の処理
                if (targetdec.magnitude < 0.5f * transform.localScale.x)
                {
                    WhiteEye.transform.position = targetpos;
                }
                else
                {
                    WhiteEye.transform.position = new Vector3(transform.position.x + 0.5f * transform.localScale.x * targetdec.normalized.x, transform.position.y + 0.5f * transform.localScale.x * targetdec.normalized.y, 0);
                }
                break;
            //目をランダムで動かすときの目の待機状態
            case MoveMode.Stay:
                //待機時間に到達すると
                if (time >= mtime)
                {
                    time = 0;
                    //次に目を動かすときにかける時間と目を移動させる地点をランダムに選択し、
                    mtime = Random.Range(BaseMoveTime - BaseMoveTime / 2f, BaseMoveTime + BaseMoveTime / 2f);
                    float f = Random.Range(-1.00f, 1.00f) * transform.localScale.x;
                    int y = Random.Range(0, 2) == 0 ? -1 : 1;
                    //2/3の確率で目を全力で外側に向ける
                    int r = Random.Range(0, 4);
                    if (r >= 2)
                    {
                        movepos = transform.position + new Vector3(f, Mathf.Sqrt(1 - Mathf.Pow(f, 2)) * y * transform.localScale.x, 0);
                    }
                    else
                    {
                        movepos = transform.position + new Vector3(f, Mathf.Sqrt(1 - Mathf.Pow(f, 2)) * y * Random.Range(0.01f, 1.00f) * transform.localScale.x, 0);
                    }
                    //現在の黒目の位置を保持
                    startpos = BlackEye.transform.position;
                    //動かす状態へ移行
                    NowMoveMode = MoveMode.Move;
                }
                break;
            //ランダムに目を動かすときの目を動かす状態
            case MoveMode.Move:
                //黒目の位置を移動にかける時間から配置
                BlackEye.transform.position = startpos + (movepos - startpos) * (time / mtime);
                //黒目の位置に合わせた白目の位置を決定
                Vector3 randomdec = BlackEye.transform.position - transform.position;
                if (randomdec.magnitude < 0.5f * transform.localScale.x)
                {
                    WhiteEye.transform.position = BlackEye.transform.position;
                }
                else
                {
                    WhiteEye.transform.position = new Vector3(transform.position.x + 0.5f * transform.localScale.x * randomdec.normalized.x, transform.position.y + 0.5f * transform.localScale.x * randomdec.normalized.y, 0);
                }

                //移動が完了している場合
                if (time >= mtime)
                {
                    time = 0;
                    //待機する時間をランダムで決定して
                    mtime = Random.Range(BaseStayTime - BaseStayTime / 2f, BaseStayTime + BaseStayTime / 2f);
                    //黒目の位置を誤差のない位置に移動
                    BlackEye.transform.position = movepos;
                    //待機状態に移行する
                    NowMoveMode = MoveMode.Stay;
                }
                break;
            //目の停止状態
            case MoveMode.Stop:
                //停止期間が終わると
                if (time >= 1f)
                {
                    time = 0;

                    //0地点を見たときの目の位置を選択
                    Vector3 dangerdec;
                    //ターゲットがある場合ターゲットを注目
                    if (EyeDirector.TargetTransform != null)
                    {
                        dangerdec = EyeDirector.TargetTransform.position - transform.position;
                    }
                    else
                    {
                        dangerdec = Vector3.zero - transform.position;
                    }

                    if(dangerdec.magnitude <= 1f * transform.localScale.x)
                    {
                        movepos = Vector3.zero;
                    }
                    else
                    {
                        movepos = new Vector3(transform.position.x + 1 * transform.localScale.x * dangerdec.normalized.x, transform.position.y + 1 * transform.localScale.x * dangerdec.normalized.y, 0);
                    }
                    mtime = 0.1f;
                    startpos = BlackEye.transform.position;
                    //注目モードに移行
                    NowMoveMode = MoveMode.Danger;
                }
                break;
            //目は一点を注目する
            case MoveMode.Danger:
                float n = Mathf.Clamp(time / mtime, 0, 1);
                BlackEye.transform.position = startpos + (movepos - startpos) * n;
                Vector3 ddec = BlackEye.transform.position - transform.position;
                if (ddec.magnitude < 0.5f * transform.localScale.x)
                {
                    WhiteEye.transform.position = BlackEye.transform.position;
                }
                else
                {
                    WhiteEye.transform.position = new Vector3(transform.position.x + 0.5f * transform.localScale.x * ddec.normalized.x, transform.position.y + 0.5f * transform.localScale.x * ddec.normalized.y, 0);
                }
                /*
                if (time>=2f)
                {
                    time = 0;
                    mtime = Random.Range(BaseStayTime - BaseStayTime / 2f, BaseStayTime + BaseStayTime / 2f);
                    NowMoveMode = MoveMode.Stay;
                }
                */
                break;
        }

        //上瞼の処理
        if (BlackEye.transform.position.y >= transform.position.y + (0.8f * transform.localScale.y))
        {
            uesr.sprite = null;
        }
        else if (BlackEye.transform.position.y >= transform.position.y + (0.4f * transform.localScale.y))
        {
            uesr.sprite = uesprite[1];
        }
        else
        {
            uesr.sprite = uesprite[0];
        }

        //下瞼の処理
        if (BlackEye.transform.position.y <= transform.position.y - (0.8f * transform.localScale.y))
        {
            shitasr.sprite = null;
        }
        else if (BlackEye.transform.position.y <= transform.position.y - (0.4f * transform.localScale.y))
        {
            shitasr.sprite = shitasprite[1];
        }
        else
        {
            shitasr.sprite = shitasprite[0];
        }

        //仮置き
        //目は一点を注視する
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(NowMoveMode == MoveMode.Stay || NowMoveMode == MoveMode.Move)
            {
                time = 0;
                NowMoveMode = MoveMode.Stop;
            }
            else if (NowMoveMode == MoveMode.Danger)
            {
                time = 0;
                mtime = Random.Range(BaseStayTime - BaseStayTime / 2f, BaseStayTime + BaseStayTime / 2f);
                NowMoveMode = MoveMode.Stay;
            }
        }

    }
}
