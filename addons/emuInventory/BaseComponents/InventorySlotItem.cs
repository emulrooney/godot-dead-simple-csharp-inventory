using Godot;

/// <summary>
/// Visual representation of an inventory item. This is the only class that can be a child node to <see cref="InventorySlot"/>.
/// It's a texture rect, but there's nothing preventing you from leaving the texture blank and instead linking
///
/// DEV NOTE: This is deliberately very empty blank, since every project will have different item data. But basically the
/// idea is that this could hold a <see cref="Resource"/> item for the **actual** item data.
/// </summary>
public partial class InventorySlotItem : TextureRect
{
	[Export] public string DisplayName { get; private set; } = "An unnamed item";

	/// <summary>
	/// Item to generate when previewing; if empty, a simple copy of the <see cref="Texture"/> is used. This must be
	/// a node inheriting <see cref="Control"/>.
	/// </summary>
	[Export] public PackedScene PreviewItem { get; private set; }
}
