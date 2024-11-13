public class XFerLoader : Loader
{
	public char[] auxPalVal;

	public XFerLoader()
	{
		Path = Loader.BasePath + "\\data\\xfer.dat";
		if (!DataLoader.ReadStreamFile(Path, out auxPalVal))
		{
		}
	}
}
