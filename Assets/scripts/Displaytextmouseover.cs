using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Displaytextmouseover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerEnter method.
{
    [SerializeField] GameObject DefinitionText;

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
       // Debug.Log("The cursor entered the selectable UI element.");
        DefinitionText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      //  Debug.Log("The cursor exited the selectable UI element.");
        DefinitionText.SetActive(false);
    }

    public void TurnOffFloatText()
    {
        DefinitionText.SetActive(false);
    }
}



  
