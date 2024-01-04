using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtend
{
    public static float CustomRound(float value)
    {
        float res = Mathf.RoundToInt(value * 2) / 2.0f;
        return res;
    }

    public static bool ApproxFloatEqual(float a, float b, float dif)
    {
        return Mathf.Abs(b - a) <= dif;
    }

    public static void CompareDot(ref float dotProduct, Vector3 dir, Vector3 refVector, ref Vector3 vecDir)
    {
        float newDot = Vector3.Dot(refVector, dir);
        if (dotProduct < newDot)
        {
            dotProduct = newDot;
            vecDir = dir;
        }
    }

}

