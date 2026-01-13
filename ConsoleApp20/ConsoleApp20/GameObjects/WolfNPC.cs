using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.GameObjects
{
    public class WolfBossNpc : GameObject, ITalkable
    {
        public bool HasTransformed { get; private set; }
        public event Action OnTransformRequested;

        public WolfBossNpc()
        {
            Symbol = 'W'; // 엄청 큰 늑대 느낌(표시는 너 마음대로)
        }

        public void Talk(PlayerCharacter player)
        {
            if (HasTransformed) return;
            HasTransformed = true;

            // 대사 띄우고 변신
            player.StartDialogue("거대한 늑대", new[]
            {
            "……",
            "거대한 늑대 : 너였구나. 여기까지 온 인간은 오랜만이다.",
            "거대한 늑대 : 이제부터는… 사냥이다."
        });

            OnTransformRequested?.Invoke();
        }
    }
}
