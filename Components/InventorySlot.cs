using Godot;

[GlobalClass]
public partial class InventorySlot : Panel
{
	[Signal] public delegate void BecameEmptyEventHandler();
	[Signal] public delegate void BecamePopulatedEventHandler(Variant content);

	[Export] private Color colorWhenMoving = new(Colors.White, 0.66f);

	public InventorySlotItem Content { get; private set; }
	
	public override void _Ready()
	{
		UpdateContent();
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (Content == null) return new Variant();
		Content.Modulate = colorWhenMoving;
		SetDragPreview(new TextureRect()
		{
			Size = new Vector2(45, 45),
			ExpandMode = TextureRect.ExpandModeEnum.FitWidth,
			Texture = Content.GetNode<TextureRect>("TextureRect").Texture
		});
		return Content;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.AsGodotObject() is InventorySlotItem;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (data.AsGodotObject() is not InventorySlotItem slotItem)
		{
			GD.PrintErr($"Non-slot item passed to {nameof(_DropData)}");
			return;
		}

		var origin = (InventorySlot)slotItem.GetParent();
		if (Content != null)
			origin.ForceUpdateContent(Content);
		else
			origin.EmitSignal(SignalName.BecameEmpty);
		
		ForceUpdateContent(slotItem);
	}

	public override void _Notification(int what)
	{
		if (what == NotificationDragEnd) UpdateContent();
	}

	public void ForceUpdateContent(InventorySlotItem slotItem)
	{
		slotItem.Reparent(this);
		EmitSignal(SignalName.BecamePopulated, slotItem);
		slotItem.Position = Vector2.Zero;
		Content = slotItem;
	}
	
	/// <summary>
	/// Update 'content' to be 1st valid child, complain if it's something 
	/// </summary>
	private void UpdateContent()
	{
		var children = GetChildren();
		if (children.Count == 0)
		{
			Content = null;
			return;
		}
		
		Content = (InventorySlotItem)children[0];
		
		Content.Modulate = Colors.White;
		
		if (Content == null)
			GD.PrintErr($"Invalid child on {Name} - should be type of {nameof(InventorySlotItem)}.");

		if (children.Count > 1)
		{
			GD.PrintErr($"Invalid child count on {Name} - should be 1.");
			foreach (var child in children)
			{
				if (child == Content) continue;
				child.QueueFree();
			}
		}
	}
}
