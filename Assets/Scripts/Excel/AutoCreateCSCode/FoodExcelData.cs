/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public partial class FoodExcelItem : ExcelItemBase
{
	public string name;
	public string iconUrl;
}

[CreateAssetMenu(fileName = "FoodExcelData", menuName = "Excel To ScriptableObject/Create FoodExcelData", order = 1)]
public partial class FoodExcelData : ExcelDataBase<FoodExcelItem>
{
}

#if UNITY_EDITOR
public class FoodAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		FoodExcelItem[] items = new FoodExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new FoodExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].name = allItemValueRowList[i]["name"];
			items[i].iconUrl = allItemValueRowList[i]["iconUrl"];
		}
		FoodExcelData excelDataAsset = ScriptableObject.CreateInstance<FoodExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(FoodExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


