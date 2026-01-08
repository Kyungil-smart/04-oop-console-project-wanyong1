using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Scene
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Render();
    public abstract void Exit();
}