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

    public static string PrintNumWithSign(this float fl, int decimalPlaces = 2)
    {
        if (fl > 0)
            return $"+{fl.ToString("F2")}";
        return fl.ToString("F2");
    }
    public static string PrintNumWithSign(this int i, int decimalPlaces = 2)
    {
        if (i > 0)
            return $"+{i}";
        return i.ToString();
    }
    public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
    public static float ClampToDecimalPlaces(this float f, int decimalPlaces)
    {
        var factor = Mathf.Pow(10, decimalPlaces);
        return Mathf.Round(f * factor) / factor;
    }

    public static string Capitalize(this string s) => s.Substring(0, 1).ToUpper() + s.Substring(1);


    public static float Remap(this float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
