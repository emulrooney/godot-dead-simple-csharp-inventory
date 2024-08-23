using Godot;

/// <summary>
/// this is a very stupid script to just update a label in the example and should be thrown into a dumpster
/// </summary>
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
