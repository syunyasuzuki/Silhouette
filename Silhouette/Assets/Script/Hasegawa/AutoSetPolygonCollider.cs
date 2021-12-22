using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetPolygonCollider : MonoBehaviour
{
    //当たり判定用のスプライトが別である場合に使う
    [SerializeField] Sprite ColliderSprite = null;

    //斜めを検知して頂点数を減らすか
    [SerializeField] bool Seimitu = true;

    //IsTriggerを有効にするか
    [SerializeField] bool IsTrigger = true;

    //スライスされた素材か
    [SerializeField] bool IsSliceMaterial = false;

    //スプライトのキャッシュ用
    Sprite MaterialSprite = null;

    //自身のゲームオブジェクトの名前をキャッシュする
    string ObjectName = "";

    private void Start()
    {

        DetectionEditorChange.ExitPlayingReady += CashSprite;
        DetectionEditorChange.ExitPlaying += PolygonSet;

    }

    //頂点情報
    private struct PointData
    {
        private Vector2Int Position;
        private Vector2Int Vector;
        //頂点情報を保存
        public void SetData(Vector2Int pos, Vector2Int vec)
        {
            Position = pos;
            Vector = vec;
        }

        //実際の頂点情報を取得
        public Vector2Int GetData()
        {
            return Position + Vector;
        }
    }

    //頂点探索時の方向
    private enum BasisPoint
    {
        Basis = 0,
        LeftDown = 1,
        RightDown = 2,
        RightUp = 3
    }

    //キャッシュするもの
    private void CashSprite()
    {
        //当たり判定用のスプライトが存在する場合
        if (ColliderSprite != null)
        {
            MaterialSprite = ColliderSprite;
        }
        //しない場合
        else
        {
            MaterialSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        ObjectName = gameObject.name;
    }

    //スプライトのピクセル単位でポリゴンコライダーをセットする
    private void PolygonSet()
    {
        //各方向の値
        Vector2Int Basis = Vector2Int.zero;
        Vector2Int LeftDown = new Vector2Int(0, 1);
        Vector2Int RightDown = new Vector2Int(1, 1);
        Vector2Int RightUp = new Vector2Int(1, 0);

        //スプライトのテクスチャ
        Texture2D texture = MaterialSprite.texture;

        //スプライトの大きさ、テクスチャ全体での位置
        int sx = (int)MaterialSprite.rect.width, sy = (int)MaterialSprite.rect.height;
        int px = (int)MaterialSprite.rect.x, py = (int)MaterialSprite.rect.y;

        //スプライトのタイル情報
        int[,] textureinfo = new int[sy, sx];

        //スライスされた素材の場合左下基点で読み込む
        if (IsSliceMaterial)
        {
            for (int y = py + sy - 1; y >= py; --y)
            {
                for (int x = px; x < px + sx; ++x)
                {
                    if (texture.GetPixel(x, y).a > 0)
                    {
                        textureinfo[sy - 1 - (y - py), x - px] = 1;
                    }
                }
            }
        }
        //スライスされてない場合左上基点で読み込む
        else
        {
            for (int y = py; y < py + sy; ++y)
            {
                for (int x = px; x < px + sx; ++x)
                {
                    if (texture.GetPixel(x, y).a > 0)
                    {
                        textureinfo[sy - 1 - y % sy, x % sx] = 1;
                    }
                }
            }
        }


        //探索済みのところを記録するやつ
        int[,] SarchedTileData = new int[sy, sx];

        //指定された位置の情報を返す
        //もし範囲外の場合0を返す
        //また探索済みとして記録する
        int SarchArray(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= sx || pos.y < 0 || pos.y >= sy)
            {
                return 0;
            }
            else
            {
                SarchedTileData[pos.y, pos.x] = 1;
                return textureinfo[pos.y, pos.x];
            }
        }

        //指定された位置の情報を返す
        //もし範囲外の場合0を返す
        //探索済みの記録は行わない
        int SarchArray2(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= sx || pos.y < 0 || pos.y >= sy)
            {
                return 0;
            }
            else
            {
                return textureinfo[pos.y, pos.x];
            }
        }

        //頂点情報を入れたリスト
        List<List<PointData>> vc2listlist = new List<List<PointData>>();

        List<PointData> vc2list = new List<PointData>();
        for (int y = 0; y < sy; ++y)
        {
            for (int x = 0; x < sx; ++x)
            {
                //探索されてない場合
                if (SarchedTileData[y, x] == 0)
                {
                    //探索済みにする
                    SarchedTileData[y, x] = 1;

                    //位置、方向の情報を初期化する
                    Vector2Int position = Vector2Int.zero;
                    Vector2Int vector = Vector2Int.zero;
                    BasisPoint nowbasis = BasisPoint.Basis;

                    //頂点探索処理
                    bool sarchmode = false;
                    //スプライト情報がある場合
                    if (textureinfo[y, x] == 1)
                    {
                        //探索しようとしているところの上、下、左、右のスプライトの情報がすべてうまっている場合処理を見送る
                        if (SarchArray2(new Vector2Int(x + 1, y)) == 1 && SarchArray2(new Vector2Int(x - 1, y)) == 1 && SarchArray2(new Vector2Int(x, y - 1)) == 1 && SarchArray2(new Vector2Int(x, y + 1)) == 1) { continue; }

                        //探索しようとしているところの上、左、右のスプライトの情報がすべてうまっていて、下のスプライトの情報がない場合処理を見送る
                        if (SarchArray2(new Vector2Int(x + 1, y)) == 1 && SarchArray2(new Vector2Int(x - 1, y)) == 1 && SarchArray2(new Vector2Int(x, y - 1)) == 1 && SarchArray2(new Vector2Int(x, y + 1)) == 0) { continue; }

                        vc2list = new List<PointData>();

                        //探索しようとしているところの上、左のスプライトの情報がすべてうまっていて、右のスプライトの情報がない場合
                        if (SarchArray2(new Vector2Int(x - 1, y)) == 1 && SarchArray2(new Vector2Int(x, y - 1)) == 1 && SarchArray2(new Vector2Int(x + 1, y)) == 0)
                        {
                            //位置を一つ上に
                            position = new Vector2Int(x, y - 1);
                            //右下方向にする
                            vector = RightDown;
                            nowbasis = BasisPoint.RightDown;
                        }
                        else
                        {
                            //位置を現在位置に
                            position = new Vector2Int(x, y);
                            //左上方向にする
                            vector = Basis;
                            nowbasis = BasisPoint.Basis;
                        }

                        //頂点情報を保存
                        PointData pd = new PointData();
                        pd.SetData(position, vector);
                        vc2list.Add(pd);
                        //頂点探索処理を有効にする
                        sarchmode = true;
                    }
                    //スプライト情報がない場合
                    //（主に未探索潰し）
                    else if (textureinfo[y, x] == 0)
                    {
                        //探索しようとしているところの上、下、左、右のスプライトの情報がすべてない場合処理を見送る
                        if (SarchArray(new Vector2Int(x + 1, y)) == 0 && SarchArray(new Vector2Int(x - 1, y)) == 0 && SarchArray(new Vector2Int(x, y - 1)) == 0 && SarchArray(new Vector2Int(x, y + 1)) == 0) { continue; }

                        //探索しようとしているところの上、左のスプライトの情報がすべてうまっている場合
                        if (SarchArray(new Vector2Int(x - 1, y)) == 1 && SarchArray(new Vector2Int(x, y - 1)) == 1)
                        {
                            vc2list = new List<PointData>();

                            //位置を左上に
                            position = new Vector2Int(x - 1, y - 1);
                            //方向を右下に
                            vector = RightDown;
                            nowbasis = BasisPoint.RightDown;

                            //頂点情報を保存
                            PointData pd = new PointData();
                            pd.SetData(position, vector);
                            vc2list.Add(pd);

                            //頂点探索処理を有効にする
                            sarchmode = true;
                        }

                    }

                    //頂点探索処理が有効になっている場合
                    while (sarchmode)
                    {
                        //現在の方向が
                        switch (nowbasis)
                        {
                            //左上の場合
                            case BasisPoint.Basis:
                                //現在の位置の左上にスプライトの情報がある場合
                                if (SarchArray(position + new Vector2Int(-1, -1)) == 1)
                                {
                                    //現在の位置の上にスプライトの情報がある場合
                                    if (SarchArray(position + new Vector2Int(0, -1)) == 1)
                                    {
                                        //現在の位置を左上にずらす
                                        position += new Vector2Int(-1, -1);
                                        //方向を左下に
                                        nowbasis = BasisPoint.LeftDown;
                                        vector = LeftDown;
                                    }
                                    //ない場合
                                    else
                                    {
                                        //現在の位置を左上にずらす
                                        position += new Vector2Int(-1, -1);
                                        //方向を右上に
                                        nowbasis = BasisPoint.RightUp;
                                        vector = RightUp;
                                    }
                                }
                                //なく
                                //現在の位置の左にスプライトの情報がある場合
                                else if (SarchArray(position + new Vector2Int(-1, 0)) == 1)
                                {
                                    //現在の位置を左にずらす
                                    position += new Vector2Int(-1, 0);
                                    //方向を左上に
                                    nowbasis = BasisPoint.Basis;
                                    vector = Basis;
                                }
                                //どれでもない場合
                                else
                                {
                                    //方向を左下に
                                    nowbasis = BasisPoint.LeftDown;
                                    vector = LeftDown;
                                }
                                break;
                            //左下の場合
                            case BasisPoint.LeftDown:
                                //現在の位置の左下にスプライトの情報がある場合
                                if (SarchArray(position + new Vector2Int(-1, 1)) == 1)
                                {
                                    //現在の位置の左にスプライトの情報がある場合
                                    if (SarchArray(position + new Vector2Int(-1, 0)) == 1)
                                    {
                                        //現在の位置を左下にずらす
                                        position += new Vector2Int(-1, 1);
                                        //方向を右下に
                                        nowbasis = BasisPoint.RightDown;
                                        vector = RightDown;
                                    }
                                    //ない場合
                                    else
                                    {
                                        //現在の位置を左下にずらす
                                        position += new Vector2Int(-1, 1);
                                        //方向を左上に
                                        nowbasis = BasisPoint.Basis;
                                        vector = Basis;
                                    }
                                }
                                //なく
                                //現在の位置の下にスプライトの情報がある場合
                                else if (SarchArray(position + new Vector2Int(0, 1)) == 1)
                                {
                                    //現在の位置を下にずらす
                                    position += new Vector2Int(0, 1);
                                    //方向を左下に
                                    nowbasis = BasisPoint.LeftDown;
                                    vector = LeftDown;
                                }
                                //どれでもない場合
                                else
                                {
                                    //方向を右下に
                                    nowbasis = BasisPoint.RightDown;
                                    vector = RightDown;
                                }
                                break;
                            //右下の場合
                            case BasisPoint.RightDown:
                                //現在の位置の右下にスプライトの情報があるある場合
                                if (SarchArray(position + new Vector2Int(1, 1)) == 1)
                                {
                                    //現在の位置の下にスプライトの情報がある場合
                                    if (SarchArray(position + new Vector2Int(0, 1)) == 1)
                                    {
                                        //現在の位置を右下にずらす
                                        position += new Vector2Int(1, 1);
                                        //方向を右上に
                                        nowbasis = BasisPoint.RightUp;
                                        vector = RightUp;
                                    }
                                    //ない場合
                                    else
                                    {
                                        //現在の位置を右下にずらす
                                        position += new Vector2Int(1, 1);
                                        //方向を左下に
                                        nowbasis = BasisPoint.LeftDown;
                                        vector = LeftDown;
                                    }
                                }
                                //なく
                                //現在の位置の右にスプライトの情報がある場合
                                else if (SarchArray(position + new Vector2Int(1, 0)) == 1)
                                {
                                    //現在の位置を右にずらす
                                    position += new Vector2Int(1, 0);
                                    //方向を右下に
                                    nowbasis = BasisPoint.RightDown;
                                    vector = RightDown;
                                }
                                //どれでもない場合
                                else
                                {
                                    //方向を右上に
                                    nowbasis = BasisPoint.RightUp;
                                    vector = RightUp;
                                }
                                break;
                            //右上の場合
                            case BasisPoint.RightUp:
                                //現在の位置の右上にスプライトの情報がある場合
                                if (SarchArray(position + new Vector2Int(1, -1)) == 1)
                                {
                                    //現在の位置の右にスプライトの情報がある場合
                                    if (SarchArray(position + new Vector2Int(1, 0)) == 1)
                                    {
                                        //現在の位置を右上にずらす
                                        position += new Vector2Int(1, -1);
                                        //方向を左上に
                                        nowbasis = BasisPoint.Basis;
                                        vector = Basis;
                                    }
                                    //ない場合
                                    else
                                    {
                                        //現在の位置を右上にずらす
                                        position += new Vector2Int(1, -1);
                                        //方向を右下に
                                        nowbasis = BasisPoint.RightDown;
                                        vector = RightDown;
                                    }
                                }
                                //なく
                                //現在の位置の上にスプライトの情報がある場合
                                else if (SarchArray(position + new Vector2Int(0, -1)) == 1)
                                {
                                    //現在の位置を上にずらす
                                    position += new Vector2Int(0, -1);
                                    //方向を右上に
                                    nowbasis = BasisPoint.RightUp;
                                    vector = RightUp;
                                }
                                //どれでもない場合
                                else
                                {
                                    //方向を左上に
                                    nowbasis = BasisPoint.Basis;
                                    vector = Basis;
                                }
                                break;
                        }

                        //頂点情報を保存する
                        PointData pd = new PointData();
                        pd.SetData(position, vector);
                        vc2list.Add(pd);

                        //頂点の数が3つ以上ある場合
                        if (vc2list.Count >= 3)
                        {
                            //一番後ろ（一番追加したのが新しい）にある頂点情報とその１つ前に追加した頂点情報のxかyが等しい、かつ一番後ろ（一番追加したのが新しい）にある頂点情報とその２つ前に追加した頂点情報のxかyが等しい場合
                            if ((vc2list[vc2list.Count - 1].GetData().x == vc2list[vc2list.Count - 2].GetData().x || vc2list[vc2list.Count - 1].GetData().y == vc2list[vc2list.Count - 2].GetData().y) && (vc2list[vc2list.Count - 1].GetData().x == vc2list[vc2list.Count - 3].GetData().x || vc2list[vc2list.Count - 1].GetData().y == vc2list[vc2list.Count - 3].GetData().y))
                            {
                                //1つ前に追加した頂点情報を削除する
                                vc2list.RemoveAt(vc2list.Count - 2);
                            }
                        }

                        //頂点の数が３つより多く、かつ一番最初の頂点情報と一番後ろの頂点情報が等しい場合
                        if (vc2list.Count > 3 && vc2list[0].GetData() == vc2list[vc2list.Count - 1].GetData())
                        {
                            //一番後ろの頂点情報を削除する
                            vc2list.RemoveAt(vc2list.Count - 1);

                            //斜めの検知が有効な場合
                            if (Seimitu)
                            {
                                //基点を初期化
                                int basepoint = 0;
                                //１つ目の斜めの情報を作成
                                Vector2Int value = vc2list[2].GetData() - vc2list[0].GetData();
                                //基点から削除予定の頂点の数を設定
                                int count = 2;

                                while (true)
                                {
                                    //合計頂点数を超える場合
                                    if (count + 2 >= vc2list.Count)
                                    {
                                        //斜めの判定がある場合
                                        if (count - basepoint > 2)
                                        {
                                            vc2list.RemoveRange(basepoint + 1, count - basepoint - 1);
                                        }
                                        break;
                                    }
                                    //超えない場合
                                    else
                                    {
                                        int subcount = count + 2;
                                        //現在参照しているところから２つ先までの移動量が保存されている移動量と同じ場合
                                        //斜めの判定をとっている幅を広げる
                                        if (vc2list[subcount].GetData() - vc2list[count].GetData() == value)
                                        {
                                            count = subcount;
                                        }
                                        //違う場合
                                        else
                                        {
                                            //斜めの判定がない場合
                                            if (count - basepoint <= 2)
                                            {
                                                //次の斜め判定をとる幅がない場合終了する
                                                if (basepoint + 3 >= vc2list.Count)
                                                {
                                                    break;
                                                }
                                                //まだ幅がある場合次の判定を作成する
                                                else
                                                {
                                                    ++basepoint;
                                                    count = basepoint + 2;
                                                    value = vc2list[count].GetData() - vc2list[basepoint].GetData();
                                                }
                                            }
                                            //斜めの判定がある場合
                                            else
                                            {
                                                //斜めになっている部分の最初と最後の頂点部分以外を削除する
                                                vc2list.RemoveRange(basepoint + 1, count - basepoint - 1);

                                                //次の斜め判定をとる幅がない場合終了する
                                                if (basepoint + 3 >= vc2list.Count)
                                                {
                                                    break;
                                                }
                                                //まだ幅がある場合次の判定を作成する
                                                else
                                                {
                                                    ++basepoint;
                                                    count = basepoint + 2;
                                                    value = vc2list[count].GetData() - vc2list[basepoint].GetData();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //全体の頂点リストに追加する
                            vc2listlist.Add(vc2list);
                            break;
                        }


                    }

                }
            }
        }

        //もし頂点情報がなにもない場合処理をはじく
        if (vc2listlist.Count <= 0)
        {
            return;
        }

        //キャッシュしてあった名前から自身を取得
        GameObject go = GameObject.Find(ObjectName);

        //ポリゴンコライダーを追加
        PolygonCollider2D PC2D = go.AddComponent<PolygonCollider2D>();
        //ポリゴンコライダーに頂点の塊の数を代入
        PC2D.pathCount = vc2listlist.Count;
        for (int i = 0; i < vc2listlist.Count; ++i)
        {
            //ポリゴンコライダーに渡すユニティサイズを加味した実際の頂点データを作成する
            List<Vector2> vc2 = new List<Vector2>();
            for (int n = 0; n < vc2listlist[i].Count; ++n)
            {
                Vector2Int data = vc2listlist[i][n].GetData();
                vc2.Add(new Vector2(-sx / 2f + data.x, sy / 2f - data.y) / MaterialSprite.pixelsPerUnit);
            }
            //ポリゴンコライダーに頂点データを渡す
            PC2D.SetPath(i, vc2);
        }

        //IsTriggerが有効な場合
        if (IsTrigger)
        {
            //IsTriggerをつける
            PC2D.GetComponent<PolygonCollider2D>().isTrigger = IsTrigger;
            //識別用の壁のタグをつける
            go.tag = "Wall";
            //レイヤーを5に
            go.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        else
        {
            //レイヤーを10に
            go.GetComponent<SpriteRenderer>().sortingOrder = 10;
        }

        //アタッチされているこのスクリプト自身を削除する
        DestroyImmediate(go.GetComponent<AutoSetPolygonCollider>());
    }
}
