using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    internal class PrologueScene : Scene
    {
        private MenuList _manualMenu;

        private readonly string[] _stroy =
        {
         "여기가 어디지? (정신을 차려보니 낯선 곳으로 이동이 되어 있었다.)",
         "방금 전까진 분명히... 기억이 잘 나지 않는다.",
         "",
         "주변은 조용했고, 공기엔 이상하게 금속 냄새가 섞여 있었다.",
         "손에 힘을 주자, 등 뒤 가방이 묵직하게 흔들렸다.",
         "",
         "나는 숨을 고르고 주변을 둘러봤다.",
         "멀리서 희미한 불빛과 사람 목소리가 들린다.",
         "",
         "‘마을… 인가?’",
         "여기서 가만히 있을 수는 없다. 우선 사람을 찾아가 보자.",
         "",
         "Enter를 누르면 마을로 이동합니다."

        };
        public PrologueScene()
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
            _manualMenu.IsCursorVisible = false; // 커서 숨김
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
                SceneManager.Change("Town");
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
