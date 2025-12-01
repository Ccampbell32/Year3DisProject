using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridEditorTool : Editor
{
    private GridManager grid;

    private List<Vector2> walledtiles = new List<Vector2>();

    private void OnEnable()
    {
        grid = (GridManager)target;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default GridManager inspector first
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("🧱 Grid Editor Tool", EditorStyles.boldLabel);

        if (GUILayout.Button("Regenerate Grid"))
        {
            foreach (Transform child in grid.transform)
                DestroyImmediate(child.gameObject);
            //grid.SendMessage("GenerateGrid");
        }

        EditorGUILayout.HelpBox(
            "Click tiles in Scene view to paint them.\n" +
            "Hold SHIFT while clicking to paint Mud.\n" +
            "Hold CTRL while clicking to paint Walls.\n" +
            "Normal click restores Normal tile.", MessageType.Info);
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TileSelector tile = hit.collider.GetComponent<TileSelector>();
                if (tile != null)
                {
                    PaintTile(tile, e.shift, e.control);
                    e.Use();
                }
            }
        }
    }

    private void PaintTile(TileSelector tile, bool shiftHeld, bool ctrlHeld)
    {
        GameObject prefab = grid.normalTilePrefab;
        bool walkable = true;
        int moveCost = 1;

        if (shiftHeld)
        {
            prefab = grid.mudTilePrefab;
            moveCost = 2;
        }
        else if (ctrlHeld)
        {
            prefab = grid.wallTilePrefab;
            walkable = false;
        }

        // Replace old tile
        Vector3 pos = tile.transform.position;
        Quaternion rot = tile.transform.rotation;
        int x = tile.x;
        int y = tile.y;

        DestroyImmediate(tile.gameObject);

        GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab, grid.transform);
        newTile.transform.position = pos;
        newTile.transform.rotation = rot;
        newTile.name = $"Tile_{x}_{y}";

        TileSelector newSelector = newTile.GetComponent<TileSelector>();
        if (newSelector == null)
            newSelector = newTile.AddComponent<TileSelector>();

        newSelector.x = x;
        newSelector.y = y;
        newSelector.isWalkable = walkable;
        newSelector.moveCost = moveCost;

        if (prefab == grid.wallTilePrefab)
            walledtiles.Add(new Vector2(x, y));
        
        Debug.Log($"Painted {newTile.name} as {(ctrlHeld ? "Wall" : shiftHeld ? "Mud" : "Normal")}");
    }

    private void OnApplicationQuit()
    {
        string path = "Assets/MattsStuff/";
        System.IO.File.WriteAllLines(path, walledtiles.ConvertAll(v => v.x + "," + v.y));
    }
}
