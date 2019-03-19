using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants  {

    public static string ARPLAYERTAG = "ARPlayer";
    public static string UNITYPLAYERTAG = "UnityPlayer";
    public static string ARPLAYERNAME = "ARPlayer";
    public static string UNITYPLAYERNAME = "UnityPlayer";

    public enum AnimationTypes
    {
        stopped, running, jumping, falling, climbing, throwing
    };

    internal static class PlayerAnimationNames {
        
        public const string jumpAnimation = "Jump";
        public const string fallAnimation = "Fall";
        public const string runAnimation = "Run";
        public const string stopAnimation = "Stop";
    }

    internal static class KingAnimationNames {
        public const string throwAnimation = "Throw";
        public const string dieAnimation = "Die";
        public const string runAnimation = "IsRunning";
    }

    internal static class RPCTags {
        public const string detonateBomb = "DetonateBomb";
        public const string updateNetworked = "UpdateNetworked";
        public const string plantSmoke = "PlantSmoke";
        public const string plantExplosion = "PlantExplosion";
    }

    internal static class PunNames {
        public static string arKing = "ARKing";
        public static string unityKing = "UnityKing";
    }

    internal static class GameObjectsTags {
        public const string gardenObject = "GardenObject";
        public const string controller = "LevelBuilder";
        public const string king = "King";
        public const string worldContainer = "WorldContainer";
    }

    internal static class Materials {
        public static string smoke = "Materials/Mat_Smoke";
        public static string explosion = "Materials/Mat_Explosion";
    }

    internal static class ShaderProperties {
        public static string smokeOrigin = "_Origin";
    }

    /**/
    public static int NetworkedPlayerID = -1;
}
