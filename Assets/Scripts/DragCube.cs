using UnityEngine;
using System.Collections;

/*  Controls touch and drag input behaviour 
 * 
 */

public class DragCube : MonoBehaviour {

    private float distance;
    private float maxDistance = 0.5f;
    private bool dragging;
    private Vector3 offset;
    private Transform target;
    private CubeProperties prop;


	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 update;

        if (Input.touchCount < 1)
        {
            dragging = false;
            return;
        }

        Touch touch = Input.touches[0];
        
        // Detect touch and assign target to object
        if(touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Cube") {
                if (hit.collider.gameObject.GetComponent<CubeProperties>().draggable == true) {
                    Debug.Log("here");
                    target = hit.transform;
                    prop = hit.transform.GetComponent<CubeProperties>();
                    distance = target.position.z - Camera.main.transform.position.z;
                    update = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, target.position.y, distance));
                    offset = target.position - update;
                    dragging = true;
                }
            }
        }

        // Control drag movement
        if(dragging && touch.phase == TouchPhase.Moved)
        {
            // Set update position to cursor position
            update = new Vector3(Input.mousePosition.x, target.position.y, distance);
            update = Camera.main.ScreenToWorldPoint(update);

            // Clamp maximum left drag
            if (update.x < transform.position.x)
                if (transform.position.x - update.x > maxDistance)
                    update.x = transform.position.x - maxDistance;

            // Clamp maximum right drag
            if (update.x > transform.position.x)
                if (update.x - transform.position.x > maxDistance)
                    update.x = transform.position.x + maxDistance;

            // Clamp the x position if an object is adjacent to it
            if ((!prop.dragLeft && update.x <= target.position.x) || (!prop.dragRight && update.x >= target.position.x))
                update.x = target.position.x;

            // Update y value
            if (prop.fall)
                update.y = target.position.y - prop.fallSpeed * Time.fixedDeltaTime;

            // Update to new position
            target.position = update + offset;

        }

        // End dragging
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, transform.position.z);
        }
	}

    
}
