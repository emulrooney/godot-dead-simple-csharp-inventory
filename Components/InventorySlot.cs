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
		if (content == null) return new Variant();
		content.Modulate = Colors.Red;
		SetDragPreview(new TextureRect()
		{
			Size = new Vector2(45, 45),
			ExpandMode = TextureRect.ExpandModeEnum.FitWidth,
			Texture = content.GetNode<TextureRect>("TextureRect").Texture
		});
		myItemIsBeingDragged = true;
		return content;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		GD.Print($"{Name} + {atPosition}");
		if (atPosition < Vector2.Zero) return false;
		
		return content == null
			   && data.AsGodotObject() is InventorySlotItem;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (data.AsGodotObject() is not InventorySlotItem slotItem)
		{
			GD.PrintErr($"Non-slot item passed to {nameof(_DropData)}");
			return;
		}

		slotItem.Reparent(this);
		slotItem.Position = Vector2.Zero;
		content = slotItem;

		EmitSignal(SignalName.BecamePopulated, slotItem);
		
	}

	public override void _Notification(int what)
	{
		if (what == NotificationDragEnd)
		{
			CallDeferred(MethodName.UpdateContent);
			myItemIsBeingDragged = false;
		}
	}

	/// <summary>
	/// Update 'content' to be 1st valid child, complain if it's something 
	/// </summary>
	private void UpdateContent()
	{
		GD.Print($"Update content on {Name}: {content}");
		
		var children = GetChildren();
		if (children.Count == 0)
		{
			content = null;
			return;
		}
		
		content = (InventorySlotItem)children[0];
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
}
