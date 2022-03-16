using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    private GameObject[] gamePieces;
    private enum Team {marines, aliens};
    [SerializeField]
    public InputAction nextTurn = null;
    Team currentTeam;
    
    public void OnEnable()
    {
        nextTurn.Enable();
    }

    public void OnDisable()
    {
        nextTurn.Disable();
    }


    //a simple function which toggles all gamePieces to be either grabbable or ungrabbable, useful when testing
    public void toggleGrabbables(bool toggle){
        var activeTeam = currentTeam == Team.marines ? "marine" : "alien";

        gamePieces = GameObject.FindGameObjectsWithTag(activeTeam);

        if (toggle == true){       
            foreach (GameObject gamePiece in gamePieces){
                gamePiece.GetComponent<Collider>().enabled = true;
            }
        }
        else{
            foreach (GameObject gamePiece in gamePieces){
                gamePiece.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void switchTeams(){
        Debug.Log("TESTING");

        if (currentTeam == Team.marines){
            currentTeam = Team.aliens;

            var oldTeam = GameObject.FindGameObjectsWithTag("marine");

            foreach (GameObject oldTeamMember in oldTeam){
                oldTeamMember.GetComponent<Collider>().enabled = false;
                oldTeamMember.GetComponent<GamePiece>().actionPoints = 4;
                oldTeamMember.GetComponent<GamePiece>().previousGridSpace.tag = "enemy";
                oldTeamMember.GetComponent<GamePiece>().previousGridSpace.GetComponent<GridSpace>().UpdateColor();
                
            }

            var newTeam = GameObject.FindGameObjectsWithTag("alien");

            foreach (GameObject newTeamMember in newTeam){
                newTeamMember.GetComponent<GamePiece>().previousGridSpace.tag = "ally";
                newTeamMember.GetComponent<GamePiece>().previousGridSpace.GetComponent<GridSpace>().UpdateColor();
            }  
        }
        else{
            currentTeam = Team.marines;

            var oldTeam = GameObject.FindGameObjectsWithTag("alien");

            foreach (GameObject oldTeamMember in oldTeam){
                oldTeamMember.GetComponent<Collider>().enabled = false;
                oldTeamMember.GetComponent<GamePiece>().actionPoints = 6;
                oldTeamMember.GetComponent<GamePiece>().previousGridSpace.tag = "enemy";
                oldTeamMember.GetComponent<GamePiece>().previousGridSpace.GetComponent<GridSpace>().UpdateColor();
            }       

            var newTeam = GameObject.FindGameObjectsWithTag("marine");

            foreach (GameObject newTeamMember in newTeam){
                newTeamMember.GetComponent<GamePiece>().previousGridSpace.tag = "ally";
                newTeamMember.GetComponent<GamePiece>().previousGridSpace.GetComponent<GridSpace>().UpdateColor();
            }  
        }

        toggleGrabbables(true);
    }
    
    void Start()
    {
        currentTeam = Team.marines;
        nextTurn.performed += ctx => switchTeams();
        nextTurn.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
