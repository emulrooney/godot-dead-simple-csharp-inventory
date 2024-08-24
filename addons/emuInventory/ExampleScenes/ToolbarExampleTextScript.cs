using System;
using Godot;

/// <summary>
/// This is a simple test script not intended for real use in production.
/// </summary>
[Obsolete]
public partial class ToolbarExampleTextScript : Label
{
	public void SetMouseoverTextFromItem(InventorySlotItem slotItem)
	{
		if (slotItem == null)
		{
			Text = "";
			return;
		}
		
		Text = $"{slotItem.Name}";
	}
	
	public void SetSelectionTextFromItem(InventorySlotItem slotItem)
	{
		switch (slotItem)
		{
			case null:
				Text = "";
				return;
			case DetailedInventorySlotItem detailedItem:
				Text = $"{slotItem.Name}: {detailedItem.Description}";
				return;
			default:
				Text = $"{slotItem.Name}";
				break;
		}
	}
}
