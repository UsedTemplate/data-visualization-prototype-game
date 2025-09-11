using System.Collections.Generic;
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

    private List<Matrix4x4>[] stageMatrices;
    private RenderParams treeRP;

    private int xCount = 185;
    private int zCount = 185;
    private float spacing = 1.55f;

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

        stageMatrices = new List<Matrix4x4>[5];
        for (int s = 0; s < stageMatrices.Length; s++)
            stageMatrices[s] = new List<Matrix4x4>();

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;
                User user = users[i];

                Vector3 pos = gridPositions[z, x] + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                float scale = GameCalculations.MapExponential(user.height);
                Vector3 size = GameCalculations.ScaleToVector3(scale);

                Matrix4x4 trs = Matrix4x4.TRS(pos, rot, size);

                int stage = GameCalculations.GetStageFromAge(user.age) - 1; // 0-4
                stageMatrices[stage].Add(trs);
            }
        }

        treeRP = new RenderParams(materialTree);
    }

    private void RenderStage(Mesh mesh, List<Matrix4x4> matrices)
    {
        int batchSize = 1023;
        for (int i = 0; i < matrices.Count; i += batchSize)
        {
            int count = Mathf.Min(batchSize, matrices.Count - i);
            Graphics.RenderMeshInstanced(treeRP, mesh, 0, matrices.GetRange(i, count).ToArray());
        }
    }

    private void Update()
    {
        RenderStage(meshTree1, stageMatrices[0]);
        RenderStage(meshTree2, stageMatrices[1]);
        RenderStage(meshTree3, stageMatrices[2]);
        RenderStage(meshTree4, stageMatrices[3]);
        RenderStage(meshTree5, stageMatrices[4]);
    }
}
