/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public partial class LevelExcelItem : ExcelItemBase
{
	public List<int> readyFacility;
	public int needFoodType;
	public int needFoodNum;
	public string remark;
}

[CreateAssetMenu(fileName = "LevelExcelData", menuName = "Excel To ScriptableObject/Create LevelExcelData", order = 1)]
public partial class LevelExcelData : ExcelDataBase<LevelExcelItem>
{
}

#if UNITY_EDITOR
public class LevelAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		LevelExcelItem[] items = new LevelExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new LevelExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].readyFacility = new List<int>(Array.ConvertAll((allItemValueRowList[i]["readyFacility"]).Split(';'), int.Parse));
			items[i].needFoodType = Convert.ToInt32(allItemValueRowList[i]["needFoodType"]);
			items[i].needFoodNum = Convert.ToInt32(allItemValueRowList[i]["needFoodNum"]);
			items[i].remark = allItemValueRowList[i]["remark"];
		}
		LevelExcelData excelDataAsset = ScriptableObject.CreateInstance<LevelExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(LevelExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


