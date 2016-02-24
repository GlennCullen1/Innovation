using UnityEngine;
using System.Collections;

public enum direction {North,South,East,West};
public class CellRecord : MonoBehaviour {

	public direction m_Direction;
	public Vector2 m_Coords;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialise(Vector2 ParentPos, Vector2 ThisPos)
	{
		m_Coords = ThisPos;
		if (ThisPos.x - ParentPos.x > 0) { // Wast
			m_Direction= direction.East;
		} else if (ThisPos.x - ParentPos.x > 0) { //West
			m_Direction= direction.West;
		} else if (ThisPos.y - ParentPos.y > 0) { //North
			m_Direction= direction.North;
		} else if (ThisPos.y - ParentPos.y < 0) { //South
			m_Direction= direction.South;
		}

	}
}
