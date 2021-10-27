using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class ComboBox
{
	// Token: 0x060001BE RID: 446 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
	public int List(Rect rect, string buttonText, GUIContent[] listContent, GUIStyle listStyle)
	{
		return this.List(rect, new GUIContent(buttonText), listContent, "button", "box", listStyle);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000E918 File Offset: 0x0000CB18
	public int List(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
	{
		return this.List(rect, buttonContent, listContent, "button", "box", listStyle);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000E944 File Offset: 0x0000CB44
	public int List(Rect rect, string buttonText, GUIContent[] listContent, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
	{
		return this.List(rect, new GUIContent(buttonText), listContent, buttonStyle, boxStyle, listStyle);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000E95C File Offset: 0x0000CB5C
	public int List(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
	{
		if (ComboBox.forceToUnShow)
		{
			ComboBox.forceToUnShow = false;
			this.isClickedComboButton = false;
		}
		bool flag = false;
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		EventType typeForControl = Event.current.GetTypeForControl(controlID);
		if (typeForControl == EventType.MouseUp)
		{
			if (this.isClickedComboButton)
			{
				flag = true;
			}
		}
		if (GUI.Button(rect, buttonContent, buttonStyle))
		{
			if (ComboBox.useControlID == -1)
			{
				ComboBox.useControlID = controlID;
				this.isClickedComboButton = false;
			}
			if (ComboBox.useControlID != controlID)
			{
				ComboBox.forceToUnShow = true;
				ComboBox.useControlID = controlID;
			}
			this.isClickedComboButton = true;
		}
		if (this.isClickedComboButton)
		{
			Rect position = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1f), rect.width, listStyle.CalcHeight(listContent[0], 1f) * (float)listContent.Length);
			GUI.Box(position, string.Empty, boxStyle);
			int num = GUI.SelectionGrid(position, this.selectedItemIndex, listContent, 1, listStyle);
			if (num != this.selectedItemIndex)
			{
				this.selectedItemIndex = num;
			}
		}
		if (flag)
		{
			this.isClickedComboButton = false;
		}
		return this.GetSelectedItemIndex();
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000EA88 File Offset: 0x0000CC88
	public int GetSelectedItemIndex()
	{
		return this.selectedItemIndex;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000EA90 File Offset: 0x0000CC90
	public bool IsShowingList()
	{
		return this.isClickedComboButton;
	}

	// Token: 0x0400023E RID: 574
	private static bool forceToUnShow;

	// Token: 0x0400023F RID: 575
	private static int useControlID = -1;

	// Token: 0x04000240 RID: 576
	private bool isClickedComboButton;

	// Token: 0x04000241 RID: 577
	private int selectedItemIndex;
}
