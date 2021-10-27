using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class WaveSpawner : MonoBehaviour
{
	// Token: 0x0600089F RID: 2207 RVA: 0x00047E80 File Offset: 0x00046080
	private void Awake()
	{
		
		this.previousBestScore = PlayerPrefs.GetInt("BestScore" + this.leaderBoardID, 0).ToString();
		Physics.gravity = new Vector3(0f, -9.81f * this.scaleFactor, 0f);
		GameObject gameObject = GameObject.Find("PlayerSpawnPosition");
		if (gameObject != null)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(WaveSpawner.playerCharacter), gameObject.transform.position, gameObject.transform.rotation);
			UnityEngine.AI.NavMeshAgent navMeshAgent = gameObject2.AddComponent<UnityEngine.AI.NavMeshAgent>();
			navMeshAgent.radius = 0.3f * this.scaleFactor;
			navMeshAgent.updatePosition = false;
			navMeshAgent.updateRotation = false;
			if (this.scaleFactor != 1f)
			{
				gameObject2.transform.localScale = new Vector3(this.scaleFactor, this.scaleFactor, this.scaleFactor);
				this.cam = Camera.main.GetComponent<ShooterGameCamera>();
				this.cam.shootingCamOffsetX *= this.scaleFactor;
				this.cam.unaimedZoom *= this.scaleFactor;
				this.cam.aimedZoom *= this.scaleFactor;
				NormalCharacterMotor component = gameObject2.GetComponent<NormalCharacterMotor>();
				component.maxForwardSpeed *= this.scaleFactor;
				component.maxBackwardsSpeed *= this.scaleFactor;
				component.maxEngagedSpeed *= this.scaleFactor;
				component.maxRotationSpeed *= this.scaleFactor;
				component.maxSidewaysSpeed *= this.scaleFactor;
				component.maxStealthSpeed *= this.scaleFactor;
				component.maxVelocityChange *= this.scaleFactor;
				component.gravity *= this.scaleFactor;
				component.scaleFactor = this.scaleFactor;
				AnimationHandler component2 = gameObject2.GetComponent<AnimationHandler>();
				component2.lowFallDistance *= this.scaleFactor;
				component2.MediumFallDistance *= this.scaleFactor;
				component2.HighFallDistance *= this.scaleFactor;
				component2.scaleFactor = this.scaleFactor;
				Health component3 = gameObject2.GetComponent<Health>();
				component3.scaleFactor = this.scaleFactor;
				foreach (Gun gun in gameObject2.GetComponentsInChildren<Gun>())
				{
					gun.scaleFactor = this.scaleFactor;
				}
			}
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00048144 File Offset: 0x00046344
	private void OnDestroy()
	{
		//GA.API.Design.NewEvent("FinishedSurvivalLevel:" + Application.loadedLevelName, float.Parse(WaveSpawner.points));
		if (this.style != null)
		{
			this.style.font = null;
		}
		if (int.Parse(WaveSpawner.points) > PlayerPrefs.GetInt("BestScore" + this.leaderBoardID, 0) && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GooglePlay && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameShield)
		{
			PlayerPrefs.SetInt("BestScore" + this.leaderBoardID, int.Parse(WaveSpawner.points));
		}
		if (Social.localUser.authenticated)
		{
			string text = string.Empty;
			switch (this.leaderBoardID)
			{
			case 1:
				text = "CgkItfLiwroeEAIQCQ";
				break;
			case 2:
				text = "CgkItfLiwroeEAIQCg";
				break;
			case 3:
				text = "CgkItfLiwroeEAIQCw";
				break;
			case 4:
				text = "CgkItfLiwroeEAIQDA";
				break;
			case 5:
				text = "CgkItfLiwroeEAIQDQ";
				break;
			case 6:
				text = "CgkItfLiwroeEAIQDg";
				break;
			}
			if (text != "CgkItfLiwroeEAIQCA")
			{
				Social.ReportScore(long.Parse(WaveSpawner.points), text, delegate(bool success)
				{
					if (success)
					{
						Debug.Log("score submitted successfully");
					}
					else
					{
						Debug.Log("score Failed to be submitted");
					}
				});
			}
			Social.ReportScore(long.Parse(WaveSpawner.points), string.Empty, delegate(bool success)
			{
				if (success)
				{
					Debug.Log("score submitted successfully");
				}
				else
				{
					Debug.Log("score Failed to be submitted");
				}
			});
		}
		Physics.gravity = new Vector3(0f, -9.81f, 0f);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00048300 File Offset: 0x00046500
	private void Start()
	{
		this.spawnPositions = (WaveSpawnPosition[])UnityEngine.Object.FindObjectsOfType(typeof(WaveSpawnPosition));
		this.currentWaveCount = this.waveStart;
		this.player = GameObject.FindGameObjectWithTag("Player").transform;
		this.layerMask = 1 << LayerMask.NameToLayer("Enemy");
		this.layerMask |= 1 << LayerMask.NameToLayer("Player");
		this.layerMask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.layerMask = ~this.layerMask;
		WaveSpawner.points = "0";
		this.style = new GUIStyle();
		this.style.font = Language.GetFont29();
		this.style.alignment = TextAnchor.MiddleCenter;
		this.currentMaterial = UnityEngine.Random.Range(0, this.zomiesMaterials.Length);
		this.currentZombie = UnityEngine.Random.Range(0, this.zombies.Length);
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00048424 File Offset: 0x00046624
	public void ChangedLanguage()
	{
		this.style = new GUIStyle();
		this.style.font = Language.GetFont29();
		this.style.alignment = TextAnchor.MiddleCenter;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00048450 File Offset: 0x00046650
	private void SpawnWave()
	{
		Vector3 position = this.player.position;
		position.y += 2f;
		List<WaveSpawner.Dropable> list = new List<WaveSpawner.Dropable>();
		for (int i = 0; i < this.dropables.Length; i++)
		{
			if ((this.dropables[i].waveNoStart == 0 || this.currentWave >= this.dropables[i].waveNoStart) && (this.dropables[i].waveNoEnd == 0 || this.currentWave <= this.dropables[i].waveNoEnd))
			{
				list.Add(this.dropables[i]);
			}
		}
		list.Sort();
		int num = list.Count - 1;
		SortedDictionary<float, WaveSpawnPosition> sortedDictionary = new SortedDictionary<float, WaveSpawnPosition>();
		foreach (WaveSpawnPosition waveSpawnPosition in this.spawnPositions)
		{
			if (this.player.transform.InverseTransformPoint(waveSpawnPosition.transform.position).z < -5f || Physics.Linecast(position, waveSpawnPosition.transform.position, this.layerMask))
			{
				sortedDictionary.Add(Vector3.Distance(this.player.position, waveSpawnPosition.transform.position), waveSpawnPosition);
			}
		}
		SortedDictionary<float, WaveSpawnPosition>.Enumerator enumerator = sortedDictionary.GetEnumerator();
		int num2 = 0;
		for (int k = 0; k < this.currentWaveCount; k++)
		{
			WaveSpawnPosition value;
			if (enumerator.MoveNext())
			{
				KeyValuePair<float, WaveSpawnPosition> keyValuePair = enumerator.Current;
				value = keyValuePair.Value;
			}
			else
			{
				enumerator = sortedDictionary.GetEnumerator();
				if (!enumerator.MoveNext())
				{
					break;
				}
				KeyValuePair<float, WaveSpawnPosition> keyValuePair2 = enumerator.Current;
				value = keyValuePair2.Value;
			}
			if (value != null)
			{
				if (num2 % 2 == 0)
				{
					this.zombies[this.currentZombie].pickupablePrefab = list[num].pickupable;
					num--;
					if (num < 0)
					{
						num = list.Count - 1;
					}
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(this.zombies[this.currentZombie].gameObject, value.transform.position, Quaternion.identity) as GameObject;
				ZombieAI component = gameObject.GetComponent<ZombieAI>();
				component.runningSpeed = this.runningSpeed;
				if (this.scaleFactor != 1f)
				{
					gameObject.transform.localScale = new Vector3(this.scaleFactor, this.scaleFactor, this.scaleFactor);
					component.scaleFactor = this.scaleFactor;
					UnityEngine.AI.NavMeshAgent component2 = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
					component2.speed *= this.scaleFactor;
				}
				if (component.bodyMaterials == null || component.bodyMaterials.Length == 0)
				{
					Renderer componentInChildren = gameObject.GetComponentInChildren<Renderer>();
					if (componentInChildren != null)
					{
						componentInChildren.sharedMaterial = this.zomiesMaterials[this.currentMaterial];
						this.currentMaterial = (this.currentMaterial + 1) % this.zomiesMaterials.Length;
					}
				}
				this.zombies[this.currentZombie].pickupablePrefab = null;
				this.currentZombie = (this.currentZombie + 1) % this.zombies.Length;
				num2++;
			}
		}
		if (sortedDictionary.Values.Count > 0)
		{
			this.currentWave++;
			Resources.UnloadUnusedAssets();
			list.Clear();
			if (this.currentWaveCount <= 19)
			{
				this.currentWaveCount += this.waveIncrement;
			}
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				this.waveSpawnedText = this.currentWave + " " + Language.Get("M_Wave", 60);
			}
			else
			{
				this.waveSpawnedText = Language.Get("M_Wave", 60) + " " + this.currentWave;
			}
			this.displayWaveSpawned = 5f;
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00048864 File Offset: 0x00046A64
	private void Update()
	{
		if (this.displayWaveSpawned > 0f)
		{
			this.displayWaveSpawned -= Time.deltaTime;
		}
		if (WaveSpawner.ZombiesNo <= 0)
		{
			this.spawnWave = true;
		}
		if (this.spawnWave)
		{
			if (this.restTime <= 0f)
			{
				this.SpawnWave();
				this.restTime = 15f;
				this.spawnWave = false;
			}
			else
			{
				this.restTime -= Time.deltaTime;
				if (Time.time - this.previousNextWaveText > 1f)
				{
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						this.nextWaveText = Mathf.CeilToInt(this.restTime) + " " + Language.Get("M_NextWave", 60);
					}
					else
					{
						this.nextWaveText = Language.Get("M_NextWave", 60) + " " + Mathf.CeilToInt(this.restTime);
					}
					this.previousNextWaveText = Time.time;
				}
			}
		}
		if (SpeechManager.enable3D != SpeechManager.previousEnable3d)
		{
			Stereoscopic3D stereoscopic3D = Camera.main.GetComponent<Stereoscopic3D>();
			if (stereoscopic3D == null)
			{
				stereoscopic3D = Camera.main.gameObject.AddComponent<Stereoscopic3D>();
			}
			if (stereoscopic3D != null)
			{
				if (SpeechManager.enable3D)
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					stereoscopic3D.enabled = true;
				}
				else
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					stereoscopic3D.enabled = false;
				}
			}
			SpeechManager.previousEnable3d = SpeechManager.enable3D;
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00048A1C File Offset: 0x00046C1C
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		if (mainmenu.pause)
		{
			return;
		}
		if (this.displayWaveSpawned > 0f)
		{
			this.style.alignment = TextAnchor.MiddleCenter;
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(1f, 51f, 1366f, 100f), this.waveSpawnedText, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, 50f, 1366f, 100f), this.waveSpawnedText, this.style);
		}
		if (this.restTime > 0f && this.restTime != 15f)
		{
			this.style.alignment = TextAnchor.MiddleCenter;
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(1f, 51f, 1366f, 100f), this.nextWaveText, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, 50f, 1366f, 100f), this.nextWaveText, this.style);
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
		{
			this.style.alignment = TextAnchor.MiddleCenter;
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(1f, 11f, 1366f, 100f), WaveSpawner.points, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, 10f, 1366f, 100f), WaveSpawner.points, this.style);
		}
		else
		{
			this.style.alignment = TextAnchor.MiddleLeft;
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(51f, 201f, 100f, 100f), WaveSpawner.points + " \\ " + this.previousBestScore, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(50f, 200f, 100f, 100f), WaveSpawner.points + " \\ " + this.previousBestScore, this.style);
		}
	}

	// Token: 0x04000B8A RID: 2954
	public ZombieAI[] zombies;

	// Token: 0x04000B8B RID: 2955
	public Material[] zomiesMaterials;

	// Token: 0x04000B8C RID: 2956
	public WaveSpawner.Dropable[] dropables;

	// Token: 0x04000B8D RID: 2957
	private WaveSpawnPosition[] spawnPositions;

	// Token: 0x04000B8E RID: 2958
	public int waveStart = 5;

	// Token: 0x04000B8F RID: 2959
	public int waveIncrement = 3;

	// Token: 0x04000B90 RID: 2960
	public static int ZombiesNo;

	// Token: 0x04000B91 RID: 2961
	public static string points = "0";

	// Token: 0x04000B92 RID: 2962
	private int currentWave;

	// Token: 0x04000B93 RID: 2963
	private int currentWaveCount;

	// Token: 0x04000B94 RID: 2964
	private bool spawnWave = true;

	// Token: 0x04000B95 RID: 2965
	public Transform player;

	// Token: 0x04000B96 RID: 2966
	private LayerMask layerMask;

	// Token: 0x04000B97 RID: 2967
	private int currentZombie;

	// Token: 0x04000B98 RID: 2968
	private float displayWaveSpawned;

	// Token: 0x04000B99 RID: 2969
	private string waveSpawnedText = string.Empty;

	// Token: 0x04000B9A RID: 2970
	private GUIStyle style;

	// Token: 0x04000B9B RID: 2971
	private int currentMaterial;

	// Token: 0x04000B9C RID: 2972
	public static string playerCharacter = "SpecOps";

	// Token: 0x04000B9D RID: 2973
	public float scaleFactor = 1f;

	// Token: 0x04000B9E RID: 2974
	private ShooterGameCamera cam;

	// Token: 0x04000B9F RID: 2975
	public int leaderBoardID;

	// Token: 0x04000BA0 RID: 2976
	public float runningSpeed = 4.1f;

	// Token: 0x04000BA1 RID: 2977
	private float restTime;

	// Token: 0x04000BA2 RID: 2978
	private string nextWaveText;

	// Token: 0x04000BA3 RID: 2979
	private float previousNextWaveText;

	// Token: 0x04000BA4 RID: 2980
	private string previousBestScore = "0";

	// Token: 0x020001A8 RID: 424
	[Serializable]
	public class Dropable : IComparable<WaveSpawner.Dropable>
	{
		// Token: 0x060008A9 RID: 2217 RVA: 0x00048D48 File Offset: 0x00046F48
		public int CompareTo(WaveSpawner.Dropable other)
		{
			if (this.waveNoStart < other.waveNoStart)
			{
				return -1;
			}
			if (this.waveNoStart > other.waveNoStart)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04000BA7 RID: 2983
		public Transform pickupable;

		// Token: 0x04000BA8 RID: 2984
		public int waveNoStart;

		// Token: 0x04000BA9 RID: 2985
		public int waveNoEnd;
	}
}
