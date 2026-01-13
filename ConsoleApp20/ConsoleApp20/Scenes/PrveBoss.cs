using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20.Scenes
{
    internal class PrveBoss : Scene
    {
        private MenuList _manualMenu;

        private readonly string[] _stroy =
{
    "…발자국을 따라 숲 깊숙한 곳으로 들어왔다.",
    "나뭇잎 사이로 스며드는 빛조차 희미해지고, 공기는 점점 차가워졌다.",
    "",
    "주변이 이상할 정도로 조용하다.",
    "새소리도, 바람 소리도… 마치 이곳만 시간이 멈춘 것처럼.",
    "",
    "그때, 멀리서 무언가 긁히는 소리가 들렸다.",
    "‘스르륵… 스르륵…’",
    "",
    "나는 숨을 죽이고 천천히 앞으로 나아갔다.",
    "발밑의 흙은 깊게 눌려 있었고, 나무껍질엔 날카로운 자국이 남아 있었다.",
    "",
    "…확신이 들었다.",
    "이건 단순한 짐승이 아니다.",
    "",
    "갑자기 숲 한가운데에서 낮고 굵은 울음소리가 터져 나왔다.",
    "‘그르르르…’",
    "",
    "땅이 미세하게 떨리고, 그림자 하나가 나무 사이를 가르며 움직였다.",
    "커다란 형체. 숨이 막힐 만큼 거대한 존재.",
    "",
    "나는 무의식적으로 가방을 움켜쥐었다.",
    "촌장이 준 ‘신비한 부적’이 손끝에서 미세하게 뜨거워진다.",
    "",
    "…이게 반응하고 있어.",
    "놈이 가까이 있다는 뜻이다.",
    "",
    "이제 도망칠 수 없다.",
    "여기서 끝을 내야 한다.",
    "",
    "Enter를 누르면 보스에게 다가갑니다.",
    "",
    "공략 : 신비한힘으로 공격하면 데미지가 들어갈거같다"
};

        public PrveBoss()
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
                SceneManager.Change("BossScene");
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
