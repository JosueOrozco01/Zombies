Código refactor bajo principios SOLID y patrones Factory/Abstract Factory

Qué hay aquí
- `Assets/Codigo/Factories` — interfaces y implementaciones por defecto para crear explosiones, partículas y granadas.
- `Assets/Codigo/Services` — servicios (IAudioService, AudioService, CoroutineRunner).
- `Assets/Codigo/Entities` — versiones refactorizadas de entidades: `PersonajeRefactor`, `ZombiesRefactor`, `EntidadBase`.
- `Assets/Codigo/Weapons` — `GranadaRefactor` que utiliza las fábricas para crear explosiones.
- `Assets/Codigo/Bootstrap/GameBootstrapper.cs` — punto único para ensamblaje (inyecta factories y servicios en componentes refactorizados).

Cómo probar en Unity
1. En la primera escena del juego, añade un GameObject vacío y pega el componente `GameplayAdaptado.Bootstrap.GameBootstrapper`.
2. Asigna los prefabs `explosionPrefab`, `particulaPrefab` y `granadaPrefab` en el inspector del bootstrapper.
3. Si quieres usar los componentes refactorizados, reemplaza las instancias de `personaje`, `zombies`, `granada` en la escena por `PersonajeRefactor`, `ZombiesRefactor`, `GranadaRefactor` (o añade los nuevos componentes junto a los existentes mientras pruebas compatibilidad).
4. Ejecuta la escena; `GameBootstrapper` intentará inyectar las dependencias en los componentes refactorizados.

Notas y compatibilidad
- No eliminé ni moví nada en `Assets/Scripts` — la versión original sigue intacta.
- Las clases refactorizadas intentan ser compatibles con las originales: `GranadaRefactor` buscará tanto `ZombiesRefactor` como la clase `zombies` original al aplicar daño.
- Hay dos carpetas de servicios (`Services` y `Servicios`) y dos posibles interfaces `IAudioService` en distintos namespaces; por ahora el código refactorizado usa `GameplayAdaptado.Services.IAudioService`. Si prefieres consolidar, puedo unificar las interfaces en un solo archivo/namespace.

Siguientes pasos recomendados
- Completar portado de toda la lógica de `personaje.cs` y `zombies.cs` a las versiones refactorizadas para lograr paridad completa.
- Añadir tests de editor o playmode para cubrir disparo, muerte y guardado.
- Opcional: eliminar o consolidar duplicados de interfaces en `Assets/Codigo/Servicios` vs `Services`.
