using ConsoleApp20.Utils;
using System;

namespace ConsoleApp20.GameObjects
{
    public class Npc : GameObject, ITalkable
    {
        public string Name { get; set; }
        public string[] Pages { get; set; } = Array.Empty<string>();

        public Npc()
        {
            Symbol = 'N';
        }

        public void Talk(PlayerCharacter player)
        {
            player.StartDialogue(Name, Pages);
        }
    }
}
