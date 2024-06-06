using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scenery
{
    [Serializable]
    public class Level
    {
        [field: SerializeField] public List<string> SceneNames { get; private set; }
    }
}
