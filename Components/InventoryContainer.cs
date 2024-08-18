using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotArray = Godot.Collections.Array;

/// <summary>
/// A container for a series of <see cref="InventorySlot"/>s, which should themselves be contained in whatever default
/// <see cref="Control"/> type is most useful. ie This should be a grandparent to the slots, with the middle node being
/// an <see cref="HBoxContainer"/> or something else that decides how to align the slots.
/// </summary>
[GlobalClass]
public partial class InventoryContainer : Control
{
	/// <summary>
	/// Item that user is mouseovering is updated; can emit null too.
	/// </summary>
	[Signal] public delegate void MouseoverItemChangedEventHandler(InventorySlot slot);
	[Signal] public delegate void ContentsChangedEventHandler(GodotArray arr);
	
	/// <summary>
	/// A series of nodes that hold <see cref="InventorySlotItem"/>s
	/// </summary>
	[Export] private Node[] contentHolders;

	/// <summary>
	/// Internal tracking of contents under each item in <see cref="contentHolders"/> 
	/// </summary>
	private GodotArray contents = new();
	
	/// <summary>
	/// Item currently selected (or in this case, just mouseovered)
	/// </summary>
	private InventorySlotItem activeSlotItem = new();

	public override void _Ready()
	{
		var itemCount = 0;
		//Build initial array and connect signals.
		foreach (var slot in CollectSlots())
		{
			contents.Add(slot.Content ?? new Variant());
			var count = itemCount; //Copy to local scope.

			slot.MouseEntered += () => { OnMouseEntered(count); };
			slot.MouseExited += () => { OnMouseExited(count); };

			slot.BecameEmpty += () =>
			{
				contents[count] = new Variant();
				EmitSignal(SignalName.ContentsChanged, contents);
			};
			slot.BecamePopulated += (content) =>
			{
				contents[count] = content;
				EmitSignal(SignalName.ContentsChanged, contents);
			};
			itemCount++; 
		} 
	}

	private void OnMouseExited(int index)
	{
		if (activeSlotItem == contents[index].As<InventorySlotItem>())
		{
			activeSlotItem = null;
			EmitSignal(SignalName.MouseoverItemChanged, activeSlotItem);
		}
	}

	private void OnMouseEntered(int index)
	{
		activeSlotItem = contents[index].As<InventorySlotItem>();
		EmitSignal(SignalName.MouseoverItemChanged, activeSlotItem);
	}
	
	/// <summary>
	/// Get all contents from scratch. <see cref="IEnumerable"/> so you can decide how to handle the contents.
	/// </summary>
	/// <returns></returns>
	public IEnumerable<InventorySlot> CollectSlots()
	{
		foreach (var holder in contentHolders)
		{
			foreach (var slot in holder.GetChildren().OfType<InventorySlot>())
			{
				yield return slot;
			}
		}
	}
}
