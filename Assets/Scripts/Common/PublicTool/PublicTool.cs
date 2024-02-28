using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RuleExcelData;

public partial class PublicTool
{
    /// <summary>
    /// Useful function for clearing the child objects 
    /// </summary>
    /// <param name="tf"></param>
    public static void ClearChildItem(UnityEngine.Transform tf)
    {
        foreach (UnityEngine.Transform item in tf)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }


}
