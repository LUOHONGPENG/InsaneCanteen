using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineItem : MonoBehaviour
{
    public LineRenderer line;
    public SpriteRenderer spLine;
    private MapSlotItem outSlot;
    private MapSlotItem inSlot;
    private bool isInit = false;

    public void Init(MapSlotItem outSlot, MapSlotItem inSlot)
    {
        this.outSlot = outSlot;
        this.inSlot = inSlot;
        isInit = true;
        UpdateLine();
    }

    /// <summary>
    /// ֮��������ƶ����ܵĻ� ���ܻ��Update��
    /// </summary>
    private void UpdateLine()
    {
/*        line.SetPosition(0, outSlot.transform.position);
        line.SetPosition(1, inSlot.transform.position);*/
        UpdateSpriteLine(new Vector2(outSlot.transform.position.x, outSlot.transform.position.y),
            new Vector2(inSlot.transform.position.x, inSlot.transform.position.y));
    }

    /// <summary>
    /// ͨ������ǶȺ;��룬��SpriteRenderer����LineRender��ֱ��
    /// ���Ҫ��������֮��������Ȼ������ֱ�Ϊ
    /// (x1,y1)��(x1,y��ֵ)
    /// (x1,y��ֵ)��(x2,y��ֵ)
    /// (x2,y��ֵ)��(x2,y2)����
    /// </summary>
    /// <param name="posStart"></param>
    /// <param name="posEnd"></param>
    private void UpdateSpriteLine(Vector2 posStart, Vector2 posEnd)
    {
        Vector2 direction = posEnd - posStart;
        float length = Mathf.Sqrt(direction.sqrMagnitude);
        spLine.transform.localPosition = (posStart + posEnd) / 2;
        spLine.transform.localScale = new Vector2(0.05f, length);

        float angle = Vector2.Angle(Vector2.up, direction);
        if (direction.x > 0)
        {
            angle = -angle;
        }
        spLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
