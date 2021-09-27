using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCtrl : MonoBehaviour
{
    //表示するスプライト（順番）
    [SerializeField] Sprite[] textsprite = null;

    //雷
    private FlashCtrl flashctrl = null;

    //
    private SpriteRenderer sr = null;

    private Color32 _Black = new Color32(0, 0, 0, 1);

    private Color32 _Default = new Color32(1, 1, 1, 1);

    private int flashcount = 0;

    [SerializeField] int flashratecount = 3;

    private Camera maincam = null;

    private bool nowflash = false;


    // Start is called before the first frame update
    void Start()
    {
        flashctrl = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();

        maincam = Camera.main;

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.color = _Black;

        if (textsprite != null)
        {
            sr.sprite = textsprite[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (flashctrl.GetFlash() != nowflash)
        {
            nowflash = flashctrl.GetFlash();
            ++flashcount;

        }
    }
}
