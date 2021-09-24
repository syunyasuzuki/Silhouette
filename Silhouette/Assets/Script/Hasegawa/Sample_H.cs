using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_H : MonoBehaviour
{

    FlashCtrl flashctrl = null;

    [SerializeField] SpriteRenderer bg = null;

    [SerializeField] Color32 DefaultColor = new Color32(0, 104, 183, 255);

    private Color32 Black = new Color32(0, 0, 0, 255);

    // Start is called before the first frame update
    void Start()
    {
        flashctrl = GameObject.Find("ThunderClouds").GetComponent<FlashCtrl>();
        bg.color = Black;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashctrl.GetFlash())
        {
            bg.color = DefaultColor;
        }
        else
        {
            bg.color = Black;
        }




    }
}
