﻿using UnityEngine;
using System.Collections;

public class Readable : object_base
{


    public override bool use()
    {
        if (CurrentObjectInHand == null)
        {
            if ((_RES == "UW1") && (link == 769))
            {//Special case for Rotworm stew recipe
                return MixRotwormStew();
            }
            return Read();
        }
        else
        {
            return ActivateByObject(CurrentObjectInHand);
        }
    }

    public override bool LookAt()
    {
        return Read();
    }

    public virtual bool Read()
    {//Returns the text of this readable.
     //ObjectInteraction objInt = this.gameObject.GetComponent<ObjectInteraction>();
     //StringController SC = objInt().getStringController();
     //UILabel ml = objInt().getMessageLog();

        switch (objInt().GetItemType())
        {
            case ObjectInteraction.BOOK://Book
            case ObjectInteraction.SCROLL://Scroll
                {
                    if (objInt().PickedUp == true)
                    {
                        if ((UWEBase._RES == UWEBase.GAME_UW1) && (link == 520))
                        {//Special case. Chasm of fire map.
                            UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs410.n01";
                            UWHUD.instance.CutScenesSmall.anim.looping = true;
                        }
                        else
                        {
                            UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(3, link - 0x200));
                        }
                        return true;
                    }
                    else
                    {
                        return base.LookAt();
                    }

                }

            default:
                {
                    UWHUD.instance.MessageScroll.Add("READABLE TYPE NOT FOUND! (" + item_id + ")");
                    return false;
                }
        }
    }

    /// <summary>
    /// Mixs the rotworm stew.
    /// </summary>
    /// <returns><c>true</c>, if rotworm stew was mixed, <c>false</c> otherwise.</returns>
    bool MixRotwormStew()
    {
        bool hasPort = false;
        ObjectInteraction port = null;
        bool hasGreenMushroom = false;
        ObjectInteraction mushroom = null;
        bool hasCorpse = false;
        ObjectInteraction corpse = null;
        bool hasExtraItems = false;
        //Find a bowl in the players inventory.
        //Check if it only contains port, a rotworm corpse and a greenmushroom.

        //000~001~148~The bowl does not contain the correct ingredients. \n
        //000~001~149~You mix the ingredients into a stew. \n
        //000~001~150~You need a bowl to mix the ingredients. \n
        Container cn = UWCharacter.Instance.playerInventory.playerContainer;
        if (cn != null)
        {
            ObjectInteraction bowl = cn.findItemOfType(142); //Finds the first bowl in the inventory;
           // if (BowlName != "")
            //{
                //GameObject bowl = GameObject.Find(BowlName);
                if (bowl != null)
                {
                    //Search for
                    Container bowlContainer = bowl.GetComponent<Container>();
                    if (bowlContainer != null)
                    {
                        for (short i = 0; i <= bowlContainer.GetCapacity(); i++)
                        {
                            ObjectInteraction foundItemObj = bowlContainer.GetItemAt(i);
                            if (foundItemObj != null)
                            {
                                switch (foundItemObj.item_id)
                                {
                                    case 184://Mushroom
                                        mushroom = foundItemObj;
                                        hasGreenMushroom = true; break;
                                    case 190://Port
                                        port = foundItemObj;
                                        hasPort = true; break;
                                    case 217://Rotworm Corpse
                                        corpse = foundItemObj;
                                        hasCorpse = true; break;
                                    default:
                                        hasExtraItems = true; break;
                                }
                            }
                        }
                        //Has a bowl. Now test contents.
                        if (
                                (hasCorpse) && (hasGreenMushroom) && (hasPort)
                                && (!hasExtraItems)
                            )
                        {//Mix port
                         //000~001~149~You mix the ingredients into a stew. \n
                            UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 149));
                            //Consume the items
                            port.consumeObject();
                            corpse.consumeObject();
                            mushroom.consumeObject();

                            ObjectInteraction bowlObjectInt = bowl.GetComponent<ObjectInteraction>();
                            bowlObjectInt.ChangeType(283);
                            Destroy(bowlContainer);
                            bowl.gameObject.AddComponent<Food>();
                            bowlObjectInt.isquant = 1;
                            bowlObjectInt.link = 1;
                            return true;
                        }
                        else
                        {//We don't have the items
                         //000~001~148~The bowl does not contain the correct ingredients. \n	
                            UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 148));
                            return true;
                        }
                    }
                }
            //}
        }
        //000~001~150~You need a bowl to mix the ingredients. \n	
        UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 150));
        return true;
    }

    public override string UseVerb()
    {
        return "read";
    }

    public override string ExamineVerb()
    {
        return "read";
    }

    public override float GetWeight()
    {
        return GameWorldController.instance.commonObject.properties[item_id].mass * 0.1f;
    }

}
