using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.GameObjects
{
    internal class Spike : GameObject, IInteractable
    {
        public Spike()
        {
            Init();
        }

        public void Interact(PlayerCharacter player)
        {
            //if (player.Position.X != this.Position.X || player.Position.Y != this.Position.Y)
            //{
            //    return;
            //}
            if (player.Health.Value <= 0) return;
            player.Health.Value -= 1;
            player.ConsumeStep(1);
            
        }

        private void Init()
        {
            Symbol = 'ㅿ';
        }

        
    }
}
