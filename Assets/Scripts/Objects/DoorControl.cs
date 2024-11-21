﻿using UnityEngine;
using System.Collections;
using UnityEngine.AI;
/// <summary>
/// Door controller for manipulating doors
/// </summary>
public class DoorControl : object_base {

		///What keys can open this
	public int KeyIndex; //THis is the same as objInt.link
		///True for open, false for closed.

		///Special cases. Sets direction of opening
	//public bool isPortcullis;
		///Is the door opening or closing. Used to keep if flying off it's hinges!
	public bool DoorBusy;
		///Sets if the lock can be picked.
	//public bool Pickable=true;
	///Is the door spiked
	//public bool Spiked;//Probably on the lock object?
	///Is it the player using the object or a trigger/trap.
	public bool PlayerUse=false;

	//Visible faces indices
	const int vTOP =0;
	const int vEAST =1;
	const int vBOTTOM= 2;
	const int vWEST= 3;
	const int vNORTH= 4;
	const int vSOUTH= 5;

	//Door headings
	const int NORTH=180;
	const int SOUTH=0;
	const int EAST=270;
	const int WEST=90;

	const int OpenRotation = -90;
	const int CloseRotation = -OpenRotation;

	public const float DefaultDoorTravelTime=1.3f;

	protected override void Start ()
	{
        ObjectInteraction doorlock = getLockObjInt();
        if (doorlock!=null)
        {
            KeyIndex = doorlock.link & 0x3F;
        }
        if (state())
		{//Make sure it is open
			if (isPortcullis()==false)
			{
				StartCoroutine(RotateDoor (this.transform,Vector3.up * doordirection() * OpenRotation,0.01f));
			}
			else
			{
				StartCoroutine(RaiseDoor (this.transform,new Vector3(0f,+1.1f,0f),0.1f));
				NavMeshObstacle nmo = this.GetComponent<NavMeshObstacle>();
				if (nmo!=null)
				{
						nmo.enabled = !state();
				}
			}	
		}
	}


		public bool isPortcullis()
		{
			switch (item_id)	
			{
				case 326:
				case 334:
					return true;	
			default:
					return false;
			}
		}

		/// <summary>
		/// Gets the lock object interaction.
		/// </summary>
		/// <returns>The lock object int.</returns>
		/// TODO: The portcullis on lvl 7 of the prison tower is linked to a door trap rather than a lock. 
		ObjectInteraction getLockObjInt()
		{
			if(link==0)
			{
					return null;
			}
			else
			{
				if (ObjectLoader.GetItemTypeAt(link) == ObjectInteraction.LOCK)
				{
					return ObjectLoader.getObjectIntAt(link);	
				}
				else
				{//Try and find in the chain of execution.						
					return null;
					//return FindObjectInChain(link, ObjectInteraction.LOCK);
				}					
			}
		}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (link != 0)
        {
            GameObject target = ObjectLoader.getGameObjectAt(link);
            if (target != null)
            {
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
    }


    /// <summary>
    /// Is the door opened or closed. Determined by item id
    /// </summary>
    public bool state()
		{
				switch (item_id)	
				{
				case 320:
				case 321:
				case 322:
				case 323:
				case 324:
				case 325:
				case 326:
				case 327:
						return false;//closed

				case 328:
				case 329:
				case 330:
				case 331:
				case 332:
				case 333:
				case 334:
				case 335:
				default:
						return true; //open
				}
		}

		/// <summary>
		/// Checks against the link on this door to tell if the door is locked.
		/// </summary>
		public bool locked()
		{
			ObjectInteraction LockObject = getLockObjInt();
			if (LockObject!=null)
			{
				//Check bit 9 of the flags (bit 1 of the variable)
				return ((LockObject.flags & 0x01)==1);
			}
			else
			{
				return false;
			}
		}

	public override bool use ()
	{
		trigger_base trigger=null;
		//If a door has a link and the link is to something that is not a lock then it is a trigger that I need to activae
		if(link !=0)
		{
			ObjectInteraction linkedObject = ObjectLoader.getObjectIntAt(link);
			if (linkedObject!=null)
			{
				if (linkedObject.GetItemType()!= ObjectInteraction.LOCK)
				{
					trigger= linkedObject.GetComponent<trigger_base>();	
				}
			}
		}

		if (CurrentObjectInHand !=null)
		{
			ActivateByObject(CurrentObjectInHand);
			//Clear the object in hand
			//UWHUD.instance.CursorIcon= UWHUD.instance.CursorIconDefault;
			CurrentObjectInHand=null;
			if (trigger!=null)
			{
				trigger.Activate(this.gameObject);
			}
			return true;	
		}
		else
		{//Normal Usage
			PlayerUse=true;
            if ((UWCharacter.AutoKeyUse) && (locked()))
            {//Try each key in the players inventory to see if it can open the door
                foreach (Transform t in GameWorldController.instance.InventoryMarker.transform)
                {
                    if (t.gameObject.GetComponent<DoorKey>() != null)
                    {
                        DoorKey key = t.gameObject.GetComponent<DoorKey>();
                        if (key.KeyId == KeyIndex)
                        {
                            ActivateByObject(key.objInt());
                            if (trigger != null)
                            {
                                trigger.Activate(this.gameObject);
                            }
                            PlayerUse = false;
                            return true;
                        }
                    }
                }
            }

            Activate(this.gameObject);			
			PlayerUse=false;
			return true;
		}
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{//Code for handling otherobjects used on this object
	//Doors can be used by keys, picks and spikes.
		//ObjectInteraction objIntUsed = ObjectUsed.GetComponent<ObjectInteraction>();
		if (ObjectUsed != null) 
		{
			switch (ObjectUsed.GetItemType())
				{
				case ObjectInteraction.KEY: //Key
				DoorKey dk = ObjectUsed.GetComponent<DoorKey>();
				if (dk !=null)
					{
					if (state()==true)
					{
						//Door is already open
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString (1,6));
						return true;
					}
						ObjectInteraction doorlock = getLockObjInt();
						if (doorlock==null)
						{
								UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,3));
								return false;
						}
						if((doorlock.link & 0x3F)==dk.owner)//This is a valid key for the door.
						{
							ToggleLock(true);
								if (locked()==true)
								{//Locked message
								UWHUD.instance.MessageScroll.Add(StringController.instance.GetString (1,4));
								}
							else
								{//Unlockedmessage
								UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,5));
								}
						return true;
						}
					else
						{
						if (link==53)
							{//There is no lock
							UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,3));
							}
						else
							{//That is the wrong key.
							UWHUD.instance.MessageScroll.Add(StringController.instance.GetString (1,2));
							}
						return true;
						}

					}					
					break;
				case ObjectInteraction.LOCKPICK:	//lockpick
                    {
                        //lock pick skill check is based on the zpos of the "lock" object * 3 vs the players (lockpick skill + 1)
                        //Locks cannot be picked if the zpos is 0xE or greater. In either case the skillcheck is attempted.
                        ObjectInteraction LockObject = getLockObjInt();
                        if (LockObject != null)
                        {
                            int skillvalue = UWCharacter.Instance.PlayerSkills.PickLock + 1;
                            int targetvalue = LockObject.zpos * 3;
                            Skills.SkillRollResult skillroll = Skills.SkillRoll(skillvalue, targetvalue);

                            if (LockObject.zpos>=0xE)
                            {//unpickable
                               if (skillroll == Skills.SkillRollResult.CriticalFailure)
                                {
                                    BreakLockPick(ObjectUsed);
                                }
                                else
                                {
                                    UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_your_lockpicking_attempt_failed_));
                                }
                            }
                            else
                            {
                                switch(skillroll)
                                {
                                    case Skills.SkillRollResult.CriticalFailure:
                                        {
                                            BreakLockPick(ObjectUsed);
                                            break;
                                        }
                                    case Skills.SkillRollResult.Failure:
                                        {
                                            UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_your_lockpicking_attempt_failed_));
                                            break;
                                        }
                                    case Skills.SkillRollResult.Success:
                                    case Skills.SkillRollResult.CriticalSuccess:
                                        {
                                            UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,StringController.str_you_succeed_in_picking_the_lock_));
                                            UnlockDoor(true);
                                            break;
                                        }
                                }
                            }                            
                        }
                        else
                        {
                            UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 3));//There is no lock on that.
                        }
                    }
                    break;
					//{
					//if (Pickable==true)
					//	{
					//		if (UWCharacter.Instance.PlayerSkills.TrySkill(Skills.SkillPicklock, Skills.DiceRoll(1,25)))
					//		{
					//		UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,StringController.str_you_succeed_in_picking_the_lock_));
					//		UnlockDoor(true);
					//		}
					//	else
					//		{														
					//		//Debug.Log ("Picklock failed!");
					//		UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,StringController.str_your_lockpicking_attempt_failed_));
					//		//objIntUsed.consumeObject();
					//		}
					//	}
					//else
					//	{
					//		UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,StringController.str_your_lockpicking_attempt_failed_));
					//	}
					//break;
					//}
				case ObjectInteraction.SPIKE:
					{
						if(Spike())
						{
                            ObjectUsed.consumeObject();
						}
						break;
					}
				default:
					return false;
				}
		}
		else
		{
			return false;
		}
		return true;
	}

    private static void BreakLockPick(ObjectInteraction ObjectUsed)
    {
        //pick break
        if (_RES == GAME_UW2)
        {
            UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 134));
        }
        else
        {
            UWHUD.instance.MessageScroll.Add("You broke your pick. \n"); //string is hard coded in uw1??
        }
        ObjectUsed.consumeObject();
    }


    /// <summary>
    /// Tells if the door is spiked closed. Ie an npc cannot use it.
    /// </summary>
    public bool Spiked()
		{
			return (owner == 63);
		}

		/// <summary>
		/// Spike this door. Blocks NPC from opening
		/// </summary>
	public bool Spike()
	{//returns true if door becomes spiked
		if (Spiked()==false)
		{
			//000~001~128~You can only spike closed doors.
			//000~001~129~The door is now spiked closed.
			//000~001~130~Please select door to spike...
			//000~001~131~The door is spiked.
			if (state()==false)
			{//Closed door
				UWHUD.instance.MessageScroll.Add (StringController.instance.GetString(1,129));
				//Spiked=true;
				owner=63;		
				return true;
			}
			else
			{
			UWHUD.instance.MessageScroll.Add (StringController.instance.GetString(1,128));
			}						
		}
		else
		{
			UWHUD.instance.MessageScroll.Add (StringController.instance.GetString(1,131));
		}
		return false;	
	}

	public override bool Activate(GameObject src)
	{
		if (locked()==false)
		{//Toggle Open and closed
			if (state()==false)
				{//Door is closed
				OpenDoor (DoorControl.DefaultDoorTravelTime);
				}
			else
				{//Door is open
					CloseDoor (DoorControl.DefaultDoorTravelTime);
				}
		}
		else
		{
			if (PlayerUse==true)
			{
				UWHUD.instance.MessageScroll.Add ("The " + StringController.instance.GetObjectNounUW(objInt()) + " is locked.");
			}
		}
		return true;
	}

	/// <summary>
	/// Opens the door.
	/// </summary>
	public void OpenDoor(float DoorTravelTime)
	{
		if (state()	==false)
			{
			if(!DoorBusy)
			{
				if (isPortcullis()==false)
				{
					if (ObjectInteraction.PlaySoundEffects)
					{
						objInt().aud.clip=MusicController.instance.SoundEffects[MusicController.SOUND_EFFECT_DOOR_MOVE];
						objInt().aud.Play();		
					}
					StartCoroutine(RotateDoor (this.transform,Vector3.up * doordirection() * OpenRotation,DoorTravelTime));
				}
				else
				{
					if (ObjectInteraction.PlaySoundEffects)
					{
						objInt().aud.clip=MusicController.instance.SoundEffects[MusicController.SOUND_EFFECT_PORTCULLIS];
						objInt().aud.Play();		
					}
					StartCoroutine(RaiseDoor (this.transform,new Vector3(0f,1.1f,0f),DoorTravelTime));
				}
				owner=0;
				item_id+=8;
				zpos+=24;
				enchantment=1;
				if (isPortcullis())
				{
					//flags=12;	
					flags=4;
					NavMeshObstacle nmo = this.GetComponent<NavMeshObstacle>();
					if (nmo!=null)
					{
						nmo.enabled = !state();
					}
				}
				else
				{
					//flags=13;	
					flags=5;	
				}
				
				//state=true;
				if(link!=0)
					{	//If it's link is to something that is not a lock then it is likely to be a trigger
						if (
								(
										(ObjectLoader.GetItemTypeAt(link) != ObjectInteraction.LOCK)
										&&
										(ObjectLoader.GetItemTypeAt(link) != ObjectInteraction.A_CLOSE_TRIGGER)
								)
							)
						{
							trigger_base tb= ObjectLoader.getObjectIntAt(link).GetComponent<trigger_base>();
							if (tb!=null)
							{
								tb.Activate(this.gameObject);
							}
						}
						else
						{//The object is linked to a lock. The next of the lock is the use trigger to use here
							ObjectInteraction LockObj = ObjectLoader.getObjectIntAt(link);
							if (LockObj!=null)
							{
								int next= LockObj.next;

								while (next!=0)
								{
									ObjectInteraction TriggerObject= ObjectLoader.getObjectIntAt(next);
									if (TriggerObject!=null)
										{
										next=0;
										trigger_base tb= TriggerObject.GetComponent<trigger_base>();
										if (tb!=null)
										{
											if 
												(
												(tb.objInt().GetItemType()!=ObjectInteraction.A_CLOSE_TRIGGER)
														&&
												(tb.objInt().GetItemType()!=ObjectInteraction.AN_UNLOCK_TRIGGER)
												)
												{
													tb.Activate(this.gameObject);											
												}
											next = tb.next;
										}	
									}	
									else
									{
										next=0;
									}
								}
							}
						}
					}
			}
		}
	}



	/// <summary>
	/// Closes the door.
	/// </summary>
	public void CloseDoor(float DoorTravelTime)
	{
		if (state()==true)
		{
			if(!DoorBusy)
			{
				if (isPortcullis()==false)
				{
					if (ObjectInteraction.PlaySoundEffects)
					{
						objInt().aud.clip=MusicController.instance.SoundEffects[MusicController.SOUND_EFFECT_DOOR_MOVE];
						objInt().aud.Play();		
					}
					StartCoroutine(RotateDoor (this.transform,Vector3.up * doordirection() * CloseRotation,DoorTravelTime));
				}
				else
				{
					if (ObjectInteraction.PlaySoundEffects)
					{
						objInt().aud.clip=MusicController.instance.SoundEffects[MusicController.SOUND_EFFECT_PORTCULLIS];
						objInt().aud.Play();		
					}
					StartCoroutine(RaiseDoor (this.transform,new Vector3(0f,-1.1f,0f),DoorTravelTime));
				}
				item_id-=8;
				zpos-=24;
				flags=0;
				enchantment=0;
				//state=false;
				if (isPortcullis())
				{
						NavMeshObstacle nmo = this.GetComponent<NavMeshObstacle>();
						if (nmo!=null)
						{
								nmo.enabled = !state();
						}
				}

				if(link!=0)
				{	//If it's link is to something that is not a lock then it is likely to be a trigger
						if
								(
								(ObjectLoader.GetItemTypeAt(link) != ObjectInteraction.LOCK)
								&&
								(ObjectLoader.GetItemTypeAt(link) != ObjectInteraction.AN_OPEN_TRIGGER)
								)
						{
							trigger_base tb= ObjectLoader.getObjectIntAt(link).GetComponent<trigger_base>();
							if (tb!=null)
							{														
								tb.Activate(this.gameObject);
							}
						}
						else
						{//The object is linked to a lock. The next of the lock is the use trigger to use here
								ObjectInteraction LockObj = ObjectLoader.getObjectIntAt(link);
								if (LockObj!=null)
								{
										int next= LockObj.next;

										while (next!=0)
										{
												
												ObjectInteraction TriggerObject= ObjectLoader.getObjectIntAt(next);

												if (TriggerObject!=null)
												{
														next=0;
														trigger_base tb= TriggerObject.GetComponent<trigger_base>();
														if (tb!=null)
														{
																
																if (tb.objInt().GetItemType()!=ObjectInteraction.AN_OPEN_TRIGGER)
																{
																	tb.Activate(this.gameObject);											
																}											
																next = tb.next;
														}

												}	
												else
												{
														next=0;
												}
										}
								}
						}
				}
			}
		}
	}


		/// <summary>
		/// Returns the multiplier for the door rotation
		/// </summary>
	int doordirection()
	{
		if (doordir==0)
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}

		/// <summary>
		/// Locks the door.
		/// </summary>
	public void LockDoor()
	{		
		ObjectInteraction LockObject = getLockObjInt();
		if (LockObject!=null)
		{
			//To lock set bit 8 of the flags
			LockObject.flags = (short)(LockObject.flags | 0x1);
		}
	}

		/// <summary>
		/// Unlocks the door.
		/// </summary>
	public void UnlockDoor(bool PlayerUse)
	{
		ObjectInteraction LockObject = getLockObjInt();
		if (LockObject!=null)
		{
			//To unlock unset bit 8 of the flags
			LockObject.flags = (short)(LockObject.flags & 0xE);
			if ((PlayerUse) && (LockObject.next!=0))
			{
				if (ObjectLoader.GetItemTypeAt(LockObject.next)==ObjectInteraction.AN_UNLOCK_TRIGGER)
				{
					ObjectInteraction triggerObj = ObjectLoader.getObjectIntAt(LockObject.next);
					if (triggerObj!=null)
					{
						triggerObj.GetComponent<object_base>().Activate(this.gameObject);		
					}
				}	
			}
		}
	}

		/// <summary>
		/// Toggles the lock state
		/// </summary>
	public void ToggleLock(bool PlayerUse)
	{
		if (locked()==false)
		{
			LockDoor();
		}
		else
		{
			UnlockDoor(PlayerUse);
		}
	}

		/// <summary>
		/// Toggles the door open or closed.
		/// </summary>
	public void ToggleDoor(float doorTravelTime, bool PlayerUse)
	{
		if (state()==false)//Closed
		{
			//Debug.Log("Toggling door open");
			UnlockDoor(PlayerUse);
			OpenDoor(doorTravelTime);	
		}
		else
		{
			//Debug.Log("Toggling door closed");
			CloseDoor (doorTravelTime);
			LockDoor();
		}
	}


		/// <summary>
		/// Rotates the door open or closed
		/// </summary>
		/// <returns>The door.</returns>
		/// <param name="door">Door.</param>
		/// <param name="turningAngle">What angle to turn to</param>
		/// <param name="traveltime">How long the door takes to open/close</param>
	IEnumerator RotateDoor(Transform door, Vector3 turningAngle, float traveltime)
	{
		Quaternion StartAngle = door.rotation;
		Quaternion EndAngle = Quaternion.Euler (door.eulerAngles+turningAngle);
		DoorBusy=true;
		for (float t = 0.0f; t<=traveltime; t+=Time.deltaTime/traveltime)
		{
			door.rotation=Quaternion.Lerp (StartAngle,EndAngle,t);
			yield return null;
		}
		DoorBusy=false;
		door.rotation = EndAngle;
		if (traveltime>1f)
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				objInt().aud.clip=MusicController.instance.SoundEffects[MusicController.SOUND_EFFECT_DOOR_FINISH];
				objInt().aud.Play();		
			}
		}
	}
	
		/// <summary>
		/// Raises the portcullis
		/// </summary>
		/// <returns>The door.</returns>
		/// <param name="door">Door.</param>
		/// <param name="TransformDir">What direction to travel in</param>
		/// <param name="traveltime">How long the door takes to raise or drop</param>
	IEnumerator RaiseDoor(Transform door, Vector3 TransformDir, float traveltime)
	{
		float rate = 1.0f/traveltime;
		float index = 0.0f;
		Vector3 StartPos = door.position;
		Vector3 EndPos = StartPos + TransformDir;
		DoorBusy=true;
		while (index <=traveltime)
		{
			door.position = Vector3.Lerp (StartPos,EndPos,index);
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		DoorBusy=false;
		door.position = EndPos;
	}

	public override bool ApplyAttack (short damage, GameObject source)
	{
		return ApplyAttack (damage);
	}

	public override bool ApplyAttack(short damage)
	{//TODO:Find out how massive doors resist damage
		if (DR()<3)
		{
			if (DR()!=0)
			{
				damage= (short) (damage/DR());
			}
		quality=(short)(quality-damage);
		if ((quality<=0))
			{
				//locked=false;
				UnlockDoor(true);
				OpenDoor(DoorControl.DefaultDoorTravelTime);
			}
		}
		return true;		
	}

	/// <summary>
	/// Gets the Damage resistance of this door
	/// </summary>
	private short DR()
	{
		switch (item_id)
		{
		case 320://0
		case 327:
				return 0;
		case 321://1
		case 328:
		case 322:
		case 329:
				return 1;
		case 323://2
		case 330:
		case 324://2
		case 331:
				return 2;
		case 325://3
		case 333:
		default:
				return 3;
		}
	}


	public override bool LookAt()
	{
		if (isPortcullis()==false)
		{

			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(),DoorQuality()));
			if( (Spiked() )&& (_RES!=GAME_UW2))
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1,131));
			}
		}
		else
		{
			UWHUD.instance.MessageScroll.Add (StringController.instance.GetFormattedObjectNameUW(objInt()));
		}
		return true;
	}


	/// <summary>
	/// Gets the door condition
	/// </summary>
	/// <returns>The quality string of the door</returns>
	private string DoorQuality()
	{//TODO:The figures here are based on food quality levels!
		
		if (quality == 0)
		{
			return StringController.instance.GetString (5,0);//brken
		}
				if ((quality >=1) && (quality <15))
		{
			return StringController.instance.GetString (5,1);//badly damaged
		}
				if ((quality >=15) && (quality <32))
		{
			return StringController.instance.GetString (5,2);//damaged
		}
				if ((quality >=32) && (quality <=40))
		{
			return StringController.instance.GetString (5,3);//sturdy
		}
		
				if ((quality >40) && (quality <48))
		{
			return StringController.instance.GetString (5,4);//massive?
		}
		else
		{
			return StringController.instance.GetString (5,5);//massive?
		}
	} 



	public override string UseVerb ()
	{
		if (state()==false)//Closed
			{
				return "open";		
			}
			else
			{
				return "close";		
			}
	}

	public override string UseObjectOnVerb_World ()
	{
		//ObjectInteraction ObjIntInHand=CurrentObjectInHand;
		if (CurrentObjectInHand != null)
		{
			switch (CurrentObjectInHand.GetItemType())	
			{
			case ObjectInteraction.KEY:
				return "turn key in lock";
			case ObjectInteraction.SPIKE:
				return "spike door";
			case ObjectInteraction.LOCKPICK:
				return "attempt lockpicking";
			}
		}

		return base.UseObjectOnVerb_Inv();
	}


	/// <summary>
	/// Creates the door model
	/// </summary>
	/// <param name="myObj">My object.</param>
	/// <param name="DoorTexturePath">Door texture path.</param>
	/// <param name="DoorKey">Door key.</param>
	/// <param name="Locked">Locked.</param>
	/// <param name="isOpen">Is open.</param>
		public static void CreateDoor(GameObject myObj, ObjectInteraction objInt)
		{
			int doorIndex=0;
				int textureIndex=0;
				//string DoorTexturePath="";
			//Try and match up the door item id with a texture.
				NavMeshObstacle navobs= myObj.AddComponent<NavMeshObstacle>();
				navobs.center= new Vector3(-.4f, 0f, 0.5f);
				navobs.size= new Vector3(0.8f, 0.1f, 1.1f);


			switch  (objInt.GetItemType())
				{
				case ObjectInteraction.HIDDENDOOR:
						{
							if (objInt.ObjectTileX <= TileMap.TileMapSizeX)
							{
								textureIndex = 	CurrentTileMap().Tiles[objInt.ObjectTileX, objInt.ObjectTileY].wallTexture;
							}
							else
							{
								textureIndex=0;
							}
							
							//textureIndex = CurrentTileMap().texture_map[CurrentTileMap().Tiles[objInt.tileX,objInt.tileY].wallTexture];
							//DoorTexturePath = _RES +"/materials/tmap/" + _RES + "_" + textureIndex.ToString("d3");
							break;	
						}
				case ObjectInteraction.DOOR:
				default:
					{
						if ((objInt.item_id>=320) && (objInt.item_id<=325))
						{//320>>58
								doorIndex=objInt.item_id-320;
						}
						else
						{//328>>58
								doorIndex= objInt.item_id-328;
						}
								if (_RES==GAME_UW2)
								{
										textureIndex= CurrentTileMap().texture_map[64+doorIndex];	
								}
								else
								{
										textureIndex= CurrentTileMap().texture_map[58+doorIndex];	
								}
						
						//DoorTexturePath =  _RES + "/textures/doors/doors_" +textureIndex.ToString("d2") +"_material";		
						break;
					}					
				}

			myObj.layer=LayerMask.NameToLayer("Doors");
			GameObject newObj;
			switch  (objInt.GetItemType())
                {
				case ObjectInteraction.PORTCULLIS:
						{
						newObj = (GameObject)GameObject.Instantiate((GameObject)Resources.Load("Models/Portcullis"));
						newObj.name=myObj.name + "_Model";
						newObj.transform.parent=myObj.transform;
						newObj.transform.position = myObj.transform.position;
						break;		
						}

				case ObjectInteraction.HIDDENDOOR:
						RenderHiddenDoor(myObj.GetComponent<DoorControl>(), textureIndex);
						break;	

				case ObjectInteraction.DOOR:
				//case ObjectInteraction.HIDDENDOOR:
				default:
						{

							GameObject myInstance = Resources.Load("Models/uw1_door") as GameObject;
							newObj = (GameObject)GameObject.Instantiate(myInstance);
							newObj.name=myObj.name + "_Model";
							newObj.transform.parent=myObj.transform;
							newObj.transform.position = myObj.transform.position;
							newObj.GetComponent<Renderer>().material=GameWorldController.instance.MaterialDoors[textureIndex];
							//
							newObj.GetComponent<MeshCollider>().enabled=false;
							MeshCollider mc = myObj.AddComponent<MeshCollider>();
							mc.isTrigger=false;
							mc.sharedMesh=newObj.GetComponent<MeshFilter>().sharedMesh;							
							break;	
							
						}
				}			
		}



		/// <summary>
		/// Renders a hidden door that matches the texture of the wall around it
		/// </summary>
		/// <param name="dc">Dc.</param>
		/// <param name="textureIndex">Texture index.</param>
		static void RenderHiddenDoor(DoorControl dc, int textureIndex)
		{

				//move the secret door to the bottom so I can match the uvs properly. I think
				dc.transform.position = new Vector3(dc.transform.position.x,0f,dc.transform.position.z);

				textureIndex=CurrentTileMap().texture_map[textureIndex];
				//Draw a cube with no slopes.
				int NumberOfVisibleFaces=6;
				//Allocate enough verticea and UVs for the faces
				Vector3[] verts =new Vector3[6*4];
				Vector2[] uvs =new Vector2[6*4];
				int tileX=dc.ObjectTileX;
				int tileY=dc.ObjectTileY;
				if (tileX==TileMap.ObjectStorageTile){return;}
				//int iDC_Floorheight=CurrentTileMap().Tiles[tileX,tileY].floorHeight;
				float Top =CurrentTileMap().Tiles[tileX,tileY].floorHeight+7; // 7f; //- iDC_Floorheight;
				float Bottom =CurrentTileMap().Tiles[tileX,tileY].floorHeight;//=-16f;//- iDC_Floorheight; //CurrentTileMap().Tiles[tileX,tileY].floorHeight;
				float floorHeight=(float)(Top*0.15f);
				float baseHeight=(float)(Bottom*0.15f);
				float dimX = 1;
				float dimY = 1;


				float doorwidth = 0.8f;
				float doorframewidth = 1.2f;
				float doorSideWidth = (doorframewidth-doorwidth)/2f;
				//float doorheight = 7f * 0.15f;
				//Uv ratios across the x axis of the door
				float uvXPos1 = 0f;
				float uvXPos2 = uvXPos1 + doorSideWidth/ 1.2f;
				float uvXPos3 =uvXPos2 + doorwidth/1.2f;
				//float uvXPos4 = 1f; // or 1.2f/1.2f

				//Now create the mesh
				GameObject Tile = new GameObject(dc.name + "_Model");

				Tile.layer=LayerMask.NameToLayer("MapMesh");

				Tile.transform.parent=dc.transform;
				Tile.transform.localPosition = Vector3.zero;

				Tile.transform.localRotation=Quaternion.Euler(0f,0f,0f);
				MeshFilter mf = Tile.AddComponent<MeshFilter>();
				MeshRenderer mr =Tile.AddComponent<MeshRenderer>();
				//MeshCollider mc = Tile.AddComponent<MeshCollider>();
				//mc.sharedMesh=null;
				Mesh mesh = new Mesh();
				mesh.subMeshCount=NumberOfVisibleFaces;//Should be no of visible faces

				Material[] MatsToUse=new Material[NumberOfVisibleFaces];
				//Now allocate the visible faces to triangles.
				int FaceCounter=0;//Tracks which number face we are now on.
				float PolySize= Top-Bottom;
				float uv0= (float)(Bottom*0.125f);
				float uv1=(PolySize / 8.0f) + (uv0);
				for (int i=0;i<6;i++)
				{
					switch(i)
					{
					case vTOP:
							{
							//Set the verts	
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 

							verts[0+ (4*FaceCounter)]=  new Vector3(0.0f, -0.02f,floorHeight);
							verts[1+ (4*FaceCounter)]=  new Vector3(0.0f, 0.02f, floorHeight);
							verts[2+ (4*FaceCounter)]=  new Vector3(-doorwidth,0.02f, floorHeight);
							verts[3+ (4*FaceCounter)]=  new Vector3(-doorwidth,-0.02f, floorHeight);

							//Allocate UVs
							uvs[0+ (4*FaceCounter)]=new Vector2(0.0f,1.0f*dimY);
							uvs[1 +(4*FaceCounter)]=new Vector2(0.0f,0.0f);
							uvs[2+ (4*FaceCounter)]=new Vector2(1.0f*dimX,0.0f);
							uvs[3+ (4*FaceCounter)]=new Vector2(1.0f*dimX,1.0f*dimY);

							break;
							}

					case vNORTH:
							{
							//north wall vertices
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 
										verts[0+ (4*FaceCounter)]=  new Vector3(-doorwidth,0.02f, baseHeight);
										verts[1+ (4*FaceCounter)]=  new Vector3(-doorwidth,0.02f, floorHeight);
										verts[2+ (4*FaceCounter)]=  new Vector3(0f,0.02f, floorHeight);
										verts[3+ (4*FaceCounter)]=  new Vector3(0f,0.02f, baseHeight);

							uvs[0+ (4*FaceCounter)]=new Vector2(uvXPos2,uv0);
							uvs[1 +(4*FaceCounter)]=new Vector2(uvXPos2,uv1);
							uvs[2+ (4*FaceCounter)]=new Vector2(uvXPos3,uv1);
							uvs[3+ (4*FaceCounter)]=new Vector2(uvXPos3,uv0);


							break;
							}

					case vWEST:
							{
							//west wall vertices
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 
										verts[0+ (4*FaceCounter)]=  new Vector3(0f,+0.02f, baseHeight);
										verts[1+ (4*FaceCounter)]=  new Vector3(0f,+0.02f, floorHeight);
										verts[2+ (4*FaceCounter)]=  new Vector3(0f,-0.02f, floorHeight);
										verts[3+ (4*FaceCounter)]=  new Vector3(0f,-0.02f, baseHeight);
										uvs[0+ (4*FaceCounter)]=new Vector2(uvXPos2,uv0);
										uvs[1 +(4*FaceCounter)]=new Vector2(uvXPos2,uv1);
										uvs[2+ (4*FaceCounter)]=new Vector2(uvXPos3,uv1);
										uvs[3+ (4*FaceCounter)]=new Vector2(uvXPos3,uv0);

							break;
							}

					case vEAST:
							{
							//east wall vertices
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 
										verts[0+ (4*FaceCounter)]=  new Vector3(-doorwidth,-0.02f, baseHeight);
										verts[1+ (4*FaceCounter)]=  new Vector3(-doorwidth,-0.02f, floorHeight);
										verts[2+ (4*FaceCounter)]=  new Vector3(-doorwidth,+0.02f*dimY, floorHeight);
										verts[3+ (4*FaceCounter)]=  new Vector3(-doorwidth,+0.02f*dimY, baseHeight);
										uvs[0+ (4*FaceCounter)]=new Vector2(uvXPos2,uv0);
										uvs[1 +(4*FaceCounter)]=new Vector2(uvXPos2,uv1);
										uvs[2+ (4*FaceCounter)]=new Vector2(uvXPos3,uv1);
										uvs[3+ (4*FaceCounter)]=new Vector2(uvXPos3,uv0);

							break;
							}

					case vSOUTH:
							{
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 
							//south wall vertices
							verts[0+ (4*FaceCounter)]=  new Vector3(0f,-0.02f, baseHeight);
							verts[1+ (4*FaceCounter)]=  new Vector3(0f,-0.02f, floorHeight);
							verts[2+ (4*FaceCounter)]=  new Vector3(-doorwidth,-0.02f, floorHeight);
							verts[3+ (4*FaceCounter)]=  new Vector3(-doorwidth,-0.02f, baseHeight);
										uvs[0+ (4*FaceCounter)]=new Vector2(uvXPos2,uv0);
										uvs[1 +(4*FaceCounter)]=new Vector2(uvXPos2,uv1);
										uvs[2+ (4*FaceCounter)]=new Vector2(uvXPos3,uv1);
										uvs[3+ (4*FaceCounter)]=new Vector2(uvXPos3,uv0);

							break;
							}
					case vBOTTOM:
							{
							//bottom wall vertices
							MatsToUse[FaceCounter]=GameWorldController.instance.MaterialMasterList[textureIndex]; 
							verts[0+ (4*FaceCounter)]=  new Vector3(0f,1.2f*dimY, baseHeight);
							verts[1+ (4*FaceCounter)]=  new Vector3(0f,0f, baseHeight);
							verts[2+ (4*FaceCounter)]=  new Vector3(-1.2f*dimX,0f, baseHeight);
							verts[3+ (4*FaceCounter)]=  new Vector3(-1.2f*dimX,1.2f*dimY, baseHeight);
							//Change default UVs
							uvs[0+ (4*FaceCounter)]=new Vector2(0.0f,0.0f);
							uvs[1 +(4*FaceCounter)]=new Vector2(0.0f,1.0f*dimY);
							uvs[2+ (4*FaceCounter)]=new Vector2(dimX,1.0f*dimY);
							uvs[3+ (4*FaceCounter)]=new Vector2(dimX,0.0f);
							break;
							}

					}
					FaceCounter++;
				}

				//Apply the uvs and create my tris
				mesh.vertices = verts;
				mesh.uv = uvs;
				FaceCounter=0;
				int [] tris = new int[6];
				for (int i=0;i<6;i++)
				{
						tris[0]=0+(4*FaceCounter);
						tris[1]=1+(4*FaceCounter);
						tris[2]=2+(4*FaceCounter);
						tris[3]=0+(4*FaceCounter);
						tris[4]=2+(4*FaceCounter);
						tris[5]=3+(4*FaceCounter);
						mesh.SetTriangles(tris,FaceCounter);
						FaceCounter++;
				}

				mr.materials= MatsToUse;//mats;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				mf.mesh=mesh;
				//mc.sharedMesh=mesh;

				//MeshCollider nmc =dc.gameObject.AddComponent<MeshCollider>();
				//nmc.isTrigger=false;
				//nmc.sharedMesh=mesh;		
				float ResolutionZ = 128.0f;
				float ceil = CurrentTileMap().CEILING_HEIGHT;
				int newZpos=CurrentTileMap().Tiles[dc.ObjectTileX, dc.ObjectTileY].floorHeight * 4;
				float BrushZ = 15f;
				float offZ = ((newZpos / ResolutionZ) * (ceil)) * BrushZ;
				offZ= offZ/100.0f;
				BoxCollider bx= dc.GetComponent<BoxCollider>();
				bx.center= new Vector3(-0.4f,0,0.525f+ offZ);
				bx.size=new Vector3(0.8f, 0.04f, 1.05f);

		}

		public override string ContextMenuDesc (int item_id)
		{
				switch  (objInt().GetItemType())
				{
					case ObjectInteraction.HIDDENDOOR:
						if (state()==false)//Closed and probably hidden
						{
								return "";		
						}
						else
						{
							return base.ContextMenuDesc (item_id);
						}
				default:
					return base.ContextMenuDesc (item_id);
				}
		}

	public override string ContextMenuUsedDesc ()
	{
		switch  (objInt().GetItemType())
		{
		case ObjectInteraction.HIDDENDOOR:
			if (state()==false)//Closed and probably hidden
			{
				return "";		
			}
			else
			{
				return base.ContextMenuUsedDesc ();	
			}
		default:
				return base.ContextMenuUsedDesc ();	
		}		
	}


    /// <summary>
    /// inds a door in the tile pointed to by the two coordinates.
    /// </summary>
    /// <returns>The door.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public static GameObject findDoor(int x, int y)
    {
        return GameObject.Find("door_" + x.ToString("D3") + "_" + y.ToString("D3"));
    }
}