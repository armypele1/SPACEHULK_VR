using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePiece : MonoBehaviour, IDrag
{
    protected BoardGrid grid;
    protected Manager manager;
    public Vector3 previousPos;

    [SerializeField]
    public GameObject previousGridSpace; 
    private Vector3 velocity = Vector3.zero;

    private Material mat;
    public float alpha = .5f;

    public Color oldColor;

    protected GameObject dummyObject;

    private GameObject lineObject;
    private bool isAttackActive;
    public int actionPoints;

    public virtual void onEndDrag()
    {          
        //disable ghost mode
        gameObject.GetComponent<Renderer>().material.SetColor("_Color",oldColor);
        Material tempMaterial = dummyObject.GetComponent<Renderer>().material;  

        //destroy dummy object
        if (dummyObject != null){
            Destroy(dummyObject);
            Destroy(tempMaterial);
        }   
        

        //snap back to grid and update grid accordingly
        SnapToGrid();
        var aliens = GameObject.FindGameObjectsWithTag("alien");
        foreach (var alien in aliens){
            if (alien.GetComponent<GamePiece>().previousPos == this.previousPos){
                Destroy(alien);
            }
        }

        //re-enable grabbable colliders now that movement has ended
        manager.toggleGrabbables(true); 

        //make grid invisible
        grid.setVisibility(false);
        onEndAttack();
    }

    public void onStartDrag()
    {
        //save previous pos before movement begins
        previousPos = grid.transform.InverseTransformPoint(transform.position);

        //disable colliders of all other grabbables while holding this object
        manager.toggleGrabbables(false);

        //get dummy object to pose in original pos
        dummyObject = GameObject.Instantiate(gameObject);
        dummyObject.GetComponent<Collider>().enabled = false;
        dummyObject.GetComponent<Renderer>().material.SetColor("_Color",oldColor);
        dummyObject.transform.SetParent(grid.transform, true);
        //set to ghost mode
        activateGhost(gameObject.GetComponent<Renderer>().material); // Set to ghost version of original

        //make grid visible
        //grid.setVisibility(true);
        
        List<GameObject> availableSpaces = grid.getAvailableSpaces(previousGridSpace, actionPoints);

        
        foreach (var space in availableSpaces){
            var rend = space.gameObject.GetComponent<Renderer>();
            rend.enabled = true;
        }
        
    }
    public Vector3 getPreviousPos(){
        return previousPos;
    }

    protected void SnapToGrid(){
        Transform nearestSpace = grid.getNearestSpace(transform);
        if (nearestSpace != null){
            
            if (nearestSpace != previousGridSpace.transform){
                actionPoints = actionPoints - nearestSpace.GetComponent<GridSpace>().cost;
            }
            

            Vector3 snappedPosition = nearestSpace.parent.parent.InverseTransformPoint(nearestSpace.position);
            Debug.Log(nearestSpace.position);
            snappedPosition.y = nearestSpace.lossyScale.y + 1;
            transform.rotation = Quaternion.identity;
            transform.localPosition = snappedPosition; 

            Debug.Log(transform.position);

            grid.updateGrid(previousGridSpace.GetComponent<GridSpace>(), nearestSpace.gameObject.GetComponent<GridSpace>());
            
            //update previousGridSpace ready for next movement
            previousGridSpace = nearestSpace.gameObject;           
        }
        else{
            transform.localPosition = getPreviousPos();
            transform.rotation = Quaternion.identity;
        }
        
    }
    
    private void activateGhost(Material mat){
        oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
        mat.SetColor("_Color",newColor);
    }

    public void onStartAttack(){
        isAttackActive = true;
        lineObject = new GameObject("lineObject");
        LineRenderer attackLine = lineObject.AddComponent<LineRenderer>();
        attackLine.material = Resources.Load("Attack", typeof(Material)) as Material;
        attackLine.widthMultiplier = 0.01f;
        StartCoroutine(SetLinePos());
    }

    private IEnumerator SetLinePos(){
        int layer_mask = LayerMask.GetMask("Map");
        while (isAttackActive){
            RaycastHit hit;     
            if (Physics.Raycast(new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z), (previousGridSpace.transform.position - gameObject.transform.position).normalized, out hit, layer_mask)){
                //var wall = hit.collider.gameObject;
                lineObject.SetActive(false);
            }
            else{
                lineObject.SetActive(true);
                LineRenderer attackLine = lineObject.GetComponent<LineRenderer>();
                attackLine.SetPosition(0, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z));
                attackLine.SetPosition(1, new Vector3(previousGridSpace.transform.position.x,previousGridSpace.transform.position.y,previousGridSpace.transform.position.z));
            }
            
            yield return null;
        }
    }

    public void onEndAttack(){
        isAttackActive = false;
        if (lineObject != null){
            Destroy(lineObject);
        }       
    }

    public virtual void Start()
    {
        grid = FindObjectOfType<BoardGrid>();
        manager = FindObjectOfType<Manager>();
        Rigidbody myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.isKinematic = true;
        oldColor = gameObject.GetComponent<Renderer>().material.color;
        isAttackActive = false;
        //so that it is not null upon first movement
        //previousGridSpace =  GameObject.Find("Square147");
        previousPos = previousGridSpace.transform.parent.parent.InverseTransformPoint(previousGridSpace.transform.position);
        previousPos.y = previousGridSpace.transform.lossyScale.y + 1;
        //set character to correct location by simulating a turn
        //onStartDrag();
        //onEndDrag();
        SnapToGrid();
        //grid.updateStartSpace(previousGridSpace.GetComponent<GridSpace>());     
    }

    // Update is called once per frame
    void Update()
    {
        //make collision possible only when it is that team's turn and other object not already being grabbed
        
    }
}
