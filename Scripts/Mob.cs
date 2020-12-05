using Godot;
using System;

public class Mob : Area2D
{
    [Export]
    public int MinSpeed = 150;

    [Export]
    public int MaxSpeed = 250;

    static private Random _random = new Random();

    public override void _Ready()
    {
        var animeSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var mobTypes = animeSprite.Frames.GetAnimationNames();
        animeSprite.Animation = mobTypes[_random.Next(0, mobTypes.Length)];
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    public void Move()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
