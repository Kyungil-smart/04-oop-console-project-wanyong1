using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    public class ForestScene : Scene
    {
        private Tile[,] _field = new Tile[9, 6];
        private PlayerCharacter _player;
        private ExitDevice _exit;
        private Vector _exitPos = new Vector(8, 1); 
        private bool _pendingExitActivation;
        private const int STEP_LIMIT = 20; 
        private int _stepsLeft;
        private MonsterSpawn _monsterSpawn;

        public ForestScene(PlayerCharacter player)
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
            _player.Position = new Vector(1, 6);
            _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

            _player.TryPushBlockHandler = TryPushBlock;

 
            AddPushBlock(7, 2);
            AddPushBlock(7, 4);
            AddPushBlock(6, 4);
            AddPushBlock(5, 4);
            AddPushBlock(4, 4);
            AddPushBlock(3, 4);
            AddPushBlock(3, 3);
            AddPushBlock(2, 4);
            AddPushBlock(1, 4);
                
      

            AddPushBlock(12, 4);
            _monsterSpawn.Spawn(_field, new Vector(7, 2), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(7, 2), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(7,5), 5, "늑대");
            _monsterSpawn.Spawn(_field, new Vector(7, 4), 5, "늑대");

            _field[3, 7].OnTileObject = new Npc()
            {
                Name = "길을 잃은 아이 1",
                Pages = new[]
                {
                "길을 잃은 아이 1 : 살려주세요.",

                "길을 잃은 아이 1 : 저는 이 숲에서 길을 잃었어요.",

                "길을 잃은 아이 1 : 도와주실 수 있나요?",
                "MY : 당연하지",

                "길을 잃은 아이 1 : 제 친구가 어딘가에 있을 거예요.",

                "길을 잃은 아이 1 : 찾아서 돌아와 주세요 저는 마을로 돌아가 있을께요.",
                
                " MY : (저기 출구가 활성화 되었다 저기로 가야겠어!)"
                }
            };
            _exit = new ExitDevice(false);
            _exit.Position = _exitPos;
            _field[_exitPos.Y, _exitPos.X].OnTileObject = _exit;

            _player.OnEnterExit = GoToForest;

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
            SceneManager.Change("Forest2");
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
            if (speaker == "길을 잃은 아이 1")
                _pendingExitActivation = true;
        }

        private void HandleDialogueClosed()
        {
            if (!_pendingExitActivation) return;
            _pendingExitActivation = false;

            if (_exit != null && !_exit.IsActive)
                _exit.SetActive(true); // ☆ -> ★
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

            Console.SetCursorPosition(x, y);
            $"STEP : {_stepsLeft}/{STEP_LIMIT}   ".Print();
        }


    }
}
