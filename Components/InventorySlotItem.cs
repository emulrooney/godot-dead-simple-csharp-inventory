using System;
using Godot;

public partial class InventorySlotItem : Control
{
	public bool IsBeingDragged { get; private set; }

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Pass;
	}

	public override void _Process(double delta)
	{
		if (IsBeingDragged)
		{
			GlobalPosition = GetGlobalMousePosition();
		}
	}

	public void StartDrag()
	{
		IsBeingDragged = true;
		Modulate = new Color(Colors.White, 0.25f);
	}

	public void ReleaseDrag()
	{
		Modulate = Colors.White;
		var t = GetTree().CreateTween();
		t.TweenProperty(this, PropertyName.Position.ToString(), Vector2.Zero, 0.1f);
		t.TweenCallback(Callable.From(() => { IsBeingDragged = false; }));
		t.Play();
	}
}
