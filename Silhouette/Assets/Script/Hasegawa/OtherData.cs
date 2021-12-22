using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//いろんなところで使うもの
//綴りミスの回避も
public class OtherData : MonoBehaviour
{
    /// <summary>
    /// リソースフォルダーまでのパスを返す
    /// 綴りミス回避用
    /// </summary>
    public static string GetResourcesPath()
    {
        return Application.dataPath + @"\Resources";
    }

    /// <summary>
    /// パスで指定されたテキストに書き出しを行う（上書き）
    /// </summary>
    public static void ExportText(string[] data, string path)
    {
        File.WriteAllLines(path, data);
    }

    /// <summary>
    /// パスで指定されたテキストを読み込む
    /// </summary>
    public static string[] InportText(string path)
    {
        return File.ReadAllLines(path);
    }

    /// <summary>
    /// UIのボタンを作成する
    /// </summary>
    public static GameObject CreateUIButton(Sprite sprite)
    {
        GameObject button = new GameObject(sprite.name);
        RectTransform rt = button.AddComponent<RectTransform>();
        button.AddComponent<Image>().sprite = sprite;
        button.AddComponent<Button>();

        GameObject scrollobj = GameObject.Find("scrollobj");
        rt.SetParent(scrollobj.transform);

        rt.anchorMin = new Vector2(1, 0.5f);
        rt.anchorMax = new Vector2(1, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localScale = Vector3.one;

        rt.sizeDelta = new Vector2(50, 50 * ((float)sprite.rect.height / (float)sprite.rect.width));
        
        return button;
    }

    /// <summary>
    /// UIの画像を作成する
    /// </summary>
    public static GameObject CreateUIImage(Sprite sprite)
    {
        GameObject image = new GameObject(sprite.texture.name);
        RectTransform rt = image.AddComponent<RectTransform>();
        image.AddComponent<Image>().sprite = sprite;

        GameObject canvas = GameObject.Find("Canvas");
        rt.SetParent(canvas.transform);

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localScale = Vector3.one;

        rt.sizeDelta = new Vector2(50, 50 * ((float)sprite.rect.height / (float)sprite.rect.width));

        return image;
    }

    /// <summary>
    /// ゲームオブジェクトを作成する
    /// </summary>
    public static GameObject CreateGameObject(Sprite sprite)
    {
        GameObject go = new GameObject(sprite.name);
        go.AddComponent<SpriteRenderer>().sprite = sprite;

        return go;
    }

    /// <summary>
    /// マウスカーソルのワールド座標を取得する
    /// </summary>
    public static Vector3 GetWorldMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z)));
    }
    
}
