using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの乗ることのできる足場を光った時だけ見えるようにする
public class PlatformCtrl : MonoBehaviour
{
    //見えるようにする足場
    [SerializeField] SpriteRenderer Platformsr = null;

    //足場を見えるようにする
    public void VisiblePlatform()
    {
        if (Platformsr == null) return;

        Platformsr.color = new Color(1, 1, 1, 1);
    }

    //足場を見えなくする
    public void UnVisiblePlatform()
    {
        if (Platformsr == null) return;

        Platformsr.color = new Color(1, 1, 1, 0);
    }

    //足場のa値を変える
    public void ChangeAlpha(float a)
    {
        if (Platformsr == null) return;

        Platformsr.color = new Color(1, 1, 1, a);
    }


}
