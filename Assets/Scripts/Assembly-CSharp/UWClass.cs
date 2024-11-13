using System.IO;

public class UWClass
{
	public static char sep;

	public const string GAME_UWDEMO = "UW0";

	public const string GAME_UW1 = "UW1";

	public const string GAME_UW2 = "UW2";

	public const string GAME_SHOCK = "SHOCK";

	public const string GAME_TNOVA = "TNOVA";

	public static string _RES = "UW1";

	public static ObjectLoader CurrentObjectList()
	{
		return UWEBase.CurrentObjectList();
	}

	public static TileMap CurrentTileMap()
	{
		return UWEBase.CurrentTileMap();
	}

	public static AutoMap CurrentAutoMap()
	{
		return UWEBase.CurrentAutoMap();
	}

	public static string CleanPath(string PathIn)
	{
		return PathIn.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
	}
}
