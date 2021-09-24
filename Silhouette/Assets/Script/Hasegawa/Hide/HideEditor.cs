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

    //タイルを配置することのできる範囲を表示するやつ
    private GameObject[] HideSizeObjects = new GameObject[4];

    //生成したタイルをまとめるやつ
    private GameObject TileMother = null;

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
                List<int> td = new List<int>();
                for(int xy = 0; xy < HideSize.x; ++xy)
                {
                    td.Add(0);
                }
                PlatformTileData.Add(td);
            }

            TileMother = new GameObject("tilemother");
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

    //タイルのデータを削除する
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

    //配置されているタイルを削除する
    private void DeletePlatformTiles()
    {
        Destroy(TileMother.gameObject);
        TileMother = new GameObject("titlemother");
    }

    //UIボタン用の削除
    public void DeleteButton()
    {
        DeletePlatformData();
        DeletePlatformTiles();
    }

    //書き出し時に使うPNGの名前
    [SerializeField] Text PNGFileName = null;

    //現在のタイル情報をPNGとして書き出す
    public void WritePlatformPng()
    {
        if (PNGFileName == null)
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

        PNGFileName.text = "";
    }

    //現在のタイル情報をPNGとして書き出す
    public void WriteBackPng()
    {
        if (PNGFileName == null)
        {
            Debug.Log("Please PNG Name !!");
            return;
        }

        Texture2D tex2d = new Texture2D(PlatformPixelPerUnit * HideSize.x, PlatformPixelPerUnit * HideSize.y);
        for (int y = 0; y < PlatformPixelPerUnit * HideSize.y; ++y)
        {
            for (int x = 0; x < PlatformPixelPerUnit * HideSize.x; ++x)
            {
                tex2d.SetPixel(x, y, new Color32(0, 104, 183, 255));
            }
        }

        byte[] bytedata = tex2d.EncodeToPNG();

        Debug.Log(Path.Combine(SpriteFolderPath, PNGFileName.text + ".png"));

        File.WriteAllBytes(Path.Combine(SpriteFolderPath, PNGFileName.text + "_back.png"), bytedata);

        PNGFileName.text = "";
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
            if (PlatformTileData.Count < HideSize.y)
            {
                while(PlatformTileData.Count <= HideSize.y)
                {
                    List<int> td = new List<int>();
                    for(int x = 0; x < HideSize.x; ++x)
                    {
                        td.Add(0);
                    }
                    PlatformTileData.Add(td);
                }
            }
            else if(PlatformTileData.Count > HideSize.y)
            {
                PlatformTileData.RemoveRange(HideSize.y, PlatformTileData.Count - HideSize.y);
            }

            for(int y = 0; y < HideSize.y; ++y)
            {
                if (PlatformTileData[y].Count < HideSize.x)
                {
                    while (PlatformTileData[y].Count <= HideSize.x)
                    {
                        PlatformTileData[y].Add(0);
                    }
                }
                else if (PlatformTileData[y].Count > HideSize.x)
                {
                    PlatformTileData[y].RemoveRange(HideSize.x, PlatformTileData[y].Count - HideSize.x);
                }
            }

            for(int y = 0; y < HideSize.y; ++y)
            {
                for(int x = 0; x < HideSize.x; ++x)
                {
                    if (PlatformTileData[y][x] == 1)
                    {
                        Debug.Log("call");
                        GameObject go = new GameObject("p" + y + "_" + x);
                        go.AddComponent<SpriteRenderer>().sprite = PlatformImg;
                        go.transform.position = new Vector3(-HideSize.x / 2f + 0.5f + x, -HideSize.y / 2f + 0.5f + y, 0);
                        go.transform.parent = TileMother.transform;
                    }
                }
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

        if (EventSystem.current.currentSelectedGameObject == null && Input.GetMouseButton(0))
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

            if (PlatformTileData[ly][lx] == 0)
            {
                GameObject go = new GameObject("chip");
                go.AddComponent<SpriteRenderer>().sprite = PlatformImg;
                go.GetComponent<SpriteRenderer>().sortingOrder = 3;
                go.transform.position = new Vector3(px, py, 0);
                go.transform.parent = TileMother.transform;
                PlatformTileData[ly][lx] = 1;
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
        DebugMode();
    }

    private void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            WritePlatformPng();
            WriteBackPng();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteButton();
        }

    }

}
