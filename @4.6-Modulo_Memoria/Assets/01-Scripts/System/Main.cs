using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public Color32 selected_color;
    public Color32 unselected_color;
    Token go_selected_1;
    Token go_selected_2;

    void Awake()
    {
        setStatics();
        //setGameStatus(Statics.Game_Status.play);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Statics.game_satus == Statics.Game_Status.play)
        {
            //USUARIO HACE CLICK
            if(Input.GetMouseButtonDown(0))
            {
                userRaycast();
            }

            //SI LOS 2 GO_SELECTED SON DISTINTOS DE NULL SE VERIFICA SI SON PAREJAS O NO
            if(go_selected_1 != null && go_selected_2 != null)
            {
                if(go_selected_1.go_par == go_selected_2.gameObject)
                {
                    Destroy(go_selected_1.gameObject);
                    Destroy(go_selected_2.gameObject);
                }
                else
                {
                    go_selected_1.gameObject.renderer.material.color = unselected_color;
                    go_selected_2.gameObject.renderer.material.color = unselected_color;
                    go_selected_1 = null;
                    go_selected_2 = null;
                }
            }

            //VERIFICO SI QUEDAN HIJOS EN EL TOKEN_PARENT
            if(Statics.Token_Root.transform.childCount == 0)
            {
                Debug.Log("fin game");
                setGameStatus(Statics.Game_Status.win);
            }
        }
        else if(Statics.game_satus == Statics.Game_Status.win)
        {
            Debug.Log("win");
            Statics.Win_Text.SetActive(true);
            setGameStatus(Statics.Game_Status.pause);
        }
	}

    private void setStatics()
    {
        Statics.main_controller = this;
        Statics.Token_Root = GameObject.Find(Constants.Tokens_Root);
        Statics.tokens_controller = Statics.Token_Root.GetComponent<TokensController>();
        Statics.game_satus = Statics.Game_Status.pause;
        Statics.Win_Text = GameObject.Find(Constants.Win_Text);
        Statics.Win_Text.SetActive(false);
    }

    public void setGameStatus(Statics.Game_Status newStatus)
    {
        Statics.game_satus = newStatus;
    }

    void userRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.collider.name);
            if(go_selected_1 == null)
            {
                go_selected_1 = hit.collider.gameObject.GetComponent<Token>();
                go_selected_1.gameObject.renderer.material.color = selected_color;
            }
            else if(go_selected_2 == null)
            {
                go_selected_2 = hit.collider.gameObject.GetComponent<Token>();
                go_selected_2.gameObject.renderer.material.color = selected_color;
            }
        }
    }
}