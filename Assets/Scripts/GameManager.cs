using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mesh meshTree1;
    [SerializeField] private Mesh meshTree2;
    [SerializeField] private Mesh meshTree3;
    [SerializeField] private Mesh meshTree4;
    [SerializeField] private Mesh meshTree5;
    [SerializeField] private Mesh meshTreeDepression;
    [SerializeField] private Mesh meshTreeBurnout;
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

    private bool PassesFilters(User user)
    {
        // Age filter
        if (FilterSettings.AgeFilter > 0 && user.age < FilterSettings.AgeFilter)
            return false;

        // Weight filter
        if (FilterSettings.WeightFilter > 0 && user.weight < FilterSettings.WeightFilter)
            return false;

        // Height filter
        if (FilterSettings.HeightFilter > 0 && user.height < FilterSettings.HeightFilter)
            return false;

        // Alcohol intake filter
        if (FilterSettings.AlchoholIntakeFilter > 0 && user.alchohol_intake < FilterSettings.AlchoholIntakeFilter)
            return false;

        // Gender filter
        if (FilterSettings.GenderFilter != Genders.None && user.gender.ToUpper() != FilterSettings.GenderFilter.ToString().ToUpper())
            return false;

        // Mood filter
        if (FilterSettings.MoodFilter != Moods.None)
        {
            if (FilterSettings.MoodFilter == Moods.Depression && !(user.depression > 0f))
                return false;
            if (FilterSettings.MoodFilter == Moods.Burnout && !(user.burnout > 0f))
                return false;
        }

        return true; // Passed all active filters
    }


    private void InitializeGame()
    {
        Vector3[,] gridPositions = GridGenerator.GenerateGridPositions(xCount, zCount, spacing, spacing, gridParent);

        stageMatrices = new List<Matrix4x4>[7];
        for (int s = 0; s < stageMatrices.Length; s++)
            stageMatrices[s] = new List<Matrix4x4>();

        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                int i = z * gridPositions.GetLength(1) + x;
                User user = users[i];

                if (!PassesFilters(user))
                    continue; // Skip this tree

                Vector3 pos = gridPositions[z, x] + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                float scale = GameCalculations.MapExponential(user.height);
                Vector3 size = GameCalculations.ScaleToVector3(scale);
                Matrix4x4 trs = Matrix4x4.TRS(pos, rot, size);

                int stage = GameCalculations.GetStageFromAge(user.age) - 1; // 0-4
                if (user.depression > 0f)
                {
                    stageMatrices[5].Add(trs);
                    continue;
                }
                if (user.burnout > 0f)
                {
                    stageMatrices[6].Add(trs);
                    continue;
                }

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
        if (stageMatrices == null || users == null) return;

        // Clear filtered stage matrices for this frame
        List<Matrix4x4>[] filteredMatrices = new List<Matrix4x4>[7];
        for (int s = 0; s < filteredMatrices.Length; s++)
            filteredMatrices[s] = new List<Matrix4x4>();

        int userIndex = 0; // Track which user corresponds to which matrix in stageMatrices

        for (int stage = 0; stage < 5; stage++) // Normal stages 0-4
        {
            for (int i = 0; i < stageMatrices[stage].Count; i++)
            {
                User user = users[userIndex];
                if (PassesFilters(user))
                    filteredMatrices[stage].Add(stageMatrices[stage][i]);

                userIndex++;
            }
        }

        // Depression trees
        for (int i = 0; i < stageMatrices[5].Count; i++)
        {
            User user = users[userIndex];
            if (PassesFilters(user))
                filteredMatrices[5].Add(stageMatrices[5][i]);

            userIndex++;
        }

        // Burnout trees
        for (int i = 0; i < stageMatrices[6].Count; i++)
        {
            User user = users[userIndex];
            if (PassesFilters(user))
                filteredMatrices[6].Add(stageMatrices[6][i]);

            userIndex++;
        }

        // Render using filtered matrices
        RenderStage(meshTree1, filteredMatrices[0]);
        RenderStage(meshTree2, filteredMatrices[1]);
        RenderStage(meshTree3, filteredMatrices[2]);
        RenderStage(meshTree4, filteredMatrices[3]);
        RenderStage(meshTree5, filteredMatrices[4]);
        RenderStage(meshTreeDepression, filteredMatrices[5]);
        RenderStage(meshTreeBurnout, filteredMatrices[6]);
    }
}
