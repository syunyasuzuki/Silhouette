using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの乗ることのできる足場を光った時だけ見えるようにする
public class PlatformCtrl : MonoBehaviour
{
    //見えるようにする足場
    List<SpriteRenderer> Platformsrs = new List<SpriteRenderer>();

    //見えるようにする背景
    List<SpriteRenderer> Backsrs = new List<SpriteRenderer>();

    public void SetPlatformsr(SpriteRenderer sr)
    {
        Platformsrs.Add(sr);
    }

    public void SetBacksr(SpriteRenderer sr)
    {
        Backsrs.Add(sr);
    }


    //足場を見えるようにする
    public void VisiblePlatform()
    {
        if (Platformsrs.Count <= 0) return;

        for(int i = 0; i < Platformsrs.Count; ++i)
        {
            Platformsrs[i].color = new Color(1, 1, 1, 1);
        }
    }

    //足場を見えなくする
    public void UnVisiblePlatform()
    {
        if (Platformsrs.Count <= 0) return;

        for(int i = 0; i < Platformsrs.Count; ++i)
        {
            Platformsrs[i].color = new Color(1, 1, 1, 0);
        }
    }

    //足場のa値を変える
    public void ChangeAlpha(float a)
    {
        if (Platformsrs.Count <= 0) return;

        for(int i = 0; i < Platformsrs.Count; ++i)
        {
            Platformsrs[i].color = new Color(1, 1, 1, a);
        }
    }

    public void ChangeBackColorDefault()
    {
        if (Backsrs.Count <= 0) return;

        for(int i = 0; i < Backsrs.Count; ++i)
        {
            Backsrs[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void ChangeBackColorBlack()
    {
        if (Backsrs.Count <= 0) return;

        for(int i = 0; i < Backsrs.Count; ++i)
        {
            Backsrs[i].color = new Color(0, 0, 0, 1);
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(Backsrs.Count);
        }
    }
}
