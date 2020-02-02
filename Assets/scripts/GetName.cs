﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetName : MonoBehaviour

    {
    public static string PlayerName;
    public static int NameSeed;
    public GameObject Inputfield;
    public GameObject textdisplay;

        void Start()
        {
          var input = gameObject.GetComponent<InputField>();
        
         /*   var se = new InputField.SubmitEvent();
            se.AddListener(SubmitName);
            input.onEndEdit = se;
        */
        
          input.onEndEdit.AddListener(SubmitName);  // This also works
        

        }

        private void SubmitName(string arg0)
        {
         
        PlayerName = arg0;
        Debug.Log("Player name is: " + PlayerName);
        NameSeed = PlayerName.Length;
        Debug.Log("Seed based on name is: " + NameSeed);
        }
    }
