using System;
using _StickBlast.Script.Game.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _StickBlast.Script.Game.Sticks
{
    public class MoveableStick : MonoBehaviour
    {
        public StickDirections stickDirection;
        public bool isBuildable = false;
        public GameObject tmpGridStick;

        public float raycastRadius = .5f;
        public LayerMask buildableLayer;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            buildableLayer = 1 << 6;
        }

        private void Update()
        {
            CheckForBuildableObjects();
        }

        private void CheckForBuildableObjects()
        {
            Vector2 center = _collider.bounds.center;
            Collider2D hit = Physics2D.OverlapCircle(center, raycastRadius, buildableLayer);

            if (hit && !Equals(hit.gameObject, gameObject)) 
            {
                if (hit.gameObject.layer == 6)
                {
                    GridStick gridStick = hit.GetComponent<GridStick>();
                    if (gridStick != null && gridStick.stickDirection == stickDirection)
                    {
                        if (!gridStick.isBuilded)
                        {
                            isBuildable = true;
                            Image gridStickImage = hit.GetComponent<Image>();
                            if (gridStickImage != null)
                            {
                                gridStickImage.color = Color.grey;
                            }

                            if (tmpGridStick == null)
                                tmpGridStick = hit.gameObject;
                        }
                        Debug.Log("Buildable Object Detected.");
                    }
                    else
                    {
                        ResetBuildableStatus();
                    }
                }
                else
                {
                    ResetBuildableStatus();
                }
            }
            else
            {
                ResetBuildableStatus();
            }
        }

        private void ResetBuildableStatus()
        {
            if (tmpGridStick)
            {
                GridStick gridStick = tmpGridStick.GetComponent<GridStick>();
                Image gridStickImage = tmpGridStick.GetComponent<Image>();

                if (gridStick != null)
                {
                    if (!gridStick.isBuilded)
                    {
                        gridStickImage.color = Color.white;
                    }
                    else
                    {
                        gridStickImage.color = Color.blue;
                    }
                }
            }

            tmpGridStick = null;
            isBuildable = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_collider) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_collider.bounds.center, raycastRadius);
        }
    }
}