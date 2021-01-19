using Snowcap.NavTiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    NavTileAgentController controller;
    public NavTileAgentController.PathWaypoint test;
    void Start()
    {
        controller = GetComponent<NavTileAgentController>();
    }

 
    void Update()
    {
        
    }



    public void Test()
    {
        controller._waypoints[0].TargetCoordinate.x = 5;
        controller._waypoints[0].TargetCoordinate.y = 1;
        test.DestinationType = NavTileAgentController.WaypointDestinationType.GridCoordinate;
        test.TargetCoordinate.x = 10;
        test.TargetCoordinate.y = 0;
        controller._waypoints.Add(test);
        controller.StartMoving();
    }
    public void Test2()
    {
        controller.StopMoving();
    }
}
