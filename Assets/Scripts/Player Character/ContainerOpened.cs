﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContainerOpened : GuiBase_Draggable
{
    //The gui element that represents the currently opened container in the inventory.
    public GameObject BackpackBg;
    public GameObject InvUp;
    public GameObject InvDown;

    public override void Start()
    {
        base.Start();
        GRLoader grInv = new GRLoader(GRLoader.INV_GR);
        BackpackBg.GetComponent<RawImage>().texture = grInv.LoadImageAt(6);
    }

    void CloseChildContainer(Container ClosingParent)
    {//Recursively closes open child containers
        ClosingParent.isOpenOnPanel = false;
        for (int i = 0; i <= ClosingParent.MaxCapacity(); i++)
        {
            if (ClosingParent.items[i] != null)
            {
                //GameObject currObj = GameObject.Find(ClosingParent.items[i]);
                ObjectInteraction currObj = ClosingParent.items[i];
                {
                    if (currObj != null)
                    {
                        Container currObjContainer = currObj.GetComponent<Container>();
                        if (currObjContainer != null)
                        {
                            CloseChildContainer(currObjContainer);
                        }
                    }
                }
            }
        }
    }

    public void OnClick()
    {
        if (Dragging == true) { return; }
        if ((UWCharacter.Instance.isRoaming == true) || (Quest.instance.InDreamWorld))
        {//No inventory use while using wizard eye.
            return;
        }
        if (UWCharacter.Instance.playerInventory.currentContainer == UWCharacter.Instance.name)
        {//Don't do anything on the top level
            UWCharacter.Instance.playerInventory.ContainerOffset = 0;
            BackpackBg.SetActive(false);
            return;
        }
        if (CurrentObjectInHand == null)
        {//Player has no object in their hand. We close up the container.
            ScrollButtonInventory.ScrollValue = 0;
            UWCharacter.Instance.playerInventory.ContainerOffset = 0;
            Container currentContainerObj = UWCharacter.Instance.playerInventory.GetCurrentContainer();
            UWCharacter.Instance.playerInventory.currentContainer = currentContainerObj.ContainerParent;
            currentContainerObj.isOpenOnPanel = false;
            //Close child containers as well
            CloseChildContainer(currentContainerObj);
            Container DestinationContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
            if (UWCharacter.Instance.playerInventory.currentContainer == UWCharacter.Instance.name)
            {
                GetComponent<RawImage>().texture = UWCharacter.Instance.playerInventory.Blank;
                BackpackBg.SetActive(false);
                if ((DestinationContainer.CountItems() >= 8) && (DestinationContainer != UWCharacter.Instance.playerInventory.playerContainer))
                {
                    InvUp.SetActive(true);
                    InvDown.SetActive(true);
                }
                else
                {
                    InvUp.SetActive(false);
                    InvDown.SetActive(false);
                }
            }
            else
            {
                GetComponent<RawImage>().texture = DestinationContainer.transform.GetComponent<ObjectInteraction>().GetEquipDisplay().texture;
                BackpackBg.SetActive(true);
                if ((DestinationContainer.CountItems() >= 8) && (DestinationContainer != UWCharacter.Instance.playerInventory.playerContainer))
                {
                    InvUp.SetActive(true);
                    InvDown.SetActive(true);
                }
                else
                {
                    InvUp.SetActive(false);
                    InvDown.SetActive(false);
                }

            }
            for (short i = 0; i < 8; i++)
            {
               //ObjectInteraction sItem = DestinationContainer.GetItemAt(i);
               UWCharacter.Instance.playerInventory.SetObjectAtSlot((short)(i + 11), DestinationContainer.GetItemAt(i));
      
            }
        }
        else
        {
            if (UWCharacter.InteractionMode != UWCharacter.InteractionModePickup)
            {//Only allow this to happen when in pickup mode.
                return;
            }
            //Move the contents out of the container into the parent.
            Container CurrentContainer = GameObject.Find(UWCharacter.Instance.playerInventory.currentContainer).GetComponent<Container>();
            Container DestinationContainer = GameObject.Find(CurrentContainer.ContainerParent).GetComponent<Container>();
            //ObjectInteraction item = CurrentObjectInHand;
            if (Container.TestContainerRules(DestinationContainer, 11, false))
            {
                //if ((item.isQuant()==false) || (item.isEnchanted()))
                if (!CurrentObjectInHand.IsStackable())
                {
                    if (DestinationContainer.AddItemToContainer(CurrentObjectInHand))
                    {//Object has moved
                     //UWHUD.instance.CursorIcon= UWHUD.instance.CursorIconDefault;
                        CurrentObjectInHand = null;
                    }
                }
                else
                {
                    if (DestinationContainer.AddItemMergedItemToContainer(CurrentObjectInHand))
                    {//Object has moved
                        //UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
                        CurrentObjectInHand = null;
                    }
                }
            }
        }
    }
}