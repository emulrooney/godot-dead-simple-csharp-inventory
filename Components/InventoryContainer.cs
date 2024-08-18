using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotArray = Godot.Collections.Array;

[GlobalClass]
public partial class InventoryContainer : Control
{
	[Signal] public delegate void MouseoverItemChangedEventHandler(InventorySlot slot);
	[Signal] public delegate void ContentsChangedEventHandler(GodotArray arr);
	[Export] private Node[] contentHolders;

	private GodotArray contents = new();
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
	/// Get all contents from scratch.
	/// </summary>
	/// <returns></returns>
	private IEnumerable<InventorySlot> CollectSlots()
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
