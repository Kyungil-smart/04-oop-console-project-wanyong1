using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ManaPotion : Item, IInteractable
{

    public ManaPotion() => Init();

    private void Init()
    {
        Symbol = 'N';
    }

    public override void Use()
    {
        Owner.ManaHeal(1);

        Inventory.Remove(this);
        Inventory = null;
        Owner = null;
    }

    public void Interact(PlayerCharacter player)
    {
        player.AddItem(this);
    }
}