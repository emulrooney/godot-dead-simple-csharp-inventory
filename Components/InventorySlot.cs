using Godot;

public partial class InventorySlot : Panel
{
	[Signal] public delegate void BecameEmptyEventHandler();
	[Signal] public delegate void BecamePopulatedEventHandler(Variant content);

	private InventorySlotItem content;
	private bool myItemIsBeingDragged = false;
	
	public override void _Ready()
	{
		UpdateContent();
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (!InBounds(atPosition) || content == null) return new Variant();

		myItemIsBeingDragged = true;
		content.StartDrag();
		return content;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.AsGodotObject() is InventorySlotItem 
			   && InBounds(atPosition);
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (data.AsGodotObject() is not InventorySlotItem slotItem)
		{
			GD.PrintErr($"Non-slot item passed to {nameof(_DropData)}");
			return;
		}

		slotItem.Reparent(this);
		content = slotItem;
	}

	public override void _Notification(int what)
	{
		if (what != NotificationDragEnd) return;
		if (content == null) return;
		
		content.ReleaseDrag();
		if (myItemIsBeingDragged)
		{
			CallDeferred(MethodName.UpdateContent);
		}
		
		myItemIsBeingDragged = false;
	}

	/// <summary>
	/// Update 'content' to be 1st valid child, complain if it's something 
	/// </summary>
	private void UpdateContent()
	{
		var children = GetChildren();
		if (children.Count == 0) return;
		
		content = children[0] as InventorySlotItem;
		content.Modulate = Colors.White;
		
		if (content == null)
			GD.PrintErr($"Invalid child on {Name} - should be type of {nameof(InventorySlotItem)}.");

		if (children.Count > 1)
		{
			GD.PrintErr($"Invalid child count on {Name} - should be 1.");
			foreach (var child in children)
			{
				if (child == content) continue;
				child.QueueFree();
			}
		}
	}

	private bool InBounds(Vector2 pos)
	{
		return pos >= Vector2.Zero && pos <= Size;
	}
}
