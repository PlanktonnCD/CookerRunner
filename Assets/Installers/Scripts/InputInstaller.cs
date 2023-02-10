using Gameplay.Scripts.Input;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/InputInstaller")]
public class InputInstaller : ScriptableObjectInstaller<InputInstaller>
{
    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<InputController>()
            .FromNew()
            .AsSingle();
    }
}