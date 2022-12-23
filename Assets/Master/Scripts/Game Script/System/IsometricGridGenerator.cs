using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arsa.RTSSystem.MapGeneration
{
    public class IsometricGridGenerator : MonoBehaviour
    {
        [Header("Setup")]
        [Space(10)]
        [SerializeField] private GameObject m_nodePrefab;
        [Space(15)]
        [Header("Settings")]
        [Space(10)]
        [SerializeField] private int m_width;
        [Space(2)]
        [SerializeField] private int m_length;
        [Space(2)]
        [SerializeField] private Vector2 m_offsets;

        private Dictionary<Vector2, int> _nodes = new Dictionary<Vector2, int>();

        private void Start()
        {
            GenerateGridPositions(m_width, m_length);

        }

        private void DrawGrid(int widht, int length)
        {
            Vector2 c_key = Vector2.zero;
            for (int x = 0; x < widht; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    c_key.x = x;
                    c_key.y = y;

                    if (_nodes[c_key] == 1)
                    {
                        GameObject c_gridGO;
                        c_gridGO = Instantiate(m_nodePrefab, transform);
                        float c_x = (x - y) * m_offsets.x;
                        float c_y = -(x + y) * m_offsets.y;
                        float c_z = 1f - (x + y) / (float)(m_width + m_width);
                        c_gridGO.transform.position = new Vector3(c_x, c_y, c_z);
                        c_gridGO.transform.name = string.Format("{0}, {1}", x, y);
                        continue;
                    }



                }
            }
        }

        private void GenerateGridPositions(int widht, int length)
        {

            for (int x = 0; x < widht; x++)
            {
                for (int z = 0; z < length; z++)
                {

                    _nodes.Add(new Vector2(x, z), 1);
                }
            }
        }

        public void GenerateGrid()
        {
            GenerateGridPositions(m_width, m_length);
            DrawGrid(m_width, m_length);
        }

        #region Count Neighbor
        public int CountAllNeighbor(int x, int z)
        {
            int c_numberOfNeighbour = 0;

            c_numberOfNeighbour = CountSquareNeighbor(x, z) + CountDiagonalNeighbor(x, z);

            return c_numberOfNeighbour;
        }

        private int CountDiagonalNeighbor(int x, int z)
        {
            //Diagonal
            int c_numberOfNeighbour = 0;

            if (_nodes.ContainsKey(new Vector2(x + 1, z - 1))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x + 1, z + 1))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x - 1, z - 1))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x - 1, z + 1))) c_numberOfNeighbour++;

            return c_numberOfNeighbour;
        }

        private int CountSquareNeighbor(int x, int z)
        {
            //Horizontal

            int c_numberOfNeighbour = 0;

            if (_nodes.ContainsKey(new Vector2(x + 1, z))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x - 1, z))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x, z + 1))) c_numberOfNeighbour++;
            if (_nodes.ContainsKey(new Vector2(x, z - 1))) c_numberOfNeighbour++;

            return c_numberOfNeighbour;
        }
        #endregion
    }
}