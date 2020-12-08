using Godot;
using System;

public class SchoolsBackyard : Node
{
	[Export]
	public PackedScene Mob;

	private Random _random = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	private float RandRange(float min, float max)
	{
		return (float)_random.NextDouble() * (max - min) + min;
	}

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
	}

	public void NewGame()
	{
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Position2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	public void OnStartTimerTimeout() 
	{
		GetNode<Timer>("MobTimer").Start();
	}

	public void OnMobTimerTimeout() 
	{
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.Offset = _random.Next();

		var mobInstance = (KinematicBody2D)Mob.Instance();
		AddChild(mobInstance);   
		
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		mobInstance.Position = mobSpawnLocation.Position;

		direction += RandRange(-Mathf.Pi / 4, Mathf.Pi /4);
		mobInstance.Rotation = direction;
	}
}
