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
	[Signal] public delegate void MouseoverItemChangedEventHandler(InventorySlotItem slot);

	/// <summary>
	/// Item that the user has selected is updated; can emit null too.
	/// </summary>
	[Signal] public delegate void SelectedItemChangedEventHandler(InventorySlotItem slot);
	
	/// <summary>
	/// <see cref="InventoryContainer.contents"/> changed, meaning something moved around
	/// </summary>
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
	/// Item currently hovered over
	/// </summary>
	private InventorySlotItem hoveredSlotItem;
	
	/// <summary>
	/// 
	/// </summary>
	private InventorySlotItem selectedSlotItem;

	public override void _Ready()
	{
		var itemCount = 0;
		//Build initial array and connect signals.
		foreach (var slot in CollectSlots())
		{
			contents.Add(slot.Content ?? new Variant());
			InitializeSlot(itemCount, slot);
			itemCount++;
		} 
	}

	/// <summary>
	/// Setup a slot & signals
	/// </summary>
	/// <param name="slotIndex"></param>
	/// <param name="slot"></param>
	private void InitializeSlot(int slotIndex, InventorySlot slot)
	{
		slot.MouseEntered += () => { OnMouseEntered(slotIndex); };
		slot.MouseExited += () => { OnMouseExited(slotIndex); };

		slot.BecameEmpty += () =>
		{
			contents[slotIndex] = new Variant();
			EmitSignal(SignalName.ContentsChanged, contents);
			OnMouseExited(slotIndex); 
		};
		
		slot.BecamePopulated += (content) =>
		{
			contents[slotIndex] = content;
			EmitSignal(SignalName.ContentsChanged, contents);
		};

		slot.BecameSelected += () =>
		{
			EmitSignal(SignalName.SelectedItemChanged, contents[slotIndex]);
		};
	}

	private void OnMouseExited(int index)
	{
		if (GetViewport().GuiGetDragData().VariantType != Variant.Type.Nil) return;
		
		if (hoveredSlotItem == contents[index].As<InventorySlotItem>())
		{
			hoveredSlotItem = null;
			EmitSignal(SignalName.MouseoverItemChanged, hoveredSlotItem);
		}
	}

	private void OnMouseEntered(int index)
	{
		if (GetViewport().GuiGetDragData().VariantType != Variant.Type.Nil) return;
		
		hoveredSlotItem = contents[index].As<InventorySlotItem>();
		EmitSignal(SignalName.MouseoverItemChanged, hoveredSlotItem);
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
