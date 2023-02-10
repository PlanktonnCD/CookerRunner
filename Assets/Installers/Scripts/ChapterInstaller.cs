using Gameplay.Scripts.Chapters;
using UI;
using UnityEngine;
using Zenject;

public class ChapterInstaller : ScriptableObjectInstaller<ChapterInstaller>
{
    [SerializeField] private ChapterManager _chapterManager;
    [SerializeField] private ChapterConfig _chapterConfig;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ChapterFactory>().FromNew().AsSingle().WithArguments(_chapterConfig);
        Container.BindInterfacesAndSelfTo<ChapterManager>().FromComponentInNewPrefab(_chapterManager).AsSingle();
    }
}