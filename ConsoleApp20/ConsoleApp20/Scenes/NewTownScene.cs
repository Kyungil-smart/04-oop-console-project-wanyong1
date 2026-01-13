using ConsoleApp20.GameObjects;
using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NewTownScene : Scene
{
    private Tile[,] _field = new Tile[20, 20];
    private PlayerCharacter _player;
    public NewTownScene(PlayerCharacter player) => Init(player);
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


        _exitPos = new Vector(_field.GetLength(1) - 1, _field.GetLength(0) - 1);

        _field[_exitPos.Y, _exitPos.X].isWall = false;

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

        _field[_exitPos.Y, _exitPos.X].OnTileObject = new ExitDevice(true);


        _field[2, 10].OnTileObject = new Npc()
        {
            Name = "마을 촌장",
            Pages = new[]
            {
                "마을 촌장 : 돌아왔구나! 무사해서 정말 다행이다.",
                "마을 촌장 : 숲 속에서 길을 잃은 아이들을 모두 데려왔다는 소식을 들었다.",
                "마을 촌장 : 오늘 밤은 마을 회관에서 간단히라도 축하 자리를 준비하마.",
                "MY : 나는 숲에 들어가서 해야할일이 있어서먼저 가보겠네.",
                "마을 촌장 : 그래, 조심하게나!",
                "마을 촌장 : 가기전에 이것을 받게나.이건 마을에서 대대로 내려오던 작은 부적이란다.",
                "마을 촌장 : 이 부적은 위험으로부터 너를 지켜줄거야.",
                "마을 촌장 : 다시한번 고마웠네!",
                "MY : (촌장에게서 부적을 받았다. 앞으로 도움이 될 것 같아. 숲으로 다시 돌아가자)"
            }


        };
        _field[3, 10].OnTileObject = new Npc()
        {
            Name = "길을 잃은 아이 1",
            Pages = new[]
            {
                "아이: 나… 나 진짜로 못 돌아오는 줄 알았어…",
                "아이: 숲에서 길이 다 똑같아 보여서 계속 빙빙 돌았거든…",
                "아이: 네가 와줘서 진짜 무서운 거 조금 덜했어.",
                "아이: 고마워…! 다음엔 절대 혼자 숲 안 갈 거야.",
            }


        };
        _field[4, 10].OnTileObject = new Npc()
        {
            Name = "길을 잃은 소년 1",
            Pages = new[]
            {
                "소년: …응, 나 괜찮아. 진짜로.",
                "소년: 겁 안 난 척 했는데… 솔직히 조금 무서웠어.",
                "소년: 숲에서 이상한 소리 들렸거든.",
                "소년: 바람 소리 같기도 하고… 누가 부르는 것 같기도 하고…",
                "소년: 너 없었으면 나 혼자서는 절대 못 나왔을 거야.",
            }


        };
        _field[5, 10].OnTileObject = new Npc()
        {
            Name = "길을 잃은 소년 2",
            Pages = new[]
            {
                 "소년2: 야! 너 진짜 대단하다!",
                 "소년2: 나 완전 멋있게 탈출하려고 했는데… 길을 완전 잃었지 뭐야.",
                 "소년2: 너 없었으면 아직도 숲 속을 헤매고 있었을걸?",
                 "소년2 : 그리고아무한테도 말 안 했는데.",
                 "소년2: 처음엔 그냥 나무 그림자인 줄 알았거든?",
                 "소년2: 근데 그림자가… 움직였어. 바람도 없었는데.",
                 "소년2: 난 무서워서 도망쳤는데…",
                 "소년2: 너… 숲으로 다시 갈 거지?",
                 "소년2: 내가 마지막에 본 곳은… 숲 깊은 데, 쓰러진 큰 나무 옆이었어.",
                 "소년2: 거기 가면… 아마 흔적이 남아있을 거야.",
                 "My : (숲으로 들어가자)"
            }


        };


        Debug.Log("타운 씬 진입");

        _player.OnEnterExit = GoToForest;
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
        _player.OnEnterExit = null;
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
    private void GoToForest()
    {
        SceneManager.Change("Forest");
    }
}