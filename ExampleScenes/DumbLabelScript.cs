using Godot;
using System;

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
