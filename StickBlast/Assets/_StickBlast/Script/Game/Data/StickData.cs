using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ScriptableObject", menuName = "Data/StickData", order = 0)]
    public class StickData : ScriptableObject
    {
        [Header("----- STICK ELEMENTS DATA -----")]
        public List<GameObject> singleMoveableObjects;
        public List<GameObject> doubleMoveableObjects;
        public List<GameObject> moreMoveableObjects;
    }
}