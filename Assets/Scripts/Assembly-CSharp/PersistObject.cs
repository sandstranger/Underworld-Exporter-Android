using UnityEngine;

public class PersistObject : UWEBase
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
