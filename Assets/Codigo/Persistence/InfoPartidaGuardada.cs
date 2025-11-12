using System.Collections.Generic;
using UnityEngine;

namespace GameplayAdaptado.Persistence
{
    // Adaptación limpia para uso desde el código nuevo.
    public static class InfoPartidaGuardada
    {
        public static bool HayPartidaGuardada = false;

        public class InfoShaggy
        {
            public Vector2 Posicion;
            public int EnergiaActual;
            public int CantBalas;
        }

        public static InfoShaggy Shaggy = new InfoShaggy();

        public class TipoInfoPaqueteBalas { public bool Activo; }
        public static List<TipoInfoPaqueteBalas> InfoPaquetes = new List<TipoInfoPaqueteBalas>();

        public class TipoInfoZombie { public bool Activo; public Vector2 Posicion; }
        public static List<TipoInfoZombie> InfoZombies = new List<TipoInfoZombie>();
    }
}
