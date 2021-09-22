using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HideEditor : MonoBehaviour
{
    private Camera maincamera = null;

    //背景の黒色の素材
    [SerializeField] Sprite BackImg = null;

    //乗ることのできる足場の素材
    [SerializeField] Sprite PlatformImg = null;

    //保存先
    string SpriteFolderPath = "Image/HidePlatformImage";

    //保存先に更にファイルを作成する場合入力する場所
    [SerializeField] string OriginalFilePath = null;

    //保存先を確定しチェックする
    private void CheckFilePath()
    {
        SpriteFolderPath = Path.Combine(Application.dataPath + @"\Resources", SpriteFolderPath);
        if (OriginalFilePath != null)
        {
            SpriteFolderPath = Path.Combine(SpriteFolderPath, OriginalFilePath);
        }
        if (!Directory.Exists(SpriteFolderPath))
        {
            Directory.CreateDirectory(SpriteFolderPath);
        }
    }

    //マウスの処理
    //
    private void MouseTask()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        maincamera = Camera.main;
        CheckFilePath();

        



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
