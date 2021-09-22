using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemPanel : MonoBehaviour
{
    [SerializeField]
    protected Text _description;

    public virtual void UpdateText() => _description.text = "абстрактная панель";        
}
