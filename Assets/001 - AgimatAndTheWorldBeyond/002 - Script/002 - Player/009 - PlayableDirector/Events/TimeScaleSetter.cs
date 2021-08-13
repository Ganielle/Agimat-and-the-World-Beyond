using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleSetter : MonoBehaviour
{
    public void SlowTime()
    {
        Time.timeScale = 0.1f;
    }

    public void ReturnToNormalTime()
    {
        Time.timeScale = 1f;
    }
}
