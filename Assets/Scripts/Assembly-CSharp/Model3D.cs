using UnityEngine;

public class Model3D : object_base
{
	protected override void Start()
	{
		base.Start();
		AdjustModelPos();
		base.gameObject.layer = LayerMask.NameToLayer("MapMesh");
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		if (GetComponent<MeshFilter>() == null)
		{
			Generate3DModel(base.gameObject);
		}
	}

	private void Generate3DModel(GameObject parent)
	{
		BoxCollider component = GetComponent<BoxCollider>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
		MeshFilter meshFilter = parent.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = parent.AddComponent<MeshRenderer>();
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
		if (isSolidModel())
		{
			MeshCollider meshCollider = parent.AddComponent<MeshCollider>();
			meshCollider.convex = true;
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		else
		{
			Rigidbody component2 = GetComponent<Rigidbody>();
			if (component2 != null)
			{
				Object.DestroyImmediate(component2);
			}
		}
	}

	public virtual int[] ModelTriangles(int meshNo)
	{
		return new int[3];
	}

	public virtual Vector3[] ModelVertices()
	{
		return new Vector3[4]
		{
			Vector3.zero,
			Vector3.zero,
			Vector3.zero,
			Vector3.zero
		};
	}

	public virtual int NoOfMeshes()
	{
		return 1;
	}

	public virtual Color ModelColour(int meshNo)
	{
		return Color.white;
	}

	public virtual bool isSolidModel()
	{
		return true;
	}

	public virtual Vector2[] ModelUVs(Vector3[] verts)
	{
		Vector2[] array = new Vector2[verts.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Vector2(verts[i].x, verts[i].z);
			array[i] *= TextureScaling();
		}
		return array;
	}

	public virtual Material ModelMaterials(int meshNo)
	{
		return GameWorldController.instance.modelMaterial;
	}

	public virtual Texture2D ModelTexture(int meshNo)
	{
		return null;
	}

	public virtual float TextureScaling()
	{
		return 1f;
	}

	protected void AdjustModelPos()
	{
		int objectTileX = base.ObjectTileX;
		int objectTileY = base.ObjectTileY;
		int num = base.xpos;
		int num2 = base.ypos;
		Vector3 position = base.transform.position;
		Vector3 zero = Vector3.zero;
		if (TileMap.ValidTile(objectTileX, objectTileY))
		{
			if (num2 == 0)
			{
				zero += new Vector3(0f, 0f, 0.06f);
			}
			if (num2 == 7)
			{
				zero += new Vector3(0f, 0f, -0.06f);
			}
			if (num == 0)
			{
				zero += new Vector3(0.06f, 0f, 0f);
			}
			if (num == 7)
			{
				zero += new Vector3(-0.06f, 0f, 0f);
			}
		}
		base.transform.position = position + zero;
	}
}
