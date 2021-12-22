using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//エディター上での再生停止を検出するもの
[InitializeOnLoad]
public class DetectionEditorChange
{
    /// <summary>
    /// 再生ボタンが押されたタイミングで実行される
    /// </summary>
    public static event Action StartPlayingReady = delegate { };

    /// <summary>
    /// 再生が開始されたタイミングで実行される
    /// </summary>
    public static event Action StartPlaying = delegate { };

    /// <summary>
    /// 停止ボタンを押したタイミングで実行される
    /// </summary>
    public static event Action ExitPlayingReady = delegate { };

    /// <summary>
    /// 停止した場合に実行される
    /// </summary>
    public static event Action ExitPlaying = delegate { };

    static DetectionEditorChange()
    {
        EditorApplication.playModeStateChanged += changestatetask;
    }

    private static void changestatetask(PlayModeStateChange state)
    {

        if(state == PlayModeStateChange.ExitingEditMode)
        {
            StartPlayingReady();
        }
        else if(state == PlayModeStateChange.EnteredPlayMode)
        {
            StartPlaying();
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ExitPlayingReady();
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            ExitPlaying();
        }

    }
}
