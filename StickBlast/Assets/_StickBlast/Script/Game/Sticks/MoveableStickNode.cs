using System;
using System.Collections.Generic;
using _StickBlast.Script.Game.Management;
using _StickBlast.Script.Game.SFX;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _StickBlast.Script.Game.Sticks
{
    public class MoveableStickNode : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
    {
        public List<MoveableStick> moveableSticks;
        public bool isGridReady = false;

        private Vector2 _offset;
        private RectTransform _rectTransform;
        private Vector3 _initialPosition;

        private StickManager _stickManager;
        private GridManager _gridManager;
        private SoundManager _soundManager;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialPosition = _rectTransform.localPosition;
        }

        private void Start()
        {
            _stickManager = EventManager.Instance.GetListener<StickManager>();
            _gridManager = EventManager.Instance.GetListener<GridManager>();
            _soundManager = EventManager.Instance.GetListener<SoundManager>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _soundManager.PlaySound(SoundManager.SoundType.Drag);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _offset);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPoint))
            {
                _rectTransform.localPosition = localPoint - _offset;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!ReturnGridReady())  
            {
                _rectTransform.localPosition = _initialPosition;
                _soundManager.PlaySound(SoundManager.SoundType.ReturnPos);
            }
            else
            {
                foreach (MoveableStick item in moveableSticks)
                {
                    if (item.tmpGridStick.GetComponent<GridStick>())
                    {
                        item.tmpGridStick.GetComponent<GridStick>().SetBuilded();
                        _soundManager.PlaySound(SoundManager.SoundType.Drop);
                        //scoreManager.IncreaseScore(20);
                        _soundManager.PlaySound(SoundManager.SoundType.Point);
                    }
                }

                for (int i = 0; i < _gridManager.blocks.Count; i++)
                {
                    //_gridManager.blocks[i].GetComponent<BlockStick>().SticksControl();
                }

                if (_stickManager)
                    _stickManager.DecreaseItemsCount();
                else
                    Debug.Log("Dont found item manager");
                Destroy(this.gameObject);
            }
        }

        public bool ReturnGridReady()
        {
            isGridReady = true;

            foreach (var stick in moveableSticks)
            {
                if (!stick.isBuildable)
                {
                    isGridReady = false;
                    break;
                }
            }
            return isGridReady;
        }
    }
}