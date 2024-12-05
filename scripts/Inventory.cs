using Godot;
using System;

public partial class Inventory : ItemList
{
	[Export] int inventorySize = 8;
	[Export] Texture2D blankIcon;
	
	private Item[] _items;
	
	public override void _Ready()
	{
		_items = new Item[inventorySize];
		
		for (int i = 0; i < inventorySize; i++)
		{
			AddItem(" ", blankIcon);
		}
	}
	
	public bool AddInventoryItem(Item item)
	{
		if (item == null || item.Quantity <= 0) return false;
		
		if (item.Quantity == 0) return true;
		
		for (int i = 0; i < inventorySize; i++)
		{
			if (_items[i] != null & _items[i].Name == item.Name)
			{
				_items[i].Quantity++;
				SetItemText(i, "x{_items[i].Quantity}");
				return true;
			}
			
			_items[i] = item;
			SetItemIcon(i, item.Icon);
			SetItemText(i, "x{item.Quantity}");
			
			return true;
		}
		
		return false;
	}

	public override void _Process(double delta)
	{
	}
}
