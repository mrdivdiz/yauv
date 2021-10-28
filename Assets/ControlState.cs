using System;

// Token: 0x02000054 RID: 84
[Serializable]
public enum ControlState
{
	// Token: 0x040000D1 RID: 209
	WaitingForFirstTouch,
	// Token: 0x040000D2 RID: 210
	WaitingForSecondTouch,
	// Token: 0x040000D3 RID: 211
	MovingCharacter,
	// Token: 0x040000D4 RID: 212
	WaitingForMovement,
	// Token: 0x040000D5 RID: 213
	ZoomingCamera,
	// Token: 0x040000D6 RID: 214
	RotatingCamera,
	// Token: 0x040000D7 RID: 215
	WaitingForNoFingers
}
