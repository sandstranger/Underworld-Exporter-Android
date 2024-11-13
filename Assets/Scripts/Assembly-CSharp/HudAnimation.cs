public class HudAnimation : GuiBase
{
	public CutsAnimator anim;

	public const int NormalCullingMask = -33;

	public override void Start()
	{
		base.Start();
		anim.SetAnimation = "Anim_Base";
		anim.PrevAnimation = "Anim_Base";
	}
}
