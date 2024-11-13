using UnityEngine;

public class UWPalette : UWEBase
{
	private float[] red = new float[256];

	private float[] blue = new float[256];

	private float[] green = new float[256];

	private float[] Defred = new float[256];

	private float[] Defblue = new float[256];

	private float[] Defgreen = new float[256];

	private void Awake()
	{
		SetPal(0, 0f, 0f, 4f);
		SetPal(1, 0f, 0f, 0f);
		SetPal(2, 252f, 148f, 252f);
		SetPal(3, 0f, 0f, 0f);
		SetPal(4, 4f, 0f, 4f);
		SetPal(5, 252f, 184f, 0f);
		SetPal(6, 252f, 108f, 0f);
		SetPal(7, 180f, 24f, 24f);
		SetPal(8, 124f, 184f, 252f);
		SetPal(9, 8f, 8f, 8f);
		SetPal(10, 124f, 192f, 120f);
		SetPal(11, 252f, 252f, 252f);
		SetPal(12, 96f, 236f, 252f);
		SetPal(13, 12f, 252f, 160f);
		SetPal(14, 252f, 124f, 108f);
		SetPal(15, 16f, 16f, 16f);
		SetPal(16, 240f, 136f, 4f);
		SetPal(17, 216f, 92f, 0f);
		SetPal(18, 200f, 56f, 0f);
		SetPal(19, 184f, 24f, 0f);
		SetPal(20, 168f, 0f, 0f);
		SetPal(21, 252f, 212f, 0f);
		SetPal(22, 244f, 148f, 0f);
		SetPal(23, 236f, 116f, 4f);
		SetPal(24, 108f, 128f, 132f);
		SetPal(25, 100f, 120f, 124f);
		SetPal(26, 92f, 108f, 112f);
		SetPal(27, 84f, 100f, 104f);
		SetPal(28, 72f, 92f, 96f);
		SetPal(29, 64f, 84f, 88f);
		SetPal(30, 56f, 72f, 76f);
		SetPal(31, 48f, 64f, 68f);
		SetPal(32, 184f, 120f, 92f);
		SetPal(33, 176f, 104f, 76f);
		SetPal(34, 168f, 92f, 60f);
		SetPal(35, 164f, 84f, 52f);
		SetPal(36, 148f, 76f, 40f);
		SetPal(37, 140f, 72f, 36f);
		SetPal(38, 132f, 68f, 32f);
		SetPal(39, 124f, 64f, 28f);
		SetPal(40, 176f, 136f, 104f);
		SetPal(41, 156f, 120f, 92f);
		SetPal(42, 140f, 104f, 84f);
		SetPal(43, 124f, 88f, 68f);
		SetPal(44, 96f, 68f, 52f);
		SetPal(45, 76f, 56f, 40f);
		SetPal(46, 60f, 40f, 32f);
		SetPal(47, 48f, 32f, 24f);
		SetPal(48, 20f, 40f, 112f);
		SetPal(49, 16f, 24f, 88f);
		SetPal(50, 16f, 36f, 96f);
		SetPal(51, 24f, 56f, 132f);
		SetPal(52, 20f, 32f, 104f);
		SetPal(53, 16f, 20f, 76f);
		SetPal(54, 16f, 28f, 88f);
		SetPal(55, 20f, 48f, 124f);
		SetPal(56, 16f, 24f, 96f);
		SetPal(57, 16f, 16f, 68f);
		SetPal(58, 16f, 20f, 76f);
		SetPal(59, 20f, 36f, 112f);
		SetPal(60, 16f, 20f, 88f);
		SetPal(61, 16f, 16f, 60f);
		SetPal(62, 16f, 16f, 68f);
		SetPal(63, 20f, 32f, 104f);
		SetPal(64, 92f, 228f, 252f);
		SetPal(65, 84f, 208f, 240f);
		SetPal(66, 76f, 196f, 220f);
		SetPal(67, 68f, 180f, 204f);
		SetPal(68, 60f, 164f, 184f);
		SetPal(69, 56f, 148f, 168f);
		SetPal(70, 52f, 136f, 148f);
		SetPal(71, 48f, 124f, 136f);
		SetPal(72, 40f, 108f, 124f);
		SetPal(73, 36f, 100f, 108f);
		SetPal(74, 32f, 88f, 100f);
		SetPal(75, 28f, 72f, 88f);
		SetPal(76, 24f, 64f, 72f);
		SetPal(77, 20f, 56f, 60f);
		SetPal(78, 16f, 40f, 48f);
		SetPal(79, 16f, 32f, 36f);
		SetPal(80, 208f, 252f, 12f);
		SetPal(81, 196f, 240f, 12f);
		SetPal(82, 180f, 220f, 12f);
		SetPal(83, 172f, 204f, 12f);
		SetPal(84, 160f, 184f, 12f);
		SetPal(85, 144f, 168f, 12f);
		SetPal(86, 132f, 148f, 12f);
		SetPal(87, 124f, 136f, 12f);
		SetPal(88, 112f, 124f, 12f);
		SetPal(89, 100f, 108f, 12f);
		SetPal(90, 92f, 100f, 12f);
		SetPal(91, 84f, 88f, 12f);
		SetPal(92, 68f, 72f, 12f);
		SetPal(93, 60f, 60f, 12f);
		SetPal(94, 48f, 48f, 12f);
		SetPal(95, 36f, 36f, 12f);
		SetPal(96, 252f, 252f, 252f);
		SetPal(97, 232f, 232f, 240f);
		SetPal(98, 204f, 204f, 220f);
		SetPal(99, 184f, 184f, 204f);
		SetPal(100, 164f, 164f, 184f);
		SetPal(101, 140f, 140f, 168f);
		SetPal(102, 124f, 124f, 148f);
		SetPal(103, 108f, 108f, 136f);
		SetPal(104, 100f, 100f, 124f);
		SetPal(105, 88f, 88f, 108f);
		SetPal(106, 76f, 76f, 100f);
		SetPal(107, 64f, 64f, 88f);
		SetPal(108, 56f, 56f, 72f);
		SetPal(109, 48f, 48f, 60f);
		SetPal(110, 36f, 36f, 48f);
		SetPal(111, 28f, 28f, 36f);
		SetPal(112, 252f, 228f, 136f);
		SetPal(113, 240f, 212f, 132f);
		SetPal(114, 220f, 196f, 124f);
		SetPal(115, 204f, 180f, 120f);
		SetPal(116, 184f, 168f, 108f);
		SetPal(117, 168f, 156f, 100f);
		SetPal(118, 156f, 140f, 96f);
		SetPal(119, 140f, 128f, 84f);
		SetPal(120, 128f, 112f, 72f);
		SetPal(121, 112f, 100f, 64f);
		SetPal(122, 100f, 92f, 56f);
		SetPal(123, 88f, 76f, 48f);
		SetPal(124, 72f, 64f, 36f);
		SetPal(125, 60f, 56f, 32f);
		SetPal(126, 52f, 40f, 24f);
		SetPal(127, 36f, 32f, 20f);
		SetPal(128, 252f, 156f, 136f);
		SetPal(129, 240f, 140f, 124f);
		SetPal(130, 220f, 128f, 108f);
		SetPal(131, 204f, 112f, 96f);
		SetPal(132, 184f, 104f, 84f);
		SetPal(133, 168f, 92f, 72f);
		SetPal(134, 156f, 84f, 64f);
		SetPal(135, 140f, 68f, 56f);
		SetPal(136, 128f, 60f, 52f);
		SetPal(137, 112f, 52f, 40f);
		SetPal(138, 100f, 48f, 36f);
		SetPal(139, 88f, 36f, 32f);
		SetPal(140, 72f, 28f, 28f);
		SetPal(141, 60f, 24f, 24f);
		SetPal(142, 52f, 20f, 20f);
		SetPal(143, 36f, 16f, 16f);
		SetPal(144, 252f, 252f, 252f);
		SetPal(145, 240f, 240f, 240f);
		SetPal(146, 220f, 220f, 220f);
		SetPal(147, 204f, 204f, 204f);
		SetPal(148, 184f, 184f, 184f);
		SetPal(149, 168f, 168f, 168f);
		SetPal(150, 148f, 148f, 148f);
		SetPal(151, 136f, 136f, 136f);
		SetPal(152, 124f, 124f, 124f);
		SetPal(153, 108f, 108f, 108f);
		SetPal(154, 100f, 100f, 100f);
		SetPal(155, 88f, 88f, 88f);
		SetPal(156, 72f, 72f, 72f);
		SetPal(157, 60f, 60f, 60f);
		SetPal(158, 48f, 48f, 48f);
		SetPal(159, 36f, 36f, 36f);
		SetPal(160, 252f, 196f, 16f);
		SetPal(161, 240f, 176f, 16f);
		SetPal(162, 220f, 160f, 12f);
		SetPal(163, 204f, 144f, 12f);
		SetPal(164, 184f, 128f, 12f);
		SetPal(165, 168f, 112f, 12f);
		SetPal(166, 156f, 100f, 12f);
		SetPal(167, 140f, 88f, 12f);
		SetPal(168, 128f, 76f, 12f);
		SetPal(169, 112f, 64f, 12f);
		SetPal(170, 100f, 56f, 12f);
		SetPal(171, 88f, 48f, 12f);
		SetPal(172, 72f, 36f, 12f);
		SetPal(173, 60f, 32f, 12f);
		SetPal(174, 52f, 24f, 12f);
		SetPal(175, 36f, 20f, 12f);
		SetPal(176, 252f, 92f, 92f);
		SetPal(177, 240f, 76f, 72f);
		SetPal(178, 228f, 64f, 64f);
		SetPal(179, 212f, 56f, 52f);
		SetPal(180, 196f, 48f, 40f);
		SetPal(181, 180f, 36f, 32f);
		SetPal(182, 168f, 32f, 24f);
		SetPal(183, 156f, 28f, 20f);
		SetPal(184, 140f, 24f, 16f);
		SetPal(185, 128f, 20f, 16f);
		SetPal(186, 112f, 16f, 16f);
		SetPal(187, 100f, 16f, 12f);
		SetPal(188, 88f, 12f, 12f);
		SetPal(189, 72f, 12f, 12f);
		SetPal(190, 64f, 12f, 12f);
		SetPal(191, 52f, 12f, 12f);
		SetPal(192, 136f, 192f, 252f);
		SetPal(193, 120f, 172f, 244f);
		SetPal(194, 104f, 160f, 232f);
		SetPal(195, 92f, 140f, 216f);
		SetPal(196, 84f, 128f, 204f);
		SetPal(197, 68f, 108f, 192f);
		SetPal(198, 60f, 100f, 176f);
		SetPal(199, 48f, 88f, 164f);
		SetPal(200, 36f, 72f, 148f);
		SetPal(201, 32f, 60f, 136f);
		SetPal(202, 24f, 52f, 124f);
		SetPal(203, 20f, 40f, 108f);
		SetPal(204, 16f, 32f, 96f);
		SetPal(205, 12f, 24f, 84f);
		SetPal(206, 12f, 20f, 72f);
		SetPal(207, 12f, 16f, 60f);
		SetPal(208, 164f, 252f, 160f);
		SetPal(209, 140f, 240f, 132f);
		SetPal(210, 120f, 220f, 104f);
		SetPal(211, 100f, 204f, 84f);
		SetPal(212, 88f, 184f, 64f);
		SetPal(213, 72f, 168f, 48f);
		SetPal(214, 64f, 148f, 32f);
		SetPal(215, 60f, 136f, 28f);
		SetPal(216, 56f, 124f, 20f);
		SetPal(217, 52f, 108f, 20f);
		SetPal(218, 48f, 100f, 16f);
		SetPal(219, 40f, 88f, 16f);
		SetPal(220, 36f, 72f, 12f);
		SetPal(221, 32f, 60f, 12f);
		SetPal(222, 28f, 48f, 12f);
		SetPal(223, 24f, 36f, 12f);
		SetPal(224, 252f, 172f, 120f);
		SetPal(225, 240f, 156f, 100f);
		SetPal(226, 220f, 140f, 84f);
		SetPal(227, 204f, 124f, 64f);
		SetPal(228, 184f, 108f, 52f);
		SetPal(229, 168f, 96f, 36f);
		SetPal(230, 148f, 88f, 28f);
		SetPal(231, 136f, 76f, 24f);
		SetPal(232, 124f, 68f, 20f);
		SetPal(233, 108f, 64f, 16f);
		SetPal(234, 100f, 56f, 16f);
		SetPal(235, 88f, 52f, 12f);
		SetPal(236, 72f, 40f, 12f);
		SetPal(237, 60f, 36f, 12f);
		SetPal(238, 48f, 28f, 12f);
		SetPal(239, 36f, 24f, 12f);
		SetPal(240, 232f, 232f, 232f);
		SetPal(241, 0f, 0f, 0f);
		SetPal(242, 24f, 4f, 4f);
		SetPal(243, 8f, 12f, 36f);
		SetPal(244, 4f, 8f, 24f);
		SetPal(245, 12f, 24f, 28f);
		SetPal(246, 24f, 24f, 8f);
		SetPal(247, 24f, 24f, 24f);
		SetPal(248, 36f, 48f, 52f);
		SetPal(249, 8f, 12f, 48f);
		SetPal(250, 0f, 0f, 0f);
		SetPal(251, 100f, 88f, 168f);
		SetPal(252, 112f, 28f, 64f);
		SetPal(253, 12f, 252f, 12f);
		SetPal(254, 0f, 0f, 0f);
		SetPal(255, 104f, 104f, 120f);
	}

	private void CyclePalette(int start, int end)
	{
		for (int num = end; num >= start; num--)
		{
			if (num == start)
			{
				UpdatePal(start, red[end], green[end], blue[end]);
			}
			else
			{
				UpdatePal(num, red[num - 1], red[num - 1], blue[num - 1]);
			}
		}
	}

	private void UpdateAnimation()
	{
		CyclePalette(16, 22);
		CyclePalette(48, 51);
	}

	public Texture2D ApplyPalette(Texture2D SrcImage)
	{
		Texture2D texture2D = new Texture2D(SrcImage.width, SrcImage.height, SrcImage.format, false);
		for (int i = 0; i <= SrcImage.width; i++)
		{
			for (int j = 0; j <= SrcImage.height; j++)
			{
				int index = (int)(SrcImage.GetPixel(i, j).r * 255f);
				texture2D.SetPixel(i, j, GetPal(index));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public Texture2D ApplyPaletteDefault(Texture2D SrcImage)
	{
		if (SrcImage == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(SrcImage.width, SrcImage.height, SrcImage.format, false);
		for (int i = 0; i <= SrcImage.width; i++)
		{
			for (int j = 0; j <= SrcImage.height; j++)
			{
				int index = (int)(SrcImage.GetPixel(i, j).r * 255f);
				texture2D.SetPixel(i, j, GetPalDef(index));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private void SetPal(int index, float Red, float Green, float Blue)
	{
		red[index] = Red / 255f;
		green[index] = Green / 255f;
		blue[index] = Blue / 255f;
		Defred[index] = Red / 255f;
		Defgreen[index] = Green / 255f;
		Defblue[index] = Blue / 255f;
	}

	private void UpdatePal(int index, float Red, float Green, float Blue)
	{
		red[index] = Red;
		green[index] = Green;
		blue[index] = Blue;
	}

	private Color GetPal(int index)
	{
		if (index != 0)
		{
			return new Color(red[index], green[index], blue[index]);
		}
		return Color.clear;
	}

	private Color GetPalDef(int index)
	{
		if (index != 0)
		{
			return new Color(Defred[index], Defgreen[index], Defblue[index]);
		}
		return Color.clear;
	}
}
