using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelDataMgr : MonoSingleton<ExcelDataMgr>
{
    public LanguageExcelData languageExcelData;
    public FoodExcelData foodExcelData;
    public FacilityExcelData facilityExcelData;
    public RuleExcelData ruleExcelData;


    public IEnumerator IE_Init()
    {
        //Basic Initialization, only happens when starting the game program
        //基本通用表格数据初始化，只用在启动游戏时载入一次到该单例类
        languageExcelData = ExcelManager.Instance.GetExcelData<LanguageExcelData, LanguageExcelItem>();
        foodExcelData = ExcelManager.Instance.GetExcelData<FoodExcelData, FoodExcelItem>();
        facilityExcelData = ExcelManager.Instance.GetExcelData<FacilityExcelData, FacilityExcelItem>();
        ruleExcelData = ExcelManager.Instance.GetExcelData<RuleExcelData, RuleExcelItem>();
        Debug.Log("Basic Initialize Excel Data");

        //Speical Initialization
        //特殊的初始化，比如规则表要在初始化时进行排序
        languageExcelData.Init();
        ruleExcelData.Init();


        yield break;
    }
}
