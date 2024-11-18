﻿using UnityEngine;
using System.Collections;
using System.IO;

public class event_processor : UWClass
{

    public struct event_block
    {
        public int[] eventheader;
        public event_base[] events;
    };

    public event_block[] events_blocks;// = new event_block[15];


    /// <summary>
    /// Initializes a new instance of the <see cref="event_base"/> class.
    /// </summary>
    public event_processor()
    {
        char[] scd_ark;
        char[] scd_ark_file_data;
        if (!DataLoader.ReadStreamFile(Loader.BasePath + GameWorldController.instance.SCD_Ark_File_Selected, out scd_ark_file_data))
        {
            Debug.Log(Loader.BasePath + GameWorldController.instance.SCD_Ark_File_Selected + " File not loaded");
            return;
        }

        int NoOfBlocks = (int)DataLoader.getValAtAddress(scd_ark_file_data, 0, 32);

        events_blocks = new event_block[NoOfBlocks];
        for (int BlockNo = 0; BlockNo <= events_blocks.GetUpperBound(0); BlockNo++)
        {
            long address_pointer = 6;
            int datalen = (int)DataLoader.getValAtAddress(scd_ark_file_data, address_pointer + (NoOfBlocks * 4 * 2) + (BlockNo * 4), 32);

            address_pointer = (BlockNo * 4) + 6;
            if ((int)DataLoader.getValAtAddress(scd_ark_file_data, address_pointer, 32) == 0)
            {
                Debug.Log("No Scd.ark data for this level");
            }
            long BlockStart = DataLoader.getValAtAddress(scd_ark_file_data, address_pointer, 32);
            int j = 0;
            address_pointer = 0;//Since I am at the start of a fresh array.
            scd_ark = new char[datalen];
            for (long i = BlockStart; i < BlockStart + datalen; i++)
            {
                scd_ark[j] = scd_ark_file_data[i];
                j++;
            }

            int add_ptr = 0;

            int noOfRows = (int)DataLoader.getValAtAddress(scd_ark, 0, 8);
            if (noOfRows != 0)
            {
                //output = output + "Unknown info 1-325\n";
                events_blocks[BlockNo] = new event_block();
                events_blocks[BlockNo].eventheader = new int[325];
                for (int i = 1; i < 324; i++)
                {
                    events_blocks[BlockNo].eventheader[i - 1] = (int)DataLoader.getValAtAddress(scd_ark, add_ptr++, 8);
                }

                add_ptr = 326;
                events_blocks[BlockNo].events = new event_base[noOfRows];//Events are cleared after firing.
                int r = 0;
                for (int i = 0; i < events_blocks[BlockNo].events.GetUpperBound(0); i++)
                {
                    //Match event type to proper classes
                    switch ((int)DataLoader.getValAtAddress(scd_ark, add_ptr + 2, 8))
                    {
                        case event_base.RowTypeCondition:
                            events_blocks[BlockNo].events[r] = new event_conditional();
                            break;
                        case event_base.RowTypeSetNPCGoal:
                            events_blocks[BlockNo].events[r] = new event_set_goal();
                            break;
                        case event_base.RowTypeSetNPCGOAL_Alt:
                            events_blocks[BlockNo].events[r] = new event_set_goal_alt();
                            break;
                        case event_base.RowTypeMoveNPC:
                            events_blocks[BlockNo].events[r] = new event_move_npc();
                            break;
                        case event_base.RowTypeKillNPC:
                            events_blocks[BlockNo].events[r] = new event_kill_npc();
                            break;
                        case event_base.RowTypeFireTriggers:
                            events_blocks[BlockNo].events[r] = new event_fire_triggers();
                            break;
                        case event_base.RowTypeScheduled:
                            events_blocks[BlockNo].events[r] = new event_scheduled();
                            break;
                        case event_base.RowTypeRemoveNPC:
                            events_blocks[BlockNo].events[r] = new event_remove_npc();
                            break;
                        case event_base.RowTypeRaceAttidude:
                            events_blocks[BlockNo].events[r] = new event_set_race_attitude();
                            break;
                        case event_base.RowTypeSetProps:
                            events_blocks[BlockNo].events[r] = new event_set_npc_props();
                            break;
                        case event_base.RowTypeKillNPCorRace:
                            events_blocks[BlockNo].events[r] = new event_kill_npc_or_race();
                            break;
                        case event_base.RowTypePlaceNPC:
                            events_blocks[BlockNo].events[r] = new event_place_npc();
                            break;
                        default:
                            events_blocks[BlockNo].events[r] = new event_base();
                            break;
                    }
                    events_blocks[BlockNo].events[r].InitRawData(BlockNo, add_ptr, scd_ark);
                    r++;
                    add_ptr += 16;
                }
            }
        }
        if (GameWorldController.instance.CreateReports)
        {
            WriteEventReport();
        }
    }


    void WriteEventReport()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "//..//_scd_report.txt", false);
        string output = "";
        for (int BlockNo = 0; BlockNo <= events_blocks.GetUpperBound(0); BlockNo++)
        {
            if (events_blocks[BlockNo].events!=null)
            {
                for (int RowNo = 0; RowNo <= events_blocks[BlockNo].events.GetUpperBound(0); RowNo++)
                {
                    if (events_blocks[BlockNo].events[RowNo] != null)
                    {
                        output += "\nBlock " + BlockNo + " Row " + RowNo + "\n";
                        output += events_blocks[BlockNo].events[RowNo].ReportEventDetails();
                    }
                }
            }
        }
        writer.Write(output);
        writer.Close();
    }


        /// <summary>
        /// Processes the events for the current level
        /// </summary>
        public void ProcessEvents()
    {
        for (int b = 0; b <= events_blocks.GetUpperBound(0); b++)
        {
            //Begin each blook in execution mode. This will allow for firing of non-conditional traps
            event_base.Executing = true;
            if (events_blocks[b].events != null)
            {
                //bool executing =false;
                for (int r = 0; r <= events_blocks[b].events.GetUpperBound(0); r++)
                {
                    if (events_blocks[b].events[r] != null)
                    {
                        events_blocks[b].events[r].Process();
                        if (events_blocks[b].events[r].clear)
                        {
                            events_blocks[b].events[r] = null;
                        }
                    }//rows null										
                }//rows loop
            }//block null
        }//blocks loop
    }



    //This neeeds a rewrite to cover xclock conditions, and processing of many more type of event.
    //public void ProcessEvents(int levelNo)
    //{
    //	return;
    /*for (int b=0; b<=events_blocks.GetUpperBound(0);b++)
    {
            if (events_blocks[b].events!=null)
            {

                    for (int r=0; r< events_blocks[b].events.GetUpperBound(0);r++)//Last block is always null
                    {
                            if (events_blocks[b].events[r]!=null)
                            {
                                    switch(events_blocks[b].events[r].type)
                                    {
                                    case event_action.RowTypeCondition:
                                            {
                                                    //Check the condition
                                                    bool test=false;
                                                    if (events_blocks[b].event_actions[r].LevelNo==levelNo+1)
                                                    {
                                                            //if (events_blocks[b].event_actions[r].Enabled)
                                                            //{
                                                            if (events_blocks[b].event_actions[r].event_isQuest)
                                                            {//Test a quest variable
                                                                    test=(1 ==  Quest.instance.QuestVariables[events_blocks[b].event_actions[r].event_variable])	;
                                                                    if (test)
                                                                    {
                                                                            Debug.Log("matched on quest " + events_blocks[b].event_actions[r].event_variable);
                                                                    }
                                                            }
                                                            else
                                                            {//test a game variable
                                                                    test=(1 ==  Quest.instance.variables[events_blocks[b].event_actions[r].event_variable])	;
                                                                    if (test)
                                                                    {
                                                                            Debug.Log("matched on variable " + events_blocks[b].event_actions[r].event_variable);
                                                                    }
                                                            }		
                                                            //}

                                                    }
                                                    if (test)
                                                    {
                                                            //work through the rows until a condition is hit
                                                            int eventNo =1;
                                                            bool EventsAvailable=true;
                                                            //events_blocks[b].event_actions[r].Enabled=false;
                                                            while (EventsAvailable)
                                                            {
                                                                    if (events_blocks[b].event_actions[r+eventNo]!=null)	
                                                                    {
                                                                            if (events_blocks[b].event_actions[r+eventNo].LevelNo != events_blocks[b].event_actions[r].LevelNo)
                                                                            {
                                                                                    EventsAvailable=false;	
                                                                            }
                                                                            else
                                                                            {										
                                                                                    switch(events_blocks[b].event_actions[r+eventNo].type)
                                                                                    {
                                                                                    case event_action.RowTypeCondition:
                                                                                            {
                                                                                                    EventsAvailable=false;
                                                                                                    break;																										
                                                                                            }
                                                                                    case event_action.RowTypeEvent:
                                                                                            {
                                                                                                    //Fire the events in this block!
                                                                                                    ObjectLoaderInfo[] objList=CurrentObjectList().objInfo;
                                                                                                    int eventTileX=events_blocks[b].event_actions[r+eventNo].EventTileX;
                                                                                                    int eventTileY=events_blocks[b].event_actions[r+eventNo].EventTileY;
                                                                                                    for (int o=256; o<=objList.GetUpperBound(0);o++)
                                                                                                    {
                                                                                                            if ( (objList[o].tileX==eventTileX) && (objList[o].tileY==eventTileY) && (objList[o].instance!=null))
                                                                                                            {
                                                                                                                    if (objList[o].instance.GetItemType()==ObjectInteraction.A_SCHEDULED_TRIGGER)	
                                                                                                                    {
                                                                                                                            objList[o].instance.GetComponent<trigger_base>().Activate(null);		
                                                                                                                    }
                                                                                                            }
                                                                                                    }
                                                                                                    //Clear the event
                                                                                                    events_blocks[b].event_actions[r+eventNo]=null;
                                                                                                    eventNo++;
                                                                                                    if (eventNo>=events_blocks[b].event_actions.GetUpperBound(0))
                                                                                                    {
                                                                                                            EventsAvailable=false;
                                                                                                    }
                                                                                                    break;
                                                                                            }
                                                                                    case event_action.RowTypeRaceAttidude:
                                                                                            {
                                                                                                    Debug.Log("Adjust race attitudes" +events_blocks[b].event_actions[r+eventNo].WhoAmIOrRace );
                                                                                                    events_blocks[b].event_actions[r+eventNo]=null;
                                                                                                    eventNo++;
                                                                                                    if (eventNo>=events_blocks[b].event_actions.GetUpperBound(0))
                                                                                                    {
                                                                                                            EventsAvailable=false;
                                                                                                    }
                                                                                                    break;
                                                                                            }
                                                                                    case event_action.RowTypeSetHome:
                                                                                            {
                                                                                                    Debug.Log("Set Home");
                                                                                                    events_blocks[b].event_actions[r+eventNo]=null;
                                                                                                    eventNo++;
                                                                                                    if (eventNo>=events_blocks[b].event_actions.GetUpperBound(0))
                                                                                                    {
                                                                                                            EventsAvailable=false;
                                                                                                    }
                                                                                                    break;
                                                                                            }
                                                                                    default:
                                                                                            //EventsAvailable=false;
                                                                                            Debug.Log("Unknown row type " + events_blocks[b].event_actions[r+eventNo].type);
                                                                                            eventNo++;
                                                                                            if (eventNo>=events_blocks[b].event_actions.GetUpperBound(0))
                                                                                            {
                                                                                                    EventsAvailable=false;
                                                                                            }
                                                                                            break;

                                                                                    }//end switch type.
                                                                            }

                                                                    }//end if null
                                                                    else
                                                                    {
                                                                            EventsAvailable=false;
                                                                    }
                                                            }//end do
                                                    }//end test
                                                    break;
                                            }//end case condition
                                    }//end switch
                            }
                    }
            }
    }*/
    //}
}
