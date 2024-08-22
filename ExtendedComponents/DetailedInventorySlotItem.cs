using Godot;

/// <summary>
/// Variation on <see cref="InventorySlotItem"/> with <see cref="Description"/>.
/// </summary>
public partial class DetailedInventorySlotItem : InventorySlotItem
{
	/// <summary>
	/// Extra text to show.
	/// </summary>
	[Export(PropertyHint.MultilineText)] public string Description { get; private set; }
}
