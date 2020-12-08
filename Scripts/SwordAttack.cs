using Godot;
using System;

public class SwordAttack : Area2D
{
    [Signal]
    public delegate void EnemyHit(KinematicBody2D enemy);

    public override void _Ready()
    {
        GetNode<Timer>("HitboxTimer").Start();
    }

    public void OnHitboxTimerTimeout()
    {
        GetParent<Player>().ACTION_STATE = SENPAI_ACTION_STATE.Idle;
        QueueFree();
    }

    public void OnSwordHitboxBodyEntered(KinematicBody2D body) 
    {
        EmitSignal("EnemyHit", new object[] { body });
    }
}
