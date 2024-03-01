using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePreviewUIMgr : MonoBehaviour
{
    public GameObject objPopup;
    public RectTransform rtLine;
    private bool isInit = false;
    private bool startPreview = false;
    private Vector3 startPos;

    public void Init()
    {
        isInit = true;
    }

    private void Update()
    {
        if (!isInit||!startPreview)
        {
            return;
        }
        UpdatePreviewLine();
    }

    private void UpdatePreviewLine()
    {
        Vector2 posStart = new Vector2(startPos.x, startPos.y);
        Vector2 posEnd = PublicTool.GetMousePos();
        Vector2 direction = posEnd - posStart;
        float length = Mathf.Sqrt(direction.sqrMagnitude);
        rtLine.transform.position = (posStart + posEnd) / 2;
        rtLine.sizeDelta = new Vector2(5f, length*100);

        float angle = Vector2.Angle(Vector2.up, direction);
        if (direction.x > 0)
        {
            angle = -angle;
        }
        rtLine.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }

    public void OnEnable()
    {
        EventCenter.Instance.AddEventListener("StartLinkSlot", StartLinkSlotEvent);
        EventCenter.Instance.AddEventListener("EndLinkSlot", EndLinkSlotEvent);
    }

    public void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("StartLinkSlot", StartLinkSlotEvent);
        EventCenter.Instance.RemoveEventListener("EndLinkSlot", EndLinkSlotEvent);
    }

    private void StartLinkSlotEvent(object arg0)
    {
        startPos = (Vector3)arg0;
        startPreview = true;
        objPopup.SetActive(true);

    }

    private void EndLinkSlotEvent(object arg0)
    {
        startPreview = false;
        objPopup.SetActive(false);

    }
}
