using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;

namespace ConsoleApp20.Scenes
{
    public class DeepForest : Scene
    {
        private Tile[,] _field;
        private PlayerCharacter _player;

        private ExitDevice _exit;
        private Vector _exitPos = new Vector(8, 1);

        private bool _pendingExitActivation;

        public DeepForest(PlayerCharacter player)
        {
            _player = player;
        }

        public override void Enter()
        {
            BuildField();

            // 플레이어 배치
            _player.Field = _field;
            _player.Position = new Vector(1, 6);
            _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

            // NPC(발자국) 배치
            _field[3, 7].OnTileObject = new Npc()
            {
                Name = "발자국",
                Pages = new[]
                {
                    "MY : (땅에 찍힌 발자국을 발견했다.)",
                    "MY : (…크다. 사람이 남긴 흔적이 아니다.)",
                    "",
                    "MY : (발자국 가장자리가 깊게 파여 있어… 무게가 엄청나.)",
                    "MY : (이건… 커다란 짐승의 발자국인 것 같다.)",
                    "",
                    "MY : (발자국이 한 방향으로 이어진다.)",
                    "MY : (숲 깊은 곳으로… 놈이 있는 곳까지 따라가볼까?)"
                }
            };

            // 출구(처음엔 비활성)
            _exit = new ExitDevice(false);
            _exit.Position = _exitPos;
            _field[_exitPos.Y, _exitPos.X].OnTileObject = _exit;

            // 출구 진입 시 씬 이동
            _player.OnEnterExit = GoToForest;

            // 대화 이벤트: 발자국과 대화 후 출구 활성화
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
        }

        public override void Exit()
        {
            // 타일에서 플레이어 제거
            _field[_player.Position.Y, _player.Position.X].OnTileObject = null;

            _player.Field = null;
            _player.OnEnterExit = null;

            _player.OnDialogueOpened -= HandleDialogueOpened;
            _player.OnDialogueClosed -= HandleDialogueClosed;
        }

        private void BuildField()
        {
            // ⚠ 기존 코드에서 배열 크기(9,6)로는 좌표 (3,7), (8,1), (1,6) 사용 불가라 10x10으로 고정
            _field = new Tile[10, 10];

            for (int y = 0; y < _field.GetLength(0); y++)
            {
                for (int x = 0; x < _field.GetLength(1); x++)
                {
                    Vector pos = new Vector(x, y);
                    _field[y, x] = new Tile(pos);

                    // 테두리 벽
                    if (y == 0 || y == _field.GetLength(0) - 1 || x == 0 || x == _field.GetLength(1) - 1)
                        _field[y, x].isWall = true;
                }
            }
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

        private void HandleDialogueOpened(string speaker)
        {
            // "발자국"과 대화를 시작했을 때만, 닫히면 출구 활성화하도록 플래그
            if (speaker == "발자국")
                _pendingExitActivation = true;
        }

        private void HandleDialogueClosed()
        {
            if (!_pendingExitActivation) return;
            _pendingExitActivation = false;

            if (_exit != null && !_exit.IsActive)
                _exit.SetActive(true); // ☆ -> ★
        }

        private void GoToForest()
        {
            // 원래 너가 쓰던 이동 대상 유지
            SceneManager.Change("PrveBoss");
        }
    }
}
