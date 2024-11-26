using Godot;
using System;

public partial class Drone : CharacterBody2D
{
	[Export]
	private float followSpeed = 200.0f; // Velocidad del Drone
	[Export]
	private float followDistance = 100.0f; // Distancia mínima del Player

	private Node2D player; // Referencia al Player
	private Vector2 velocity; // Velocidad actual del Drone

	public override void _Ready()
	{
		// Busca al nodo Player en el árbol de nodos
		player = GetParent().GetNode<Node2D>("Player");

		if (player == null)
		{
			GD.PrintErr("Player no encontrado en el nodo padre.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (player == null) return;

		Vector2 directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
		float distanceToPlayer = (player.GlobalPosition - GlobalPosition).Length();

		// Si está más lejos de la distancia mínima, se mueve hacia el Player
		if (distanceToPlayer > followDistance)
		{
			velocity = directionToPlayer * followSpeed;
		}
		else
		{
			// Si está dentro de la distancia mínima, se detiene
			velocity = Vector2.Zero;
		}

		// Mueve el Drone
		Velocity = velocity;
		MoveAndSlide();
	}
}
