/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public partial class FacilityExcelItem : ExcelItemBase
{
	public string name;
}

[CreateAssetMenu(fileName = "FacilityExcelData", menuName = "Excel To ScriptableObject/Create FacilityExcelData", order = 1)]
public partial class FacilityExcelData : ExcelDataBase<FacilityExcelItem>
{
}

#if UNITY_EDITOR
public class FacilityAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		FacilityExcelItem[] items = new FacilityExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new FacilityExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].name = allItemValueRowList[i]["name"];
		}
		FacilityExcelData excelDataAsset = ScriptableObject.CreateInstance<FacilityExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(FacilityExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


