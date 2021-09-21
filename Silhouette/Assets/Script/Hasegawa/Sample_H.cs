using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_H : MonoBehaviour
{

    FlashCtrl flashctrl = null;




    // Start is called before the first frame update
    void Start()
    {
        flashctrl = GameObject.Find("yuka").GetComponent<FlashCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(flashctrl.GetFlash());
    }
}
