using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh meshTree;
    [SerializeField] private Material materialTree;
    [SerializeField] private Mesh meshLeaf;
    [SerializeField] private Material materialLeaf1;
    [SerializeField] private Material materialLeaf2;
    [SerializeField] private Material materialLeaf3;
    [SerializeField] private Material materialLeaf4;
    [SerializeField] private Material materialLeaf5;
    [SerializeField] private Transform gridParent;
    private Vector3[] treeOffsets;
    private Vector3[] leafOffsets;
    private Quaternion[] treeOrientations;
    private Quaternion[] leafOrientations;
    private Matrix4x4[] treeMatrices;

    private Matrix4x4[] leafMatricesStage1;
    private Matrix4x4[] leafMatricesStage2;
    private Matrix4x4[] leafMatricesStage3;
    private Matrix4x4[] leafMatricesStage4;
    private Matrix4x4[] leafMatricesStage5;


    private Vector3[] treePositions;
    private Vector3[] leafPositions;
    private RenderParams treeRP;
    private RenderParams leafRP1;
    private RenderParams leafRP2;
    private RenderParams leafRP3;
    private RenderParams leafRP4;
    private RenderParams leafRP5;


    private int xCount = 145;
    private int zCount = 145;
    private float spacing = 2f;

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

        leafMatricesStage1 = new Matrix4x4[count];
        leafMatricesStage2 = new Matrix4x4[count];
        leafMatricesStage3 = new Matrix4x4[count];
        leafMatricesStage4 = new Matrix4x4[count];
        leafMatricesStage5 = new Matrix4x4[count];

        treeOrientations = new Quaternion[count];
        leafOrientations = new Quaternion[count];

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;

                treeOffsets[i] = Vector3.zero;
                leafOffsets[i] = new Vector3(0, 4.520493f, -0.5f);

                treePositions[i] = gridPositions[z, x];
                leafPositions[i] = gridPositions[z, x];

                Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                treeOrientations[i] = randomYRotation;
                leafOrientations[i] = randomYRotation;
            }
        }

        treeRP = new RenderParams(materialTree);
        leafRP1 = new RenderParams(materialLeaf1);
        leafRP2 = new RenderParams(materialLeaf2);
        leafRP3 = new RenderParams(materialLeaf3);
        leafRP4 = new RenderParams(materialLeaf4);
        leafRP5 = new RenderParams(materialLeaf5);
    }

    private void RenderTrees()
    {
        if (treePositions.Length <= 0) return;
        for (var i = 0; i < treePositions.Length; i++)
        {
            float height = users[i].height;
            float scale = GameCalculations.MapExponential(height);
            treeMatrices[i].SetTRS(treePositions[i] + (treeOffsets[i] * scale), treeOrientations[i], GameCalculations.ScaleToVector3(scale));
        }

        Graphics.RenderMeshInstanced(treeRP, meshTree, 0, treeMatrices);
    }

    private void RenderLeaves()
    {
        if (leafPositions.Length <= 0) return;
        for (var i = 0; i < leafPositions.Length; i++)
        {
            User user = users[i];
            float height = user.height;
            float scale = GameCalculations.MapExponential(height);
            Vector3 size = GameCalculations.ScaleToVector3(scale);

            switch (GameCalculations.GetStageFromAge(user.age)) {
                case 1: leafMatricesStage1[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], size); break;
                case 2: leafMatricesStage2[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], size); break;
                case 3: leafMatricesStage3[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], size); break;
                case 4: leafMatricesStage4[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], size); break;
                case 5: leafMatricesStage5[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], size); break;
            }

            // leafMatricesStage1[i].SetTRS(leafPositions[i] + (leafOffsets[i] * scale), leafOrientations[i], GameCalculations.ScaleToVector3(scale));
            }

        Graphics.RenderMeshInstanced(leafRP1, meshLeaf, 0, leafMatricesStage1);
        Graphics.RenderMeshInstanced(leafRP2, meshLeaf, 0, leafMatricesStage2);
        Graphics.RenderMeshInstanced(leafRP3, meshLeaf, 0, leafMatricesStage3);
        Graphics.RenderMeshInstanced(leafRP4, meshLeaf, 0, leafMatricesStage4);
        Graphics.RenderMeshInstanced(leafRP5, meshLeaf, 0, leafMatricesStage5);
    }

    private void Update()
    {
        RenderTrees();
        RenderLeaves();
    }
}
