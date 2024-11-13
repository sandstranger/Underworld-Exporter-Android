using UnityEngine;

public class SpellEffectMazeNavigation : SpellEffect
{
	public override void ApplyEffect()
	{
		MeshRenderer[] componentsInChildren = GameWorldController.instance.LevelModel.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i <= componentsInChildren.GetUpperBound(0); i++)
		{
			if (componentsInChildren[i].sharedMaterial != null && componentsInChildren[i].sharedMaterial.name.Contains("_maze"))
			{
				componentsInChildren[i].material.SetColor("_Color", Color.yellow);
			}
		}
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		MeshRenderer[] componentsInChildren = GameWorldController.instance.LevelModel.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i <= componentsInChildren.GetUpperBound(0); i++)
		{
			if (componentsInChildren[i] != null && componentsInChildren[i].sharedMaterial != null && componentsInChildren[i].sharedMaterial.name.Contains("_maze"))
			{
				componentsInChildren[i].material.SetColor("_Color", Color.white);
			}
		}
		base.CancelEffect();
	}
}
