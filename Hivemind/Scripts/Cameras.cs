using System.Collections;
using UnityEngine;

public class Cameras : MonoBehaviour {
    
    public GameObject background;
    public Transform target;

    public Transform mainCamera;
    public Transform supportCamera;

    public float offsetX = 0;
    public float offsetY = 0;

    float offsetZ = -10;
    float width;

    public bool lockToTarget = true;
    [HideInInspector] public bool lockedToTarget = true;

    Vector2 newTargetPos;
    [Range(0f, 10f)] public float cameraMoveSpeed = 10.0f;

    public float zoomRate = 0.3f;
    public float normalZoomLevel = 9.5f;
    public float runZoomLevel = 14f;
    public float runXOffset = 5f;

    bool returningFromZoom = false;
    
    float defaultDistanceForRelock = 1f;
    float actualDistanceForRelock;

    IEnumerator zoomCoroutine;

    void Start()
    {
        // If main camera not set, gets it
        if (!mainCamera) mainCamera = Camera.main.transform;

        // If support camera not set, tries to get the 2nd child
        if (!supportCamera) supportCamera = transform.GetChild(1);

        // If background object not set, finds one
        if (background == null) background = FindObjectOfType<BackgroundGenerator>().gameObject;

        // Setting the support camera distance from the main camera based on background's width.
        for (int i = 0; i < background.transform.childCount; i++)
        {
            if (background.transform.GetChild(i).name.Contains("Background"))
                width += background.transform.GetChild(i).GetComponent<SpriteRenderer>().bounds.size.x;
        }

        // Setting the main camera to center and the support camera to the side
        mainCamera.localPosition = new Vector3(0, mainCamera.localPosition.y, offsetZ);
        supportCamera.localPosition = new Vector3(width, supportCamera.localPosition.y, offsetZ);

        transform.SetParent(target);
    }

    void LateUpdate()
    {
        if (!FindObjectOfType<AdvancedHivemind>()) return;
        //if (target == null) target = FindObjectOfType<AdvancedHivemind>().hivemind[0].Character.transform;

        if (!target)
        {
            Camera.main.orthographicSize -= Time.deltaTime * 0.25f;
            supportCamera.GetComponent<Camera>().orthographicSize -= Time.deltaTime * 0.25f;
            return;
        }

        if (transform.position.x < -width / 2 || transform.position.y > width / 2)
        {
            //target.transform.position = 
        }

        if (lockedToTarget && lockToTarget)
        {

            // Sets the main camera's position to target's position
            transform.position = new Vector3(target.transform.position.x - offsetX, target.transform.position.y + offsetY, offsetZ);

        }
        else if (!returningFromZoom)
        {
            //newTargetPos = new Vector2(target.transform.position.x, target.transform.position.y + offsetY);

            /* Lerp version */
            transform.position = Vector2.Lerp(transform.position, newTargetPos, Time.deltaTime * cameraMoveSpeed);
            if (Vector2.Distance(transform.position, newTargetPos) < defaultDistanceForRelock / cameraMoveSpeed)
            {
                if (lockToTarget) lockedToTarget = true;
            }

            /* MoveTowards version 
            transform.position = Vector2.MoveTowards(transform.position, newTargetPos, cameraMoveSpeed * Time.deltaTime);
            if (transform.position == newTargetPos && lockToTarget) lockedToTarget = true;
            */
        }
        else
        {
            newTargetPos = new Vector2(target.transform.position.x, target.transform.position.y + offsetY);

            transform.position = Vector2.Lerp(transform.position, newTargetPos, Time.deltaTime * cameraMoveSpeed);
            if (Vector2.Distance(transform.position, newTargetPos) < defaultDistanceForRelock / cameraMoveSpeed)
            {
                if (lockToTarget) lockedToTarget = true;
                returningFromZoom = false;
            }
        }

        // Sets the support camera's x-position to the opposite side of the map.
        if (Mathf.Sign(transform.position.x) == Mathf.Sign(supportCamera.localPosition.x))
        {
            supportCamera.localPosition = new Vector3(supportCamera.localPosition.x * -1, supportCamera.localPosition.y, supportCamera.localPosition.z);
            //supportCamera.GetComponent<Camera>().depth *= -1;
        }
    }

    public void ChangeTargetSmooth(GameObject target, float cameraSpeed)
    {
        ChangeTarget(target);
        cameraMoveSpeed = cameraSpeed;
        actualDistanceForRelock = defaultDistanceForRelock / cameraMoveSpeed;
    }

    public void ChangeTarget(GameObject target)
    {
        lockedToTarget = false;
        this.target = target.transform;
        newTargetPos = new Vector2(target.transform.position.x, target.transform.position.y + offsetY);
        transform.SetParent(this.target);
    }

    public void SetRunXOffset(int direction)
    {
        //offsetX = Mathf.Sign(direction) * -runXOffset;
        lockedToTarget = false;
        newTargetPos = new Vector2(target.transform.position.x + Mathf.Sign(direction) * runXOffset, target.transform.position.y + offsetY);
    }

    public void ActivateRunCamera(bool value)
    {
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        if (value)
        {
            zoomCoroutine = ChangeZoomLevel(runZoomLevel);
            returningFromZoom = false;
        }
        else
        {
            zoomCoroutine = ChangeZoomLevel(normalZoomLevel);
            lockedToTarget = false;
            returningFromZoom = true;
            newTargetPos = new Vector2(target.transform.position.x, target.transform.position.y + offsetY);
        }
        StartCoroutine(zoomCoroutine);
    }

    IEnumerator ChangeZoomLevel(float targetValue)
    {
        float beginningValue = mainCamera.GetComponent<Camera>().orthographicSize;

        if (beginningValue > targetValue)
        {
            while (mainCamera.GetComponent<Camera>().orthographicSize > targetValue)
            {
                mainCamera.GetComponent<Camera>().orthographicSize -= zoomRate;
                supportCamera.GetComponent<Camera>().orthographicSize -= zoomRate;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (beginningValue < targetValue)
        {
            while (mainCamera.GetComponent<Camera>().orthographicSize < targetValue)
            {
                mainCamera.GetComponent<Camera>().orthographicSize += zoomRate;
                supportCamera.GetComponent<Camera>().orthographicSize += zoomRate;
                yield return new WaitForSeconds(0.001f);
            }
        }

    }
}
