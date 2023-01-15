using Survivors.Base.Managers.Interfaces;
using Survivors.ScriptableObjets.Animation;

namespace Survivors.Base.Interfaces
{
    public interface IAnimationController : IUpdate
    {
        void SetAnimationProfile(in AnimationProfileScriptableObject animationProfileScriptableObject);

        void SetCurrentState(in STATE state);
    }
}