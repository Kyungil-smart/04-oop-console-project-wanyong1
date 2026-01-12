using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Utils
{
    public class DialogueBox
    {
        private MenuList _menu;
        private string _speaker;
        private string[] _pages;
        private int _index;

        public bool IsActive { get; private set; }

        public event Action OnOpened;
        public event Action OnClosed;

        public DialogueBox()
        {
            _menu = new MenuList();
            _menu.IsCursorVisible = false;
        }

        public void Open(string speaker, string[] pages)
        {
            if (pages == null || pages.Length == 0) return;

            _speaker = string.IsNullOrEmpty(speaker) ? "NPC" : speaker;
            _pages = pages;
            _index = 0;

            IsActive = true;
            BuildMenu();
            OnOpened?.Invoke();
        }

        public void Close()
        {
            if (!IsActive) return;

            IsActive = false;
            _speaker = null;
            _pages = null;
            _index = 0;

            _menu = new MenuList();
            _menu.IsCursorVisible = false;

            OnClosed?.Invoke();
        }

        public void Update()
        {
            if (!IsActive) return;

            if (InputManager.GetKey(ConsoleKey.Enter))
            {
                _index++;

                if (_pages == null || _index >= _pages.Length)
                {
                    Close();
                    return;
                }

                BuildMenu();
            }
        }

        public void Render(int x, int y)
        {
            if (!IsActive) return;
            _menu.Render(x, y);
        }

        private void BuildMenu()
        {
            _menu = new MenuList();
            _menu.IsCursorVisible = false;

            // 한 페이지가 여러 줄이면 '\n'로 분리해서 출력
            string page = _pages[_index] ?? "";
            string[] lines = page.Replace("\r\n", "\n").Split('\n');

            foreach (var line in lines)
                _menu.Add(line, null);

            _menu.Add("", null);

            bool last = (_pages != null && _index == _pages.Length - 1);
            _menu.Add(last ? "Enter : 닫기" : "Enter : 다음", null);

            _menu.Reset();
        }
    }
}
