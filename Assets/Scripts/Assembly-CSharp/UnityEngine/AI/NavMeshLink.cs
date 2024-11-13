using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-101)]
	[AddComponentMenu("Navigation/NavMeshLink", 33)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshLink : MonoBehaviour
	{
		[SerializeField]
		private int m_AgentTypeID;

		[SerializeField]
		private Vector3 m_StartPoint = new Vector3(0f, 0f, -2.5f);

		[SerializeField]
		private Vector3 m_EndPoint = new Vector3(0f, 0f, 2.5f);

		[SerializeField]
		private float m_Width;

		[SerializeField]
		private int m_CostModifier = -1;

		[SerializeField]
		private bool m_Bidirectional = true;

		[SerializeField]
		private bool m_AutoUpdatePosition;

		[SerializeField]
		private int m_Area;

		private NavMeshLinkInstance m_LinkInstance = default(NavMeshLinkInstance);

		private Vector3 m_LastPosition = Vector3.zero;

		private Quaternion m_LastRotation = Quaternion.identity;

		private static readonly List<NavMeshLink> s_Tracked = new List<NavMeshLink>();

		public int agentTypeID
		{
			get
			{
				return m_AgentTypeID;
			}
			set
			{
				m_AgentTypeID = value;
				UpdateLink();
			}
		}

		public Vector3 startPoint
		{
			get
			{
				return m_StartPoint;
			}
			set
			{
				m_StartPoint = value;
				UpdateLink();
			}
		}

		public Vector3 endPoint
		{
			get
			{
				return m_EndPoint;
			}
			set
			{
				m_EndPoint = value;
				UpdateLink();
			}
		}

		public float width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
				UpdateLink();
			}
		}

		public int costModifier
		{
			get
			{
				return m_CostModifier;
			}
			set
			{
				m_CostModifier = value;
				UpdateLink();
			}
		}

		public bool bidirectional
		{
			get
			{
				return m_Bidirectional;
			}
			set
			{
				m_Bidirectional = value;
				UpdateLink();
			}
		}

		public bool autoUpdate
		{
			get
			{
				return m_AutoUpdatePosition;
			}
			set
			{
				SetAutoUpdate(value);
			}
		}

		public int area
		{
			get
			{
				return m_Area;
			}
			set
			{
				m_Area = value;
				UpdateLink();
			}
		}

		private void OnEnable()
		{
			AddLink();
			if (m_AutoUpdatePosition && m_LinkInstance.valid)
			{
				AddTracking(this);
			}
		}

		private void OnDisable()
		{
			RemoveTracking(this);
			m_LinkInstance.Remove();
		}

		public void UpdateLink()
		{
			m_LinkInstance.Remove();
			AddLink();
		}

		private static void AddTracking(NavMeshLink link)
		{
			if (s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(UpdateTrackedInstances));
			}
			s_Tracked.Add(link);
		}

		private static void RemoveTracking(NavMeshLink link)
		{
			s_Tracked.Remove(link);
			if (s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(UpdateTrackedInstances));
			}
		}

		private void SetAutoUpdate(bool value)
		{
			if (m_AutoUpdatePosition != value)
			{
				m_AutoUpdatePosition = value;
				if (value)
				{
					AddTracking(this);
				}
				else
				{
					RemoveTracking(this);
				}
			}
		}

		private void AddLink()
		{
			NavMeshLinkData link = default(NavMeshLinkData);
			link.startPosition = m_StartPoint;
			link.endPosition = m_EndPoint;
			link.width = m_Width;
			link.costModifier = m_CostModifier;
			link.bidirectional = m_Bidirectional;
			link.area = m_Area;
			link.agentTypeID = m_AgentTypeID;
			m_LinkInstance = NavMesh.AddLink(link, base.transform.position, base.transform.rotation);
			if (m_LinkInstance.valid)
			{
				m_LinkInstance.owner = this;
			}
			m_LastPosition = base.transform.position;
			m_LastRotation = base.transform.rotation;
		}

		private bool HasTransformChanged()
		{
			if (m_LastPosition != base.transform.position)
			{
				return true;
			}
			if (m_LastRotation != base.transform.rotation)
			{
				return true;
			}
			return false;
		}

		private void OnDidApplyAnimationProperties()
		{
			UpdateLink();
		}

		private static void UpdateTrackedInstances()
		{
			foreach (NavMeshLink item in s_Tracked)
			{
				if (item.HasTransformChanged())
				{
					item.UpdateLink();
				}
			}
		}
	}
}
