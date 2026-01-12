using ConsoleApp20.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameManager
{
    public static bool IsGameOver { get; set; }
    public const string GameName =
        @"

⠆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⡃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⡅⠀⠀⠀⠠⠐⠈⠀⠀⠁⠀⠈⠀⠀⠁⠀⠈⠀⠀⠁⠀⠈⠀⠀⠐⠀⠀⠂⠀⠐⠀⠀⠂⠀⠐⠀⠀⠂⠀⠐⠀⠀⠂⠀⠐⠀⠀⠂⠀⠀
⠆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⢀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⡃⠀⠀⠀⠂⠀⠀⠀⠀⠀⠂⠁⢀⠁⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠂⠈⠀⠀⠐⠀⠈⠀⠀⠐⠀⠈⠀⠀⠐⠀⠈⠀⠀⠐⠀
⠆⠀⠀⠀⠀⠀⡀⠈⢰⠋⠉⡆⢸⠀⠀⠨⡍⢉⠉⡹⠀⠀⢸⠭⠭⠭⠅⠀⠀⠀⢐⡖⠒⢆⠐⡖⠒⢦⠀⡤⠒⠒⠢⠀⠀⠀⠀⠀⠀⠀
⡃⠀⠀⠀⠁⠀⠀⠀⢱⡀⠠⡑⢸⠑⠀⢤⠩⣌⠭⡨⠄⠠⢌⢇⠧⡪⡢⠄⠀⠂⢐⡇⠦⡃⠀⡧⡠⠊⢰⠁⠀⠢⡢⠀⠀⠀⠐⠈⠀⠀
⡅⠀⠀⠀⢀⠀⠀⠀⠀⠈⠉⠀⢸⠀⠀⠀⠀⢸⠀⠀⠀⠀⢸⣀⣀⣀⡀⠀⠀⠀⠐⠅⠀⠣⠐⠅⠀⠀⠀⠓⠤⠤⠎⠀⠀⠀⠀⠀⠀⠀
⡆⠀⠀⠀⠀⠀⠀⠐⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀
⡆⠀⠀⠀⠂⠀⠀⡀⠀⠀⠐⠀⠀⠠⠀⠈⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⠐⠀⠀⠂⠁⠀⠀⠀⠀⠀⠀⠀⠄⠀⠀⠄⠀
⠆⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⢀⠀⠁⠀⠀⠀⠀⠀⠄⠀⠂⠁⠀⠀⠠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠈⠀⠀⠀⠀⠀⠀⠀
⡃⠀⠀⠀⠈⠀⠀⠠⠐⠀⠀⠀⠀⠁⠀⠀⠂⠀⠀⠀⠀⠀⡀⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⡀⠀⠀⠀⠀⠀⠀⠄⠂⠀⠀
⡅⠀⠀⠀⠠⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⢀⠀⠀⠀⠀⠀⠁⠀⠀⠀⠀⠀⠄⠀⠀⠐⠀⠀⠄⠂⠁⠀⠀⠀⠀⠀⠀⠂⠀⠐⠀⠀⠀⠀⠀

";
    private PlayerCharacter _player;

    public void Run()
    {
        Init();
        
        while (!IsGameOver)
        {
            // 렌더링
            Console.Clear();
            SceneManager.Render();
            // 키입력 받고
            InputManager.GetUserInput();

            if (InputManager.GetKey(ConsoleKey.L))
            {
                SceneManager.Change("Log");
            }

            // 데이터 처리
            SceneManager.Update();
        }
    }

    private void Init()
    {
        IsGameOver = false;
        SceneManager.OnChangeScene += InputManager.ResetKey;
        _player = new PlayerCharacter();
        
        SceneManager.AddScene("Title", new TitleScene());
        SceneManager.AddScene("Story", new StoryScene());
        SceneManager.AddScene("Town", new TownScene(_player));
        SceneManager.AddScene("Manual", new ManualScene());
        SceneManager.AddScene("Log", new LogScene());
        SceneManager.AddScene("PrologueScene", new PrologueScene());
        SceneManager.AddScene("Forest", new ForestScene(_player));
        SceneManager.AddScene("Forest2", new ForestScene2(_player));
        SceneManager.AddScene("Forest3", new ForestScene3(_player));

        SceneManager.Change("Title");
        
        Debug.Log("게임 데이터 초기화 완료");
    }
}