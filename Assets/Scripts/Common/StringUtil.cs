using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtil
{
    public static string ToLanguageText(this string str)
    {
        return PublicTool.GetLanguageText(str);
    }
}
