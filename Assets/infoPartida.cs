using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class infoPartidaGuardada
{
    public static bool hayPartidaGuardada = false;

    public static class infoShaggy
    {
        public static Vector2 posicion;
        public static int energiaActual;
        public static int cantBalas;
    }

    // para guardar las balas
    public class TipoInfoPaqueteBalas
    {
        public bool activo;
    }

    public static List<TipoInfoPaqueteBalas> infoPaqueteBalas = new List<TipoInfoPaqueteBalas>();

    // para guardar los zombies
    public class TipoInfoPaqueteBalas
    {
        public bool activo;
    }

    public static List<TipoInfoPaqueteBalas> infoPaqueteBalas = new List<TipoInfoPaqueteBalas>();
}
