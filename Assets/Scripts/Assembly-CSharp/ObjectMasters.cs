using System.IO;
using System.Text;
using UnityEngine;

public class ObjectMasters
{
	public struct ObjectProperties
	{
		public int index;

		public int objClass;

		public int objSubClass;

		public int objSubClassIndex;

		public int type;

		public string desc;

		public int WorldIndex;

		public int isUseable;

		public int isMoveable;

		public int InventoryIndex;

		public int startFrame;

		public int useSprite;
	}

	public ObjectProperties[] objProp;

	public ObjectMasters()
	{
		objProp = new ObjectProperties[500];
		switch (UWClass._RES)
		{
		case "UW0":
		case "UW1":
#if !UNITY_EDITOR			
			Load(Application.dataPath + "//..//uw1_object_settings.txt");
#else
			Load(Path.Combine(Application.persistentDataPath, "uw1_object_settings.txt"));
#endif
			break;
		default:
			Load(Application.dataPath + "//..//" + UWEBase._RES.ToLower() + "_object_settings.txt");
			break;
		}
	}

	private bool Load(string fileName)
	{
		int num = 0;
		StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
		using (streamReader)
		{
			string text;
			do
			{
				text = streamReader.ReadLine();
				if (text != null)
				{
					string[] array = text.Split('\t');
					if (array.Length > 0)
					{
						objProp[num].index = int.Parse(array[0]);
						objProp[num].objClass = int.Parse(array[1]);
						objProp[num].objSubClass = int.Parse(array[2]);
						objProp[num].objSubClassIndex = int.Parse(array[3]);
						objProp[num].type = int.Parse(array[4]);
						objProp[num].desc = array[5];
						objProp[num].WorldIndex = int.Parse(array[6]);
						objProp[num].InventoryIndex = int.Parse(array[7]);
						objProp[num].isUseable = int.Parse(array[8]);
						objProp[num].isMoveable = int.Parse(array[9]);
						objProp[num].startFrame = int.Parse(array[10]);
						objProp[num].useSprite = int.Parse(array[11]);
						num++;
					}
				}
			}
			while (text != null);
			streamReader.Close();
			return true;
		}
	}
}
