using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TownScene : Scene
{
    private Tile[,] _field = new Tile[20, 20];
    private PlayerCharacter _player;
    public TownScene(PlayerCharacter player) => Init(player);
    private Vector _exitPos; // 출구 위치
    private const string NEXT_SCENE_KEY = "Forest"; //다음 씬 키
    public void Init(PlayerCharacter player)
    {
        _player = player;

        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _field[y, x] = new Tile(pos);

                // 테두리 벽
                if (y == 0 || y == _field.GetLength(0) - 1 || x == 0 || x == _field.GetLength(1) - 1)
                {
                    _field[y, x].isWall = true;
                }
            }
        }

        // ✅ 출구: 마지막 칸
        _exitPos = new Vector(_field.GetLength(1) - 1, _field.GetLength(0) - 1);

        // ✅ 출구칸 벽 해제
        _field[_exitPos.Y, _exitPos.X].isWall = false;

        // ✅ 접근 가능하게 옆칸 2개도 벽 해제 (범위 체크 포함)
        if (_exitPos.X - 1 >= 0)
            _field[_exitPos.Y, _exitPos.X - 1].isWall = false;

        if (_exitPos.Y - 1 >= 0)
            _field[_exitPos.Y - 1, _exitPos.X].isWall = false;
    }


    public override void Enter()
    {
        _player.Field = _field;
        _player.Position = new Vector(4, 2);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

        _field[_exitPos.Y, _exitPos.X].OnTileObject = new ExitDevice();//출구

        //_field[3, 5].OnTileObject = new Potion() { Name = "Potion1" };
        //_field[2, 15].OnTileObject = new Potion() { Name = "Potion2" };
        //_field[7, 3].OnTileObject = new ManaPotion() { Name = "Potion3" };
        //_field[8, 18].OnTileObject = new ManaPotion() { Name = "Potion4" };
        //_field[4, 15].OnTileObject = new Monster() { _monsterName = "고블린" };
        _field[2, 10].OnTileObject = new Npc()
        {
            Name = "??????",
            Pages = new[]
       {
                "…눈을 뜬 모양이군.\n놀랄 필요는 없어. 네가 깨어나길 기다리고 있었지.",
    "여기가 어디냐고?\n간단히 말하면 ‘밖과 단절된 곳’이야.\n그리고… 네가 여기 온 건 우연이 아니다.",
    "내가 너를 불렀다.\n너의 ‘의지’가 필요했거든.\n기억이 흐릿한 건 부작용이야. 미안하지만.",
    "설명은 짧게 하지.\n이곳에서 나가려면 ‘숲’으로 가야 해.\n숲 안쪽에는 미로가 있고, 그 길을 통과해야만 앞으로 나아갈 수 있다.",
    "미로는 단순한 길찾기가 아니야.\n길이 바뀌기도 하고, 방해하는 것들이 나타날 수도 있지.\n하지만 멈추면 끝이야.",
    "먼저, 기본적인 방법을 알려주겠다.\n이동 : 방향키(← → ↑ ↓)\n공격 : Space\n인벤토리 : I\n아이템 사용 : Enter",
    "그리고 중요한 것.\n누군가와 대화하거나 상호작용할 때는\n대상 근처에서 F를 눌러라.",
    "미로 안에서는 당황하지 마.\nHP와 MP를 확인하고,\n필요하면 포션으로 회복해.\n무리하면 다시 돌아오는 것도 방법이다.",
    "이제 선택은 네 몫이야.\n숲으로 향해.\n출구를 찾아서… 이곳의 ‘진짜 이유’를 마주해라.",
    "행운을 빌지.\n…그리고, 여기서의 대화는 이걸로 끝이다."

        }
        };

        Debug.Log("타운 씬 진입");
    }

    public override void Update()
    {
        _player.Update();
        if (_player.Position.X == _exitPos.X && _player.Position.Y == _exitPos.Y)
        {
            SceneManager.Change("Forest");
            return;
        }
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