using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Inventory
{
    private List<Item> _items = new List<Item>();
    public bool IsActive { get; set; }
    public MenuList _itemMenu = new MenuList();
    private PlayerCharacter _owner;
    
    public Inventory(PlayerCharacter owner)
    {
        _owner = owner;
    }

    public void Add(Item item)
    {
        if (_items.Count >= 10) return;
        
        _items.Add(item);
        _itemMenu.Add(item.Name, item.Use);
        item.Inventory = this;
        item.Owner = _owner;
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        _itemMenu.Remove();
    }

    public void Render()
    {
        if (!IsActive) return;
        
        _itemMenu.Render(15, 1);
    }

    public void Select()
    {
        if(!IsActive) return;
        _itemMenu.Select();
    }

    public void SelectUp()
    {
        if(!IsActive) return;
        _itemMenu.SelectUp();
    }

    public void SelectDown()
    {
        if(!IsActive) return;
        _itemMenu.SelectDown();
    }
    public bool TrySelectSkill()
    {
        if (IsActive) return false; // 인벤토리 켜져있으면 스킬 사용 불가
        return true;                // 사용 가능
    }

    public bool TrySelectAttack()
    {
        if (IsActive) return false; // 인벤토리 켜져있으면 공격 불가
        return true;
    }
}