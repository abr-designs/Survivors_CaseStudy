using Survivors.ScriptableObjets.Animation;

namespace Survivors.Base.Interfaces
{
    public interface IAnimationController
    {
        void SetAnimationProfile(in AnimationProfileScriptableObject animationProfileScriptableObject);

        void SetCurrentState(in STATE state);
    }
}