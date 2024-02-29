using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityPreviewUIMgr : MonoBehaviour
{
    public GameObject objPopup;
    public Image imgPreview;
    private bool isInit = false;

    public void Init()
    {
        isInit = true;
    }

    private void Update()
    {
        if (!isInit)
        {
            return;
        }
        imgPreview.transform.position = PublicTool.GetMousePos();
    }

    public void OnEnable()
    {
        EventCenter.Instance.AddEventListener("StartDragFacility", StartDragFacilityEvent);
        EventCenter.Instance.AddEventListener("EndDragFacility", EndDragFacilityEvent);
    }

    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("StartDragFacility", StartDragFacilityEvent);
        EventCenter.Instance.RemoveEventListener("EndDragFacility", EndDragFacilityEvent);
    }

    private void StartDragFacilityEvent(object arg0)
    {
        //设置图片表现
        FacilityExcelItem item = PublicTool.GetFacilityItem((int)arg0);
        imgPreview.sprite = Resources.Load("Sprite/Facility/" + item.iconUrl, typeof(Sprite)) as Sprite;
        imgPreview.SetNativeSize();
        //显示
        objPopup.SetActive(true);
    }

    private void EndDragFacilityEvent(object arg0)
    {
        //隐藏
        objPopup.SetActive(false);
    }
}
