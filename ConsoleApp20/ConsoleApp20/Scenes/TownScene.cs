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
             "마을 촌장 : …처음 보는 얼굴이군.\n이 마을엔 외지인이 드문데, 이런 곳까지 오다니.",
             "마을 촌장 : 갑작스러운 말이지만…\n무례하게 들리지 않기를 바란다.\n부탁 하나를 해도 되겠나?",
             "MY : 물론이지, 무슨 일이야?",
             "마을 촌장 : 며칠 전, 아이 몇 명이 숲 안으로 들어갔네.\n해가 지도록 돌아오지 않았지.\n마을 사람들 모두가 걱정하고 있어.",
             "그 숲은 평범한 숲이 아니네.\n길이 자주 바뀌고,\n익숙한 사람일수록 더 쉽게 길을 잃는다네.",
             "그래서… 우리 마을 사람들은\n섣불리 안으로 들어갈 수가 없지.\n오히려 외지인이 더 적합할지도 몰라.",
             "괜찮다면 부탁하고 싶네.\n숲에 들어가 아이들을 찾아주겠나?",
             "MY : 알겠어. 내가 찾아볼게.",
             "무리는 하지 말게.\n위험하다고 느껴지면 돌아와도 된다.\n그 선택을 탓하는 사람은 없을 걸세.",
             "MY : 걱정하지마 반드시 찾아낼께",
             "마을 촌장 : 말이라도  고맙네.\n부디 무사히 돌아오게.",
             " …부탁하겠네."


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