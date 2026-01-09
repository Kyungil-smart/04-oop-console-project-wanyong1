using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.GameObjects
{
    public class Monster : GameObject, IDamageable
    {
        public int Hp { get; private set; } = 5;
        public bool IsDead => Hp <= 0;
        public string _monsterName{  get; set; }

        public event Action<Monster> OnDead;

        public Monster() => Init();
        private void Init()
        {
            Symbol = 'M';
        }
        public Monster(int hp = 5)
        {
            Hp = hp;

        }


        public void TakeDamage(int damage)
        {
            if (IsDead) return; // 이미 죽었으면 무시

            Hp -= damage;

            if (Hp <= 0)
            {
                Hp = 0;
                OnDead?.Invoke(this);   //  “나 죽었어”만 알림
            }
        }
    }
}
