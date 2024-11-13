using UnityEngine;

public class Action_Change_Type : MonoBehaviour
{
	public string ObjectToChange;

	public int ObjectClass;

	public int SubClass;

	public int SubClassIndex;

	public void PerformAction()
	{
		ChangeType();
	}

	private void ChangeType()
	{
		switch (ObjectClass)
		{
		case 7:
			ChangeTypeClass7();
			return;
		case 10:
			ChangeTypeClass10();
			return;
		}
		Debug.Log("Action Change Type: New Class to be implemented " + ObjectClass + "_" + SubClass);
	}

	private void ChangeTypeClass7()
	{
		int subClass = SubClass;
		if (subClass == 7)
		{
			ChangeType_7_7();
		}
		else
		{
			Debug.Log("Action Change Type: New Class to be implemented " + SubClass);
		}
	}

	private void ChangeTypeClass10()
	{
		int subClass = SubClass;
		if (subClass == 2)
		{
			ChangeType_10_2();
		}
	}

	private void ChangeType_10_2()
	{
		switch (SubClassIndex)
		{
		case 2:
			DeleteModel();
			CreateForceDoor();
			break;
		case 6:
			DeleteModel();
			CreateGreatLord();
			break;
		}
	}

	private void ChangeType_7_7()
	{
		switch (SubClassIndex)
		{
		case 7:
			DeleteModel();
			CreateForceBridge();
			break;
		case 8:
			DeleteModel();
			CreateElephantJorp();
			break;
		}
	}

	private void DeleteModel()
	{
		GameObject gameObject = GameObject.Find(ObjectToChange);
		if (!(gameObject == null))
		{
			GameObject gameObject2 = gameObject.transform.Find(gameObject.name + "_Model").gameObject;
			if (gameObject2 != null)
			{
				Object.Destroy(gameObject2);
			}
		}
	}

	private void CreateElephantJorp()
	{
		Debug.Log("Creating elephant");
		GameObject gameObject = GameObject.Find(ObjectToChange);
		if (!(gameObject == null))
		{
			loadGameObjectResourceAsModel(gameObject, "Models/elephant");
		}
	}

	private void CreateForceBridge()
	{
		Debug.Log("Creating ForceBridge");
		GameObject gameObject = GameObject.Find(ObjectToChange);
		if (!(gameObject == null))
		{
			loadGameObjectResourceAsModel(gameObject, "Models/force_bridge");
		}
	}

	private void CreateForceDoor()
	{
		Debug.Log("Creating ForceDoor");
		GameObject gameObject = GameObject.Find(ObjectToChange);
		if (!(gameObject == null))
		{
			loadGameObjectResourceAsModel(gameObject, "Models/force_door");
		}
	}

	private void CreateGreatLord()
	{
		Debug.Log("Creating GreatLord");
		GameObject gameObject = GameObject.Find(ObjectToChange);
		if (!(gameObject == null))
		{
			loadGameObjectResourceAsModel(gameObject, "Models/GreatLordSnaq");
		}
	}

	private void loadGameObjectResourceAsModel(GameObject myObj, string path)
	{
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original);
		gameObject.name = myObj.name + "_Model";
		gameObject.transform.parent = myObj.transform;
		gameObject.transform.position = myObj.transform.position;
		gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}
}
