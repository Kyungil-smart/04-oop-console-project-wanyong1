using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    internal class Finsh : Scene
    {
        private MenuList _manualMenu;

        private readonly string[] _stroy =
        {
         "힘들었따",

        };
        public Finsh()
        {
            Init();
        }
        public void Init()
        {
            _manualMenu = new MenuList();

            for (int i = 0; i < _stroy.Length; i++)
            {
                _manualMenu.Add(_stroy[i], null);
            }
            _manualMenu.IsCursorVisible = false;
            _manualMenu.Reset();
        }

        public override void Enter()
        {
            _manualMenu.Reset();
        }
        public override void Update()
        {
            if (InputManager.GetKey(ConsoleKey.Enter))
            {
                SceneManager.Change("TitleScene");
            }
        }
        public override void Render()
        {
            _manualMenu.Render(8, 5);
        }
        public override void Exit()
        {

        }
    }
}
