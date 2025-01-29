using System;
using System.Collections;
using System.Collections.Generic;
using _StickBlast.Script.Game.Management;
using UnityEngine;

public class GridManager : MonoBehaviour, IEventListener
{ 
    [Header("----- Grid Elements -----")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject horizontalStickPrefab;
    [SerializeField] private GameObject verticalStickPrefab;
    [SerializeField] private GameObject centerPrefab;
    [SerializeField] private RectTransform gridPanel;

    [Header("----- Grid Settings -----")]
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float nodeSize;
    [SerializeField] private float spacing;
    
    [Header("----- Stick Elements -----")]
    [SerializeField] private List<GameObject> nodes = new List<GameObject>();    
    [SerializeField] private List<GameObject> sticks = new List<GameObject>();    
    [SerializeField] private List<GameObject> blocks = new List<GameObject>();
    
    private RectTransform _gridContainer;


    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        nodes = new List<GameObject>();
        sticks = new List<GameObject>();

        float gridWidth = columns * (nodeSize + spacing) - spacing;
        float gridHeight = rows * (nodeSize + spacing) - spacing;

        GameObject gridContainerObject = new GameObject("GridContainer", typeof(RectTransform));
        gridContainerObject.transform.SetParent(gridPanel, false);
        RectTransform gridContainer = gridContainerObject.GetComponent<RectTransform>();
        
        gridContainer.anchorMin = new Vector2(0.5f, 0.5f);
        gridContainer.anchorMax = new Vector2(0.5f, 0.5f);
        gridContainer.pivot = new Vector2(0.5f, 0.5f);
        gridContainer.sizeDelta = new Vector2(gridWidth, gridHeight);
        gridContainer.localScale = Vector3.one;
        
        Vector2 startPosition = new Vector2(-gridWidth / 2 + nodeSize / 2, gridHeight / 2 - nodeSize / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2 nodePosition = startPosition + new Vector2(j * (nodeSize + spacing), -i * (nodeSize + spacing));

                GameObject newNode = Instantiate(nodePrefab, gridContainer);
                RectTransform nodeRect = newNode.GetComponent<RectTransform>();
                nodeRect.anchoredPosition = nodePosition;
                nodeRect.sizeDelta = new Vector2(nodeSize, nodeSize);
                nodes.Add(newNode);

                if (j > 0)
                {
                    Vector2 horizontalPosition = nodePosition + new Vector2(-(nodeSize + spacing) / 2, 0);
                    GameObject horizontalStick = Instantiate(horizontalStickPrefab, gridContainer);

                    RectTransform horizontalRect = horizontalStick.GetComponent<RectTransform>();
                    horizontalRect.anchoredPosition = horizontalPosition;
                    horizontalRect.sizeDelta = new Vector2(nodeSize, spacing);
                    sticks.Add(horizontalStick);

                    BoxCollider2D collider = horizontalStick.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        collider.size = new Vector2(horizontalRect.sizeDelta.x, horizontalRect.sizeDelta.y);
                    }
                }

                if (i > 0)
                {
                    Vector2 verticalPosition = nodePosition + new Vector2(0, (nodeSize + spacing) / 2);
                    GameObject verticalStick = Instantiate(verticalStickPrefab, gridContainer);

                    RectTransform verticalRect = verticalStick.GetComponent<RectTransform>();
                    verticalRect.anchoredPosition = verticalPosition;
                    verticalRect.sizeDelta = new Vector2(spacing, nodeSize);
                    sticks.Add(verticalStick);

                    BoxCollider2D collider = verticalStick.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        collider.size = new Vector2(verticalRect.sizeDelta.x, verticalRect.sizeDelta.y);
                    }
                }
            }
        }
        
        /*for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < columns - 1; j++)
            {
                Vector2 topLeft = nodes[i * columns + j].GetComponent<RectTransform>().anchoredPosition;
                Vector2 topRight = nodes[i * columns + (j + 1)].GetComponent<RectTransform>().anchoredPosition;
                Vector2 bottomLeft = nodes[(i + 1) * columns + j].GetComponent<RectTransform>().anchoredPosition;
                Vector2 bottomRight = nodes[(i + 1) * columns + (j + 1)].GetComponent<RectTransform>().anchoredPosition;

                Vector2 centerPosition = (topLeft + topRight + bottomLeft + bottomRight) / 4;

                GameObject squareBlock = Instantiate(centerPrefab, gridContainer); 
                //squareBlock.GetComponent<BlockStick>().row = i;
                blocks.Add(squareBlock);
                RectTransform squareRect = squareBlock.GetComponent<RectTransform>();
                squareRect.anchoredPosition = centerPosition;
                squareRect.sizeDelta = new Vector2(nodeSize, nodeSize);
                squareBlock.transform.SetSiblingIndex(0);
            }
        }*/

        foreach (var node in nodes)
        {
            node.transform.SetSiblingIndex(gridContainer.childCount - 1);
        }
        
        gridContainer.anchoredPosition = Vector2.zero;
        gridContainer.offsetMin = Vector2.zero;
        gridContainer.offsetMax = Vector2.zero;
    }
    
    
    
}
