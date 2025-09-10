using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private Transform gridParent;
    private float[] cubeOffsets;
    private Matrix4x4[] matrices;

    private Vector3[] positions;
    private RenderParams rp;

    private int xCount = 256;
    private int zCount = 256;
    private float spacing = 1f;


    void Start()
    {
        Vector3[,] gridPositions = GridGenerator.GenerateGridPositions(xCount, zCount, spacing, spacing, gridParent);
        int count = xCount * zCount;

        positions = new Vector3[count];
        cubeOffsets = new float[count];
        matrices = new Matrix4x4[count];

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                int i = z * gridPositions.GetLength(1) + x;

                // cube.transform.position = gridPositions[z, x];
                // cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                // cube.name = $"Cube - {i}";

                cubeOffsets[i] = gridPositions[z, x].y;
                positions[i] = gridPositions[z, x];
            }
        }

        rp = new RenderParams(material);
    }

    void Update()
    {
        for (var i = 0; i < positions.Length; i++)
        {
            matrices[i].SetTRS(positions[i], Quaternion.identity, Vector3.one);
        }

        Graphics.RenderMeshInstanced(rp, mesh, 0, matrices);
    }
}
