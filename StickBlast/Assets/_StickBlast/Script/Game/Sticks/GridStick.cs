using _StickBlast.Script.Game.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _StickBlast.Script.Game.Sticks
{
    public class GridStick : MonoBehaviour
    {
        
        public StickDirections stickDirection;
        public bool isBuilded = false;

        public void SetBuilded()
        {
            isBuilded = true;
            GetComponent<Image>().color = Color.blue;
        }

        public void SetReadyForBuild()
        {
            isBuilded = false;
            GetComponent<Image>().color = Color.white;
        }
        
    }
}