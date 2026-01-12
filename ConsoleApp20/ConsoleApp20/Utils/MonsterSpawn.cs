using ConsoleApp20.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Utils
{
    public  class MonsterSpawn
    {
        // 지정 좌표 스폰
        public  Monster Spawn(Tile[,] field, Vector pos, int hp = 5, string name = null, bool autoRemoveOnDead = true)
        {
            if (field == null) return null;

            int h = field.GetLength(0);
            int w = field.GetLength(1);

            if (pos.X < 0 || pos.Y < 0 || pos.X >= w || pos.Y >= h) return null;

            if (field[pos.Y, pos.X].isWall) return null;

            if (field[pos.Y, pos.X].OnTileObject != null) return null;

            
            var monster = new Monster(hp)
            {
                Position = pos,
                _monsterName = name
            };

            field[pos.Y, pos.X].OnTileObject = monster;

            if (autoRemoveOnDead)
                MonsterRemove.BindAutoRemove(field, monster);

            return monster;
        }
    }
}
