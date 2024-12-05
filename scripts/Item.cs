using Godot;
using System;

public class Item {
	public string Name;
	public Texture2D Icon;
	public int Quantity = 1;
	
	public Item(string name, Texture2D icon, int quantity = 1)
	{
		Name = name;
		Icon = icon;
		Quantity = quantity;
	}
}
