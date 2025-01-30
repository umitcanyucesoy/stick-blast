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
            GetComponent<Image>().color = new Color(0, 19, 255, 255);
        }

        public void SetReadyForBuild()
        {
            isBuilded = false;
            GetComponent<Image>().color = Color.white;
        }
        
    }
}