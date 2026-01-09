using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Utils
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        bool IsDead { get; }
    }

}
