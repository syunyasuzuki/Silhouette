using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//対象の移動速度に応じてスクロール速度を合わせるやつ
public class BackgroundCtrl : MonoBehaviour
{

    //背景に配置する偽物
    [SerializeField] GameObject[] Background = null;

    private List<SpriteRenderer> Backgroundsr = new List<SpriteRenderer>();

    [SerializeField] SpriteRenderer[] Other = null;

    [SerializeField] GameObject Target = null;

    private float x = 0;

    /// <summary>
    /// 偽物のa値を変える
    /// </summary>
    public void ChangeAlpha(byte a)
    {
        for (int i = 0; i < Backgroundsr.Count; ++i)
        {
            Backgroundsr[i].color = new Color32(a, a, a, 255);
        }
    }

    /// <summary>
    /// 偽物の色を灰色にする
    /// </summary>
    public void ChangeDefaultColor()
    {
        for (int i = 0; i < Backgroundsr.Count; ++i)
        {
            Backgroundsr[i].color = new Color32(128, 128, 128, 255);
        }
    }

    /// <summary>
    /// 偽物の色を黒色にする
    /// </summary>
    public void ChangeDummyColor()
    {
        for (int i = 0; i < Backgroundsr.Count; ++i)
        {
            Backgroundsr[i].color = new Color32(0, 0, 0, 255);
        }
    }

    private void Start()
    {
        for(int i = 0; i < Background.Length; ++i)
        {
            SpriteRenderer sr = Background[i].GetComponent<SpriteRenderer>();
            Backgroundsr.Add(sr);
        }

        for (int i = 0; i < Other.Length; ++i)
        {
            Backgroundsr.Add(Other[i]);
        }

        ChangeDummyColor();

        x = Target.transform.position.x;
    }

    private void Update()
    {
        float x2 = Target.transform.position.x - x;
        x = Target.transform.position.x;
        for(int i = 0; i < Background.Length; ++i)
        {
            if (i > 0)
            {
                Background[i].transform.position += new Vector3(-x2 / (i + 1), 0, 0);
            }

            if (Mathf.Abs(Target.transform.position.x - Background[i].transform.position.x) >= 28.8 * 2.1f)
            {
                Background[i].transform.position = new Vector3(Target.transform.position.x, 0, 0);
            }
        }

    }
}
