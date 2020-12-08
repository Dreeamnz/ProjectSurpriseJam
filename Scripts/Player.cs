using Godot;
using System;

public class Player : Area2D
{
	[Export]
	public int Speed = 400;

	[Export]
	public PackedScene SwordHitbox;

	[Export]
	public SENPAI_ACTION_STATE ACTION_STATE;

	private SENPAI_DIRECTION_STATE _DIRECTION_STATE;
	private Vector2 _screenSize;
	
	[Signal]
	public delegate void Hit();
	 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		_screenSize = GetViewport().Size;
	}

	public void OnPlayerAreaEntered(Area2D area)
	{
		Hide();
		EmitSignal("Hit");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	}
	
	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public override void _Process(float delta)
	{
		MoveSenpai(delta, InputSenpaiState());
	}

	private bool InputSenpaiState()
	{
		bool isInputPressed = false;
		if(Input.IsActionPressed("ui_right")) 
		{
			if(Input.IsActionPressed("ui_up"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.UpRight;
			else if(Input.IsActionPressed("ui_down"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.DownRight;
			else
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.Right;
			
			isInputPressed = true;
		}

		if(Input.IsActionPressed("ui_left")) {
			if(Input.IsActionPressed("ui_up"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.UpLeft;
			else if(Input.IsActionPressed("ui_down"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.DownLeft;
			else
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.Left;
			
			isInputPressed = true;
		}

		if (Input.IsActionPressed("ui_down"))
		{
			if(Input.IsActionPressed("ui_left"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.DownLeft;
			else if(Input.IsActionPressed("ui_right"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.DownRight;
			else
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.Down;
			
			isInputPressed = true;
		}

		if (Input.IsActionPressed("ui_up"))
		{
			if(Input.IsActionPressed("ui_left"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.UpLeft;
			else if(Input.IsActionPressed("ui_right"))
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.UpRight;
			else
				_DIRECTION_STATE = SENPAI_DIRECTION_STATE.Up;
			
			isInputPressed = true;
		}

		if(ACTION_STATE != SENPAI_ACTION_STATE.Attacking && 
			Input.IsActionJustPressed("attack"))
		{
			ACTION_STATE = SENPAI_ACTION_STATE.Attacking;
			GenerateHitbox();
		}

		return isInputPressed;
	}

	private void GenerateHitbox()
	{
		var senpaiSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		var swordHitbox = (Area2D)SwordHitbox.Instance();
		AddChild(swordHitbox);

		var hbSprite = swordHitbox.GetNode<Sprite>("square");
		var hbCollision = swordHitbox.GetNode<CollisionShape2D>("hbCollision");

		float scaleX = 0, scaleY = 0, posX = 0, posY = 0;
		switch (_DIRECTION_STATE)
			{
				case SENPAI_DIRECTION_STATE.Up:
				{
					posY = -40;	
					scaleX = (float)1.5;
				}break;
				case SENPAI_DIRECTION_STATE.UpLeft:
				{
					posY = -20;
					posX = -20;
					scaleY = (float)0.7;
				}break;
				case SENPAI_DIRECTION_STATE.UpRight:
				{
					posY = -20;
					posX = 20;
					scaleY = (float)0.7;
				}break;
				case SENPAI_DIRECTION_STATE.Down:
				{
					posY = 40;	
					scaleX = (float)1.5;
				}break;
				case SENPAI_DIRECTION_STATE.DownLeft:
				{
					posY = 20;
					posX = -20;
					scaleY = (float)0.7;
				}break;
				case SENPAI_DIRECTION_STATE.DownRight:
				{
					posY = 20;
					posX = 20;
					scaleY = (float)0.7;
				}break;
				case SENPAI_DIRECTION_STATE.Left:
				{
					posX = -20;
					scaleY = (float)1.5;
				}break;
				case SENPAI_DIRECTION_STATE.Right:
				{
					posX = 20;
					scaleY = (float)1.5;
				}break;
			}
		
		var spriteScale = new Vector2(
			senpaiSprite.Scale.x + scaleX, 
			senpaiSprite.Scale.y + scaleY);
		var spritePosition = new Vector2(posX, posY);
		
		hbSprite.Scale = spriteScale;
		hbSprite.Position = spritePosition;
	}

	private void AnimateSenpai(bool isMoving)
	{
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		if(ACTION_STATE == SENPAI_ACTION_STATE.Attacking)
		{ }
		else if(isMoving) 
		{
			ACTION_STATE = SENPAI_ACTION_STATE.Running;
			animatedSprite.Play("Run" + _DIRECTION_STATE.ToString());
		}
		else
		{
			ACTION_STATE = SENPAI_ACTION_STATE.Idle;
			animatedSprite.Play("Idle" + _DIRECTION_STATE.ToString());
		}
	}

	private void MoveSenpai(float delta, bool isInputPressed)
	{
		var velocity = new Vector2();

		if(isInputPressed)
		{
			if(_DIRECTION_STATE.ToString().Contains("Right"))
				velocity.x += 1;
			if(_DIRECTION_STATE.ToString().Contains("Left"))
				velocity.x -= 1;
			if(_DIRECTION_STATE.ToString().Contains("Up"))
				velocity.y -= 1;
			if(_DIRECTION_STATE.ToString().Contains("Down"))
				velocity.y += 1;
		}

		bool isMoving = velocity.Length() > 0; 
		if(isMoving)
			velocity = velocity.Normalized() * Speed;

		AnimateSenpai(isMoving);

		if(ACTION_STATE != SENPAI_ACTION_STATE.Attacking)
		{
			Position += velocity * delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.x, 0, _screenSize.x),
				y: Mathf.Clamp(Position.y, 0, _screenSize.y)
			);
		}
	}
}

public enum SENPAI_DIRECTION_STATE 
{
	Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight
}

public enum SENPAI_ACTION_STATE
{
	Running, Attacking, Idle
}