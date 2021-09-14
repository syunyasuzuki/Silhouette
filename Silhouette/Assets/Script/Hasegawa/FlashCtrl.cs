using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCtrl : MonoBehaviour
{
    DummyCtrl dummyctrl = null;

    PlatformCtrl platformctrl = null;

    ThunderSECtrl thundersectrl = null;

    [SerializeField] SpriteRenderer FlashSprite = null;

    [SerializeField] Vector2 FlashInterval = new Vector2(5, 10);

    [SerializeField, Range(0.1f, 1f)] float PrimaryFlashValue = 0.5f;

    [SerializeField] float PrimaryFlashTime = 0.15f;

    [SerializeField] float SecondaryFlashRate = 0.5f;

    [SerializeField] float SecondaryFlashTime = 1f;

    float nextflashtime = 0;

    float time = 0;

    int secondcount = 0;

    private bool flash = false;

    enum Mode
    {
        PrimaryFlash = 0,
        RateTime = 1,
        SecondaryFlash = 2,
        NotFlash = 4,
        StopFlash = 10
    }

    Mode nowmode = Mode.NotFlash;

    // Start is called before the first frame update
    void Start()
    {
        dummyctrl = GetComponent<DummyCtrl>();
        platformctrl = GetComponent<PlatformCtrl>();
        thundersectrl = GetComponent<ThunderSECtrl>();
        nextflashtime = Random.Range(FlashInterval.x, FlashInterval.y);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        switch (nowmode)
        {
            case Mode.NotFlash:
                if (time >= nextflashtime)
                {
                    nowmode = Mode.PrimaryFlash;
                    dummyctrl.ChangeDefaultColor();
                    platformctrl.VisiblePlatform();
                    time = 0;
                    secondcount = 0;
                    FlashSprite.color = new Color(1, 1, 1, PrimaryFlashValue);
                }
                break;
            case Mode.PrimaryFlash:
                if (time >= PrimaryFlashTime)
                {
                    if (secondcount % 2 == 0)
                    {
                        dummyctrl.ChangeDummyColor();
                        platformctrl.UnVisiblePlatform();
                        FlashSprite.color = new Color(1, 1, 1, 0);
                    }
                    else
                    {
                        dummyctrl.ChangeDefaultColor();
                        platformctrl.VisiblePlatform();
                        FlashSprite.color = new Color(1, 1, 1, PrimaryFlashValue);
                    }

                    ++secondcount;
                    time = 0;

                    if (secondcount == 5)
                    {
                        nowmode = Mode.RateTime;
                        secondcount = 0;
                    }
                }
                break;
            case Mode.RateTime:
                if (time >= SecondaryFlashRate)
                {
                    time = 0;
                    dummyctrl.ChangeDefaultColor();
                    platformctrl.VisiblePlatform();
                    FlashSprite.color = new Color(1, 1, 1, 1);
                    nowmode = Mode.SecondaryFlash;
                    flash = true;
                    thundersectrl.PlaySE();
                }
                break;
            case Mode.SecondaryFlash:
                float alpha = 1 - 1f * (time / SecondaryFlashTime);
                FlashSprite.color = new Color(1, 1, 1, alpha);
                dummyctrl.ChangeAlpha(alpha);
                platformctrl.ChangeAlpha(alpha);
                if (time >= SecondaryFlashTime)
                {
                    nowmode = Mode.NotFlash;
                    time = 0;
                    dummyctrl.ChangeDummyColor();
                    platformctrl.UnVisiblePlatform();
                    nextflashtime = Random.Range(FlashInterval.x, FlashInterval.y);
                    FlashSprite.color = new Color(1, 1, 1, 0);
                    flash = false;
                }
                break;
        }
    }

    public bool GetFlash()
    {
        return flash;
    }

    public void StopFlash()
    {
        nowmode = Mode.StopFlash;
        FlashSprite.color = new Color(1, 1, 1, 1);
        dummyctrl.ChangeDefaultColor();
        platformctrl.VisiblePlatform();
    }
}
