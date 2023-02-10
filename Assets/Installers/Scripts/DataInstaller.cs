using Gameplay.Scripts.DataProfiling;
using UnityEngine;
using Zenject;

public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<DataManager>().FromNew().AsSingle();
    }
}