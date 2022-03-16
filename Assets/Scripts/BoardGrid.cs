using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    private GameObject Boardgame;
    private float rayRange;
    private float unitDistance;
    public Transform getNearestSpace(Transform entity){
        float zero = 0;
        float smallestDistance = 1 / zero;
        float currentDistance = 0;
        Transform closestSpace = null;
        Vector3 point = Vector3.zero;

        foreach (Transform space in transform)
        {
            if (space.gameObject.GetComponent<Renderer>().enabled == true && space.gameObject.tag == "free"){
                //adjust position used to calculate distance so that it lines up with centre of player character
                var adjustedSpacePos = space.position;
                adjustedSpacePos.y += unitDistance*1.5f;
                currentDistance = Vector3.Distance(adjustedSpacePos, entity.position);                    
            
                if (currentDistance < smallestDistance){
                    smallestDistance = currentDistance;
                    closestSpace = space;
                }
            }            
        }

        //if dist is too far go back to original position
        if (smallestDistance > unitDistance){
            return null;
        }
        else{          
            return closestSpace;
        }     
        
    }
    public List<GameObject> getAvailableSpaces(GameObject originSpace, int actionPoints){      
        List<GameObject> availableSpaces = new List<GameObject>();
        //first add the origin space to the available spaces list so that it is visible
        originSpace.GetComponent<GridSpace>().cost = 0;
        availableSpaces.Add(originSpace);
        int originalActionPoints = actionPoints;
        return getAvailableSpacesHelper(availableSpaces, originSpace, actionPoints, originalActionPoints);
        
    }
    private List<GameObject> getAvailableSpacesHelper(List<GameObject> availableSpaces, GameObject originSpace, int actionPoints, int originalActionPoints){
        int layer_mask = LayerMask.GetMask("Grid");
        if (actionPoints > 0){  
            actionPoints -= 1;           
            RaycastHit hit; 
            if (Physics.Raycast(originSpace.transform.position, Vector3.forward, out hit, rayRange, layer_mask)){
                var availableSpace = hit.collider.gameObject;
                if (!(availableSpaces.Contains(availableSpace)))
                {
                    availableSpaces.Add(availableSpace);
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;

                }  
                else if (availableSpace.GetComponent<GridSpace>().cost > originalActionPoints - actionPoints){
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;
                }
                if (actionPoints > 0 && availableSpace.tag == "free"){
                        availableSpaces = getAvailableSpacesHelper(availableSpaces, availableSpace, actionPoints, originalActionPoints);
                }
          
            } 
            if (Physics.Raycast(originSpace.transform.position, Vector3.right, out hit, rayRange, layer_mask)){
                var availableSpace = hit.collider.gameObject;
                if (!(availableSpaces.Contains(availableSpace)))
                {
                    availableSpaces.Add(availableSpace);     
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;     
                } 
                else if (availableSpace.GetComponent<GridSpace>().cost > originalActionPoints - actionPoints){
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;
                }
                if (actionPoints > 0 && availableSpace.tag == "free"){
                        availableSpaces = getAvailableSpacesHelper(availableSpaces, availableSpace, actionPoints, originalActionPoints);
                }
            }
            if (Physics.Raycast(originSpace.transform.position, Vector3.back, out hit, rayRange, layer_mask)){
                var availableSpace = hit.collider.gameObject;
                if (!(availableSpaces.Contains(availableSpace)))
                {
                    availableSpaces.Add(availableSpace);   
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;      
                } 
                else if (availableSpace.GetComponent<GridSpace>().cost > originalActionPoints - actionPoints){
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;
                }
                if (actionPoints > 0 && availableSpace.tag == "free"){
                        availableSpaces = getAvailableSpacesHelper(availableSpaces, availableSpace, actionPoints, originalActionPoints);
                    } 
            }
            if (Physics.Raycast(originSpace.transform.position, Vector3.left, out hit, rayRange, layer_mask)){
                var availableSpace = hit.collider.gameObject;
                if (!(availableSpaces.Contains(availableSpace)))
                {
                    availableSpaces.Add(availableSpace);
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;
                }  
                else if (availableSpace.GetComponent<GridSpace>().cost > originalActionPoints - actionPoints){
                    availableSpace.GetComponent<GridSpace>().cost = originalActionPoints - actionPoints;
                }
                if (actionPoints > 0 && availableSpace.tag == "free"){
                    availableSpaces = getAvailableSpacesHelper(availableSpaces, availableSpace, actionPoints, originalActionPoints);
                }
            }                
        }
        return availableSpaces;
    }

    //add enum or string param for movement, kill, death etc?
    public void updateGrid(GridSpace oldSpace, GridSpace newSpace){
        oldSpace.gameObject.tag = "free";
        newSpace.gameObject.tag = "ally";

        oldSpace.UpdateColor();
        newSpace.UpdateColor();
    }
    public void updateStartSpace(GridSpace space, string team){
        space.gameObject.tag = team;
        space.UpdateColor();
    }

    public void setVisibility(bool isVisible){
        foreach (Transform space in transform)
        {
            var rend = space.gameObject.GetComponent<Renderer>();
            rend.enabled = isVisible;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {   
         Boardgame = GameObject.Find("Boardgame");  
         unitDistance = Boardgame.transform.lossyScale.x;
         rayRange = unitDistance * 2.5f;
         Debug.Log(rayRange); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
