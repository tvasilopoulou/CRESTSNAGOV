using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class OnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pinButton;
    private GameObject InfoBox;
    private GameObject ToggleTracker;

    //TODO: THIS IS TO BE REMOVED!!!!
    public Text who_is_clicked;
    

    private string pinName = "";


    public void AddOnClick (){
        InfoBox = GameObject.Find("InfoBox");
        ToggleTracker = GameObject.Find("ToggleTracker");

        if(!ToggleTracker.GetComponent<Text>().text.Contains(pinButton.name) || ToggleTracker.GetComponent<Text>().text == ""){
            pinName = pinButton.name;
            ToggleTracker.GetComponent<Text>().text = pinName;
            InfoBox.GetComponent<TextMeshProUGUI>().text = pinButton.GetComponent<Text>().text;
            who_is_clicked.text = pinName;
        }
        else{
            pinName = "";
            InfoBox.GetComponent<TextMeshProUGUI>().text = "";
            ToggleTracker.GetComponent<Text>().text = "";
        }

              
    }
}

