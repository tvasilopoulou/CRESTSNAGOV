using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class Routes : MonoBehaviour
{
    [Serializable]
    public class Location{
        public double lat;
        public double lon;
    }

    [Serializable]
    public class Route{
        public Location [] vertices;
        public string name;
        public string type;
    }

    [Serializable]
    public class RoutesClass{
        public Route [] routes;
    }

    private double latSpan = 0.0058;
    private double lonSpan = 0.01273;

    private double minLat = 44.72256;
    private double minLon = 26.16191;

    private GameObject[] pinButtons; 
    private string [] pinButtonsText;

    public GameObject borderPrefab;
    public GameObject linePrefab;

    private RectTransform ParentCanvas;
    private GameObject InfoBox;

    private GameObject map;
    
    void Start()
    {
        // set GameObjects placed in Canvas
        map = GameObject.Find("mapSnagov");
        InfoBox = GameObject.Find("InfoBox");
        ParentCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();  
        // when message from specific websocket received, place a pin 
        Positions.OnRoutesReceived += CreateRoutes;
    }

    void CreateRoutes(string jsonMessage){
        RoutesClass routesClass = JsonUtility.FromJson<RoutesClass>(jsonMessage);
        // Location location;
        Debug.Log(routesClass);
        foreach(Route route in routesClass.routes){
            Debug.Log(route.name);
            // location.lon = 0.0f;
            foreach(Location loc in route.vertices){
                //loc.latitude, loc.longitude -> waypoint
                Debug.Log("here");
                GameObject pinButton = (GameObject)Instantiate(borderPrefab);

                double xPlacement = (double)(0.7f *(loc.lat - minLat))/(double)latSpan;
                // // 450 is "size" of map left to write
                double yPlacement = (double)(0.59f *(loc.lon - minLon))/(double)lonSpan;
                // // 140 is "size" of map left to write
                
                pinButton.transform.SetParent(ParentCanvas, false);
                pinButton.GetComponent<RectTransform>().anchoredPosition = new Vector3((float)xPlacement - 0.33f, (float)yPlacement - 0.1f, 0);

                // if(!(location.longitude == 0.0f)){
                //     GameObject line = (GameObject)Instantiate(line);
                //     line.GetComponent<RectTransform>().height = Math.Abs(pinButton.GetComponent<RectTransform>().anchoredPosition.y - location.longitude);
                //     location.longitude = pinButton.GetComponent<RectTransform>().anchoredPosition.y

                //     // pinButton.GetComponent<RectTransform>().anchoredPosition

                //     loc = location;

                // }


            }
            // each route being created
            // instatiate line and change height!
        }

        // all area waypoints drawn out


    }

}
