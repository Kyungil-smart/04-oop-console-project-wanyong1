using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    public class ForestScene3 : Scene
    {
        private Tile[,] _field = new Tile[10, 10];
        private PlayerCharacter _player;
        private ExitDevice _exit;
        private Vector _exitPos = new Vector(8, 1); 
        private bool _pendingExitActivation;
        private const int STEP_LIMIT = 20; 
        private int _stepsLeft;
        private MonsterSpawn _monsterSpawn;

        public ForestScene3(PlayerCharacter player)
        {
            Init(player);
        }


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
                        _field[y, x].isWall = true;
                }
            }
            _monsterSpawn = new MonsterSpawn();

        }

        public override void Enter()
        {
            BuildField();             
            _stepsLeft = STEP_LIMIT;   

            _player.OnStepConsumed += HandleStepConsumed;

            _player.Field = _field;
            _player.Position = new Vector(1, 8);
            _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

          
            _player.TryPushBlockHandler = TryPushBlock;

      
            AddPushBlock(2, 1);
            AddPushBlock(1, 4);
            AddPushBlock(2, 4);
            AddPushBlock(3, 4);
            AddPushBlock(1, 5);
            AddPushBlock(2, 8);
            AddPushBlock(3, 5);
            AddPushBlock(6, 5);
            AddPushBlock(8, 5);
            AddPushBlock(3, 6);
            AddPushBlock(1, 7);
            AddPushBlock(2, 7);
            AddPushBlock(4, 7);
            AddPushBlock(7, 2);
            AddPushBlock(6, 7);
            AddPushBlock(8, 7);
            AddPushBlock(3, 8);
            AddPushBlock(5, 8);
            AddPushBlock(7, 8);
            AddPushBlock(8, 2);

            _monsterSpawn.Spawn(_field, new Vector(5, 1), 5, "늑대");
       
            _monsterSpawn.Spawn(_field, new Vector(5, 3), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(4, 3), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(3, 3), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(6, 3), 5, "늑대");
            _player.OnEnterExit = GoToForest;


            _field[1, 4].OnTileObject = new Npc()
            {
                Name = "길을 잃은 소년",
                Pages = new[]
                {
                "NPC : 살려주세요.",
                "NPC : 숲에서 길을 잃었어요",
                "NPC : 도와주실 수 있나요?",
                "NPC : 제 친구는 어디에 있나요?",
                "My : 친구는 이미 마을에 안전하게 도착했어",
                "NPC : 정말요? 고마워요!",
                "NPC : 더 깊은 숲으로 가면 친구가 더 있을거에요 그 친구들도 찾아주세요",
                "(저기 출구가 활성화 되었다 저기로 가야겠어!)"

                }
            };
            _exit = new ExitDevice(false);
            _exit.Position = _exitPos;
            _field[_exitPos.Y, _exitPos.X].OnTileObject = _exit;

       
            _player.OnDialogueOpened += HandleDialogueOpened;
            _player.OnDialogueClosed += HandleDialogueClosed;
        }

        public override void Update()
        {
            _player.Update();
        }

        public override void Render()
        {
            PrintField();
            _player.Render();
            DrawStepCounter();

        }
        private void GoToForest()
        {
            _player.Health.Value = 5;
            _player.Mana.Value = 5;
            SceneManager.Change("Forest3");
        }

        public override void Exit()
        {
            _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
            _player.Field = null;

      
            _player.TryPushBlockHandler = null;
            _player.OnDialogueOpened -= HandleDialogueOpened;
            _player.OnDialogueClosed -= HandleDialogueClosed;
            _player.OnEnterExit = null;
            _player.OnStepConsumed -= HandleStepConsumed;

        }

        private void PrintField()
        {
            for (int y = 0; y < _field.GetLength(0); y++)
            {
                for (int x = 0; x < _field.GetLength(1); x++)
                    _field[y, x].Print();

                Console.WriteLine();
            }
        }


        public void AddPushBlock(int x, int y)
        {
            if (!IsInBounds(x, y)) return;
            if (_field[y, x].isWall) return;
            if (_field[y, x].OnTileObject != null) return;

            var block = new PushBlock { Position = new Vector(x, y) };
            _field[y, x].OnTileObject = block;
        }

        private bool TryPushBlock(Vector playerPos, Vector dir)
        {
            Vector blockPos = playerPos + dir;     
            Vector targetPos = blockPos + dir;      

        
            if (!IsInBounds(blockPos.X, blockPos.Y)) return false;
            if (!IsInBounds(targetPos.X, targetPos.Y)) return false;

            var blockObj = _field[blockPos.Y, blockPos.X].OnTileObject;
            var block = blockObj as PushBlock;
            if (block == null) return false;


            if (_field[targetPos.Y, targetPos.X].isWall) return false;
            if (_field[targetPos.Y, targetPos.X].OnTileObject != null) return false;


            _field[blockPos.Y, blockPos.X].OnTileObject = null;
            _field[targetPos.Y, targetPos.X].OnTileObject = block;
            block.Position = targetPos;

            return true;
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _field.GetLength(1) && y < _field.GetLength(0);
        }
        private void HandleDialogueOpened(string speaker)
        {

            if (speaker == "길을 잃은 소년")
                _pendingExitActivation = true;
        }

        private void HandleDialogueClosed()
        {
            if (!_pendingExitActivation) return;
            _pendingExitActivation = false;

            if (_exit != null && !_exit.IsActive)
                _exit.SetActive(true); //
        }
        private void BuildField()
        {
            _field = new Tile[10, 10];

            for (int y = 0; y < _field.GetLength(0); y++)
            {
                for (int x = 0; x < _field.GetLength(1); x++)
                {
                    Vector pos = new Vector(x, y);
                    _field[y, x] = new Tile(pos);

                    if (y == 0 || y == _field.GetLength(0) - 1 || x == 0 || x == _field.GetLength(1) - 1)
                        _field[y, x].isWall = true;
                }
            }
        }
        private void HandleStepConsumed()
        {
            _stepsLeft--;
            if (_stepsLeft > 0) return;

            RestartLevel();
        }

        private void RestartLevel()
        {
     
            _player.Health.Value = 5;
            _player.Mana.Value = 5;

            Exit();
            Enter();
        }
        private void DrawStepCounter()
        {
            int x = 0;
            int y = Console.WindowHeight - 3;
            if (y < 0) return;

            Console.SetCursorPosition(x, y);
            $"STEP : {_stepsLeft}/{STEP_LIMIT}   ".Print();
        }


    }
}
