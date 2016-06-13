using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {
	IList<Grid> m_paths = null;
	float m_speed = 0.05f;
	float m_minDis = 0.1f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_paths == null) {
			return;
		}
		if (m_paths.Count == 0) {
			return;
		}
		Vector3 destination = m_paths [0].transform.position;
		Vector3 curPosition = transform.position;
		curPosition.y = destination.y;
		if (Vector3.Distance (destination, curPosition) < m_minDis) {
			transform.position = new Vector3(destination.x, transform.position.y, destination.z);
			m_currentGrid = m_paths[0];
			m_paths.RemoveAt (0);
		} else {
			float dx = destination.x - curPosition.x;
			float dz = destination.z - curPosition.z;
			if(dx != 0)
				dx = dx > 0 ? m_speed : -m_speed;
			if(dz != 0)
				dz = dz > 0 ? m_speed : -m_speed;
			transform.Translate(new Vector3(dx, 0, dz));
		}
	}

	public Grid m_currentGrid;
	public void MoveToGrid(Grid grid){
		IList<Grid> paths = Map.Instance.AStar.Find (m_currentGrid, grid);
		if (paths == null) {
			Debug.Log ("not found path!");
		} else {
			Debug.Log("find path!");
			Map.Instance.PrintPath(paths);
			m_paths = paths;
		}
	}
}
