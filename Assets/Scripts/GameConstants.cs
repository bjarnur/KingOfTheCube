﻿using System.Collections;
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
    }

    internal static class GameObjectsTags {
        public const string gardenObject = "GardenObject";
        public const string king = "King";
    }
}
