using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCtrl : MonoBehaviour
{
    DummyCtrl dummyctrl = null;

    [SerializeField] SpriteRenderer FlashSprite = null;

    [SerializeField] Vector2 FlashInterval = new Vector2(5, 10);

    [SerializeField] float PrimaryFlashTime = 0.15f;

    [SerializeField] float SecondaryFlashTime = 1f;

    float nextflashtime = 0;

    float count = 0;

    int secondcount = 0;

    enum Mode
    {
        PrimaryFlash = 0,
        SecondaryFlash = 1,
        NotFlash = 4
    }

    Mode nowmode = Mode.NotFlash;

    // Start is called before the first frame update
    void Start()
    {
        dummyctrl = GetComponent<DummyCtrl>();
        nextflashtime = Random.Range(FlashInterval.x, FlashInterval.y);
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        switch (nowmode)
        {
            case Mode.NotFlash:
                if (count >= nextflashtime)
                {
                    nowmode = Mode.PrimaryFlash;
                    dummyctrl.ChangeDefaultColor();
                    count = 0;
                    secondcount = 0;
                    FlashSprite.color = new Color(1, 1, 1, 1);

                }
                break;
            case Mode.PrimaryFlash:
                if (count >= PrimaryFlashTime)
                {
                    if (secondcount % 2 == 0)
                    {
                        dummyctrl.ChangeDummyColor();
                        FlashSprite.color = new Color(1, 1, 1, 0);
                    }
                    else
                    {
                        dummyctrl.ChangeDefaultColor();
                        FlashSprite.color = new Color(1, 1, 1, 1);
                    }

                    ++secondcount;

                    if (secondcount == 8)
                    {
                        nowmode = Mode.SecondaryFlash;
                        secondcount = 0;
                        count = 0;
                    }
                }
                break;
            case Mode.SecondaryFlash:
                FlashSprite.color = new Color(1, 1, 1, 1 - 1f * (count / SecondaryFlashTime));
                if (count >= SecondaryFlashTime)
                {
                    nowmode = Mode.NotFlash;
                    count = 0;
                    dummyctrl.ChangeDummyColor();
                    nextflashtime = Random.Range(FlashInterval.x, FlashInterval.y);
                    FlashSprite.color = new Color(1, 1, 1, 0);
                }
                break;
        }
    }
}
