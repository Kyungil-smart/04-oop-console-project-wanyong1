using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.GameObjects
{
    public class BossWolfMonster : GameObject, IDamageable
    {
        public int Hp { get; private set; } = 999; // 부적 없으면 사실상 못잡게 크게
        public bool IsDead => Hp <= 0;

        public event Action<BossWolfMonster> OnDead;

        public BossWolfMonster()
        {
            Symbol = 'ⓦ'; // 보스 표시(원하는 문자로)
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;

            Hp -= amount;
            if (Hp <= 0)
            {
                Hp = 0;
                OnDead?.Invoke(this);
            }
        }
    }
}
