using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh meshTree;
    [SerializeField] private Material materialTree;
    [SerializeField] private Mesh meshLeaf;
    [SerializeField] private Material materialLeaf;
    [SerializeField] private Transform gridParent;
    private Vector3[] treeOffsets;
    private Vector3[] leafOffsets;
    private Matrix4x4[] treeMatrices;
    private Matrix4x4[] leafMatrices;

    private Vector3[] treePositions;
    private Vector3[] leafPositions;
    private RenderParams treeRP;
    private RenderParams leafRP;

    private Quaternion baseOrientation = Quaternion.identity;
    private float baseScale = 0.2f;
    
    private int xCount = 185;
    private int zCount = 185;
    private float spacing = 1.3837837837837f;

    DataAPI api = new DataAPI("http://localhost:3069");
    User[] users;

    void Start()
    {
        StartCoroutine(api.GetRequest("/retrieveAll", (_users) =>
        {
            if (_users != null)
            {
                users = _users;
                InitializeGame();
            }
            else
            {
                Debug.LogError("Failed to load trees");
            }
        }));
    }

    private void InitializeGame()
    {
        Vector3[,] gridPositions = GridGenerator.GenerateGridPositions(xCount, zCount, spacing, spacing, gridParent);

        int count = xCount * zCount;

        treePositions = new Vector3[count];
        leafPositions = new Vector3[count];

        treeOffsets = new Vector3[count];
        leafOffsets = new Vector3[count];

        treeMatrices = new Matrix4x4[count];
        leafMatrices = new Matrix4x4[count];

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;

                treeOffsets[i] = Vector3.zero;
                leafOffsets[i] = new Vector3(0, 4.520493f, -0.5f);

                treePositions[i] = gridPositions[z, x];
                leafPositions[i] = gridPositions[z, x];
            }
        }

        treeRP = new RenderParams(materialTree);
        leafRP = new RenderParams(materialLeaf);
    }

    private void RenderTrees()
    {
        if (treePositions.Length <= 0) return;
        for (var i = 0; i < treePositions.Length; i++)
        {
            float height = users[i].height;
            float scale = GameCalculations.MapExponential(height);
            treeMatrices[i].SetTRS(treePositions[i] + (treeOffsets[i] * scale), baseOrientation, GameCalculations.ScaleToVector3(scale));
        }

        Graphics.RenderMeshInstanced(treeRP, meshTree, 0, treeMatrices);
    }

    private void RenderLeaves()
    {
        if (leafPositions.Length <= 0) return;
        for (var i = 0; i < leafPositions.Length; i++)
        {
            float height = users[i].height;
            float scale = GameCalculations.MapExponential(height);
            leafMatrices[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), baseOrientation, GameCalculations.ScaleToVector3(scale));
        }

        Graphics.RenderMeshInstanced(leafRP, meshLeaf, 0, leafMatrices);
    }

    private void Update()
    {
        RenderTrees();
        RenderLeaves();
    }
}
