using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utilities 
{
    private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>();

    public static bool PointIsOverUi(Vector2 position)
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position =position;

        tempRaycastResults.Clear();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, tempRaycastResults);

        return tempRaycastResults.Count > 0;
    }
}
