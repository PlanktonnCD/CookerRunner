using Gameplay.Scripts;
using Gameplay.Scripts.CameraScripting;
using UnityEngine;
using Zenject;

public class CameraInstaller : ScriptableObjectInstaller<CameraInstaller>
{
    [SerializeField] private CameraConfig _cameraConfig;
    public override void InstallBindings()
    {
        Container.Bind<CameraController>().FromNew().AsSingle().WithArguments(_cameraConfig);
    }
}