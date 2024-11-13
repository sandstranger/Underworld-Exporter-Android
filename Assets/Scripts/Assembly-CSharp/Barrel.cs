using System.Linq;
using UnityEngine;

public class Barrel : Container
{
	protected void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("MapMesh");
		Rigidbody component = GetComponent<Rigidbody>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
		BoxCollider component2 = GetComponent<BoxCollider>();
		if (component2 != null)
		{
			Object.DestroyImmediate(component2);
		}
		MeshFilter meshFilter = base.gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		Material[] array = new Material[NoOfMeshes()];
		Mesh mesh = new Mesh();
		mesh.subMeshCount = NoOfMeshes();
		mesh.vertices = ModelVertices();
		Vector2[] array2 = ModelUVs(mesh.vertices);
		for (int i = 0; i < NoOfMeshes(); i++)
		{
			mesh.SetTriangles(ModelTriangles(i), i);
			array[i] = ModelMaterials(i);
			array[i].SetColor("_Color", ModelColour(i));
		}
		if (array2.GetUpperBound(0) > 0)
		{
			mesh.uv = array2;
		}
		meshRenderer.materials = array;
		for (int j = 0; j < NoOfMeshes(); j++)
		{
			meshRenderer.materials[j].SetColor("_Color", ModelColour(j));
		}
		meshFilter.mesh = mesh;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;
	}

	protected virtual int NoOfMeshes()
	{
		return 1;
	}

	protected virtual int[] ModelTriangles(int meshNo)
	{
		return new int[180]
		{
			2, 1, 0, 3, 2, 0, 4, 2, 3, 5,
			4, 3, 6, 4, 5, 7, 6, 5, 1, 9,
			8, 0, 1, 8, 11, 10, 1, 2, 11, 1,
			12, 11, 2, 4, 12, 2, 13, 12, 4, 6,
			13, 4, 10, 14, 9, 1, 10, 9, 16, 15,
			10, 11, 16, 10, 17, 16, 11, 12, 17, 11,
			18, 17, 12, 13, 18, 12, 19, 14, 10, 15,
			19, 10, 14, 19, 21, 20, 14, 21, 21, 22,
			23, 20, 21, 23, 14, 20, 24, 9, 14, 24,
			20, 23, 25, 24, 20, 25, 9, 24, 26, 8,
			9, 26, 24, 25, 27, 26, 24, 27, 29, 18,
			13, 28, 29, 13, 22, 29, 28, 23, 22, 28,
			28, 13, 6, 30, 28, 6, 23, 28, 30, 25,
			23, 30, 30, 6, 7, 31, 30, 7, 25, 30,
			31, 27, 25, 31, 22, 21, 19, 29, 22, 19,
			18, 29, 19, 17, 18, 19, 16, 17, 19, 15,
			16, 19, 5, 3, 0, 7, 5, 0, 31, 7,
			0, 27, 31, 0, 26, 27, 0, 8, 26, 0
		}.Reverse().ToArray();
	}

	protected virtual Vector3[] ModelVertices()
	{
		return new Vector3[32]
		{
			new Vector3(-5f / 128f, 0f, -3f / 32f),
			new Vector3(-0.05859375f, 15f / 128f, -0.1210938f),
			new Vector3(3f / 64f, 15f / 128f, -0.1289063f),
			new Vector3(0.03515625f, 0f, -0.09765625f),
			new Vector3(0.125f, 15f / 128f, -0.05859375f),
			new Vector3(3f / 32f, 0f, -5f / 128f),
			new Vector3(0.1289063f, 15f / 128f, 3f / 64f),
			new Vector3(13f / 128f, 0f, 0.03515625f),
			new Vector3(-0.09765625f, 0f, -0.03515625f),
			new Vector3(-0.1289063f, 15f / 128f, -3f / 64f),
			new Vector3(-0.05859375f, 15f / 64f, -0.1210938f),
			new Vector3(3f / 64f, 15f / 64f, -0.1289063f),
			new Vector3(0.125f, 15f / 64f, -0.05859375f),
			new Vector3(0.1289063f, 15f / 64f, 3f / 64f),
			new Vector3(-0.1289063f, 15f / 64f, -3f / 64f),
			new Vector3(-5f / 128f, 45f / 128f, -3f / 32f),
			new Vector3(0.03515625f, 45f / 128f, -0.09765625f),
			new Vector3(3f / 32f, 45f / 128f, -5f / 128f),
			new Vector3(13f / 128f, 45f / 128f, 0.03515625f),
			new Vector3(-0.09765625f, 45f / 128f, -0.03515625f),
			new Vector3(-0.1210938f, 15f / 64f, 0.05859375f),
			new Vector3(-3f / 32f, 45f / 128f, 0.04296875f),
			new Vector3(-0.03515625f, 45f / 128f, 13f / 128f),
			new Vector3(-3f / 64f, 15f / 64f, 0.1289063f),
			new Vector3(-0.1210938f, 15f / 128f, 0.05859375f),
			new Vector3(-3f / 64f, 15f / 128f, 0.1289063f),
			new Vector3(-3f / 32f, 0f, 0.04296875f),
			new Vector3(-0.03515625f, 0f, 13f / 128f),
			new Vector3(0.05859375f, 15f / 64f, 0.125f),
			new Vector3(0.04296875f, 45f / 128f, 3f / 32f),
			new Vector3(0.05859375f, 15f / 128f, 0.125f),
			new Vector3(0.04296875f, 0f, 3f / 32f)
		};
	}

	protected virtual Vector2[] ModelUVs(Vector3[] verts)
	{
		Vector2[] array = new Vector2[verts.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Vector2(verts[i].x, verts[i].z);
			array[i] *= 4f;
		}
		return array;
	}

	public virtual Color ModelColour(int meshNo)
	{
		return Color.white;
	}

	public virtual Material ModelMaterials(int meshNo)
	{
		return GameWorldController.instance.modelMaterial;
	}
}
