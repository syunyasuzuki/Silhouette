using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class HideEditor : MonoBehaviour
{
    private Camera maincamera = null;

    //背景の黒色の素材
    [SerializeField] Sprite BackImg = null;

    //乗ることのできる足場の素材
    [SerializeField] Sprite PlatformImg = null;

    //足場のユニティサイズ
    [SerializeField] int PlatformPixelPerUnit = 50;

    //保存先
    string SpriteFolderPath = "Image/HidePlatformImage";

    //保存先に更にファイルを作成する場合入力する場所
    [SerializeField] string OriginalFilePath = null;

    //保存先を確定しチェックする
    private void CheckFilePath()
    {
        SpriteFolderPath = Path.Combine(Application.dataPath , SpriteFolderPath);
        if (OriginalFilePath != null)
        {
            SpriteFolderPath = Path.Combine(SpriteFolderPath, OriginalFilePath);
        }
        if (!Directory.Exists(SpriteFolderPath))
        {
            Directory.CreateDirectory(SpriteFolderPath);
        }
    }

    //タイルを配置する際の全体の大きさ
    private Vector2Int HideSize = new Vector2Int(4, 8);

    //横の大きさ
    [SerializeField] Slider Xsize = null;

    //縦の大きさ
    [SerializeField] Slider Ysize = null;

    //足場のタイルデータ
    List<List<int>> PlatformTileData = new List<List<int>>();

    //背景のタイルデータ
    List<List<int>> BackTileData = new List<List<int>>();

    //タイルを配置することのできる範囲を表示するやつ
    private GameObject[] HideSizeObjects = new GameObject[4];

    //生成した足場のタイルをまとめるやつ
    private GameObject PlatformMother = null;

    //生成した背景のタイルをまとめるやつ
    private GameObject BackMother = null;

    //足場を表示するか
    [SerializeField] Toggle IsVisiblePlatform = null;

    //足場の表示に変更があった場合の処理
    private void CheckIsVisiblePlatform()
    {
        if(PlatformMother.activeSelf != IsVisiblePlatform.isOn)
        {
            PlatformMother.SetActive(IsVisiblePlatform.isOn);
        }
    }

    //足場を編集するか（true == 足場の編集、false == 背景の編集）
    [SerializeField] Toggle PlatformMode = null;

    //タイルを配置することのできる範囲を表示する
    private void SetHideSize()
    {
        if (HideSizeObjects[0] == null)
        {
            for (int i = 0; i < 4; ++i)
            {
                HideSizeObjects[i] = new GameObject("HideSize" + i);
                HideSizeObjects[i].AddComponent<SpriteRenderer>().sprite = BackImg;
                HideSizeObjects[i].GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            }

            Xsize.value = HideSize.x;
            Ysize.value = HideSize.y;

            for(int sy = 0; sy < HideSize.y; ++sy)
            {
                List<int> pd = new List<int>();
                List<int> bd = new List<int>();
                for(int xy = 0; xy < HideSize.x; ++xy)
                {
                    pd.Add(0);
                    bd.Add(0);
                }
                PlatformTileData.Add(pd);
                BackTileData.Add(bd);
            }

            PlatformMother = new GameObject("tilemother");
            BackMother = new GameObject("backmother");
        }

        float x = HideSize.x / 2f + 0.5f;
        float y = HideSize.y / 2f + 0.5f;

        HideSizeObjects[0].transform.localScale = new Vector3(HideSize.x, 1, 1);
        HideSizeObjects[1].transform.localScale = new Vector3(HideSize.x, 1, 1);
        HideSizeObjects[2].transform.localScale = new Vector3(1, HideSize.y, 1);
        HideSizeObjects[3].transform.localScale = new Vector3(1, HideSize.y, 1);

        HideSizeObjects[2].transform.position = new Vector3(-x, 0, 0);
        HideSizeObjects[3].transform.position = new Vector3(x, 0, 0);
        HideSizeObjects[0].transform.position = new Vector3(0, y, 0);
        HideSizeObjects[1].transform.position = new Vector3(0, -y, 0);

    }

    //足場のタイルのデータを削除する
    private void DeletePlatformData()
    {
        for (int sy = 0; sy < HideSize.y; ++sy)
        {
            for (int sx = 0; sx < HideSize.x; ++sx)
            {
                PlatformTileData[sy][sx] = 0;
            }
        }
    }

    //背景のタイルのデータを削除する
    private void DeleteBackData()
    {
        for(int sy = 0; sy < HideSize.y; ++sy)
        {
            for(int sx = 0; sx < HideSize.x; ++sx)
            {
                BackTileData[sy][sx] = 0;
            }
        }
    }

    //生成されている足場のタイルを削除する
    private void DeletePlatformTiles()
    {
        Destroy(PlatformMother.gameObject);
        PlatformMother = new GameObject("titlemother");
    }

    //生成されている背景のタイルを削除する
    private void DeleteBackTiles()
    {
        Destroy(BackMother.gameObject);
        BackMother = new GameObject("backmother");
    }

    //足場の削除（UI用）
    public void DeletePlatformButton()
    {
        DeletePlatformData();
        DeletePlatformTiles();
    }

    //背景の削除（UI用）
    public void DeleteBackButton()
    {
        DeleteBackData();
        DeleteBackTiles();
    }

    //足場のデータを背景にも反映させる（上書き）
    public void OverwritePlatformToBack()
    {
        DeleteBackTiles();
        for(int y = 0; y < HideSize.y; ++y)
        {
            for(int x = 0; x < HideSize.x; ++x)
            {
                BackTileData[y][x] = PlatformTileData[y][x];

                if (BackTileData[y][x] == 1)
                {
                    GameObject go = new GameObject("b" + y + "_" + x);
                    go.AddComponent<SpriteRenderer>().sprite = BackImg;
                    go.transform.position = new Vector3(-HideSize.x / 2f + 0.5f + x, -HideSize.y / 2f + 0.5f + y, 0);
                    go.transform.parent = BackMother.transform;
                }
            }
        }
    }


    //書き出し時に使うPNGの名前
    [SerializeField] InputField PNGFileName = null;

    //書き出したPNGを使用してオブジェクトを作成し、
    //タイルデータから頂点データを作成し、
    //オブジェクトのスプライトに沿ったポリゴンコライダーを生成する
    private void CreatePolygonCollider()
    {
        GameObject cpcgo = new GameObject(PNGFileName + "_p_obj");
        FileStream fs = new FileStream(Path.Combine(SpriteFolderPath, PNGFileName.text + "_platform.png"), FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] tx = br.ReadBytes((int)br.BaseStream.Length);
        fs.Close();
        br.Close();
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(tx);
        tex.filterMode = FilterMode.Point;
        cpcgo.AddComponent<SpriteRenderer>().sprite= Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), PlatformPixelPerUnit);
        PolygonCollider2D pc2 = cpcgo.AddComponent<PolygonCollider2D>();








    }

    //現在のタイル情報をPNGとして書き出す
    public void ExportPlatformPng()
    {
        if (PNGFileName.text == null && PNGFileName.text.Length <= 1)
        {
            Debug.Log("Please PNG Name !!");
            return;
        }

        Texture2D tex2d = new Texture2D(PlatformPixelPerUnit * HideSize.x, PlatformPixelPerUnit * HideSize.y);
        for (int y = 0; y < HideSize.y; ++y)
        {
            for (int x = 0; x < HideSize.x; ++x)
            {
                if (PlatformTileData[y][x] == 1)
                {
                    for (int py = 0; py < PlatformPixelPerUnit; ++py)
                    {
                        for (int px = 0; px < PlatformPixelPerUnit; ++px)
                        {
                            tex2d.SetPixel(PlatformPixelPerUnit * x + px, PlatformPixelPerUnit * y + py, PlatformImg.texture.GetPixel(px, py));
                        }
                    }
                }
                else
                {
                    for (int py = 0; py < PlatformPixelPerUnit; ++py)
                    {
                        for (int px = 0; px < PlatformPixelPerUnit; ++px)
                        {
                            tex2d.SetPixel(PlatformPixelPerUnit * x + px, PlatformPixelPerUnit * y + py, new Color(0, 0, 0, 0));
                        }
                    }
                }
            }
        }

        byte[] bytedata = tex2d.EncodeToPNG();

        Debug.Log(Path.Combine(SpriteFolderPath, PNGFileName.text + ".png"));

        File.WriteAllBytes(Path.Combine(SpriteFolderPath, PNGFileName.text + "_platform.png"), bytedata);
    }

    //現在のタイル情報をPNGとして書き出す
    public void ExportBackPng()
    {
        if (PNGFileName.text == null && PNGFileName.text.Length <= 1)
        {
            Debug.Log("Please PNG Name !!");
            return;
        }

        Texture2D tex2d = new Texture2D(PlatformPixelPerUnit * HideSize.x, PlatformPixelPerUnit * HideSize.y);
        for (int y = 0; y < HideSize.y; ++y)
        {
            for (int x = 0; x < HideSize.x; ++x)
            {
                if (BackTileData[y][x] == 1)
                {
                    for (int py = 0; py < PlatformPixelPerUnit; ++py)
                    {
                        for (int px = 0; px < PlatformPixelPerUnit; ++px)
                        {
                            tex2d.SetPixel(PlatformPixelPerUnit * x + px, PlatformPixelPerUnit * y + py, new Color32(0, 10, 40, 255));
                        }
                    }
                }
                else
                {
                    for (int py = 0; py < PlatformPixelPerUnit; ++py)
                    {
                        for (int px = 0; px < PlatformPixelPerUnit; ++px)
                        {
                            tex2d.SetPixel(PlatformPixelPerUnit * x + px, PlatformPixelPerUnit * y + py, new Color(0, 0, 0, 0));
                        }
                    }
                }
            }
        }

        byte[] bytedata = tex2d.EncodeToPNG();

        Debug.Log(Path.Combine(SpriteFolderPath, PNGFileName.text + ".png"));

        File.WriteAllBytes(Path.Combine(SpriteFolderPath, PNGFileName.text + "_back.png"), bytedata);
    }



    //スライダーの値が変更された場合全体の大きさを変更する
    private void CheckChangeSlider()
    {
        bool check = false;
        if ((int)Xsize.value != HideSize.x)
        {
            HideSize.x = (int)Xsize.value;
            check = true;
        }

        if ((int)Ysize.value != HideSize.y)
        {
            HideSize.y = (int)Ysize.value;
            check = true;
        }

        if (check)
        {
            SetHideSize();
            DeletePlatformTiles();
            DeleteBackTiles();
            if (PlatformTileData.Count < HideSize.y)
            {
                while(PlatformTileData.Count <= HideSize.y)
                {
                    List<int> pd = new List<int>();
                    List<int> bd = new List<int>();
                    for(int x = 0; x < HideSize.x; ++x)
                    {
                        pd.Add(0);
                        bd.Add(0);
                    }
                    PlatformTileData.Add(pd);
                    BackTileData.Add(bd);
                }
            }
            else if(PlatformTileData.Count > HideSize.y)
            {
                PlatformTileData.RemoveRange(HideSize.y, PlatformTileData.Count - HideSize.y);
                BackTileData.RemoveRange(HideSize.y, BackTileData.Count - HideSize.y);
            }

            for(int y = 0; y < HideSize.y; ++y)
            {
                if (PlatformTileData[y].Count < HideSize.x)
                {
                    while (PlatformTileData[y].Count <= HideSize.x)
                    {
                        PlatformTileData[y].Add(0);
                        BackTileData[y].Add(0);
                    }
                }
                else if (PlatformTileData[y].Count > HideSize.x)
                {
                    PlatformTileData[y].RemoveRange(HideSize.x, PlatformTileData[y].Count - HideSize.x);
                    BackTileData[y].RemoveRange(HideSize.x, BackTileData[y].Count - HideSize.x);
                }
            }

            for(int y = 0; y < HideSize.y; ++y)
            {
                for(int x = 0; x < HideSize.x; ++x)
                {
                    if (PlatformTileData[y][x] == 1)
                    {
                        GameObject go = new GameObject("p" + y + "_" + x);
                        go.AddComponent<SpriteRenderer>().sprite = PlatformImg;
                        go.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        go.transform.position = new Vector3(-HideSize.x / 2f + 0.5f + x, -HideSize.y / 2f + 0.5f + y, 0);
                        go.transform.parent = PlatformMother.transform;
                    }

                    if (BackTileData[y][x] == 1)
                    {
                        GameObject go = new GameObject("b" + y + "_" + x);
                        go.AddComponent<SpriteRenderer>().sprite = BackImg;
                        go.transform.position = new Vector3(-HideSize.x / 2f + 0.5f + x, -HideSize.y / 2f + 0.5f + y, 0);
                        go.transform.parent = BackMother.transform;
                    }
                }
            }

            if (!IsVisiblePlatform.isOn)
            {
                PlatformMother.SetActive(false);
            }
        }
    }

    //マウスの処理
    private void MouseTask()
    {
        float x = Input.GetAxisRaw("Mouse ScrollWheel");
        if (x != 0)
        {
            maincamera.orthographicSize = Mathf.Clamp(maincamera.orthographicSize - x, 5, 30);
        }

        if (EventSystem.current.currentSelectedGameObject == null && (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1)))
        {
            Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(maincamera.transform.position.z)));
            
            //範囲内か
            float sx = HideSize.x / 2f;
            float sy = HideSize.y / 2f;
            if (mouseposition.x < -sx || mouseposition.x > sx || mouseposition.y < -sy || mouseposition.y > sy) { return; }

            //参照するリスト番号
            int lx = Mathf.RoundToInt(mouseposition.x + HideSize.x / 2f - 0.5f);
            int ly = Mathf.RoundToInt(mouseposition.y + HideSize.y / 2f - 0.5f);

            //配置するワールド座標
            float px, py;

            if (HideSize.x % 2 == 0) 
            {
                px = Mathf.Floor(mouseposition.x) + 0.5f;
            }
            else 
            {
                px = Mathf.Round(mouseposition.x);

            }
            if (HideSize.y % 2 == 0) 
            {
                py = Mathf.Floor(mouseposition.y) + 0.5f;
            }
            else 
            {
                py = Mathf.Round(mouseposition.y);
            }

            if (PlatformMode.isOn)
            {
                if (PlatformTileData[ly][lx] == 0 && Input.GetMouseButton(0))
                {
                    GameObject go = new GameObject("p" + ly + "_" + lx);
                    go.AddComponent<SpriteRenderer>().sprite = PlatformImg;
                    go.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    go.transform.position = new Vector3(px, py, 0);
                    go.transform.parent = PlatformMother.transform;
                    PlatformTileData[ly][lx] = 1;
                }

                if (PlatformTileData[ly][lx] == 1 && Input.GetMouseButton(1))
                {
                    GameObject go = GameObject.Find("p" + ly + "_" + lx);
                    Destroy(go.gameObject);
                    PlatformTileData[ly][lx] = 0;
                }
            }
            else
            {
                if (BackTileData[ly][lx] == 0 && Input.GetMouseButton(0))
                {
                    GameObject go = new GameObject("b" + ly + "_" + lx);
                    go.AddComponent<SpriteRenderer>().sprite = BackImg;
                    go.transform.position = new Vector3(px, py, 0);
                    go.transform.parent = BackMother.transform;
                    BackTileData[ly][lx] = 1;
                }

                if (BackTileData[ly][lx] == 1 && Input.GetMouseButton(1))
                {
                    GameObject go = GameObject.Find("b" + ly + "_" + lx);
                    Destroy(go.gameObject);
                    BackTileData[ly][lx] = 0;
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        maincamera = Camera.main;
        CheckFilePath();
        SetHideSize();
    }

    // Update is called once per frame
    void Update()
    {
        MouseTask();
        CheckChangeSlider();
        CheckIsVisiblePlatform();
        DebugMode();
    }

    private void DebugMode()
    {

        if (PNGFileName.isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExportPlatformPng();
                ExportBackPng();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DeleteBackButton();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeletePlatformButton();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                OverwritePlatformToBack();
            }
        }
    }

}
