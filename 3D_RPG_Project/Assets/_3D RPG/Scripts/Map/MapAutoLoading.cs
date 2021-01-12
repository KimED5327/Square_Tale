using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAutoLoading : MonoBehaviour
{
    private void Start()
    {
        MapManager.instance.ActiveMap();
        Notification.instance.ShowSlideMessage(MapManager.instance.GetCurrentMapName());
    }
}