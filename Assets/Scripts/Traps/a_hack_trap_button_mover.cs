﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves buttons button up and down
/// </summary>
/// Trap zpos is the lower position of the button. owner is how much it moves up.
public class a_hack_trap_button_mover : a_hack_trap {

    public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
    {//950-954-- not sure yet if this is the correct behaviour to hard code indices
        switch (src.owner)
        {
            case 1://moves buttons 1/2
                MoveButton(950);
                MoveButton(951);
                break;
            case 2://Moves buttons 2/3 
                MoveButton(951);
                MoveButton(952);
                break;
            case 4: //moves buttons 3/4
                MoveButton(952);
                MoveButton(953);
                break;
            case 8:// moves buttons 4/5
                MoveButton(953);
                MoveButton(954);
                break;
            case 16://moves buttons 5/1
                MoveButton(954);
                MoveButton(950);
                break;
            default:
                Debug.Log("unknown switch to move " + src.owner);
                break;
        }
    }

    /// <summary>
    /// Begin moving the button. 
    /// </summary>
    /// <param name="index"></param>
    void MoveButton(int index)
    {
        if (CurrentObjectList().objInfo[index].instance != null)
        {
            ObjectInteraction button = CurrentObjectList().objInfo[index].instance;
            if (button.zpos == zpos)
            {
                MoveButton((short)(zpos + owner), button);
            }
            else
            {//Move back
                MoveButton(zpos , button);
            }
        }
        
    }

    /// <summary>
    /// Move the button to the new z position
    /// </summary>
    /// <param name="NewZpos"></param>
    /// <param name="buttonToMove"></param>
    void MoveButton(short NewZpos, ObjectInteraction buttonToMove)
    {
        buttonToMove.zpos = NewZpos;
        buttonToMove.objectloaderinfo.zpos = NewZpos;
        Vector3 newPos = ObjectLoader.CalcObjectXYZ(buttonToMove.objectloaderinfo.index, 0);
        buttonToMove.transform.position = newPos;
    }

    public override void PostActivate(object_base src)
    {
        Debug.Log("Overridden PostActivate to test " + this.name);
        base.PostActivate(src);
    }

}
