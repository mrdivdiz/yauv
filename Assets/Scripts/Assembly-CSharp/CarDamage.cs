using System;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class CarDamage : MonoBehaviour
{
	// Token: 0x06000ADE RID: 2782 RVA: 0x000822B8 File Offset: 0x000804B8
	private void Start()
	{
		this.myTransform = base.transform;
		if (this.meshFilters.Length == 0)
		{
			this.m_meshFilters = base.GetComponentsInChildren<MeshFilter>();
			int num = 0;
			for (int i = 0; i < this.m_meshFilters.Length; i++)
			{
				if (this.m_meshFilters[i].GetComponent<Collider>() == null)
				{
					num++;
				}
			}
			this.meshFilters = new MeshFilter[num];
			num = 0;
			for (int i = 0; i < this.m_meshFilters.Length; i++)
			{
				if (this.m_meshFilters[i].GetComponent<Collider>() == null)
				{
					this.meshFilters[num] = this.m_meshFilters[i];
					num++;
				}
			}
		}
		if (this.meshCollider != null)
		{
			this.colliderVerts = this.meshCollider.sharedMesh.vertices;
		}
		this.originalMeshData = new CarDamage.permaVertsColl[this.meshFilters.Length];
		for (int i = 0; i < this.meshFilters.Length; i++)
		{
			this.originalMeshData[i].permaVerts = this.meshFilters[i].mesh.vertices;
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.tag == "Body" || transform.gameObject.name == "Body")
			{
				this.body = transform.gameObject;
			}
		}
		if (this.body)
		{
			this.sign = Mathf.Cos(this.body.transform.localEulerAngles.y * 0.017453292f);
		}
		this.cardynamics = base.GetComponent<CarDynamics>();
		if (this.cardynamics != null)
		{
			this.wheelLayer = this.cardynamics.allWheels[0].transform.gameObject.layer;
		}
		else
		{
			this.wheelLayer = LayerMask.NameToLayer("Wheel");
		}
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00082518 File Offset: 0x00080718
	private void Update()
	{
		if (!this.sleep && this.repair && this.bounceBackSpeed > 0f)
		{
			this.sleep = true;
			for (int i = 0; i < this.meshFilters.Length; i++)
			{
				Vector3[] vertices = this.meshFilters[i].mesh.vertices;
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] += (this.originalMeshData[i].permaVerts[j] - vertices[j]) * (Time.deltaTime * this.bounceBackSpeed);
					if ((this.originalMeshData[i].permaVerts[j] - vertices[j]).magnitude >= this.bounceBackSleepCap)
					{
						this.sleep = false;
					}
				}
				this.meshFilters[i].mesh.vertices = vertices;
				this.meshFilters[i].mesh.RecalculateNormals();
				this.meshFilters[i].mesh.RecalculateBounds();
			}
			if (this.meshCollider != null)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = this.colliderVerts;
				mesh.triangles = this.meshCollider.sharedMesh.triangles;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				this.meshCollider.sharedMesh = mesh;
			}
			if (this.sleep)
			{
				this.repair = false;
			}
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x000826CC File Offset: 0x000808CC
	private void OnCollisionEnter(Collision collision)
	{
		if (this.collisionParticle1 != null && collision.contacts.Length > 0)
		{
			UnityEngine.Object.Instantiate(this.collisionParticle1, collision.contacts[0].point, Quaternion.identity);
		}
		if (this.collisionParticle2 != null && collision.contacts.Length > 0)
		{
			UnityEngine.Object.Instantiate(this.collisionParticle2, collision.contacts[0].point, Quaternion.identity);
		}
		if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameShield)
		{
			return;
		}
		if (collision.contacts.Length > 0)
		{
			Vector3 relativeVelocity = collision.relativeVelocity;
			relativeVelocity.y *= this.YforceDamp;
			float num = Vector3.Angle(collision.contacts[0].normal, relativeVelocity);
			float num2 = Mathf.Abs(Mathf.Cos(num * 0.017453292f));
			if (relativeVelocity.magnitude * num2 >= this.minForce)
			{
				this.sleep = false;
				this.vec = this.myTransform.InverseTransformDirection(relativeVelocity) * this.multiplier * 0.1f;
				for (int i = 0; i < this.meshFilters.Length; i++)
				{
					if (this.meshFilters[i].gameObject.layer != this.wheelLayer)
					{
						this.DeformMesh(this.meshFilters[i].mesh, this.originalMeshData[i].permaVerts, collision, num2, this.meshFilters[i].transform, this.sign);
					}
				}
				if (this.meshCollider != null)
				{
					Mesh mesh = new Mesh();
					mesh.vertices = this.meshCollider.sharedMesh.vertices;
					mesh.triangles = this.meshCollider.sharedMesh.triangles;
					this.DeformMesh(mesh, this.colliderVerts, collision, num2, this.meshCollider.transform, 1f);
					this.meshCollider.sharedMesh = mesh;
					this.meshCollider.sharedMesh.RecalculateNormals();
					this.meshCollider.sharedMesh.RecalculateBounds();
				}
			}
		}
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00082900 File Offset: 0x00080B00
	private void DeformMesh(Mesh mesh, Vector3[] originalMesh, Collision collision, float cos, Transform meshTransform, float sign)
	{
		Vector3[] vertices = mesh.vertices;
		foreach (ContactPoint contactPoint in collision.contacts)
		{
			Vector3 a = meshTransform.InverseTransformPoint(contactPoint.point);
			for (int j = 0; j < vertices.Length; j++)
			{
				if ((a - vertices[j]).magnitude < this.deformRadius)
				{
					vertices[j] += (this.vec * (this.deformRadius - (a - vertices[j]).magnitude) / this.deformRadius * cos + UnityEngine.Random.onUnitSphere * this.deformNoise) * sign;
					if (this.maxDeform > 0f && (vertices[j] - originalMesh[j]).magnitude > this.maxDeform)
					{
						vertices[j] = originalMesh[j] + (vertices[j] - originalMesh[j]).normalized * this.maxDeform;
					}
				}
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	// Token: 0x04001227 RID: 4647
	public MeshCollider meshCollider;

	// Token: 0x04001228 RID: 4648
	public MeshFilter[] meshFilters;

	// Token: 0x04001229 RID: 4649
	private MeshFilter[] m_meshFilters;

	// Token: 0x0400122A RID: 4650
	public float deformNoise = 0.03f;

	// Token: 0x0400122B RID: 4651
	public float deformRadius = 0.5f;

	// Token: 0x0400122C RID: 4652
	private float bounceBackSleepCap = 0.002f;

	// Token: 0x0400122D RID: 4653
	public float bounceBackSpeed = 2f;

	// Token: 0x0400122E RID: 4654
	private Vector3[] colliderVerts;

	// Token: 0x0400122F RID: 4655
	private CarDamage.permaVertsColl[] originalMeshData;

	// Token: 0x04001230 RID: 4656
	private bool sleep = true;

	// Token: 0x04001231 RID: 4657
	public float maxDeform = 0.5f;

	// Token: 0x04001232 RID: 4658
	private float minForce = 5f;

	// Token: 0x04001233 RID: 4659
	public float multiplier = 0.1f;

	// Token: 0x04001234 RID: 4660
	public float YforceDamp = 1f;

	// Token: 0x04001235 RID: 4661
	[HideInInspector]
	public bool repair;

	// Token: 0x04001236 RID: 4662
	private Vector3 vec;

	// Token: 0x04001237 RID: 4663
	private Transform myTransform;

	// Token: 0x04001238 RID: 4664
	private GameObject body;

	// Token: 0x04001239 RID: 4665
	private CarDynamics cardynamics;

	// Token: 0x0400123A RID: 4666
	private float sign = 1f;

	// Token: 0x0400123B RID: 4667
	private int wheelLayer;

	// Token: 0x0400123C RID: 4668
	public GameObject collisionParticle1;

	// Token: 0x0400123D RID: 4669
	public GameObject collisionParticle2;

	// Token: 0x0200023C RID: 572
	private struct permaVertsColl
	{
		// Token: 0x0400123E RID: 4670
		public Vector3[] permaVerts;
	}
}
