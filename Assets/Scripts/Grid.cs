using UnityEngine;
using System.Collections;
using System;

public class Grid : MonoBehaviour {
	public GridState State;
	
	public int Row;
	public int Col;
	public Grid Parent;

	public int GetF(Grid start, Grid end){
		int gValue = GetG();
		int hValue = GetH (end);
		return gValue + hValue;
	}
	//G值表示当前标记离开始标记的路径耗费
	public int GetG(){
		int gValue = 0;
		if (Parent != null) {
			gValue = Parent.GetG() + 10;
		}
		return gValue;
	}
	//H值表示当前标记离目标方格的路径估值耗费
	public int GetH(Grid end){
		float dr = Math.Abs (Row - end.Row) * 10;
		float dc = Math.Abs (Col - end.Col) * 10;
		float dis = Mathf.Sqrt (dr * dr + dc * dc);
		return (int)dis;
		//int hValue = Math.Abs (Row - end.Row) + Math.Abs (Col - end.Col);
		//return hValue;
	}
}
