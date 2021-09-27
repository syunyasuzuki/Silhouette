using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一定間隔で雷を光らせる
public class FlashCtrl : MonoBehaviour
{
    //後ろの偽物の影のコントローラー
    DummyCtrl dummyctrl = null;

    //足場のコントローラー
    PlatformCtrl platformctrl = null;

    //雷の音のコントローラー
    ThunderSECtrl thundersectrl = null;

    //タイトルロゴ
    [SerializeField] SpriteRenderer Titlelogo = null;

    //ゲーム開始ロゴ
    [SerializeField] SpriteRenderer GameStartlogo = null;

    //タイトルロゴの消えるまでの時間
    [SerializeField] float TitlelogoLifeTime = 4f;

    //タイトルが表示されているか
    private bool titlevisible = false;

    //タイトルからゲームに移動する際の時間
    private float titlechangetime = 0;

    //雷
    [SerializeField] SpriteRenderer FlashSprite = null;

    //雷の光る間隔（x = 最低、y = 最高）
    [SerializeField] Vector2 FlashInterval = new Vector2(5, 10);

    //最初の連続する雷の明るさ
    [SerializeField, Range(0.1f, 1f)] float PrimaryFlashValue = 0.5f;

    //最初の連続する雷の一回の光の長さ
    [SerializeField] float PrimaryFlashTime = 0.15f;

    //最後の雷の光るまでのラグ
    [SerializeField] float SecondaryFlashRate = 0.5f;

    //最後の雷の光っている長さ
    [SerializeField] float SecondaryFlashTime = 1f;

    //次の雷が光るまでの時間
    float nextflashtime = 0;

    //雷を制御するための時間のカウント
    float time = 0;

    //最初の連続した雷が光った回数を制御するためのカウント
    int primaryflashcount = 0;

    //雷が光っているかどうか
    private bool flash = false;

    //ゲームの状態
    enum GameMode
    {
        Title = 0,
        Change = 1,
        Game = 2
    }

    //現在の雷の状態
    GameMode nowgamemode = GameMode.Title;

    //雷の状態
    enum Mode
    {
        PrimaryFlash = 0,//最初の連続した雷
        RateTime = 1,//最後の雷に移るまでの待機
        SecondaryFlash = 2,//最後の雷
        NotFlash = 4,//光っていない
        StopFlash = 10//外部からの光の停止状態
    }

    //現在の雷の状態
    Mode nowmode = Mode.NotFlash;

    //タイトル中の処理
    private void TitleModeTask()
    {
        if (nowgamemode == GameMode.Title && Input.GetKeyDown(KeyCode.Return))
        {
            nowgamemode = GameMode.Change;
            nextflashtime = TitlelogoLifeTime;
            time = 0;
            titlechangetime = 0;
            
            Player_test.move_check = false;
        }
        titlechangetime += Time.deltaTime;
        if (nowgamemode == GameMode.Change)
        {
            Titlelogo.color = GameStartlogo.color = new Color(1, 1, 1, 1f - 1f * (titlechangetime / TitlelogoLifeTime));
            
            if (titlechangetime >= TitlelogoLifeTime)
            {
                nowgamemode = GameMode.Game;
                Titlelogo.color = GameStartlogo.color = new Color(1, 1, 1, 0);
            }
        }
    }

    //ゲーム中の処理
    private void GameModeTask()
    {
        switch (nowmode)
        {
            case Mode.NotFlash:
                if (time >= nextflashtime)
                {
                    nowmode = Mode.PrimaryFlash;
                    dummyctrl.ChangeDefaultColor();
                    platformctrl.VisiblePlatform();
                    platformctrl.ChangeBackColorDefault();
                    time = 0;
                    primaryflashcount = 0;
                    FlashSprite.color = new Color(1, 1, 1, PrimaryFlashValue);
                }
                break;
            case Mode.PrimaryFlash:
                if (time >= PrimaryFlashTime)
                {
                    if (primaryflashcount % 2 == 0)
                    {
                        dummyctrl.ChangeDummyColor();
                        platformctrl.UnVisiblePlatform();
                        platformctrl.ChangeBackColorBlack();
                        FlashSprite.color = new Color(1, 1, 1, 0);
                        if (titlevisible == false && nowgamemode == GameMode.Title)
                        {
                            Titlelogo.color = GameStartlogo.color = new Color(1, 1, 1, 0);
                        }
                    }
                    else
                    {
                        dummyctrl.ChangeDefaultColor();
                        platformctrl.VisiblePlatform();
                        platformctrl.ChangeBackColorDefault();
                        FlashSprite.color = new Color(1, 1, 1, PrimaryFlashValue);
                        if (titlevisible == false && nowgamemode == GameMode.Title)
                        {
                            Titlelogo.color = GameStartlogo.color = new Color(1, 1, 1, 1);
                        }
                    }

                    ++primaryflashcount;
                    time = 0;

                    if (primaryflashcount == 5)
                    {
                        nowmode = Mode.RateTime;
                        primaryflashcount = 0;
                    }
                }
                break;
            case Mode.RateTime:
                if (time >= SecondaryFlashRate)
                {
                    time = 0;
                    dummyctrl.ChangeDefaultColor();
                    platformctrl.VisiblePlatform();
                    platformctrl.ChangeBackColorDefault();
                    FlashSprite.color = new Color(1, 1, 1, 1);
                    nowmode = Mode.SecondaryFlash;
                    flash = true;
                    thundersectrl.PlaySE();
                    if (titlevisible == false && nowgamemode == GameMode.Title)
                    {
                        titlevisible = true;
                        Titlelogo.color = GameStartlogo.color = new Color(1, 1, 1, 1);
                    }
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
                    platformctrl.ChangeBackColorBlack();
                    nextflashtime = Random.Range(FlashInterval.x, FlashInterval.y);
                    FlashSprite.color = new Color(1, 1, 1, 0);
                    flash = false;
                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dummyctrl = GetComponent<DummyCtrl>();
        platformctrl = GetComponent<PlatformCtrl>();
        thundersectrl = GetComponent<ThunderSECtrl>();
        nextflashtime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        TitleModeTask();
        GameModeTask();
    }

    /// <summary>
    /// シーンをタイトルへ移動させる
    /// </summary>
    public void SceneChangeTitle()
    {

    }

    /// <summary>
    /// 雷が光っているかを取得する
    /// </summary>
    public bool GetFlash()
    {
        return flash;
    }

    /// <summary>
    /// 雷を光ったまま止める
    /// </summary>
    public void StopFlash()
    {
        nowmode = Mode.StopFlash;
        FlashSprite.color = new Color(1, 1, 1, 1);
        dummyctrl.ChangeDefaultColor();
        platformctrl.VisiblePlatform();
    }
}
