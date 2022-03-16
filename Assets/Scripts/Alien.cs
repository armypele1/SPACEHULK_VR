using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alien : GamePiece
{
    // Start is called before the first frame update
    void Awake()
    {
        actionPoints = 6;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void onEndDrag()
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

        //re-enable grabbable colliders now that movement has ended
        manager.toggleGrabbables(true); 

        //make grid invisible
        grid.setVisibility(false);
        onEndAttack();
    }
}
