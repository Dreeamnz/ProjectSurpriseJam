using Godot;
using System;

public class Player : Area2D
{
	[Export]
	public int Speed = 400;
	
	private Vector2 _screenSize;
	 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size	
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
