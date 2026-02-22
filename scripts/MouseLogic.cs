using UnityEngine;
using System.Collections.Generic;
using System;

public class MouseLogic : MonoBehaviour
{
    public static MouseLogic Instance { get; private set; }

    private List<Node> selectedNodes = new List<Node>();
    private List<Node> canBeSelectedNodes = new List<Node>();

    [SerializeField] private LayerMask layerMask;
    private bool mouseHold;

    private Node lastNode;
    private const int LEAST_TO_SELECT = 3;

    public event EventHandler OnNodesPope;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        mouseHold = Input.GetKey(KeyCode.Mouse0);

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, int.MaxValue, layerMask);



        if (hit && hit.transform.TryGetComponent(out Node node))
        {

            
           if(selectedNodes.Count > 0)
            {
                if (mouseHold && !selectedNodes.Contains(node) && NodeManager.Instance.GetNieghaborNodes(lastNode).Contains(node) && node.GetIsSelectable())
                {
                    //ADDING_ANOTHER_NODE!
                    node.NodeSelected();
                    selectedNodes.Add(node);
                    lastNode = node;
                    //CHECK_IF_THERE_ARE_BOMBS_AROUND
                }
            }
            else if(mouseHold && node.GetIsSelectable())
            {
                //FIRST_SELECTED_NODE!
                lastNode = node;
                node.NodeSelected();
                selectedNodes.Add(node);
                canBeSelectedNodes = NodeManager.Instance.GetNodeGroup(node);
                canBeSelectedNodes.Add(node);
                node.OnCanBeSelected();
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {

         /*   for (int i = 0; i < NodeManager.Instance.nodesArray.Length; i++)
            {
                NodeManager.Instance.BombNode(NodeManager.Instance.nodesArray[3, 2])[i].GetComponent<SpriteRenderer>().color = Color.white;
            }/*/

            Pop();
        }
    }

    private void Pop()
    {

        if(selectedNodes.Count < LEAST_TO_SELECT)
        {
            for (int i = 0; i < canBeSelectedNodes.Count; i++)
            {
                canBeSelectedNodes[i].CanNotBeSelected();
            }
            for (int i = 0; i < selectedNodes.Count; i++)
            {
                selectedNodes[i].NodeRemoveSelected();
            }
            lastNode = null;
            selectedNodes = new List<Node>();
            canBeSelectedNodes = new List<Node>();
            return;
        }

        //MORE_THAN_THE_LEAST_AMOUNT
        OnNodesPope?.Invoke(this, EventArgs.Empty);


        for (int i = 0; i < selectedNodes.Count; i++)
        {
            selectedNodes[i].NodeRemoveSelected();
            selectedNodes[i].HideTheNode();
            selectedNodes[i].SetIsSelected(false);
            //selectedNodes[i].GenerateAnotherInstance();

        }
        for (int i = 0; i < canBeSelectedNodes.Count; i++)
        {
            canBeSelectedNodes[i].CanNotBeSelected();
        }
        
        lastNode = null;
        selectedNodes = new List<Node>();
        canBeSelectedNodes = new List<Node>();

        
    }
}
