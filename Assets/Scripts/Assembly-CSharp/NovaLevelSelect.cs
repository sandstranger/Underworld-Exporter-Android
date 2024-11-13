using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NovaLevelSelect : GuiBase
{
	public Dropdown Select;

	public static string MapSelected;

	private string[] pFiles;

	public override void Start()
	{
		base.Start();
		if (File.Exists(Application.dataPath + "//..//tnova_path.txt"))
		{
			string path = Application.dataPath + "//..//tnova_path.txt";
			StreamReader streamReader = new StreamReader(path, Encoding.Default);
			string text = streamReader.ReadLine().TrimEnd() + "Maps";
			pFiles = Directory.GetFiles(text);
			for (int i = 0; i <= pFiles.GetUpperBound(0); i++)
			{
				Select.options.Add(new Dropdown.OptionData(pFiles[i].Replace(text + "\\", "")));
			}
		}
	}

	public void SelectOption()
	{
		MapSelected = pFiles[Select.value];
	}
}
