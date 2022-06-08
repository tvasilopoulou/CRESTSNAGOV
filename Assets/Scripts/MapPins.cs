using System.Collections;
using System.Net.WebSockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class MapPins : MonoBehaviour
{
    public GameObject ambulancePrefab;
    public GameObject cameraPrefab;
    public GameObject codPrefab;
    public GameObject firecarPrefab;
    public GameObject limocarPrefab;
    public GameObject policecarPrefab;
    public GameObject riskhighPrefab;
    public GameObject risklowPrefab;
    public GameObject riskmoderatePrefab;
    public GameObject sportcarPrefab;
    public GameObject teamk9Prefab;
    public GameObject teamsecurityPrefab;
    public GameObject truckcarPrefab;
    public GameObject ugvPrefab;
    public GameObject uavPrefab;

    
    private RectTransform ParentCanvas;
    private GameObject InfoBox;

    private GameObject map;

    private GameObject[] pinButtons; 
    private string [] pinButtonsText;

    private double latSpan = 0.0058;
    private double lonSpan = 0.01273;

    private double minLat = 44.72256;
    private double minLon = 26.16191;
    private string detections = "";


    class Telemetry{
        public double latitude;
        public double longitude;
        public string item;
        public string speed;
        public string altitude;
        public string message_type;
    
    }
//  <bounds minlat="44.7225600" minlon="26.1619100" maxlat="44.7283600" maxlon="26.1746400"/>

    void Start()
    {
        // set GameObjects placed in Canvas
        map = GameObject.Find("mapSnagov");
        InfoBox = GameObject.Find("InfoBox");
        ParentCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();  
        // when message from specific websocket received, place a pin 
        Positions.OnReceived += PlacePin;
    }

    // Update is called once per frame
    void PlacePin(string jsonMessage){
        
        // if message is not in correct format -> discard
        if(!(jsonMessage.StartsWith("{\"latitude\":") && jsonMessage.Contains("longitude") && jsonMessage.Contains("item") 
            && jsonMessage.Contains("speed") && jsonMessage.Contains("altitude") && jsonMessage.Contains("id") && jsonMessage.Contains("message_type") )){
                return;
        }

        // test JSON message breakdown
        // Telemetry data = JsonUtility.FromJson<Telemetry>(jsonMessage);
        // Debug.Log(Telemetry.item);

        double latitude = retrieveLat(jsonMessage);
        double longitude = retrieveLon(jsonMessage);
        string item = retrieveItem(jsonMessage);
        string speed = retrieveSpeed(jsonMessage);
        string altitude = retrieveAltitude(jsonMessage);
        string id = retrieveId(jsonMessage);

        Debug.Log(latitude);
        Debug.Log(longitude);
        Debug.Log(item);
        Debug.Log(speed);
        Debug.Log(altitude);
        Debug.Log(id);

        // if telemtry is outside of the area in map -> discard message
        if(!(latitude >= 44.7225600 && latitude <= 44.7283600 && longitude >= 26.16191 && longitude <= 26.17464)){
            Debug.Log("out of bounds");
            return;
        }

        GameObject pinButton;

        // if item is already in map -> continue moving
        if(!GameObject.Find("Pin"+id) || !detections.Contains(item+id)){
            Debug.Log("new item");

            pinButton = (GameObject)Instantiate(itemLocated(item));
            detections += item + id;
        }
        // else -> new item placed in map
        else{
            Debug.Log("item located in map");
            pinButton = GameObject.Find("Pin"+id);
        }

        Debug.Log("message ok");
        double xPlacement = (double)(0.7f *(latitude - minLat))/(double)latSpan;
        // // 450 is "size" of map left to write
        double yPlacement = (double)(0.59f *(longitude - minLon))/(double)lonSpan;
        // // 140 is "size" of map left to write

        pinButton.name = "Pin" + id;
        
        pinButton.transform.SetParent(ParentCanvas, false);
        pinButton.GetComponent<RectTransform>().anchoredPosition = new Vector3((float)xPlacement - 0.33f, (float)yPlacement - 0.1f, 0);
        Debug.Log(pinButton.GetComponent<RectTransform>().anchoredPosition);
        
        pinButton.GetComponent<Text>().text = "Latitude: " + latitude 
                    + Environment.NewLine + "Longitude: " + longitude + Environment.NewLine + "Item: " + item + Environment.NewLine
                    + "Speed: " + speed + Environment.NewLine + "Altitude: " + altitude + Environment.NewLine + "ID: " + id;
        

        // add control on whether there are multiple buttons in one screen
        // add functionality when it's idle for a long time to disappear
        if (InfoBox.GetComponent<TextMeshProUGUI>().text != "") InfoBox.GetComponent<TextMeshProUGUI>().text = pinButton.GetComponent<Text>().text;
    }

    private double retrieveLat(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        double lat = Convert.ToDouble(jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Split(' ')[1]);
        return lat;
    }

    private double retrieveLon(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        double lon = Convert.ToDouble(jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Split(' ')[3]);
        return lon;
    }

    private string retrieveItem(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        string item = jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Replace("\"", "").Split(' ')[5];
        return item;
    }

    private string retrieveSpeed(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        string speed = jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Replace("\"", "").Split(' ')[7];
        return speed;
    }

    private string retrieveAltitude(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        string altitude = jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Replace("\"", "").Split(' ')[9];
        return altitude;
    }

    private string retrieveId(string jsonMessage){
        jsonMessage = jsonMessage.Replace("{", "").Replace("}", "").Replace(",", "").Replace(":", "");
        string id = jsonMessage.Replace("\"latitude\"", "").Replace("\"longitude\"", "").Replace("\"", "").Split(' ')[11];
        return id;
    }


    // private GameObject itemLocated(string item){
    //     switch (item){
    //         case "ambulance":
    //             return ambulancePrefab;
    //         case "camera":
    //             return cameraPrefab;
    //         case "cod":
    //             return codPrefab;
    //         case "firecar":
    //             return firecarPrefab;
    //         case "limocar":
    //             return limocarPrefab;
    //         case "policecar":
    //             return policecarPrefab;
    //         case "riskhigh":
    //             return riskhighPrefab;
    //         case "risklow":
    //             return risklowPrefab;
    //         case "riskmoderate":
    //             return riskmoderatePrefab;
    //         case "sportcar":
    //             return sportcarPrefab;
    //         case "teamk9":
    //             return teamk9Prefab;
    //         case "teamsecurity":
    //             return teamsecurityPrefab;
    //         case "truckcar":
    //             return truckcarPrefab;
    //         case "ugv":
    //             return ugvPrefab;
    //         case "uav":
    //             return uavPrefab;
    //         default:
    //             return riskmoderatePrefab;
            
    //     }

    // }

    private GameObject itemLocated(string item){
        if(item.IndexOf("ambulance", StringComparison.OrdinalIgnoreCase) >= 0) return ambulancePrefab;
        else if(item.IndexOf("camera", StringComparison.OrdinalIgnoreCase) >= 0) return cameraPrefab;
        else if(item.IndexOf("cod", StringComparison.OrdinalIgnoreCase) >= 0) return codPrefab;
        else if(item.IndexOf("firecar", StringComparison.OrdinalIgnoreCase) >= 0) return firecarPrefab;
        else if(item.IndexOf("limocar", StringComparison.OrdinalIgnoreCase) >= 0) return limocarPrefab;
        else if(item.IndexOf("policecar", StringComparison.OrdinalIgnoreCase) >= 0) return policecarPrefab;
        else if(item.IndexOf("riskhigh", StringComparison.OrdinalIgnoreCase) >= 0) return riskhighPrefab;
        else if(item.IndexOf("risklow", StringComparison.OrdinalIgnoreCase) >= 0) return risklowPrefab;
        else if(item.IndexOf("riskmoderate", StringComparison.OrdinalIgnoreCase) >= 0) return riskmoderatePrefab;
        else if(item.IndexOf("sportcar", StringComparison.OrdinalIgnoreCase) >= 0) return sportcarPrefab;
        else if(item.IndexOf("teamk9", StringComparison.OrdinalIgnoreCase) >= 0) return teamk9Prefab;
        else if(item.IndexOf("teamsecurity", StringComparison.OrdinalIgnoreCase) >= 0) return teamsecurityPrefab;
        else if(item.IndexOf("truckcar", StringComparison.OrdinalIgnoreCase) >= 0) return truckcarPrefab;
        else if(item.IndexOf("ugv", StringComparison.OrdinalIgnoreCase) >= 0) return ugvPrefab;
        else if(item.IndexOf("uav", StringComparison.OrdinalIgnoreCase) >= 0) return uavPrefab;
        else return riskmoderatePrefab;

    }

    
}
