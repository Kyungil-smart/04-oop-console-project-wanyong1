using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;

namespace ConsoleApp20.Scenes
{
    namespace ConsoleApp20.Scenes
    {
        public class ForestBossScene : Scene
        {
            private Tile[,] _field;
            private PlayerCharacter _player;

            private ExitDevice _exit;
            private Vector _exitPos = new Vector(8, 1);

            private WolfBossNpc _wolfNpc;
            private BossWolfMonster _boss;
            private bool _wasInBossRange = false;
            private bool _HealthZero;
            private bool _battleStarted;

            public ForestBossScene(PlayerCharacter player)
            {
                _player = player;
            }

            public override void Enter()
            {
                BuildField();

                _player.Field = _field;
                _player.Position = new Vector(1, 8);
                _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

                // 출구는 처음 비활성
                _exit = new ExitDevice(false);
                _exit.Position = _exitPos;
                _field[_exitPos.Y, _exitPos.X].OnTileObject = _exit;

                // 늑대 NPC 배치
                _wolfNpc = new WolfBossNpc { Position = new Vector(5, 4) };
                _wolfNpc.OnTransformRequested += TransformToBoss;
                _field[_wolfNpc.Position.Y, _wolfNpc.Position.X].OnTileObject = _wolfNpc;

                // 플레이어 턴(행동)마다 보스 근접공격 체크
                _player.OnMoved += HandlePlayerMoved;
                _player.OnAttacked += HandlePlayerAttacked;


                // 출구 진입
                _player.OnEnterExit = GoToNextScene;

                _player.OnRestartRequested += RestartLevel;
            }

            public override void Update()
            {
                if (_HealthZero)
                {
                    return;
                }
                _player.Update();
                if (_player.Health.Value <= 0)
                {
                    _HealthZero = true;
                    RestartLevel();
                    _HealthZero = false;

                }
            }

            public override void Render()
            {
                PrintField();
                _player.Render();
            }

            public override void Exit()
            {
                _player.OnMoved -= HandlePlayerMoved;
                _player.OnAttacked -= HandlePlayerAttacked;

                _player.OnRestartRequested -= RestartLevel;
                _player.OnEnterExit = null;

                if (_wolfNpc != null) _wolfNpc.OnTransformRequested -= TransformToBoss;
                if (_boss != null) _boss.OnDead -= HandleBossDead;

                _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
                _player.Field = null;
            }

            private void TransformToBoss()
            {
                if (_battleStarted) return;
                _battleStarted = true;

                var pos = _wolfNpc.Position;

                _field[pos.Y, pos.X].OnTileObject = null;

                _boss = new BossWolfMonster { Position = pos };
                _boss.OnDead += HandleBossDead;
                _field[pos.Y, pos.X].OnTileObject = _boss;
            }

            


            private void HandleBossDead(BossWolfMonster boss)
            {
                // 보스 제거
                var p = boss.Position;
                _field[p.Y, p.X].OnTileObject = null;

                // 출구 활성화
                _exit.SetActive(true);

                _player.StartDialogue("나", new[]
                {
                "……끝났다.",
                "출구가 열렸다. 이제 나갈 수 있어."
                }
            
                );
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

            private void RestartLevel()
            {
                _player.TownChiefRewardGiven = false;

                _player.Health.Value = 5;
                _player.Mana.Value = 5;
                _player.HasMysticAura = false;

                Exit();
                SceneManager.Change("NewTownScene");
            }
            private void GoToNextScene()
            {
                if (_exit == null || !_exit.IsActive) return;

                _player.Health.Value = 5;
                _player.Mana.Value = 5;


                SceneManager.Change("Finsh");
            }
            private void HandlePlayerMoved()
            {
                // 전투 전/보스 없음/사망이면 상태 초기화
                if (!_battleStarted || _boss == null || _boss.IsDead)
                {
                    _wasInBossRange = false;
                    return;
                }

                bool inRange = IsAdjacent8(_player.Position, _boss.Position);

                if (inRange && !_wasInBossRange)
                    BossAttack();

                // 범위 상태 갱신 (밖으로 나가면 다시 ‘진입 즉시 공격’ 가능)
                _wasInBossRange = inRange;
            }

            private void HandlePlayerAttacked()
            {
                if (!_battleStarted || _boss == null || _boss.IsDead) return;

                // 플레이어가 공격하면 즉시 재공격(8방 범위일 때만)
                if (IsAdjacent8(_player.Position, _boss.Position))
                    BossAttack();
            }

            private void BossAttack()
            {
                _player.Health.Value -= 3;
                _player.SetHudMessage("거대한 늑대의 발톱!  HP -3", 40);
            }

            private bool IsAdjacent8(Vector a, Vector b)
            {
                int dx = Math.Abs(a.X - b.X);
                int dy = Math.Abs(a.Y - b.Y);

                // 8방 인접(대각 포함), 같은 칸 제외
                return dx <= 1 && dy <= 1 && !(dx == 0 && dy == 0);
            }


        }
    }
}
