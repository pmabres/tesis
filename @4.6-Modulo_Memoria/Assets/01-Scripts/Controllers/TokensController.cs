using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokensController : MonoBehaviour {

    public int cantTokens;
    public GameObject token_Prefab;
    public List<string> tokens_types = new List<string>();
    List<Token> tokens_list = new List<Token>();
    GameObject child1;
    GameObject child2;

    Token[,] token_matrix = new Token[4, 4];
    
    [HideInInspector]
    bool status_tokens;
    bool tokens_hide;

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(status_tokens && !tokens_hide && Statics.game_satus != Statics.Game_Status.play)
        {
            StartCoroutine(ShowAndHide());
        }
        else if(!status_tokens)
        {
            generateTokenInstances(cantTokens/2);
        }
	}

    /// <summary>
    /// Metodo que genera los GameObject de las distintas cuevas.
    /// </summary>
    /// <param name="cant"></param>
    private void generateTokenInstances(int cant)
    {
        status_tokens = true;
        int posX = 0;
        int posY = 1;
        float newPosZ = 0;
        int randToken;

        for (int i = 0; i < cant; i++)
        {
            do
            {
                randToken = Mathf.FloorToInt(Random.Range(0, 8));
            } while (tokens_types[randToken] == "");

            child1 = (GameObject)Instantiate(token_Prefab);
            child2 = (GameObject)Instantiate(token_Prefab);

            child1.transform.parent = Statics.Token_Root.gameObject.transform;
            child2.transform.parent = Statics.Token_Root.gameObject.transform;

            Token child_token1 = child1.AddComponent<Token>();
            Token child_token2 = child2.AddComponent<Token>();

            child1.name = "token1_" + tokens_types[randToken];
            child2.name = "token2_" + tokens_types[randToken];

            child_token1.name = child1.name;
            child_token2.name = child2.name;

            child_token1.go_par = child2;
            child_token2.go_par = child1;

            float newPosX;
            float newPosY;

            do
            {
                posX = Mathf.FloorToInt(Random.Range(0, 4));
                posY = Mathf.FloorToInt(Random.Range(0, 4));
            } while (token_matrix[posX,posY] != null);
                
            token_matrix[posX, posY] = child_token1;

            newPosX = (7 * posX);
            newPosY = (5 * posY);

            child1.transform.localPosition = new Vector3(newPosX, newPosY, newPosZ);
            child1.transform.localScale = new Vector3(2, 2, 2);

            do
            {
                posX = Mathf.FloorToInt(Random.Range(0, 4));
                posY = Mathf.FloorToInt(Random.Range(0, 4));
            } while (token_matrix[posX, posY] != null);

            token_matrix[posX, posY] = child_token2;

            newPosX = (7 * posX);
            newPosY = (5 * posY);

            child2.transform.localPosition = new Vector3(newPosX, newPosY, newPosZ);
            child2.transform.localScale = new Vector3(2, 2, 2);    

            tokens_types[randToken] = "";
        }
        
    }
     
    /// <summary>
    /// Este metodo se usara para hacer un fade de los tokens mediante itween. 
    /// *17-02-2015: Actualmente solo modifica la escala de los mismos. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowAndHide ()
    {
        tokens_hide = true;
        yield return new WaitForSeconds(5f);
        for (int f = 0; f < 4; f++)
        {
            for (int c = 0; c < 4; c++)
            {
                token_matrix[f, c].gameObject.transform.localScale = new Vector3(1, 1, 1);
            }            
        }
        Statics.main_controller.setGameStatus(Statics.Game_Status.play);
    }

    public void resetTokens()
    {
        Statics.Win_Text.SetActive(false);
        //ACA IRIA UN METODO QUE LEA DE LA BD EL SIGUENTE GRUPO DE PALABRAS.
        //
        tokens_types.Clear();
        tokens_types.Add("Calabaza");
        tokens_types.Add("Jamon");
        tokens_types.Add("Jabon");
        tokens_types.Add("Parlante");
        tokens_types.Add("Vaso");
        tokens_types.Add("Botella");
        tokens_types.Add("Papel");
        tokens_types.Add("Carton");
        token_matrix = new Token[4, 4];
        status_tokens = false;
        tokens_hide = false;
    }
}
