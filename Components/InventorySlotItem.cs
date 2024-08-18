using Godot;

/// <summary>
/// Visual representation of an inventory item. This is the only class that can be a child node to <see cref="InventorySlot"/>.
/// </summary>
public partial class InventorySlotItem : Control
{
    /// DEV NOTE: This is deliberately blank, since every project will have different item data. But basically the idea
    /// is that this could hold a <see cref="Resource"/> item for the **actual** item data.
}
