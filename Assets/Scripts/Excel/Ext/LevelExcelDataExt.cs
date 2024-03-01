using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelExcelData
{
    public Dictionary<int, LevelExcelItem> dicLevel = new Dictionary<int, LevelExcelItem>();
    public int maxLevel = 0;

    public void Init()
    {
        dicLevel.Clear();
        for(int i = 0; i < items.Length; i++)
        {
            dicLevel.Add(items[i].id, items[i]);
            if(i == items.Length - 1)
            {
                maxLevel = items[i].id;
            }
        }
    }

    public LevelExcelItem GetLevelItem(int id)
    {
        if (dicLevel.ContainsKey(id))
        {
            return dicLevel[id];
        }
        else
        {
            return null;
        }
    }
}
