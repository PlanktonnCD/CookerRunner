using UI;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private UIConfig _uiConfig;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<UIManager>()
            .FromComponentInNewPrefab(_uiManager)
            .AsSingle();
        Container
            .Bind<UIFactory>()
            .FromNew()
            .AsSingle()
            .WithArguments(_uiConfig);
    }
}