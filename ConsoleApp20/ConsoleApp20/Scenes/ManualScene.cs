using System;

namespace ConsoleApp20.Scenes
{
    public class ManualScene : Scene
    {
        private MenuList _manualMenu;

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
            Init();
        }

        public void Init()
        {
            _manualMenu = new MenuList();

            for (int i = 0; i < _lines.Length; i++)
            {
                _manualMenu.Add(_lines[i], null);
            }

            _manualMenu.IsCursorVisible = false; // 커서 숨김
            _manualMenu.Reset();
        }

        public override void Enter()
        {
            _manualMenu.Reset();
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
            // 위치는 원하는 대로 조절
            _manualMenu.Render(4, 2);
        }

        public override void Exit()
        {
        }
    }
}
