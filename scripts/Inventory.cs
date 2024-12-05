using Godot;
using System;

public partial class Inventory : CanvasLayer
{
	[Export] int inventorySize = 8;
	[Export] Texture2D blankIcon;
	private ItemList _itemList;
	
	private Item[] _items;
	
	public override void _Ready()
	{
		_itemList = GetNode<ItemList>("ItemList");
		_items = new Item[inventorySize];
		
		for (int i = 0; i < inventorySize; i++)
		{
			_itemList.AddItem(" ", blankIcon);
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
				_itemList.SetItemText(i, "{_items[i].Quantity}");
				return true;
			}
			
			_items[i] = item;
			_itemList.SetItemIcon(i, item.Icon);
			_itemList.SetItemText(i, "{item.Quantity}");
			
			return true;
		}
		
		return false;
	}

	public override void _Process(double delta)
	{
	}
}
