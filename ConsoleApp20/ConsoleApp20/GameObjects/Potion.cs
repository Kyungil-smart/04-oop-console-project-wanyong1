using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Potion : Item, IInteractable
{

    public Potion() => Init();
    
    private void Init()
    {
        Symbol = 'I';
    }

    public override void Use()
    {
        Owner.Heal(1);
        
        Inventory.Remove(this);
        Inventory = null;
        Owner = null;
    }

    public void Interact(PlayerCharacter player)
    {
        player.AddItem(this);
    }
}