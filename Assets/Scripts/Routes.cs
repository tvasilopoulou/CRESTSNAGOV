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
    // public GameObject lineRend;
    public Material newMaterialRef;

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
        Location location = new Location();
        Debug.Log(routesClass);
        foreach(Route route in routesClass.routes){
            Debug.Log(route.name);
            location.lon = 0.0f;
            GameObject previousObj = null;
            foreach(Location loc in route.vertices){
                //loc.latitude, loc.longitude -> waypoint
                GameObject border = (GameObject)Instantiate(borderPrefab);

                double xPlacement = (double)(0.7f *(loc.lat - minLat))/(double)latSpan;
                // // 450 is "size" of map left to write
                double yPlacement = (double)(0.59f *(loc.lon - minLon))/(double)lonSpan;
                // // 140 is "size" of map left to write
                
                border.transform.SetParent(ParentCanvas, false);
                border.GetComponent<RectTransform>().anchoredPosition = new Vector3((float)xPlacement - 0.33f, (float)yPlacement - 0.1f, 0);

                if(previousObj!=null){
                    Debug.Log("here");
                    DrawLine(previousObj, border);
                }

                previousObj = border;
            }
            previousObj = null;
        }



    }

    private void DrawLine(GameObject previousObj, GameObject border){
        var go = new GameObject();
        go.transform.localScale = new Vector3(0.00001f, 0.00001f, 0.00001f);

        go.transform.SetParent(ParentCanvas, false);

        var lr = go.AddComponent<LineRenderer>();
        lr.GetComponent<Renderer>().material = newMaterialRef;
 
        lr.SetPosition(0, border.transform.position);
        lr.SetPosition(1, previousObj.transform.position);
        lr.SetWidth(0.002f, 0.002f);

    }
}
