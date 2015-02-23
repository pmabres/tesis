using UnityEngine;
using UnityEngine.UI;
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
                    Statics.multipler++;
                    Statics.inMultipler = true;
                    //falta definir un score de otra forma segun nivel.
                    AddScore(5);
                }
                else
                {
                    go_selected_1.gameObject.renderer.material.color = unselected_color;
                    go_selected_2.gameObject.renderer.material.color = unselected_color;
                    go_selected_1 = null;
                    go_selected_2 = null;
                    Statics.multipler = 0;
                    Statics.inMultipler = false;
                    Statics.Score_Text.text = "SCORE= " + Statics.round_score.ToString();
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

    /// <summary>
    /// Metodo que llena el script estatico con sus respectivos objetos, para un control general.
    /// </summary>
    private void setStatics()
    {
        Statics.main_controller = this;
        Statics.Token_Root = GameObject.Find(Constants.Tokens_Root);
        Statics.tokens_controller = Statics.Token_Root.GetComponent<TokensController>();
        Statics.game_satus = Statics.Game_Status.pause;
        Statics.Win_Text = GameObject.Find(Constants.Win_Text);
        Statics.Win_Text.SetActive(false);
        Statics.Score_Text = GameObject.Find(Constants.Score_Text).GetComponent<Text>();
    }
    
    /// <summary>
    /// Metodo publico para setear el gameStatus
    /// </summary>
    /// <param name="newStatus"></param>
    public void setGameStatus(Statics.Game_Status newStatus)
    {
        Statics.game_satus = newStatus;
    }

    /// <summary>
    /// Metodo privado que genera el raycast
    /// </summary>
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

    void AddScore(float score)
    {
        Statics.round_score += (int) (score * Statics.multipler);
        if(Statics.multipler > 1)
        {
            Statics.Score_Text.text = "SCORE= " + Statics.round_score.ToString() + " X" + Statics.multipler.ToString();
        }
        else
        {
            Statics.Score_Text.text = "SCORE= " + Statics.round_score.ToString();
        }
    }
}