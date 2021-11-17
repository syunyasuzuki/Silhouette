using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCtrl : MonoBehaviour
{
    //背景に配置する偽物
    [SerializeField] GameObject DummyObject = null;

    //偽物の数
    [SerializeField] int CreateNumberOfDummy = 10;

    [SerializeField] float Min_pos = -5f;

    [SerializeField] float Max_pos = 100f;

    //偽物一つのデータ
    private struct DummyData
    {
        private GameObject dummy;
        private SpriteRenderer sr;

        /// <summary>
        /// 偽物の作成
        /// </summary>
        public void Create(GameObject d, float min, float max)
        {
            dummy = Instantiate(d);
            dummy.transform.localScale = new Vector3(Random.Range(1f, 1.5f), Random.Range(1f, 1.5f), 1);
            dummy.transform.position = new Vector3(Random.Range(min, max), -4, 0);
            sr = dummy.GetComponent<SpriteRenderer>();
        }

        //偽物の削除
        public void Delete()
        {
            Destroy(dummy.gameObject);
            sr = null;
        }

        //a値の変更
        public void ChangeAlpha(byte a)
        {
            sr.color = new Color32(60, 60, 60, a);
        }

        //色を黒色にする
        public void Color_LightBlack()
        {
            //sr.color = new Color(1f / 255 * 33, 1f / 255 * 33, 1f / 255 * 33, 1);
            sr.color = new Color32(0, 0, 0, 255);
        }

        //色を灰色にする
        public void Color_Default()
        {
            //sr.color = new Color(1f / 255 * 199, 1f / 255 * 21, 1f / 255 * 133, 1);
            sr.color = new Color32(128, 128, 128, 255);
        }
    }

    //生成した偽物
    DummyData[] DummyDataList = null;

    //偽物を生成する
    private void CreateDummyObjects()
    {
        DummyDataList = new DummyData[CreateNumberOfDummy];
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Create(DummyObject,Min_pos,Max_pos);
            DummyDataList[i].Color_LightBlack();
        }


    }

    /// <summary>
    /// 偽物のa値を変える
    /// </summary>
    public void ChangeAlpha(float a)
    {
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            //DummyDataList[i].ChangeAlpha(a);
        }
    }

    /// <summary>
    /// 偽物の色を灰色にする
    /// </summary>
    public void ChangeDefaultColor()
    {
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Color_Default();
        }
    }

    /// <summary>
    /// 偽物の色を黒色にする
    /// </summary>
    public void ChangeDummyColor()
    {
        for (int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Color_LightBlack();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDummyObjects();
    }

}
