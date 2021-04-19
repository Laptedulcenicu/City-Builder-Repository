﻿using System.Collections.Generic;
using GameRig.Scripts.Utilities.ConstantValues;
using UnityEngine;

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is Point)
        {
            Point p = obj as Point;
            return this.X == p.X && this.Y == p.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return "P(" + this.X + ", " + this.Y + ")";
    }
}

public enum CellType
{
    Empty,
    Road,
    Structure,
    Nature,
    Barrier,
    None
}

public class Grid
{
    private CellType[,] _grid;
    private string[,] cellIndex;
    private int _width;
    private int _height;
    
    private readonly List<Point> roadList = new List<Point>();
    private readonly List<Point> structureList = new List<Point>();
    public List<Point> StructureList => structureList;
    

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new CellType[width, height];
    }
    
    

    
    public CellType this[int i, int j]
    {
        get { return _grid[i, j]; }
        set
        {
            if (value == CellType.Road)
            {
                
                roadList.Add(new Point(i, j));
            }
            
            if (value == CellType.Structure )
            {
                StructureList.Add(new Point(i, j));
            }

            _grid[i, j] = value;
        }
    }



    public static bool IsCellWakable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }

        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    public Point GetRandomRoadPoint()
    {
        if (roadList.Count == 0)
        {
            return null;
        }

        return roadList[Random.Range(0, roadList.Count)];
    }
    //
    // public Point GetRandomSpecialStructurePoint()
    // {
    //     if (_specialStructure.Count == 0)
    //     {
    //         return null;
    //     }
    //
    //     return _specialStructure[Random.Range(0, _specialStructure.Count)];
    // }
    //
    // public Point GetRandomHouseStructurePoint()
    // {
    //     if (_houseStructure.Count == 0)
    //     {
    //         return null;
    //     }
    //
    //     return _houseStructure[Random.Range(0, _houseStructure.Count)];
    // }
    //
    // public List<Point> GetAllHouses()
    // {
    //     return _houseStructure;
    // }
    //
    // internal List<Point> GetAllSpecialStructure()
    // {
    //     return _specialStructure;
    // }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWakableAdjacentCells((int) cell.X, (int) cell.Y, isAgent);
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0)
        {
            adjacentCells.Add(new Point(x - 1, y));
        }

        if (x < _width - 1)
        {
            adjacentCells.Add(new Point(x + 1, y));
        }

        if (y > 0)
        {
            adjacentCells.Add(new Point(x, y - 1));
        }

        if (y < _height - 1)
        {
            adjacentCells.Add(new Point(x, y + 1));
        }

        return adjacentCells;
    }

    public List<Point> GetWakableAdjacentCells(int x, int y, bool isAgent)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWakable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent) == false)
            {
                adjacentCells.RemoveAt(i);
            }
        }

        return adjacentCells;
    }

    public List<Point> GetAdjacentCellsOfType(int x, int y, CellType type)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }

        return adjacentCells;
    }

    
    public CellType[] GetAllAdjacentCellTypes(int x, int y)
    {
        CellType[] neighbours = {CellType.None, CellType.None, CellType.None, CellType.None};
        if (x > 0)
        {
            neighbours[0] = _grid[x - 1, y];
        }

        if (x < _width - 1)
        {
            neighbours[2] = _grid[x + 1, y];
        }

        if (y > 0)
        {
            neighbours[3] = _grid[x, y - 1];
        }

        if (y < _height - 1)
        {
            neighbours[1] = _grid[x, y + 1];
        }

        return neighbours;
    }
}