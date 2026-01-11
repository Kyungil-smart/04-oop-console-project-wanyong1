using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    public class FrostScene : Scene
    {
        private Tile[,] _field = new Tile[20, 20];
        private PlayerCharacter _player;
        public FrostScene(PlayerCharacter player) => Init(player);
        private Vector _exitPos; // 출구 위치

        public void Init(PlayerCharacter player)
        {
            _player = player;

            for (int y = 0; y < _field.GetLength(0); y++)
            {
                for (int x = 0; x < _field.GetLength(1); x++)
                {
                    Vector pos = new Vector(x, y);
                    _field[y, x] = new Tile(pos);

                    if (y == 0 || y == _field.GetLength(0) - 1 || x == 0 || x == _field.GetLength(1) - 1)
                    {
                        _field[y, x].isWall = true;
                    }
                }
            }
        }

        public override void Enter()
        {
            _player.Field = _field;
            _player.Position = new Vector(1, 1);
            _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;
            Random rng = new Random();

            // 지정 스폰
            MonsterSpawn.Spawn(_field, new Vector(8, 18), hp: 3, name: "Goblin");

            // 랜덤 스폰
            MonsterSpawn.SpawnRandom(_field, rng, hp: 2, name: "Slime");
        }
        public override void Update()
        {
            _player.Update();
           

        }
        public override void Render()
        {
            PrintField();
            _player.Render();
            
        }
        public override void Exit()
        {
            _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
            _player.Field = null;
        }
        private void PrintField()
        {
            for (int y = 0; y < _field.GetLength(0); y++)
            {
                for (int x = 0; x < _field.GetLength(1); x++)
                {
                    _field[y, x].Print();
                }
                Console.WriteLine();
            }
        }
       
    }
}
