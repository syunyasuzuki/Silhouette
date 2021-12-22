using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeData
{
    public Vector3 Position;
    public Vector3 Scale;

    public EyeData(Vector3 position, Vector3 scale)
    {
        Position = position;
        Scale = scale;
    }

    public enum MoveMode
    {
        Stay = 0,
        Move = 1,
        Stop = 2,
        Danger = 3
    }
    public MoveMode NowMoveMode;

    public float BaseMoveTime;
    public float BaseStayTime;
    public float Time;
    public float ChangeTime;
    public Vector3 StartPos;
    public Vector3 EndPos;


}
