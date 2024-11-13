using UnityEngine;

public class TileInfo : Loader
{
	public TileMap map;

	public short tileType;

	public short floorHeight;

	public short ceilingHeight;

	private short _floorTexture;

	public short wallTexture;

	public int indexObjectList;

	public short doorBit;

	public bool isDoor;

	public bool Render = true;

	public short DimX = 1;

	public short DimY = 1;

	public bool Grouped = false;

	public bool[] VisibleFaces = new bool[6] { true, true, true, true, true, true };

	public short North;

	public short South;

	public short East;

	public short West;

	public bool hasBridge;

	public bool isNothing;

	public short roomRegion;

	public short tileX;

	public short tileY;

	public short flags;

	public short noMagic;

	public short shockSlopeFlag;

	public short shockCeilingTexture;

	public short shockSteep;

	public short UseAdjacentTextures;

	public short shockTextureOffset;

	public short shockNorthOffset;

	public short shockSouthOffset;

	public short shockEastOffset;

	public short shockWestOffset;

	public short shockShadeUpper;

	public short shockShadeLower;

	public short shockNorthCeilHeight;

	public short shockSouthCeilHeight;

	public short shockEastCeilHeight;

	public short shockWestCeilHeight;

	public short shockFloorOrientation;

	public short shockCeilOrientation;

	public bool TerrainChange;

	public int[] SHOCKSTATE = new int[4];

	public bool NeedsReRender = false;

	public short PressureTriggerIndex = 0;

	private int _terrain;

	public short floorTexture
	{
		get
		{
			return _floorTexture;
		}
		set
		{
			_floorTexture = value;
			UpdateTerrain();
		}
	}

	public bool isLand
	{
		get
		{
			return !isWater && !isLava && !isNothing;
		}
	}

	public bool isWater
	{
		get
		{
			return TileMap.isTerrainWater(terrain);
		}
	}

	public bool isIce
	{
		get
		{
			return TileMap.isTerrainIce(terrain);
		}
	}

	public bool isLava
	{
		get
		{
			return TileMap.isTerrainLava(terrain);
		}
	}

	public int terrain
	{
		get
		{
			return _terrain;
		}
	}

	public TileInfo()
	{
	}

	public TileInfo(TileMap tm, short X, short Y, int FirstTileInt, int SecondTileInt, short CeilingTexture)
	{
		short tile = getTile(FirstTileInt);
		short newfloorHeight = (short)(getHeight(FirstTileInt) * 2);
		short newceilingHeight = 0;
		short newFlags = (short)((FirstTileInt >> 7) & 3);
		short newnoMagic = (short)((FirstTileInt >> 14) & 1);
		short newdoorBit = (short)((FirstTileInt >> 15) & 1);
		short @object = getObject(SecondTileInt);
		short floorTex = getFloorTex(FirstTileInt);
		short wallTex = getWallTex(SecondTileInt);
		InitTileInfo(tm, X, Y, tile, newfloorHeight, newceilingHeight, floorTex, wallTex, CeilingTexture, newFlags, newnoMagic, newdoorBit, @object);
	}

	public TileInfo(TileMap tm, short X, short Y, short newtileType, short newfloorHeight, short newceilingHeight, short newfloorTexture, short newwallTexture, short newceilTexture, short newFlags, short newnoMagic, short newdoorBit, int newindexObjectList)
	{
		InitTileInfo(tm, X, Y, newtileType, newfloorHeight, newceilingHeight, newfloorTexture, newwallTexture, newceilTexture, newFlags, newnoMagic, newdoorBit, newindexObjectList);
	}

	private void InitTileInfo(TileMap tm, short X, short Y, short newtiletype, short newfloorHeight, short newceilingHeight, short newfloorTexture, short newwallTexture, short newceilTexture, short newFlags, short newnoMagic, short newdoorBit, int newindexObjectList)
	{
		map = tm;
		tileType = newtiletype;
		tileX = X;
		tileY = Y;
		floorHeight = newfloorHeight;
		ceilingHeight = newceilingHeight;
		floorTexture = newfloorTexture;
		wallTexture = newwallTexture;
		shockCeilingTexture = newceilTexture;
		flags = newFlags;
		noMagic = newnoMagic;
		doorBit = newdoorBit;
		indexObjectList = newindexObjectList;
		Grouped = false;
		if (floorTexture < 0)
		{
			floorTexture = 0;
		}
		if (floorTexture >= 262)
		{
			floorTexture = 0;
		}
		if (wallTexture >= 256)
		{
			wallTexture = 0;
		}
		switch (UWClass._RES)
		{
		case "UW0":
		case "UW1":
			_terrain = GameWorldController.instance.terrainData.Terrain[46 + map.texture_map[floorTexture + 48]];
			break;
		case "UW2":
			_terrain = GameWorldController.instance.terrainData.Terrain[map.texture_map[floorTexture]];
			break;
		default:
			_terrain = 0;
			break;
		}
		if (UWClass._RES == "UW0" && map.texture_map[floorTexture + 48] == 56)
		{
			_terrain = 16;
		}
		shockSlopeFlag = 2;
		if (tileType >= 2)
		{
			shockSteep = 2;
		}
		North = wallTexture;
		South = wallTexture;
		East = wallTexture;
		West = wallTexture;
		isNothing = TileMap.isTextureNothing(map.texture_map[floorTexture]);
		if (isNothing)
		{
			Debug.Log("instance of isnothing Why the hell does this exist?" + X + "," + Y);
		}
	}

	private short getTile(int tileData)
	{
		return (short)(tileData & 0xF);
	}

	private short getHeight(int tileData)
	{
		return (short)((tileData & 0xF0) >> 4);
	}

	private short getFloorTex(long tileData)
	{
		return (short)((tileData >> 10) & 0xF);
	}

	private short getWallTex(long tileData)
	{
		return (short)(tileData & 0x3F);
	}

	private short getObject(long tileData)
	{
		return (short)(tileData >> 6);
	}

	public void TileNeedsUpdate()
	{
		NeedsReRender = true;
		GameWorldController.WorldReRenderPending = true;
	}

	public int VisibleWallTexture(int direction)
	{
		if (!VisibleFaces[direction])
		{
			return -1;
		}
		switch (direction)
		{
		case 5:
			return South;
		case 4:
			return North;
		case 1:
			return East;
		case 3:
			return West;
		default:
			return floorTexture;
		}
	}

	private void UpdateTerrain()
	{
		switch (UWClass._RES)
		{
		case "UW0":
		case "UW1":
			_terrain = GameWorldController.instance.terrainData.Terrain[46 + map.texture_map[_floorTexture + 48]];
			break;
		case "UW2":
			_terrain = GameWorldController.instance.terrainData.Terrain[map.texture_map[_floorTexture]];
			break;
		default:
			_terrain = 0;
			break;
		}
	}
}
