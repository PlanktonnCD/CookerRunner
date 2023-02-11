using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using UnityEngine;
using Zenject;

public class DishInstaller : ScriptableObjectInstaller<DishInstaller>
{
    [SerializeField] private IngredientsConfig _ingredientsConfig;
    [SerializeField] private DishesConfig _dishesConfig;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DishesConfig>().FromInstance(_dishesConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<IngredientsConfig>().FromInstance(_ingredientsConfig).AsSingle();
    }
}