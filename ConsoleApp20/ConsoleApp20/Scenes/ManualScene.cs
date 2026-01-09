using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    public class ManualScene :Scene
    {
        private Ractangle _window;

        private readonly string[] _lines =
        {
           "==============================게임방법==============================",
           "",
           "이동 : 방향키 (→ ← ↑ ↓)",
           "공격 : Space",
           "인벤토리 : I",
           "아이템  사용 : Enter",
           "",
           "Enter를 누르면 원래 창으로 돌아갑니다."
        };

        public ManualScene() 
        {
            _window = new Ractangle(4, 2, 60, 60);
        }
        public override void Enter()
        {
            Console.WriteLine("게임 방법 씬 입장");
        }

        public override void Update()
        {
            // 뒤로가기
            if (InputManager.GetKey(ConsoleKey.Enter))
            {
                SceneManager.ChangePrevScene();
            }
        }

        public override void Render()
        {
            _window.Draw();

            int innerX = _window.X + 2;
            int innerY = _window.Y + 1;

            // 타이틀(첫 줄)은 가운데 느낌으로 출력
            string title = _lines[0];
            int titleX = innerX + Math.Max(0, (_window.Width - 4 - title.GetTextWidth()) / 2);

            Console.SetCursorPosition(titleX, innerY);
            title.Print(ConsoleColor.Yellow);

            // 나머지 줄 출력
            int y = innerY + 2;
            for (int i = 1; i < _lines.Length; i++)
            {
                if (y >= _window.Y + _window.Height - 1) break; // 창 밖 방지
                Console.SetCursorPosition(innerX, y);
                _lines[i].Print();
                y++;
            }
        }

        public override void Exit()
        {

        }
    }
}
