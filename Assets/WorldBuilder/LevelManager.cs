using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    protected LevelManager() { }

    public AxisType CurrentAxis = AxisType.Z_axis;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    [SerializeField]
    public enum AxisType
    {
        Y_axis,
        Z_axis
    }
}
