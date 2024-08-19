using Godot;

/// <summary>
/// A single slot in an <see cref="InventoryContainer"/>. This handles most of the logic related to handling a click,
/// emitting signals to be consumed by the parent container. Each slot should either have zero children *or* a single
/// <see cref="InventorySlotItem"/>.
/// </summary>
[GlobalClass]
public partial class InventorySlot : Panel
{
	[Signal] public delegate void BecameEmptyEventHandler();
	[Signal] public delegate void BecamePopulatedEventHandler(Variant content);

	/// <summary>
	/// Modulate the preview image to this colour.
	/// </summary>
	[Export] private Color colorWhenMoving = new(Colors.White, 0.66f);

	/// <summary>
	/// If null, slot is empty.
	/// </summary>
	public InventorySlotItem Content { get; private set; }
	
	public override void _Ready()
	{
		CheckAndUpdateContent();
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (Content == null) return new Variant();
		Content.Modulate = colorWhenMoving;

		SetDragPreview(GetDragPreviewItem(Content));

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
		
		//Check if we should switch with previous slot
		if (Content != null)
			origin.SetContent(Content);
		else
			origin.EmitSignal(SignalName.BecameEmpty);
		
		SetContent(slotItem);
	}

	public override void _Notification(int what)
	{
		//Only care about rechecking after finished
		if (what == NotificationDragEnd) CheckAndUpdateContent();
	}

	/// <summary>
	/// Set the content. Doesn't do anything to the origin.
	/// </summary>
	/// <param name="slotItem"></param>
	public void SetContent(InventorySlotItem slotItem)
	{
		slotItem.Reparent(this);
		EmitSignal(SignalName.BecamePopulated, slotItem);
		slotItem.Position = Vector2.Zero;
		Content = slotItem;
	}
	
	/// <summary>
	/// If set, gets <see cref="InventorySlotItem.PreviewItem"/>. Otherwise, generates a simple copy of the item's
	/// texture.
	/// </summary>
	/// <param name="slotItem"></param>
	/// <returns></returns>
	private Control GetDragPreviewItem(InventorySlotItem slotItem)
	{
		if (slotItem.PreviewItem != null && slotItem.PreviewItem.Instantiate() is Control ci)
		{
			return ci;
		}
		
		//None set: fall back
		return new TextureRect
		{
			Size = this.Size,
			ExpandMode = TextureRect.ExpandModeEnum.FitWidth,
			Texture = Content.Texture
		};
	}
	
	/// <summary>
	/// Update <see cref="Content"/> to be 1st valid child, complain if it's something else and clear anything other
	/// than the first item.
	/// </summary>
	private void CheckAndUpdateContent()
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
