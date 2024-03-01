using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageExcelData
{
    public Dictionary<string, string> dicKeyCN = new Dictionary<string, string>();

    public void Init()
    {
        dicKeyCN.Clear();
        for (int i = 0; i < items.Length; i++)
        {
            LanguageExcelItem languageItem = items[i];
            string key = languageItem.key;
            if (key.Length > 0)
            {
                if (!dicKeyCN.ContainsKey(key))
                {
                    dicKeyCN.Add(key, languageItem.descCN);
                }
            }
        }
    }

    public string GetText(string key)
    {
        if (GameGlobal.languageType == LanguageType.CN)
        {
            if (dicKeyCN.ContainsKey(key))
            {
                return dicKeyCN[key];
            }
            else
            {
                return "";
            }
        }
        return "";
    }
}
