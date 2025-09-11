using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh meshTree1;
    [SerializeField] private Mesh meshTree2;
    [SerializeField] private Mesh meshTree3;
    [SerializeField] private Mesh meshTree4;
    [SerializeField] private Mesh meshTree5;
    [SerializeField] private Material materialTree;
    [SerializeField] private Transform gridParent;
    private Vector3[] treeOffsets;
    private Quaternion[] treeOrientations;

    private Matrix4x4[] treeMatrices1;
    private Matrix4x4[] treeMatrices2;
    private Matrix4x4[] treeMatrices3;
    private Matrix4x4[] treeMatrices4;
    private Matrix4x4[] treeMatrices5;

    private Vector3[] treePositions;
    private RenderParams treeRP;

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
        treeOffsets = new Vector3[count];

        treeOrientations = new Quaternion[count];

        treeMatrices1 = new Matrix4x4[count];
        treeMatrices2 = new Matrix4x4[count];
        treeMatrices3 = new Matrix4x4[count];
        treeMatrices4 = new Matrix4x4[count];
        treeMatrices5 = new Matrix4x4[count];

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;

                treeOffsets[i] = Vector3.zero;
                treePositions[i] = gridPositions[z, x] + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

                Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                treeOrientations[i] = randomYRotation;
            }
        }

        treeRP = new RenderParams(materialTree);
    }

    private void RenderTrees()
    {
        if (treePositions.Length <= 0) return;
        for (var i = 0; i < treePositions.Length; i++)
        {
            User user = users[i];
            float height = user.height;
            float scale = GameCalculations.MapExponential(height);
            Vector3 size = GameCalculations.ScaleToVector3(scale);

            switch (GameCalculations.GetStageFromAge(user.age))
            {
                case 1: treeMatrices1[i].SetTRS(treePositions[i], treeOrientations[i], size); break;
                case 2: treeMatrices2[i].SetTRS(treePositions[i], treeOrientations[i], size); break;
                case 3: treeMatrices3[i].SetTRS(treePositions[i], treeOrientations[i], size); break;
                case 4: treeMatrices4[i].SetTRS(treePositions[i], treeOrientations[i], size); break;
                case 5: treeMatrices5[i].SetTRS(treePositions[i], treeOrientations[i], size); break;
            }
        }

        Graphics.RenderMeshInstanced(treeRP, meshTree1, 0, treeMatrices1);
        Graphics.RenderMeshInstanced(treeRP, meshTree2, 0, treeMatrices2);
        Graphics.RenderMeshInstanced(treeRP, meshTree3, 0, treeMatrices3);
        Graphics.RenderMeshInstanced(treeRP, meshTree4, 0, treeMatrices4);
        Graphics.RenderMeshInstanced(treeRP, meshTree5, 0, treeMatrices5);
    }

    private void Update()
    {
        RenderTrees();
    }
}
