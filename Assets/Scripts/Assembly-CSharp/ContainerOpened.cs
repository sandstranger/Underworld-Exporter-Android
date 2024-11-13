using UnityEngine;
using UnityEngine.UI;

public class ContainerOpened : GuiBase_Draggable
{
	public GameObject BackpackBg;

	public GameObject InvUp;

	public GameObject InvDown;

	public override void Start()
	{
		base.Start();
		GRLoader gRLoader = new GRLoader(18);
		BackpackBg.GetComponent<RawImage>().texture = gRLoader.LoadImageAt(6);
	}

	private void CloseChildContainer(Container ClosingParent)
	{
		ClosingParent.isOpenOnPanel = false;
		for (int i = 0; i <= ClosingParent.MaxCapacity(); i++)
		{
			if (!(ClosingParent.items[i] != null))
			{
				continue;
			}
			ObjectInteraction objectInteraction = ClosingParent.items[i];
			if (objectInteraction != null)
			{
				Container component = objectInteraction.GetComponent<Container>();
				if (component != null)
				{
					CloseChildContainer(component);
				}
			}
		}
	}

	public void OnClick()
	{
		if (Dragging || UWCharacter.Instance.isRoaming || Quest.instance.InDreamWorld)
		{
			return;
		}
		if (UWCharacter.Instance.playerInventory.currentContainer == UWCharacter.Instance.name)
		{
			UWCharacter.Instance.playerInventory.ContainerOffset = 0;
			BackpackBg.SetActive(false);
		}
		else if (UWEBase.CurrentObjectInHand == null)
		{
			ScrollButtonInventory.ScrollValue = 0;
			UWCharacter.Instance.playerInventory.ContainerOffset = 0;
			Container currentContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
			UWCharacter.Instance.playerInventory.currentContainer = currentContainer.ContainerParent;
			currentContainer.isOpenOnPanel = false;
			CloseChildContainer(currentContainer);
			Container currentContainer2 = UWCharacter.Instance.playerInventory.GetCurrentContainer();
			if (UWCharacter.Instance.playerInventory.currentContainer == UWCharacter.Instance.name)
			{
				GetComponent<RawImage>().texture = UWCharacter.Instance.playerInventory.Blank;
				BackpackBg.SetActive(false);
				if (currentContainer2.CountItems() >= 8 && currentContainer2 != UWCharacter.Instance.playerInventory.playerContainer)
				{
					InvUp.SetActive(true);
					InvDown.SetActive(true);
				}
				else
				{
					InvUp.SetActive(false);
					InvDown.SetActive(false);
				}
			}
			else
			{
				GetComponent<RawImage>().texture = currentContainer2.transform.GetComponent<ObjectInteraction>().GetEquipDisplay().texture;
				BackpackBg.SetActive(true);
				if (currentContainer2.CountItems() >= 8 && currentContainer2 != UWCharacter.Instance.playerInventory.playerContainer)
				{
					InvUp.SetActive(true);
					InvDown.SetActive(true);
				}
				else
				{
					InvUp.SetActive(false);
					InvDown.SetActive(false);
				}
			}
			for (short num = 0; num < 8; num++)
			{
				UWCharacter.Instance.playerInventory.SetObjectAtSlot((short)(num + 11), currentContainer2.GetItemAt(num));
			}
		}
		else
		{
			if (Character.InteractionMode != 2)
			{
				return;
			}
			Container component = GameObject.Find(UWCharacter.Instance.playerInventory.currentContainer).GetComponent<Container>();
			Container component2 = GameObject.Find(component.ContainerParent).GetComponent<Container>();
			if (!Container.TestContainerRules(component2, 11, false))
			{
				return;
			}
			if (!UWEBase.CurrentObjectInHand.IsStackable())
			{
				if (component2.AddItemToContainer(UWEBase.CurrentObjectInHand))
				{
					UWEBase.CurrentObjectInHand = null;
				}
			}
			else if (component2.AddItemMergedItemToContainer(UWEBase.CurrentObjectInHand))
			{
				UWEBase.CurrentObjectInHand = null;
			}
		}
	}
}
