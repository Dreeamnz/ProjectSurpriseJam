using Godot;
using System;

public class Steering : Node {
    public const float DEFAULT_MASS = (float)2.0,
        DEFAULT_MAX_SPEED = (float)200.0;

    public static Vector2 follow(Vector2 velocity, Vector2 globalPosition, 
        Vector2 targetPosition, float maxSpeed = DEFAULT_MAX_SPEED, 
        float mass = DEFAULT_MASS)
    {
        var desiredVelocity = (targetPosition - globalPosition).Normalized() * maxSpeed;
        var steering = (desiredVelocity - velocity) / mass;
        
        return velocity + steering;
    }
}