using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CustomFunctions
{
    public static bool hasComponent<T>(this Component root, out T component)
    {
        component = root.GetComponent<T>();
        return component != null;
    }
    public static bool hasComponent<T>(this Component root)
    {
        var c = root.GetComponent<T>();
        return c != null;
    }

    public static int RoundToIntWithClamp(float value, int min)
    {
        var i = Mathf.RoundToInt(value);
        if (i < min)
            return min;
        return i;
    }

    public static Vector2 ToV2(this Vector3 v) => new Vector2(v.x, v.y);
    public static Vector3 ToV3(this Vector2 v) => new Vector3(v.x, v.y, 0);
}
