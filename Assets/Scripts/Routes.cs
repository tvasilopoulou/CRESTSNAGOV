using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routes : MonoBehaviour
{
    class Location{
        double latitude;
        double longitude;
    }

    class Route{
        Location [] vertices;
        string name;
        string type;
    }

    class RoutesClass{
        Route [] routes;
    }
}
