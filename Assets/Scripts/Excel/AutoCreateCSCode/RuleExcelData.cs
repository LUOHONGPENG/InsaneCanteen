/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public partial class RuleExcelItem : ExcelItemBase
{
	public int resultID;
	public string name;
	public int priority;
	public List<int> array_foodID;
	public List<int> array_foodNum;
}

[CreateAssetMenu(fileName = "RuleExcelData", menuName = "Excel To ScriptableObject/Create RuleExcelData", order = 1)]
public partial class RuleExcelData : ExcelDataBase<RuleExcelItem>
{
}

#if UNITY_EDITOR
public class RuleAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		RuleExcelItem[] items = new RuleExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new RuleExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].resultID = Convert.ToInt32(allItemValueRowList[i]["resultID"]);
			items[i].name = allItemValueRowList[i]["name"];
			items[i].priority = Convert.ToInt32(allItemValueRowList[i]["priority"]);
			items[i].array_foodID = new List<int>(Array.ConvertAll((allItemValueRowList[i]["array_foodID"]).Split(';'), int.Parse));
			items[i].array_foodNum = new List<int>(Array.ConvertAll((allItemValueRowList[i]["array_foodNum"]).Split(';'), int.Parse));
		}
		RuleExcelData excelDataAsset = ScriptableObject.CreateInstance<RuleExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(RuleExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


