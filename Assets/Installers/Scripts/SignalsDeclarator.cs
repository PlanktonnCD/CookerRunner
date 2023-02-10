using Gameplay.Scripts.DataProfiling;
using Movement;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.Signals
{
    public class SignalsDeclarator : ScriptableObjectInstaller<SignalsDeclarator>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<CanStartRunSignal>();
            Container.DeclareSignal<CoinChangeSignal>();
        }
    }
}