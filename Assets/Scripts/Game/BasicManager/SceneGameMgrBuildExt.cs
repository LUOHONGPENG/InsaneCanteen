using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneGameMgr 
{

    /// <summary>
    /// ����Facility���ź�
    /// </summary>
    /// <param name="arg0"></param>
    private void SetFacilityEvent(object arg0)
    {
        SetFacilityInfo info = (SetFacilityInfo)arg0;
        //���Ŀ��λ���������
        if (sceneGameData.CheckSetFaclityValid(info.typeID, info.posID))
        {
            //�����ݲ����Facility
            FacilitySetData newFacilityData = sceneGameData.AddFacility(info.typeID, info.posID);
            //�ڱ��ֲ����Facility
            mapMgr.AddFacility(newFacilityData);
        }
        else
        {
            //֮�󲹷���
            Debug.Log("Not Enough Space");
        }
    }

    private void DeleteFacilityEvent(object arg0)
    {
        int keyID = (int)arg0;
        FacilitySetData tarData = sceneGameData.GetFacility(keyID);

        //����ɾ����
        if (tarData.typeID == 3001)
        {
            Debug.Log("Invalid Delete Target");
        }
        else
        {
            //����һ�°���ص���ɾ��
            for (int i = 0; i < tarData.listSlotOut.Count; i++)
            {
                //��ɾ�Է� ��ɾ�Լ�
                Vector2Int otherInfo = tarData.listSlotOut[i];
                if (otherInfo.x >= 0)
                {
                    FacilitySetData otherData = sceneGameData.GetFacility(otherInfo.x);
                    otherData.DisjoinSlotIn(otherInfo.y);
                }
                tarData.DisjoinSlotOut(i);
            }
            for (int i = 0; i < tarData.listSlotIn.Count; i++)
            {
                //��ɾ�Է� ��ɾ�Լ�
                Vector2Int otherInfo = tarData.listSlotIn[i];
                if (otherInfo.x >= 0)
                {
                    FacilitySetData otherData = sceneGameData.GetFacility(otherInfo.x);
                    otherData.DisjoinSlotOut(otherInfo.y);
                }
                tarData.DisjoinSlotIn(i);
            }
            //���ֲ�ɾ��
            mapMgr.UpdateLine();

            //��ɾ���ֲ��Facility
            mapMgr.RemoveFacility(keyID);
            //��ɾ���ݲ��
            sceneGameData.RemoveFacility(keyID);
        }
    }

    private void SetLinkEvent(object arg0)
    {
        SetLinkInfo info = (SetLinkInfo)arg0;
        //���ݲ�-���׷�����
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.JoinSlotOut(info.outSlotID, info.inKeyID, info.inSlotID);
        //���ݲ�-��׷�����
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataIn.JoinSlotIn(info.inSlotID, info.outKeyID, info.outSlotID);
        //���ֲ�
        mapMgr.UpdateLine();
    }

    private void DeleteLinkEvent(object arg0)
    {
        DeleteLinkInfo info = (DeleteLinkInfo)arg0;
        FacilitySetData tarDataOut = sceneGameData.GetFacility(info.outKeyID);
        tarDataOut.DisjoinSlotOut(info.outSlotID);
        FacilitySetData tarDataIn = sceneGameData.GetFacility(info.inKeyID);
        tarDataIn.DisjoinSlotIn(info.inSlotID);
        //���ֲ�
        mapMgr.UpdateLine();
    }
}
