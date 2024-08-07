using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class interacao : MonoBehaviour
{
    public Camera cameraRayCast;
    private Rigidbody rbPlayer;
    public int distanceToMove = 10;
    public RaycastHit hit;

    public string falas;


    public GameObject[] dialogo;
    private int index;
    public int index1;
    public int index2;
    public int index3;
    public int index4;

    public TextMeshProUGUI texto;
    public TextMeshProUGUI texto2;
    public TextMeshProUGUI texto3;
    public TextMeshProUGUI texto4;
    public GameObject painelInformacao2;
    public GameObject painelInformacao3;
    public GameObject painelInformacao4;


    public GameObject indicator;
    public GameObject painelInformacao1;
    public TextMeshProUGUI texto1;
    public GameObject gabarito;
    public string teste;
    public Transform posicao;
    public GameObject personagemEncarregado;
    public GameObject conjuntoFuncionarios;

    public GameObject centralFerragens;
    public GameObject blocoConjuntoFuncinarios;
    public GameObject blocoEncarregado;
    public GameObject blocoEngenheiro;
    public GameObject escavacao;

    public GameObject blocoFerreiro;
    public GameObject painelInformacao;
    public GameObject blocoEngenheiro2;
    public GameObject blocoConcreto;
    public GameObject ferragens;
    public GameObject formas;
    public GameObject concretagem;

 
    public bool inicio;

    public GameObject blocoFinal;
    public GameObject painelInformacao5;
    public TextMeshProUGUI texto5;
    public int index5;
    void Start()
    {
        index = 0;
        falas = "Aperte <F> para interagir";
        texto.text = falas;

        index1 = 0;
        falas = "Aperte <F> para interagir";
        texto1.text = falas;
        index2 = 0;
        falas = "Aperte <F> para interagir";
        texto2.text = falas;

        index3 = 0;
        falas = "Aperte <F> para interagir";
        texto3.text = falas;

        falas = "Aperte <F> para interagir";

        index4 = 0;
        falas = "Aperte <F> para interagir";
        texto4.text = falas;

        index5 = 0;
        falas = "Aperte <F> para interagir";
        texto5.text = falas;


        blocoConjuntoFuncinarios.SetActive(false);
        blocoEncarregado.SetActive(false);
        blocoFerreiro.SetActive(false);
        blocoConcreto.SetActive(false);
        blocoFinal.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("GamePlayer");
        }
        Ray ray = cameraRayCast.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Debug.DrawRay(ray.origin, ray.direction * distanceToMove, Color.red, 0.5f);

        if (Physics.Raycast(ray, out hit, distanceToMove))
        {


            if (hit.collider.gameObject.tag == "Personagem")
            {



                painelInformacao.SetActive(true);

                if (index > 2)
                {
                    index = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto.text = falas;
                                indicator.SetActive(true);
                                break;
                            case 1:
                                falas = "FUNDAÇÕES: são elementos estruturais responsáveis por transmitir as casgas da constrção para o solo, garantindo e estabildade e segurança da edificação.";
                                texto.text = falas;
                                indicator.SetActive(true);
                                break;
                            case 2:
                                falas = "Sua primeira tarefa nessa missão é ir à central de forma e selecionar a opção forma do gabarito. ";
                                texto.text = falas;
                                indicator.SetActive(true);
                                hit.collider.gameObject.tag = teste;
                                blocoEncarregado.SetActive(true);
                                break;

                        }
                    }

                }

            }
            if (hit.collider.gameObject.tag == "Encarregado")
            {
                painelInformacao1.SetActive(true);

                if (index1 > 2)
                {
                    index1 = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto1.text = falas;
                                break;
                            case 1:
                                falas = "Olá, trabalho na central de formas, vamos as etapas de nossa obra com o gabarito";
                                texto1.text = falas;
                                break;
                            case 2:
                                falas = "Vá até o local da obra para ver o gabarito instalado. Em seguida vamos checar a equipe de escavação.";
                                texto1.text = falas;
                                gabarito.SetActive(true);
                                blocoConjuntoFuncinarios.SetActive(true);
                                blocoEngenheiro2.SetActive(false);
                                blocoFerreiro.SetActive(true);
                                // posicao.transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
                                break;
                        }
                    }
                }
            }
            if (hit.collider.gameObject.tag == "f")
            {
                painelInformacao3.SetActive(true);

                if (index3 > 2)
                {
                    index3 = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto3.text = falas;
                                break;
                            case 1:
                                falas = "Olá, trabalho na central de ferragens, vamos a esta etapa";
                                texto3.text = falas;
                                break;
                            case 2:
                                falas = "Vá até o local da obra para ver se está tudo correto. Em seguida vá até o encarregado da concretagem.";
                                texto3.text = falas;
                                ferragens.SetActive(true);
                                formas.SetActive(true);
                                blocoConcreto.SetActive(true);
                                // posicao.transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
                                break;
                        }
                    }
                }
            }
            if (hit.collider.gameObject.tag == "EquipeEscavacao")
            {
                painelInformacao2.SetActive(true);

                if (index2 > 2)
                {
                    index2 = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto2.text = falas;
                                break;
                            case 1:
                                falas = "Olá, somos a equipe de escavação. ";
                                texto2.text = falas;
                                break;
                            case 2:
                                falas = "Vá até o local da obra para ver a escavação. Em seguida vá a central de ferragens, falar o funcionário";
                                texto2.text = falas;
                                // escavacao.SetActive(true);
                                posicao.transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
                                //blocoEngenheiro.SetActive(true);
                                blocoEncarregado.SetActive(false);
                                blocoFerreiro.SetActive(true);
                                break;
                        }
                    }
                }




            }
            if (hit.collider.gameObject.tag == "concretagem")
            {
                painelInformacao4.SetActive(true);

                if (index4 > 2)
                {
                    index4 = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto4.text = falas;
                                blocoConjuntoFuncinarios.SetActive(false);
                                blocoEngenheiro.SetActive(false);
                                break;
                            case 1:
                                falas = "Olá, bem vindo a central de concretagem. ";
                                texto4.text = falas;
                                break;
                            case 2:
                                falas = "Estamos concluindo a parte da fundação.";
                                texto4.text = falas;
                                break;
                            case 3:
                                falas = "Vá até o local da obra para ver a concretagem, em seguinda volta ao engeheiro";
                                texto4.text = falas;
                                concretagem.SetActive(true);
                                blocoEngenheiro2.SetActive(true);
                                blocoFinal.SetActive(true);
                                break;

                        }
                    }
                }
            }
            if (hit.collider.gameObject.tag == "Final")
            {
                painelInformacao5.SetActive(true);

                if (index5 > 2)
                {
                    index5 = 2;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        index++;

                        switch (index)
                        {
                            case 0:
                                falas = "Aperte <F> para interagir";
                                texto5.text = falas;
                                blocoConjuntoFuncinarios.SetActive(false);
                                blocoEngenheiro.SetActive(false);
                                break;
                            case 1:
                                falas = "Parabéns, você foi um(a) excelente técnico em Edificações. Te aguardo para a próxima etapa que será a superestrutura.";
                                texto5.text = falas;
                                break;
                            

                        }
                    }
                }
            }
        }
        else
        {

            Debug.Log("Não Personagem");
            index = 0;

            painelInformacao.SetActive(false);
            painelInformacao1.SetActive(false);
            painelInformacao2.SetActive(false);
            painelInformacao3.SetActive(false);
            painelInformacao4.SetActive(false);

        }
    }
}

