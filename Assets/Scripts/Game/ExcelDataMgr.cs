using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelDataMgr : MonoSingleton<ExcelDataMgr>
{
    public FoodExcelData foodExcelData;
    public FacilityExcelData facilityExcelData;
    public RuleExcelData ruleExcelData;


    public IEnumerator IE_Init()
    {
        //Basic Initialization, only happens when starting the game program
        //����ͨ�ñ������ݳ�ʼ����ֻ����������Ϸʱ����һ�ε��õ�����
        foodExcelData = ExcelManager.Instance.GetExcelData<FoodExcelData, FoodExcelItem>();
        facilityExcelData = ExcelManager.Instance.GetExcelData<FacilityExcelData, FacilityExcelItem>();
        ruleExcelData = ExcelManager.Instance.GetExcelData<RuleExcelData, RuleExcelItem>();
        Debug.Log("Basic Initialize Excel Data");

        //Speical Initialization
        //����ĳ�ʼ������������Ҫ�ڳ�ʼ��ʱ��������
        ruleExcelData.Init();


        yield break;
    }
}