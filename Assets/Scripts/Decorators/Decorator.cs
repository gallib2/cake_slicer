using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{
    const string CANDLE = "Candle";
    const string CHERRY = "Cherry";

    [SerializeField]
    private DecoratorType type;

	public DecoratorType Type
	{
		get { return type; }
	}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //        if (hit.collider != null)
    //        {
    //            Debug.Log("hit hit hit hit" + hit.collider.gameObject.name);
    //            Debug.Log("layer layer layer" + hit.transform.gameObject.layer);
    //            if (hit.collider.gameObject.CompareTag(CANDLE))
    //            {
    //                Destroy(hit.collider.gameObject);
    //            }
    //        }
    //    }
    //}

    //private void OnMouseDown()
    //{
    //    Debug.Log("----------------------------hellooo");
    //}

}

public enum DecoratorType
{
    CHERRY,
    CANDLE
}
