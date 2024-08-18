using Godot;

/// <summary>
/// this is a very stupid script to just update a label in the example and should be thrown into a dumpster
/// </summary>
public partial class DumbLabelScript : Label
{
    public void SetTextFromItem(InventorySlotItem slotItem)
    {
        if (slotItem == null)
        {
            Text = "";
            return;
        }
        
        Text = $"Selected: {slotItem.Name}";
    }
    
}
