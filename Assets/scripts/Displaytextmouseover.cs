using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Displaytextmouseover : MonoBehaviour
{

    [SerializeField] GameObject DefinitionText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("mouseover script is active");   
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse has enteres GameObject.");

        DefinitionText.SetActive(true);
   


    }
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
      
        DefinitionText.SetActive(true);
        Color m_MouseOverColor = Color.red;


    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
      

        DefinitionText.SetActive(true);
    }
}
