using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目の設置、目の配置場所のデータの書き出し
//目の配置場所のデータを読み込み目を実際に配置する
//目を配置した際に目がマウスに追従するかランダムに動くかを選択できる
public class EyeDirector : MonoBehaviour
{
    //各目の部位
    [SerializeField] GameObject BlackEye = null;
    [SerializeField] GameObject WhiteEye = null;
    [SerializeField] GameObject EyeBody = null;

    //ターゲットする対象
    [SerializeField] Transform Target = null;
    public static Transform TargetTransform { get; private set; } = null;

    //実行するモードの種類
    enum Mode
    {
        //配置
        EditMode = 0,
        //目はターゲットを注視し続ける
        //ターゲットがない場合はマウスを注目する
        MoveMode = 1,
        //目はランダムに動き続ける
        //外部から入力があった場合ターゲットを注目する、ターゲットがない場合0地点を注目する
        RandomMoveMode = 2,
    }

    //選択されているモード
    [SerializeField] Mode NowMode = Mode.EditMode;

    //配置の時に使うポインター
    [SerializeField] GameObject point = null;

    //配置した時のデータを保存する構造体
    struct Data
    {
        public Vector3 Position { get; private set; }
        public Vector3 Scale { get;  private set; }
        public int Number { get;  private set; }
        public Data(Vector3 p, Vector3 s, int n)
        {
            Position = p;
            Scale = s;
            Number = n;
        }
    }

    //目の配置データの塊
    List<Data> eyedata = null;

    //マウスのポインター
    GameObject mouseposition = null;
    int count = 0;

    private void Awake()
    {
        TargetTransform = Target;
    }

    //書き出し、読み込み時の名前
    string stagename = "";

    // Start is called before the first frame update
    void Start()
    {
        stagename = "EyeData/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

#if !UNITY_EDITOR
        //書き出し時にモードが配置のままだった場合ランダムモードに変える
        if (NowMode == Mode.EditMode) NowMode = Mode.RandomMoveMode;
#endif
        switch (NowMode)
        {
            case Mode.EditMode:
                //ポインター用オブジェクトを作成
                mouseposition = Instantiate(point);
                //データを初期化
                eyedata = new List<Data>();
                break;
            case Mode.MoveMode:
                //データを読み込む
#if UNITY_EDITOR
                string[] data = OtherData.InportText(OtherData.GetResourcesPath() + "/" + stagename + ".txt");
#else
                TextAsset texa = Resources.Load(stagename, typeof(TextAsset)) as TextAsset;
                string[] data = texa.text.Split('\n');
#endif
                //データがない場合はじく
                if (data == null || data.Length <= 0) return;
                //目の合計の数を取得
                int n = int.Parse(data[0]);
                //目を生成
                for (int i = 0; i < n; ++i)
                {
                    string[] edata = data[i + 1].Split(',');
                    GameObject p = Instantiate(BlackEye);
                    GameObject c = Instantiate(WhiteEye);
                    GameObject e = Instantiate(EyeBody);
                    e.transform.position = new Vector3(float.Parse(edata[0]), float.Parse(edata[1]), 0);
                    e.transform.localScale = new Vector3(float.Parse(edata[2]), float.Parse(edata[3]), 1);
                    e.GetComponent<EyeCtrl>().SetEyes(p, c, int.Parse(edata[4]) * 10, false);
                }
                break;
            case Mode.RandomMoveMode:
                //データを読み込む
#if UNITY_EDITOR
                string[] data2 = OtherData.InportText(OtherData.GetResourcesPath() + "/" + stagename + ".txt");
#else
                TextAsset texa2 = Resources.Load(stagename, typeof(TextAsset)) as TextAsset;
                string[] data2 = texa2.text.Split('\n');
#endif
                //データがない場合はじく
                if (data2 == null || data2.Length <= 0) return;
                //目の合計の数を取得
                int n2 = int.Parse(data2[0]);
                //目を生成
                for (int i = 0; i < n2; ++i)
                {
                    string[] edata = data2[i + 1].Split(',');
                    GameObject p = Instantiate(BlackEye);
                    GameObject c = Instantiate(WhiteEye);
                    GameObject e = Instantiate(EyeBody);
                    e.transform.position = new Vector3(float.Parse(edata[0]), float.Parse(edata[1]), 0);
                    e.transform.localScale = new Vector3(float.Parse(edata[2]), float.Parse(edata[3]), 1);
                    e.GetComponent<EyeCtrl>().SetEyes(p, c, int.Parse(edata[4]) * 10, true);
                    e.GetComponent<EyeCtrl>().SetEyeState(Random.Range(0.05f, 0.1f), Random.Range(0.1f, 0.2f));
                }
                break;
        }
    }

#if UNITY_EDITOR
    //書き出し時に消える
    void Update()
    {
        //目を配置する場合のみ
        if (NowMode == Mode.EditMode)
        {
            //マウスの位置を取得
            mouseposition.transform.position = OtherData.GetWorldMousePosition();

            //マウスのスクロールを取得
            float ms = Input.GetAxisRaw("Mouse ScrollWheel");
            if (ms != 0)
            {
                //スクロールしていた場合ポインターの目の大きさを変更

                if (mouseposition.transform.localScale.x + ms * 0.4f < 0.05f)
                {
                    mouseposition.transform.localScale = new Vector3(0.05f, 0.05f, 1);
                }
                else
                {
                    float n = mouseposition.transform.localScale.x + ms * 0.4f;
                    mouseposition.transform.localScale = new Vector3(n, n, 1);
                }
            }

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (x != 0 || y != 0)
            {
                Camera.main.transform.position += new Vector3(0.25f * x, 0.25f * y, 0);
            }

            //クリックした場所で目の配置データを作成する
            if (Input.GetMouseButtonDown(0))
            {
                Data data = new Data(mouseposition.transform.position, mouseposition.transform.localScale, count);
                GameObject g = Instantiate(point);
                g.transform.position = mouseposition.transform.position;
                g.transform.localScale = mouseposition.transform.localScale;
                g.GetComponent<SpriteRenderer>().sortingOrder = 100;
                ++count;
                mouseposition.GetComponent<SpriteRenderer>().sortingOrder = 100;
                eyedata.Add(data);
            }

            //Eボタンで現在保存されている全てのデータを書き出し（上書き）
            if (Input.GetKeyDown(KeyCode.E))
            {
                string[] data = new string[eyedata.Count + 1];
                data[0] = eyedata.Count.ToString();
                for(int i = 0; i < eyedata.Count; ++i)
                {
                    data[i + 1] = eyedata[i].Position.x + "," + eyedata[i].Position.y + "," + eyedata[i].Scale.x + "," + eyedata[i].Scale.y + "," + eyedata[i].Number;
                }
                OtherData.ExportText(data, OtherData.GetResourcesPath() + "/" + stagename + ".txt");
            }
        }
    }

#endif

}
