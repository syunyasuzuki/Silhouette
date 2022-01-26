using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

//読み込んだタイルを並べて完成したマップを一枚のPNGとして書き出す
public class MapEditor : MonoBehaviour
{
    //読み込んだスプライト
    Sprite[] sprites = null;

    //読み込んだスプライトの１タイルに対する大きさ（基本は１、１）
    Vector2Int tilescale = Vector2Int.one;

    //ボタンを追従させる親オブジェクト
    [SerializeField] RectTransform scollobj = null;

    //素材までのフォルダーの名前
    [SerializeField] string SpriteSheetFolderName = "";

    private enum SheetName
    {
        Plane = 0,
        Object = 1,
        Collider = 2
    }

    [SerializeField] SheetName sheetname = SheetName.Plane;

    //UI配置する際の親オブジェクト
    private GameObject Canvas = null;

    //最大で編集できるマップサイズ
    [SerializeField] Vector2Int MaxMapSize = new Vector2Int(1000, 100);

    //現在のマップサイズ
    [SerializeField] Vector2Int MapSize = new Vector2Int(8, 5);

    //マップの大きさを視覚的にわかるようにする枠
    private GameObject[] MapFlame = new GameObject[4];

    //マップの枠を現在のマップの大きさに合わせて配置しなおす
    private void SetMapFlame()
    {
        MapFlame[0].transform.localScale = MapFlame[1].transform.localScale = new Vector3(MapSize.x, 1, 1);
        MapFlame[2].transform.localScale = MapFlame[3].transform.localScale = new Vector3(1, MapSize.y, 1);

        MapFlame[0].transform.position = new Vector3(0, MapSize.y / 2f + 0.5f, 0);
        MapFlame[1].transform.position = new Vector3(0, - MapSize.y / 2f - 0.5f, 0);
        MapFlame[2].transform.position = new Vector3(-MapSize.x / 2f - 0.5f, 0, 0);
        MapFlame[3].transform.position = new Vector3(MapSize.x / 2f + 0.5f, 0, 0);
    }

    //マップのタイルデータ
    private List<List<int>> MapData = null;

    //選択されている素材の番号
    private int SelectTileNumber = 0;

    //現在選択中のタイルを表示するやつ
    Image SelectTile = null;

    //押されたボタンによって選択中のタイルを変更する
    //UIに配置したボタンをクリックしたときに実行する
    private void ButtonEvent(int n)
    {
        SelectTileNumber = n;
        SelectTile.sprite = sprites[n];
    }

    //書き出し先のフォルダーの名前
    [SerializeField] string ExportFolderName = "";

    //書き出し時のPNGの名前
    [SerializeField] string ExportPNGName = "Test";

    //当たり判定用に書き出すか
    [SerializeField] bool IsColliderPNG = false;

    // Start is called before the first frame update
    void Start()
    {
        //UIのCanvasを取得
        Canvas = GameObject.Find("Canvas");

        //指定されたスプライトを取得する
        string spritepath = "";
        switch (sheetname)
        {
            case SheetName.Plane:
                spritepath = "tileset2";
                break;
            case SheetName.Object:
                spritepath = "bush tree";
                break;
            case SheetName.Collider:
                spritepath = "tileset2_collider";
                break;
        }
        if(SpriteSheetFolderName!=null || SpriteSheetFolderName != "")
        {
            spritepath = SpriteSheetFolderName + "/" + spritepath;
        }
        sprites = Resources.LoadAll<Sprite>(spritepath);

        //PixelPerUnitの正方形を１タイルサイズとした場合のspriteの大きさを求める
        int ppu = (int)sprites[0].pixelsPerUnit;
        tilescale = new Vector2Int((int)sprites[0].rect.width / ppu, (int)sprites[0].rect.height / ppu);

        //取得したスプライトを使用してUIボタンを作成して一覧表示する
        for (int i = 0; i < sprites.Length; ++i)
        {
            GameObject button = OtherData.CreateUIButton(sprites[i]);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(i % 2 * 60, -60 * (i / 2));
            int n = i;
            button.GetComponent<Button>().onClick.AddListener(() => { ButtonEvent(n); });
        }

        //現在選択中のタイルを左上に表示する
        GameObject selecttile = OtherData.CreateUIImage(sprites[0]);
        selecttile.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, -30);
        SelectTile = selecttile.GetComponent<Image>();

        MapData = new List<List<int>>();

        //すでにマップのデータがある場合読み込む
        string datapath = OtherData.GetResourcesPath() + @"\" + ExportPNGName + ".txt";
        if (File.Exists(datapath))
        {
            string[] data = OtherData.InportText(datapath);
            string[] size = data[0].Split(',');
            MapSize = new Vector2Int(int.Parse(size[0]), int.Parse(size[1]));
            for(int y = 0; y < MapSize.y; ++y)
            {
                string[] d = data[y + 1].Split(',');
                List<int> xdata = new List<int>();
                for(int x = 0; x < MapSize.x; ++x)
                {
                    xdata.Add(int.Parse(d[x]));
                }
                MapData.Insert(0,xdata);
            }

            for(int y = 0; y < MapSize.y; ++y)
            {
                for(int x = 0; x < MapSize.x; ++x)
                {
                    if (MapData[y][x] >= 0)
                    {
                        float xpos = MapSize.x % 2 == 0 ? -MapSize.x / 2f + 0.5f + x : -(MapSize.x - 1) / 2f + x;
                        float ypos = MapSize.y % 2 == 0 ? -MapSize.y / 2f + 0.5f + y : -(MapSize.y - 1) / 2f + y;
                        GameObject go = OtherData.CreateGameObject(sprites[MapData[y][x]]);
                        go.name = y + "_" + x;
                        if (tilescale == Vector2Int.one)
                        {
                            go.transform.position = new Vector3(xpos, ypos);
                        }
                        else
                        {
                            go.transform.position = new Vector3(xpos + tilescale.x / 2f - 0.5f, ypos - 0.5f);
                        }
                    }
                    
                }
            }


        }
        else
        {
            //マップデータを初期化
            for (int y = 0; y < MapSize.y; ++y)
            {
                List<int> xdata = new List<int>();
                for (int x = 0; x < MapSize.x; ++x)
                {
                    xdata.Add(-1);
                }
                MapData.Add(xdata);
            }
        }

        //枠を作成配置する
        for(int i = 0; i < 4; ++i)
        {
            MapFlame[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        SetMapFlame();
    }

    float scrollvalue = 0;

    //指定された地点を基準にタイルスケールの範囲内にタイルデータがない場合true返す（指定された地点は判定に含まれない）
    //範囲外になる場合、タイルデータがある場合falseを返す
    private bool CheckMapdata(Vector2Int pos)
    {
        for(int y = 0; y < tilescale.y; ++y)
        {
            for(int x = 0; x < tilescale.x; ++x)
            {
                if (y == 0 && x == 0) continue;
                if (pos.x + x >= MapData[0].Count || pos.y - y < 0) return false;
                if (MapData[pos.y - y][pos.x + x] != -1) return false;
            }
        }
        return true;
    }

    //クリック位置にタイルを配置して、マップ情報を書き換えるマウスの処理
    private void MouseTask()
    {

        Vector3 mouseposition = OtherData.GetWorldMousePosition();

        float x = Input.GetAxisRaw("Mouse ScrollWheel");

        if (Canvas.activeSelf)
        {
            //タイルの一覧の上でスクロールした場合に一覧をスクロールする
            if (x != 0)
            {
                scrollvalue += 640 * x * -1;
                if (scrollvalue < 0) scrollvalue = 0;

                scollobj.anchoredPosition = new Vector2(-125, 180 + scrollvalue);
            }
        }
        else
        {
            if (x != 0)
            {
                Camera.main.orthographicSize += 2 * x;
            }
        }

        //UIに触れてなく、クリックされた場合
        if (EventSystem.current.currentSelectedGameObject == null)
        {

            //範囲外をはじく
            float xsize = MapSize.x / 2f, ysize = MapSize.y / 2f;
            if(mouseposition.x <-xsize || mouseposition.x > xsize || mouseposition.y < -ysize || mouseposition.y > ysize) { return; }

            //マップデータの参照位置をもとめる
            int xlistpos = Mathf.RoundToInt(mouseposition.x + MapSize.x / 2f - 0.5f), ylistpos = Mathf.RoundToInt(mouseposition.y + MapSize.y / 2f - 0.5f);

            //タイルの配置する位置をもとめる
            float xworldpos = MapSize.x % 2 == 0 ? Mathf.Floor(mouseposition.x) + 0.5f : Mathf.Round(mouseposition.x);
            float yworldpos = MapSize.y % 2 == 0 ? Mathf.Floor(mouseposition.y) + 0.5f : Mathf.Round(mouseposition.y);

            if (Input.GetMouseButton(0))
            {
                if (CheckMapdata(new Vector2Int(xlistpos, ylistpos)))
                {
                    if (MapData[ylistpos][xlistpos] == -1)
                    {
                        GameObject go = OtherData.CreateGameObject(sprites[SelectTileNumber]);
                        go.name = ylistpos + "_" + xlistpos;
                        if (tilescale == Vector2Int.one)
                        {
                            go.transform.position = new Vector3(xworldpos, yworldpos);
                        }
                        else
                        {
                            go.transform.position = new Vector3(xworldpos + tilescale.x / 2f - 0.5f, yworldpos - tilescale.y / 2 + 0.5f);
                        }
                    }
                    else if (MapData[ylistpos][xlistpos] >= 0)
                    {
                        GameObject go = GameObject.Find(ylistpos + "_" + xlistpos);
                        go.GetComponent<SpriteRenderer>().sprite = sprites[SelectTileNumber];
                    }
                    if (tilescale == Vector2Int.one)
                    {
                        MapData[ylistpos][xlistpos] = SelectTileNumber;
                    }
                    else
                    {
                        for (int yp = 0; yp < tilescale.y; ++yp)
                        {
                            for (int xp = 0; xp < tilescale.x; ++xp)
                            {
                                MapData[ylistpos - yp][xlistpos + xp] = -2;
                            }
                        }
                        MapData[ylistpos][xlistpos] = SelectTileNumber;
                    }
                }
            }
            if (Input.GetMouseButton(1) && MapData[ylistpos][xlistpos] >= 0)
            {
                GameObject dgo = GameObject.Find(ylistpos + "_" + xlistpos);
                Destroy(dgo.gameObject);
                if (tilescale == Vector2Int.one)
                {
                    MapData[ylistpos][xlistpos] = -1;
                }
                else
                {
                    for (int yp = 0; yp < tilescale.y; ++yp)
                    {
                        for (int xp = 0; xp < tilescale.x; ++xp)
                        {
                            MapData[ylistpos - yp][xlistpos + xp] = -1;
                        }
                    }
                }
            }
        }
    }

    //カメラを動かすか
    [SerializeField] bool CameraMove = true;

    //カメラ操作時の処理
    private void CameraCtrlTask()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Camera.main.transform.position += new Vector3(10 * Time.deltaTime * x, 10 * Time.deltaTime * y, 0);
        if (Camera.main.orthographicSize < 5)
        {
            Camera.main.orthographicSize = 5;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);
        }
    }

    //ステージを一枚のPNGとして書き出す
    private void ExportStageData()
    {
        int PixelsPerUnit = (int)sprites[0].pixelsPerUnit;
        Texture2D tex2d = new Texture2D(PixelsPerUnit * MapSize.x, PixelsPerUnit * MapSize.y);
        //タイルデータをもとにテクスチャを埋める
        //
        //Unityのテクスチャは読み込むと左下を基点にする
        //Texture2Dも左下を基点にするため
        //forで0から回した値を最大値-1から引いて逆にしてる
        for (int y = 0; y < MapSize.y; ++y)
        {
            for (int x = 0; x < MapSize.x; ++x)
            {
                if (MapData[y][x] >= 0)
                {
                    int rectposx = (int)sprites[MapData[y][x]].rect.x, rectposy = (int)sprites[MapData[y][x]].rect.y;
                    int tilesizex = PixelsPerUnit * tilescale.x, tilesizey = PixelsPerUnit * tilescale.y;
                    for (int py = 0; py < tilesizey; ++py)
                    {
                        for (int px = 0; px < tilesizex; ++px)
                        {
                            tex2d.SetPixel(PixelsPerUnit * x + px, PixelsPerUnit * y + PixelsPerUnit - 1 - py, sprites[MapData[y][x]].texture.GetPixel(rectposx + px, rectposy + tilesizey - 1 -  py));
                        }
                    }
                }
                else if(MapData[y][x] != -2)
                {
                    for (int py = 0; py < PixelsPerUnit; ++py)
                    {
                        for (int px = 0; px < PixelsPerUnit; ++px)
                        {
                            tex2d.SetPixel(PixelsPerUnit * x + px, PixelsPerUnit * y + PixelsPerUnit - 1 - py, new Color(0, 0, 0, 0));
                        }
                    }
                }
            }
        }

        //完成したテクスチャをバイト配列に変換
        byte[] bytedata = tex2d.EncodeToPNG();

        //pngの書き出し先のパスを確定
        string pngpath = OtherData.GetResourcesPath();
        if(ExportFolderName != null && ExportFolderName != "")
        {
            pngpath += "/" + ExportFolderName;
        }
        pngpath += "/" + ExportPNGName;
        if (IsColliderPNG)
        {
            pngpath += "_Collider";
        }
        pngpath += ".png";
        
        Debug.Log(pngpath);
        //pngを書き出し
        File.WriteAllBytes(pngpath, bytedata);

        //タイルデータの書き出し先のパスを確定
        string datapath = OtherData.GetResourcesPath() + @"\" + ExportPNGName + ".txt";
        //書き出しようのデータを作成
        string[] data = new string[MapSize.y + 1];
        data[0] = MapSize.x + "," + MapSize.y;
        for(int y = 0; y < MapSize.y; ++y)
        {
            string d = "";
            for(int x = 0; x < MapSize.x; ++x)
            {
                d += MapData[MapSize.y - y - 1][x] + ",";
            }
            data[y + 1] = d;
        }
        //タイルデータを書き出し
        OtherData.ExportText(data, datapath);
    }

    // Update is called once per frame
    void Update()
    {
        //UIの表示・非表示を切り替える
        if (Input.GetKeyDown(KeyCode.V))
        {
            Canvas.SetActive(!Canvas.activeSelf);
        }

        MouseTask();

        //カメラの動作を切り替える
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraMove = !CameraMove;
        }
        if (CameraMove)
        {
            CameraCtrlTask();
        }

        //書き出しをする
        if (Input.GetKeyDown(KeyCode.O))
        {
            ExportStageData();
        }
    }
}
