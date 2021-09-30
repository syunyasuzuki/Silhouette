using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSarchTest : MonoBehaviour
{

    const int SizeX = 15;

    const int SizeY = 12;

    int[,] TileData = new int[12, 15]
    {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,0,0,0,0,1,1,1,1,1},
        {1,1,1,1,1,0,0,0,0,0,0,1,1,1,1},
        {1,1,1,1,1,0,0,0,0,0,0,1,1,1,1},
        {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
        {1,0,0,0,1,1,1,0,0,1,1,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,1,1,1,1,1,1,1,0,0,0,1},
        {1,1,1,1,1,1,0,0,0,1,1,0,1,1,1},
        {1,1,1,0,0,1,0,0,0,1,0,0,1,1,1},
        {1,1,1,1,0,0,0,0,0,0,0,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    };

    PolygonCollider2D pc2 = null;

    int[,] SarchedTileData = new int[12, 15];

    //指定された位置の情報を返す
    //もし範囲外の場合0を返す
    //また探索済みとして記録する
    private int SarchArray(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= SizeX || pos.y < 0 || pos.y >= SizeY)
        {
            return 0;
        }
        else
        {
            SarchedTileData[pos.y, pos.x] = TileData[pos.y, pos.x] + 1;
            return TileData[pos.y, pos.x];
        }
    }

    private enum BasisPoint
    {
        Basis = 0,
        LeftDown = 1,
        RightDown = 2,
        RightUp = 3
    }

    private Vector2Int Basis = Vector2Int.zero;
    private Vector2Int LeftDown = new Vector2Int(0, 1);
    private Vector2Int RightDown = new Vector2Int(1, 1);
    private Vector2Int RightUp = new Vector2Int(1, 0);



    // Start is called before the first frame update
    void Start()
    {
        pc2 = gameObject.AddComponent<PolygonCollider2D>();

        //頂点情報を入れたリスト
        List<List<Vector2>> vc2listlist = new List<List<Vector2>>();

        for (int y = 0; y < SizeY; ++y)
        {
            for (int x = 0; x < SizeX; ++x)
            {
                if (SarchedTileData[y, x] == 0)
                {
                    SarchedTileData[y, x] = TileData[y, x] + 1;

                    if (SarchedTileData[y, x] == 2)
                    {
                        if(SarchArray(new Vector2Int(x - 1, y - 1)) == 1 && SarchArray(new Vector2Int(x, y - 1)) == 1 && SarchArray(new Vector2Int(x - 1, y)) == 1) { break; }
                        List<Vector2> vc2list = new List<Vector2>();

                        vc2list.Add(new Vector2(-SizeX / 2f + x, SizeY / 2f - y));
                        Vector2Int position = new Vector2Int(x, y);
                        Vector2Int vector = Basis;
                        BasisPoint nowbasis = BasisPoint.Basis;
                        while (true)
                        {
                            switch (nowbasis)
                            {
                                case BasisPoint.Basis:
                                    if (SarchArray(position + new Vector2Int(-1, -1)) == 1)
                                    {
                                        if (SarchArray(position + new Vector2Int(0, -1)) == 1)
                                        {
                                            //水色
                                            position += new Vector2Int(-1, -1);
                                            nowbasis = BasisPoint.LeftDown;
                                            vector = LeftDown;
                                        }
                                        else
                                        {
                                            //オレンジ
                                            position += new Vector2Int(-1, -1);
                                            nowbasis = BasisPoint.RightUp;
                                            vector = RightUp;
                                        }
                                    }
                                    else if (SarchArray(position + new Vector2Int(-1, 0)) == 1)
                                    {
                                        //緑
                                        position += new Vector2Int(-1, 0);
                                        nowbasis = BasisPoint.Basis;
                                        vector = Basis;
                                    }
                                    else
                                    {
                                        //青
                                        nowbasis = BasisPoint.LeftDown;
                                        vector = LeftDown;
                                    }
                                    break;
                                case BasisPoint.LeftDown:
                                    if (SarchArray(position + new Vector2Int(-1, 1)) == 1)
                                    {
                                        if (SarchArray(position + new Vector2Int(-1, 0)) == 1)
                                        {
                                            //水色
                                            position += new Vector2Int(-1, 1);
                                            nowbasis = BasisPoint.RightDown;
                                            vector = RightDown;
                                        }
                                        else
                                        {
                                            //オレンジ
                                            position += new Vector2Int(-1, 1);
                                            nowbasis = BasisPoint.Basis;
                                            vector = Basis;
                                        }
                                    }
                                    else if (SarchArray(position + new Vector2Int(0, 1)) == 1)
                                    {
                                        //緑
                                        position += new Vector2Int(0, 1);
                                        nowbasis = BasisPoint.LeftDown;
                                        vector = LeftDown;
                                    }
                                    else
                                    {
                                        //青
                                        nowbasis = BasisPoint.RightDown;
                                        vector = RightDown;
                                    }
                                    break;
                                case BasisPoint.RightDown:
                                    if (SarchArray(position + new Vector2Int(1, 1)) == 1)
                                    {
                                        if (SarchArray(position + new Vector2Int(0, 1)) == 1)
                                        {
                                            //水色
                                            position += new Vector2Int(1, 1);
                                            nowbasis = BasisPoint.RightUp;
                                            vector = RightUp;
                                        }
                                        else
                                        {
                                            //オレンジ
                                            position += new Vector2Int(1, 1);
                                            nowbasis = BasisPoint.LeftDown;
                                            vector = LeftDown;
                                        }
                                    }
                                    else if (SarchArray(position + new Vector2Int(1, 0)) == 1)
                                    {
                                        //緑
                                        position += new Vector2Int(1, 0);
                                        nowbasis = BasisPoint.RightDown;
                                        vector = RightDown;
                                    }
                                    else
                                    {
                                        //青
                                        nowbasis = BasisPoint.RightUp;
                                        vector = RightUp;
                                    }
                                    break;
                                case BasisPoint.RightUp:
                                    if (SarchArray(position + new Vector2Int(1, -1)) == 1)
                                    {
                                        if (SarchArray(position + new Vector2Int(1, 0)) == 1)
                                        {
                                            //水色
                                            position += new Vector2Int(1, -1);
                                            nowbasis = BasisPoint.Basis;
                                            vector = Basis;
                                        }
                                        else
                                        {
                                            //オレンジ
                                            position += new Vector2Int(1, -1);
                                            nowbasis = BasisPoint.RightDown;
                                            vector = RightDown;
                                        }
                                    }
                                    else if (SarchArray(position + new Vector2Int(0, -1)) == 1)
                                    {
                                        //緑
                                        position += new Vector2Int(0, -1);
                                        nowbasis = BasisPoint.RightUp;
                                        vector = RightUp;
                                    }
                                    else
                                    {
                                        //青
                                        nowbasis = BasisPoint.Basis;
                                        vector = Basis;
                                    }
                                    break;
                            }
                            vc2list.Add(new Vector2(-SizeX / 2f + position.x + vector.x, SizeY / 2f - position.y - vector.y));

                            if (vc2list.Count > 3 && vc2list[0] == vc2list[vc2list.Count - 1])
                            {
                                vc2list.RemoveAt(vc2list.Count - 1);
                                vc2listlist.Add(vc2list);
                                break;
                            }

                        }
                    }
                }
            }
        }



        pc2.pathCount = vc2listlist.Count;
        for (int i = 0; i < vc2listlist.Count; ++i)
        {
            pc2.SetPath(i, vc2listlist[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
