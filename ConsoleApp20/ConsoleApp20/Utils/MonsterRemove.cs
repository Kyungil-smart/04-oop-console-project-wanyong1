using ConsoleApp20.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Utils
{
    public static class MonsterRemove
    {

        private static readonly Dictionary<Monster, Action<Monster>> _handlers = new Dictionary<Monster, Action<Monster>>();


        public static void BindAutoRemove(Tile[,] field, Monster monster)
        {
            if (field == null || monster == null) return;

            if (_handlers.TryGetValue(monster, out var oldHandler))
            {
                monster.OnDead -= oldHandler;
                _handlers.Remove(monster);
            }

            Action<Monster> handler = _ => Remove(field, monster);

            _handlers[monster] = handler;
            monster.OnDead += handler;
        }


        public static bool Remove(Tile[,] field, Monster monster)
        {
            if (field == null || monster == null) return false;

            int h = field.GetLength(0);
            int w = field.GetLength(1);

            int x = monster.Position.X;
            int y = monster.Position.Y;

            if (x < 0 || y < 0 || x >= w || y >= h) return false;

            if (field[y, x].OnTileObject == monster)
            {
                field[y, x].OnTileObject = null;

                if (_handlers.TryGetValue(monster, out var handler))
                {
                    monster.OnDead -= handler;
                    _handlers.Remove(monster);
                }

                return true;
            }

            return false;
        }

        public static bool RemoveAt(Tile[,] field, Vector pos)
        {
            if (field == null) return false;

            int h = field.GetLength(0);
            int w = field.GetLength(1);

            if (pos.X < 0 || pos.Y < 0 || pos.X >= w || pos.Y >= h) return false;

            if (field[pos.Y, pos.X].OnTileObject is Monster m)
                return Remove(field, m);

            return false;
        }
    }
}
