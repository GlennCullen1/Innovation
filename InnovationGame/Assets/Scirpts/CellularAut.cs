using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellularAut : MonoBehaviour {

	//Basic Board Variables
	public float m_BranchChance = 0.25f;
	public int m_NumItterations = 20;
	GameObject[,] m_Board;
	public GameObject m_BlankCellPrefab;
	public GameObject m_StartCellPrefab;
	public GameObject m_WorkingCellPrefab;
	public GameObject m_ClosedCellPrefab;
	public Vector2 m_StartPos;
	public List<Vector2> WorkingSet;
	//Algorithm Variables

	// Use this for initialization
	void Start () {
		m_Board = new GameObject[m_NumItterations, m_NumItterations];
		WorkingSet = new List<Vector2>();
		Vector2 dimensions;
		dimensions.x = m_BlankCellPrefab.GetComponent<Renderer> ().bounds.size.x;
		dimensions.y = m_BlankCellPrefab.GetComponent<Renderer> ().bounds.size.z;

		for(int cntx =0; cntx < m_NumItterations; cntx++) {
			for(int cnty =0; cnty< m_NumItterations; cnty++) {
				m_Board[cntx,cnty] = (GameObject)Instantiate(m_BlankCellPrefab,
				                                              new Vector3(cntx*dimensions.x,0,cnty*dimensions.y), // negative to ensure origin at "bottom left"
				                                              Quaternion.identity);
			}
		}

		SetupBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			try{
				Itterate();
			}
			catch(System.Exception e)
			{

			}
		}
	}

	void ReplaceCell(int XCoord, int YCoord, GameObject NewPrefab)
	{
		Vector3 pos = m_Board [(int)XCoord, (int)YCoord].transform.position;
		Destroy (m_Board [(int)XCoord, (int)YCoord]);
		m_Board [(int)XCoord, (int)YCoord] = (GameObject)Instantiate (NewPrefab, pos, Quaternion.identity);
	}
	void ReplaceCell (GameObject Cell, GameObject NewPrefab)
	{
		int XCoord = (int)Cell.GetComponent<CellRecord> ().m_Coords.x;
		int YCoord = (int)Cell.GetComponent<CellRecord> ().m_Coords.y;
		Vector3 pos = m_Board [(int)XCoord, (int)YCoord].transform.position;
		Destroy (m_Board [(int)XCoord, (int)YCoord]);
		m_Board [(int)XCoord, (int)YCoord] = (GameObject)Instantiate (NewPrefab, pos, Quaternion.identity);
	}
	void SetupBoard()
	{
		ReplaceCell ((int)m_StartPos.x, (int)m_StartPos.y, m_StartCellPrefab); 
		ActivateCell (new Vector2 (m_StartPos.x, m_StartPos.y + 1), m_StartPos);
	}

	void ActivateCell(Vector2 cell, Vector2 parentcell)
	{
		//replace the cell with working set graphic, add a cell record to it, add it to working set
		ReplaceCell ((int)cell.x, (int)cell.y, m_WorkingCellPrefab);
		CellRecord record = m_Board [(int)cell.x, (int)cell.y].AddComponent<CellRecord> ();
		record.Initialise (parentcell, cell);
		WorkingSet.Add (cell);
	}

	void Itterate()
	{
		List<Vector2> TempSet = new List<Vector2>(WorkingSet);
		WorkingSet.Clear ();
		foreach (Vector2 vect in TempSet) {
			GameObject obj = m_Board[(int) vect.x, (int)vect.y];
			if(Random.Range(0.0f,1.0f) <m_BranchChance)
			{
				Branch(obj);
			}
			else
			{
				switch(obj.GetComponent<CellRecord>().m_Direction)
				{
				case direction.North:
					ActivateCell(new Vector2(obj.GetComponent<CellRecord>().m_Coords.x,
					                         obj.GetComponent<CellRecord>().m_Coords.y+1),
					             obj.GetComponent<CellRecord>().m_Coords);
					break;
				case direction.South:
					ActivateCell(new Vector2(obj.GetComponent<CellRecord>().m_Coords.x,
					                         obj.GetComponent<CellRecord>().m_Coords.y-1),
					             obj.GetComponent<CellRecord>().m_Coords);
					break;
				case direction.East:
					ActivateCell(new Vector2(obj.GetComponent<CellRecord>().m_Coords.x+1,
					                         obj.GetComponent<CellRecord>().m_Coords.y),
					             obj.GetComponent<CellRecord>().m_Coords);
					break;
				case direction.West:
					ActivateCell(new Vector2(obj.GetComponent<CellRecord>().m_Coords.x-1,
					                         obj.GetComponent<CellRecord>().m_Coords.y),
					             obj.GetComponent<CellRecord>().m_Coords);
					break;
				}
			}
			ReplaceCell(obj, m_ClosedCellPrefab);
		}
	}

	void Branch(GameObject cell)
	{
		List<direction> directions = new List<direction>();
		direction dir = cell.GetComponent<CellRecord> ().m_Direction;
		switch (dir) {
		case direction.North:
			directions.Add(direction.North);
			directions.Add(direction.East);
			directions.Add(direction.West);
			break;
		case direction.East:
			directions.Add(direction.North);
			directions.Add(direction.East);
			directions.Add(direction.South);
			break;
		case direction.South:
			directions.Add(direction.South);
			directions.Add(direction.East);
			directions.Add(direction.West);
			break;
		case direction.West:
			directions.Add(direction.North);
			directions.Add(direction.South);
			directions.Add(direction.West);
			break;
		}

		int numberofbranches = Random.Range (1, directions.Count + 1);
		while (numberofbranches >0) {
			direction targetcell  = directions[Random.Range (0,directions.Count)];
			switch(targetcell) {
			case direction.North:
				if (m_Board[(int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y+1].GetComponent<CellRecord>()== null)
				   {
					ActivateCell(new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y+1),
					             new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y));
				   }
				directions.Remove(direction.North);
				numberofbranches --;
				break;
			case direction.East:
				if (m_Board[(int)cell.GetComponent<CellRecord>().m_Coords.x+1, (int)cell.GetComponent<CellRecord>().m_Coords.y].GetComponent<CellRecord>()== null)
				{
					ActivateCell(new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x+1, (int)cell.GetComponent<CellRecord>().m_Coords.y),
					             new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y));
				}
				directions.Remove(direction.East);
				numberofbranches --;
				break;
			case direction.South:
				if (m_Board[(int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y-1].GetComponent<CellRecord>()== null)
				{
						ActivateCell(new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y-1),
					             new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y));
				}
				directions.Remove(direction.South);
				numberofbranches --;
				break;
			case direction.West:
				if (m_Board[(int)cell.GetComponent<CellRecord>().m_Coords.x-1, (int)cell.GetComponent<CellRecord>().m_Coords.y].GetComponent<CellRecord>()== null)
				{
							ActivateCell(new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x-1, (int)cell.GetComponent<CellRecord>().m_Coords.y),
						             new Vector2((int)cell.GetComponent<CellRecord>().m_Coords.x, (int)cell.GetComponent<CellRecord>().m_Coords.y));
				}
				directions.Remove(direction.West);
				numberofbranches --;
				break;
			}
		}
	}
}
