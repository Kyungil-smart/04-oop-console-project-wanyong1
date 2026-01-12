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

            // 범위 체크
            if (pos.X < 0 || pos.Y < 0 || pos.X >= w || pos.Y >= h) return null;

            // 벽이면 스폰 불가
            if (field[pos.Y, pos.X].isWall) return null;

            // 이미 뭔가 있으면 스폰 불가
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

        // 랜덤 스폰 (통로 + 빈칸)
        public  Monster SpawnRandom(Tile[,] field, Random rng, int hp = 5, string name = null, bool autoRemoveOnDead = true, int maxTries = 500)
                                          
        {
            if (field == null || rng == null) return null;

            int h = field.GetLength(0);
            int w = field.GetLength(1);

            for (int i = 0; i < maxTries; i++)
            {
                int x = rng.Next(0, w);
                int y = rng.Next(0, h);

                if (field[y, x].isWall) continue;
                if (field[y, x].OnTileObject != null) continue;

                return Spawn(field, new Vector(x, y), hp, name, autoRemoveOnDead);
            }

            return null;
        }
    }
}
