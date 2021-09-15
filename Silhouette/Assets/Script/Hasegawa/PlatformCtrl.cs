using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr = null;

    public void VisiblePlatform()
    {
        sr.color = new Color(1, 1, 1, 1);
    }

    public void UnVisiblePlatform()
    {
        sr.color = new Color(1, 1, 1, 0);
    }

    public void ChangeAlpha(float a)
    {
        sr.color = new Color(1, 1, 1, a);
    }


}
