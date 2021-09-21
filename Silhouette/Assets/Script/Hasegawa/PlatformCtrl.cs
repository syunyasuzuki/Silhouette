using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr = null;

    public void VisiblePlatform()
    {
        if (sr == null) return;

        sr.color = new Color(1, 1, 1, 1);
    }

    public void UnVisiblePlatform()
    {
        if (sr == null) return;

        sr.color = new Color(1, 1, 1, 0);
    }

    public void ChangeAlpha(float a)
    {
        if (sr == null) return;

        sr.color = new Color(1, 1, 1, a);
    }


}
