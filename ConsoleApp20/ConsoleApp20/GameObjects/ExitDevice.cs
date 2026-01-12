using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Utils
{
    public class ExitDevice : GameObject
    {
        public bool IsActive { get; private set; }

        private const char INACTIVE = '☆';
        private const char ACTIVE = '★';

        public ExitDevice(bool active = false)
        {
            SetActive(active);
        }

        public void SetActive(bool active)
        {
            IsActive = active;
            Symbol = active ? ACTIVE : INACTIVE;
        }
    }
}
