using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextReplace : MonoBehaviour
{
    protected TextMeshProUGUI txContent;
    protected Text txContentOld;


    void OnEnable()
    {
        if (txContent == null)
        {
            txContent = this.gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (txContentOld == null)
        {
            txContentOld = this.gameObject.GetComponent<Text>();
        }

        RefreshContent();
    }

    private void RefreshContent()
    {
        if (txContent != null)
        {
            txContent.text = PublicTool.GetLanguageText(this.name);

        }

        if (txContentOld != null)
        {
            txContentOld.text = PublicTool.GetLanguageText(this.name);

        }
    }

}
