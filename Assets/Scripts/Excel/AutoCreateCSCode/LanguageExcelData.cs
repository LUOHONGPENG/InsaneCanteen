/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public partial class LanguageExcelItem : ExcelItemBase
{
	public string key;
	public string descCN;
}

[CreateAssetMenu(fileName = "LanguageExcelData", menuName = "Excel To ScriptableObject/Create LanguageExcelData", order = 1)]
public partial class LanguageExcelData : ExcelDataBase<LanguageExcelItem>
{
}

#if UNITY_EDITOR
public class LanguageAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		LanguageExcelItem[] items = new LanguageExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new LanguageExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].key = allItemValueRowList[i]["key"];
			items[i].descCN = allItemValueRowList[i]["descCN"];
		}
		LanguageExcelData excelDataAsset = ScriptableObject.CreateInstance<LanguageExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(LanguageExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


