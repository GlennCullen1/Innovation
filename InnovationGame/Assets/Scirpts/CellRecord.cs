using UnityEngine;
using System.Collections;

public enum direction {North,South,East,West};
public class CellRecord : MonoBehaviour {

	public direction m_Direction;
	public Vector2 m_Coords;
	Vector3 m_ParentPos;
	// Use this for initialization
	void Start () {
		m_ParentPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	 
		Debug.DrawLine (transform.position, m_ParentPos,Color.red);

	}

	public void Initialise(Vector2 ParentPos, Vector2 ThisPos)
	{

		m_Coords = ThisPos;
		if (ThisPos.x - ParentPos.x > 0) { // east
			m_Direction= direction.East;
			m_ParentPos.x = transform.position.x+gameObject.GetComponent<Renderer> ().bounds.size.x;
		} else if (ThisPos.x - ParentPos.x < 0) { //West
			m_Direction= direction.West;
			m_ParentPos.x = transform.position.x-gameObject.GetComponent<Renderer> ().bounds.size.x;
		} else if (ThisPos.y - ParentPos.y > 0) { //North
			m_Direction= direction.North;
			m_ParentPos.z = transform.position.z+gameObject.GetComponent<Renderer> ().bounds.size.z;
		} else if (ThisPos.y - ParentPos.y < 0) { //South
			m_Direction= direction.South;
			m_ParentPos.z = transform.position.z-gameObject.GetComponent<Renderer> ().bounds.size.z;
		}

	}
}
