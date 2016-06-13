using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum GridState{
	FULL = 0,
	FREE = 1
}


public class Map : MonoBehaviour {
	//---- singleton ----
	private Map(){}
	private static Map inst;
	public static Map Instance{
		get{
			if(inst == null){
				inst = GameObject.Find("Map").GetComponent<Map>();
			}
			return inst;
		}
	}
	private A_Star m_aStar;
	public A_Star AStar{
		get{
			if(m_aStar == null){
				m_aStar = new A_Star(m_grids);
			}
			return m_aStar;
		}
	}

	IList<GameObject> m_pathNodes = new List<GameObject> ();
	public void PrintPath(IList<Grid> pathGrids){
		for (int i = 0; i < m_pathNodes.Count; i++) {
			DestroyObject(m_pathNodes[i]);
		}
		for (int i = 0; i < pathGrids.Count - 1; i++) {
			GameObject go = Instantiate (m_pathNode) as GameObject;
			go.transform.SetParent (transform);
			Grid grid = pathGrids[i];
			Vector3 pos = grid.transform.position;
			go.transform.position = new Vector3(pos.x, 1.1f, pos.z);
			m_pathNodes.Add(go);
		}
	}
	void PrintDestination(Grid des){
		Vector3 v = des.transform.position;
		v.y = 1.1f;
		m_desNode.transform.position = v;
		m_desNode.SetActive(true);
	}

	//-------------------
	int row = 0;
	int col = 0;
	Grid[,] m_grids;

	NPC m_NPC;

	Object m_gridEmpty;
	Object m_girdFull;
	Object m_pathNode;
	Object m_UIText;
	GameObject m_desNode;
	Canvas m_canvas;
	void Awake(){
		m_gridEmpty = Resources.Load("Prefabs/gridEmpty");
		m_girdFull = Resources.Load("Prefabs/gridFull");
		m_pathNode = Resources.Load("Prefabs/pathNode");
		m_UIText = Resources.Load("Prefabs/Text");
		m_desNode = Instantiate (Resources.Load ("Prefabs/desNode")) as GameObject;
		m_desNode.SetActive (false);
		m_canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();
	}

	void Start () {
		CreateMap ();
	}
	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			if(hit.collider.gameObject.tag == "gridEmpty"){
				Grid grid = hit.collider.gameObject.GetComponent<Grid>();
				ClearGridData();
				m_NPC.MoveToGrid(grid);
				PrintDestination(grid);
			}
		}
	}

	void CreateMap(){
		row = 6;
		col = 7;
		m_grids = new Grid[6, 7];
		int[,] config = new int[6, 7]{
			{0,0,0,0,0,0,0},
			{0,0,0,1,0,0,0},
			{0,0,0,1,0,0,0},
			{0,0,0,1,0,0,0},
			{0,1,0,1,0,0,0},
			{0,0,0,0,0,0,0}
		};
		for (int r = 0; r < row; r++) {
			for(int c = 0; c < col; c++){
				GridState state = GridState.FREE;
				if(config[r, c] == 1){
					state = GridState.FULL;
				}
				m_grids[r, c] = AddGrid(r, c, state);
			}
		}
		m_NPC = AddNPC (3, 1);
	}
	void ClearGridData(){
		for (int r = 0; r < row; r++) {
			for(int c = 0; c < col; c++){
				m_grids[r, c].Parent = null;
			}
		}
	}


	Grid AddGrid(int r, int c, GridState state){
		GameObject gridGO = null;
		if (state == GridState.FREE) {
			gridGO = Instantiate (m_gridEmpty) as GameObject;
		} else if (state == GridState.FULL) {
			gridGO = Instantiate (m_girdFull) as GameObject;
		}
		gridGO.transform.SetParent (transform);
		int posX = r - row/2;
		int posZ = c - col/2 + 1;
		gridGO.transform.position = new Vector3 (posX, 0, posZ);
		Grid grid = gridGO.AddComponent<Grid> ();
		grid.State = state;
		grid.Row = r;
		grid.Col = c;

		GameObject textObj = Instantiate (m_UIText) as GameObject;
		textObj.transform.SetParent (m_canvas.transform);
		Vector3 textPos = Camera.main.WorldToScreenPoint (gridGO.transform.position);
		textPos.x -= 15;
		textPos.y -= 15;
		textObj.transform.position = textPos;
		textObj.transform.SetParent (m_canvas.transform);
		Text textUI = textObj.GetComponent<Text> ();
		textUI.text = "(" + r + "," + c + ")";

		return grid;
	}

	NPC AddNPC(int r, int c){
		if (m_NPC != null) {
			return m_NPC;
		}
		NPC npc = null;
		GameObject go = Instantiate(Resources.Load("Prefabs/NPC")) as GameObject;
		go.transform.SetParent (transform);
		Grid empty = m_grids [r, c];
		if (empty != null) {
			Vector3 pos = empty.transform.position;
			pos.y = 1;
			go.transform.position = pos;
			
			npc = go.AddComponent<NPC> ();
			npc.m_currentGrid = empty;
		}
		return npc;
	}
}
