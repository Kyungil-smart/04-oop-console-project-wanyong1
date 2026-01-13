using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.GameObjects
{
    public class MysticAmulet : Item
    {
        public MysticAmulet()
        {
            Name = "신부한 부적";
        }
        public override void Use()
        {
            if (Owner != null)
            {
                Owner.HasMysticAura = true;
            }
               
            Inventory.Remove(this);
            Inventory = null;
            Owner = null;
            Console.WriteLine("신비한 힘이 느껴진다...");
        }
    }
}
