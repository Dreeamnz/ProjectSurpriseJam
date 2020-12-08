using Godot;
using System;

public class Mob : KinematicBody2D
{
    [Export]
    public int MaxSpeed = 250;

    static private Random _random = new Random();

    public Vector2 Velocity = Vector2.Zero;
    
    public override void _Ready()
    {
         var animeSprite = GetNode<AnimatedSprite>("AnimatedSprite");
         var mobTypes = animeSprite.Frames.GetAnimationNames();
         animeSprite.Animation = mobTypes[_random.Next(0, mobTypes.Length)];
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        var swordNode = GetParent().GetNode<Area2D>("Player").GetNodeOrNull<Area2D>("SwordHitbox");
        if(swordNode != null && !swordNode.IsConnected("EnemyHit", this, "HurtSignalReceived"))
            swordNode.Connect("EnemyHit", this, "HurtSignalReceived");
    }

    public override void _PhysicsProcess(float delta) 
    {
        var targetGlobalPosition = GetParent().GetNode<Area2D>("Player").Position;
        Velocity = Steering.follow(Velocity, GlobalPosition, targetGlobalPosition, MaxSpeed);
        Velocity = MoveAndSlide(Velocity);
    }

    public void HurtSignalReceived(KinematicBody2D enemy)
    {
        if(GetInstanceId() == enemy.GetInstanceId())
            QueueFree();
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
