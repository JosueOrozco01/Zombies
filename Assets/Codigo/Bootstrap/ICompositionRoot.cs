namespace GameplayAdaptado.Bootstrap
{
    // Composition root contract: single interface where the game wiring is exposed
    public interface ICompositionRoot
    {
        GameplayAdaptado.IGameObjectFactory GameObjectFactory { get; }
        GameplayAdaptado.IAudioService AudioService { get; }

        // Initialize wiring (can be called manually by tests)
        void Initialize();
    }
}
