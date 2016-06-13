/*A星算法的步骤：
{
	将起点区域添加到open列表中，该区域有最小的和值。
重复以下：
	将open列表中最小F值的区域X移除，然后添加到close列表中。
	对于与X相邻的每一块可通行且不在close列表中的区域T：
	如果T不在open列表中：添加到open列表，把X设为T的前驱
	如果T已经在open列表中：检查F是否更小。如果是，更新F的前驱
直到：
	终点添加到了close列表。（已找到路径）
	终点未添加到close列表且open列表已空。（未找到路径）
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star {
	int m_row;
	int m_col;
	Grid[,] m_grids;
	public A_Star(Grid[,] grids){
		m_grids = grids;
		m_row = m_grids.GetLength (0);
		m_col = m_grids.GetLength (1);
	}
	public IList<Grid> Find(Grid start, Grid end){
		IList<Grid> openList = new List<Grid> ();
		IList<Grid> closeList = new List<Grid> ();
		openList.Add (start);

		int result = 0;
		do {
			result = UpdateList (openList, closeList, start, end);
		} while(result == 0);

		if (result == 2) {
			IList<Grid> paths = new List<Grid> ();
			Grid parent = end;
			do {
				paths.Insert(0, parent);
				parent = parent.Parent;
			} while(parent != null);
			return paths;
		}
		return null;
	}
	int UpdateList(IList<Grid> openList, IList<Grid> closeList, Grid start, Grid end){
		if (openList.Count == 0) {
			//Debug.Log("no path");
			return 1;
		}

		Grid minGrid = GetMinFGrid (openList, start, end);
		Debug.Log("get minFVal ("+ minGrid.Row + "," + minGrid.Col + ") ");

		if (minGrid == end) {
			Debug.Log ("reach end!");
			return 2;
		}
		openList.Remove (minGrid);
		closeList.Add (minGrid);
		IList<Grid> nearGrids = GetNearGrid (minGrid);
		for (int i = 0; i < nearGrids.Count; i++) {
			Grid grid = nearGrids[i];
			if(grid.State == GridState.FULL){
				continue;
			}
			if(closeList.Contains(grid)){
				continue;
			}
			if(!openList.Contains(grid)){
				openList.Add(grid);
				grid.Parent = minGrid;
				Debug.Log("add ("+ grid.Row + "," + grid.Col + ") next to ("+ minGrid.Row + "," + minGrid.Col + ")");
			}else{
				int minValue = minGrid.GetF(start, end);
				int value = grid.GetF(start, end);
				if(value < minValue){
					grid.Parent = minGrid;
					Debug.Log("reset ("+ grid.Row + "," + grid.Col + ") next ("+ minGrid.Row + "," + minGrid.Col + ")");
				}
			}
		}
		return 0;
	}
	Grid GetMinFGrid(IList<Grid> list, Grid start, Grid end){
		int minV = list [0].GetF(start, end);
		Grid minGrid = list [0];
		for (int i = 1; i < list.Count; i++) {
			int v = list[i].GetF(start, end);
			if(v <= minV){
				minV = v;
				minGrid = list[i];
			}
		}
		return minGrid;
	}
	List<Grid> GetNearGrid(Grid grid){
		List<Grid> nearGrids = new List<Grid> ();

		int r = grid.Row;
		int c = grid.Col;

		if (r > 0) {
			nearGrids.Add(m_grids[r - 1, c]);
		}
		if (c > 0) {
			nearGrids.Add(m_grids[r, c - 1]);
		}
		if (r < m_row - 1) {
			nearGrids.Add(m_grids[r + 1, c]);
		}
		if (c < m_col - 1) {
			nearGrids.Add(m_grids[r, c + 1]);
		}
		return nearGrids;
	}

}
