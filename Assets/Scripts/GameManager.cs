using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh meshTree;
    [SerializeField] private Material materialTree;
    [SerializeField] private Mesh meshLeaf;
    [SerializeField] private Material materialLeaf;
    [SerializeField] private Transform gridParent;
    private float[] treeOffsets;
    private float[] leafOffsets;
    private Matrix4x4[] treeMatrices;
    private Matrix4x4[] leafMatrices;

    private Vector3[] treePositions;
    private Vector3[] leafPositions;
    private RenderParams treeRP;
    private RenderParams leafRP;

    private int xCount = 256;
    private int zCount = 256;
    private float spacing = 1f;

    void Start()
    {
        Vector3[,] gridPositions = GridGenerator.GenerateGridPositions(xCount, zCount, spacing, spacing, gridParent);
        int count = xCount * zCount;

        treePositions = new Vector3[count];
        leafPositions = new Vector3[count];

        treeOffsets = new float[count];
        leafOffsets = new float[count];

        treeMatrices = new Matrix4x4[count];
        leafMatrices = new Matrix4x4[count];

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;

                treeOffsets[i] = gridPositions[z, x].y;
                leafOffsets[i] = gridPositions[z, x].y + 0.9f;

                treePositions[i] = gridPositions[z, x];
                leafPositions[i] = gridPositions[z, x];
            }
        }

        treeRP = new RenderParams(materialTree);
        leafRP = new RenderParams(materialLeaf);
    }

    private void RenderTrees()
    {
        for (var i = 0; i < treePositions.Length; i++)
        {
            treeMatrices[i].SetTRS(treePositions[i], Quaternion.identity, new Vector3(0.2f, 0.2f, 0.2f));
        }

        Graphics.RenderMeshInstanced(treeRP, meshTree, 0, treeMatrices);
    }

    private void RenderLeaves()
    {
        for (var i = 0; i < leafPositions.Length; i++)
        {
            leafMatrices[i].SetTRS(leafPositions[i] + new Vector3(0, leafOffsets[i], 0), Quaternion.identity, new Vector3(0.2f, 0.2f, 0.2f));
        }

        Graphics.RenderMeshInstanced(leafRP, meshLeaf, 0, leafMatrices);
    }

    void Update()
    {
        RenderTrees();
        RenderLeaves();
    }
}
