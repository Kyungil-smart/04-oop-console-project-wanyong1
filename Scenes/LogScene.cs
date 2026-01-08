using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogScene : Scene
{
    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            SceneManager.ChangePrevScene();
        }
    }

    public override void Render()
    {
        Debug.Render();
    }

    public override void Enter() { }
    public override void Exit() { }
}