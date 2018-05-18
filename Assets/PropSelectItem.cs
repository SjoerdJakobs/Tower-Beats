using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropSelectItem : MonoBehaviour
{
    public delegate void PropItemSelected(PropSelectItem item);
    public static PropItemSelected s_OnPropItemSelected;

    [SerializeField] private Image m_Image;

    public void SetImage(Sprite sprite)
    {
        m_Image.sprite = sprite;
    }

    public void Select()
    {
        if( s_OnPropItemSelected != null) s_OnPropItemSelected(this);
    }
}
