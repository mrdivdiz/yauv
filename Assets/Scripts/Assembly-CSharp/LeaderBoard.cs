using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class LeaderBoard : MonoBehaviour
{
	// Token: 0x04000C11 RID: 3089
	public LeaderBoard.Board[] leaderboards;

	// Token: 0x04000C12 RID: 3090
	public Font largestFont;

	// Token: 0x04000C13 RID: 3091
	public Texture friendsFilter;

	// Token: 0x04000C14 RID: 3092
	public Texture showProfile;

	// Token: 0x04000C15 RID: 3093
	public Texture backButton;

	// Token: 0x04000C16 RID: 3094
	public Texture changeRecords;

	// Token: 0x04000C17 RID: 3095
	public Texture changePage;

	// Token: 0x04000C18 RID: 3096
	public Texture l1;

	// Token: 0x04000C19 RID: 3097
	public Texture r1;

	// Token: 0x04000C1A RID: 3098
	//public PSXLeaderBoard.Score[] scores;

	// Token: 0x04000C1B RID: 3099
	private GUIStyle style;

	// Token: 0x04000C1C RID: 3100
	private GUIStyle largeStyle;

	// Token: 0x04000C1D RID: 3101
	private GUIStyle largestStyle;

	// Token: 0x04000C1E RID: 3102
	private GUIStyle entryNormalStyle;

	// Token: 0x04000C1F RID: 3103
	private GUIStyle entryHoverStyle;

	// Token: 0x04000C20 RID: 3104
	private int currentSelection;

	// Token: 0x04000C21 RID: 3105
	private int currentLeaderboard;

	// Token: 0x04000C22 RID: 3106
	private bool acceptMovement = true;

	// Token: 0x04000C23 RID: 3107
	private int totlalRows = 3;

	// Token: 0x04000C24 RID: 3108
	public AudioClip hoverSound;

	// Token: 0x04000C25 RID: 3109
	public Texture leftArrow;

	// Token: 0x04000C26 RID: 3110
	public Texture rightArrow;

	// Token: 0x04000C27 RID: 3111
	private int currentPage;

	// Token: 0x04000C28 RID: 3112
	private bool showFriendsOnly;

	// Token: 0x04000C29 RID: 3113
	private bool online = true;

	// Token: 0x04000C2A RID: 3114
	private int recordsPerPage = 14;

	// Token: 0x020001C4 RID: 452
	[Serializable]
	public class Board
	{
		// Token: 0x04000C2B RID: 3115
		public string boardID;

		// Token: 0x04000C2C RID: 3116
		public string boardTitle;

		// Token: 0x04000C2D RID: 3117
		public Texture screenshot;
	}
}
