using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    private Node node;
    [SerializeField] private GameObject selectedPrefab;
    private GameObject selectedVisual;
    private Animator nodeAnimator;
    private SpriteRenderer selectedVisualSpriteRenderer;

    private const string OVER = "Over";
    private const string SPAWN = "Spawn";
    private const string NEIGHABOUR = "N";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        nodeAnimator = GetComponent<Animator>();
        node = GetComponent<Node>();
    }

    void Start()
    {

        node.OnNodeCanBeSelected += Node_OnNodeCanBeSelected;
        node.OnNodeCanNotBeSelected += Node_OnNodeCanNotBeSelected;
        node.OnNodeGenerated += Node_OnNodeGenerated;
        node.OnNodeSelected += Node_OnNodeSelected;
        node.OnNodeNotSelected += Node_OnNodeNotSelected;
        selectedVisual = Instantiate(selectedPrefab, transform);
        selectedVisualSpriteRenderer = selectedVisual.GetComponent<SpriteRenderer>();
        DisableCanBeSelectedVisual();

    }

    private void Node_OnBomb(object sender, System.EventArgs e)
    {
        selectedVisual.SetActive(true);
        selectedVisualSpriteRenderer.color = Color.red;
    }

    private void Node_OnNodeNotSelected(object sender, System.EventArgs e)
    {
        nodeAnimator.SetBool(OVER, false);
    }

    private void Node_OnNodeSelected(object sender, System.EventArgs e)
    {
        nodeAnimator.SetBool(OVER , true);
        transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedVisualSpriteRenderer.sortingOrder = 0;
        selectedVisualSpriteRenderer.color = Color.orange;
    }

    private void Node_OnNodeGenerated(object sender, System.EventArgs e)
    {
        nodeAnimator.SetTrigger(SPAWN);
    }

    private void Node_OnNodeCanNotBeSelected(object sender, System.EventArgs e)
    {
        DisableCanBeSelectedVisual();
    }

    private void Node_OnNodeCanBeSelected(object sender, System.EventArgs e)
    {
        EnableCanBeSelectedVisual();
    }
    public void EnableCanBeSelectedVisual()
    {
        nodeAnimator.SetBool(NEIGHABOUR, true);
        transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
        selectedVisualSpriteRenderer.sortingOrder = 2;
        selectedVisual.SetActive(true);
    }

    public void DisableCanBeSelectedVisual()
    {
        nodeAnimator.SetBool(NEIGHABOUR, false);
        selectedVisualSpriteRenderer.color = Color.white;
        transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedVisualSpriteRenderer.sortingOrder = 0;
        selectedVisual.SetActive(false);
    }
}
