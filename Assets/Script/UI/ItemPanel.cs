using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemPanel : MonoBehaviour
{
    [SerializeField]
    protected Text description;
    public virtual void UpdateText()
    {
        description.text = "абстрактная панель";
    }
}
