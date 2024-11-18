﻿using UnityEngine;
using System.Collections;

public class event_scheduled : event_base
{
    //AN event that fires a scheduled trigger in the specified tile X & Y

    public override void ExecuteEvent()
    {
        base.ExecuteEvent();
        int eventTileX = RawData[3];
        int eventTileY = RawData[4];

        ObjectLoaderInfo[] objList = CurrentObjectList().objInfo;
        for (int o = 256; o <= objList.GetUpperBound(0); o++)
        {
            if ((objList[o].ObjectTileX == eventTileX) && (objList[o].ObjectTileY == eventTileY) && (objList[o].instance != null))
            {
                if (objList[o].instance.GetItemType() == ObjectInteraction.A_SCHEDULED_TRIGGER)
                {
                    objList[o].instance.GetComponent<trigger_base>().Activate(null);
                }
            }
        }
    }

    public override string EventName()
    {
        return "event_scheduled";
    }

    public override string summary()
    {
        return base.summary() + "\n\t\tTileX=" + (int)RawData[3] + ",TileY=" + (int)RawData[4];
    }
}
