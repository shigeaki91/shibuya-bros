using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] CharacterDatas _characterDatas;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_characterDatas).As<CharacterDatas>();
    }
}