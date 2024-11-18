﻿using UnityEngine;
using System.Collections;

public class an_oscillator_trap : trap_base
{
    //Theory on this. Need to prove with some hex editing.

    //X is the current direction.
    //owner is the max floor height to reach.
    //Y or zpos(likely) is possibly the min height. 
    GameObject platformTile;

    /// <summary>
    /// The position of the center of the tile.
    /// </summary>
    public Vector3 TileVector;

    /// <summary>
    /// The tile X that contains this trigger.
    /// </summary>	
    public int TileXToWatch;

    /// <summary>
    /// The tile Y that contains this trigger.
    /// </summary>
    public int TileYToWatch;

    /// <summary>
    /// The contact area that detects the presence of objects.
    /// </summary>
    public Vector3 ContactArea = new Vector3(0.59f, 0.15f, 0.59f);

    /// <summary>
    /// The colliders that are in contact with the trigger.
    /// </summary>
    public Collider[] colliders;

    protected override void Start()
    {
        base.Start();

    }

    public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
    {
        TileXToWatch = triggerX;
        TileYToWatch = triggerY;
        TileVector = CurrentTileMap().getTileVector(TileXToWatch, TileYToWatch);

        if (platformTile == null)
        {
            platformTile = GameWorldController.FindTile(triggerX, triggerY, TileMap.SURFACE_FLOOR);
        }
        if (platformTile == null)
        {
            return;
        }
        if (CurrentTileMap().Tiles[triggerX, triggerY].floorHeight / 2 >= owner)
        {
            xpos = 0;
        }
        else if (CurrentTileMap().Tiles[triggerX, triggerY].floorHeight / 2 <= quality)
        {
            xpos = 1;
        }

        if (xpos == 1)
        {//moving up
            MoveTileUp(triggerX, triggerY);
        }
        else
        {//moving down
            CurrentTileMap().Tiles[triggerX, triggerY].floorHeight -= 2;
            StartCoroutine(MoveTile(platformTile.transform, new Vector3(0f, -0.3f, 0f), 0.1f));
        }
    }

    private void MoveTileUp(int triggerX, int triggerY)
    {
        CurrentTileMap().Tiles[triggerX, triggerY].floorHeight += 2;
        StartCoroutine(MoveTile(platformTile.transform, new Vector3(0f, 0.3f, 0f), 0.1f));
        if (CurrentTileMap().Tiles[triggerX, triggerY].floorHeight >= 30)
        {
            if (
                    (TileMap.visitTileX == triggerX)
                    &&
                    (TileMap.visitTileY == triggerY)
            )
            {//Kill the player if they are in the tile  (ouch my head)
                UWCharacter.Instance.CurVIT -= 1000;
            }
        }
    }

    protected IEnumerator MoveTile(Transform platform, Vector3 dist, float traveltime)
    {
        //Co-routine to move the tile to it's target position.
        float rate = 1.0f / traveltime;
        float index = 0.0f;
        Vector3 StartPos = platform.position;
        Vector3 EndPos = StartPos + dist;
        this.transform.position = StartPos;
        TileVector = CurrentTileMap().getTileVector(TileXToWatch, TileYToWatch);
        colliders = Physics.OverlapBox(TileVector, ContactArea);
        while (index < 1.0f)
        {
            Vector3 OldPosition = platform.position;
            platform.position = Vector3.Lerp(StartPos, EndPos, index);
            float height = TileVector.y;
            MoveObjectsInContact(height);
            this.transform.position = platform.position;
            index += rate * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        platform.position = EndPos;
        this.transform.position = EndPos;
    }


    /// <summary>
    /// Moves the objects in contact.
    /// </summary>
    /// <param name="Height">Height.</param>
    /// Could be better.
    public void MoveObjectsInContact(float Height)
    {
        for (int i = 0; i <= colliders.GetUpperBound(0); i++)
        {
            if (colliders[i].gameObject.GetComponent<ObjectInteraction>() != null)
            {
                if (colliders[i].gameObject.GetComponent<ObjectInteraction>().isMoveable())
                {
                    Vector3 objPosition = colliders[i].gameObject.transform.position;
                    UnFreezeMovement(colliders[i].gameObject);
                    colliders[i].gameObject.transform.position = new Vector3(objPosition.x, Height, objPosition.z);
                }
            }
        }
    }

    public override bool WillFireRepeatedly()
    {
        return true;
    }
}
