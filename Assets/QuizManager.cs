using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class QuizManager : MonoBehaviour
{
    public GameObject factPanel; // Refer�ncia para o painel do fato
    public GameObject questionPanel; // Refer�ncia para o painel da pergunta
    public TextMeshProUGUI factText; // Texto do fato
    public TextMeshProUGUI questionText; // Texto da pergunta
    public Button[] answerButtons; // Bot�es de resposta
    public AudioSource audioSource; // AudioSource para tocar os sons
    public AudioSource correctSound; // Som de acerto
    public AudioSource incorrectSound; // Som de erro
    public GameObject Canva;

    private List<List<int>> listaGrande = new List<List<int>>();
    

    public AudioSource fechadoSom;

    public CollisionFunction collisionFunction;
    [SerializeField]
    private GhostText ghostText;
    public PlayerMovement playerMovement;
    private string[][] temasFactos;
    private string[][] temasPerguntas;
    private string[][] temasRespostasCertas;
    public GameObject player;
    public GameObject[] books;
    private int bookIndex = 0;
    // Dados dos temas
    private string[] cienciaFactos = {

        "O DNA cont�m as instru��es gen�ticas que regulam o desenvolvimento e funcionamento dos organismos. Ele � passado de gera��o em gera��o, garantindo a hereditariedade das caracter�sticas entre os seres vivos.",
        "Efeito de Estufa: Fen�meno em que gases na atmosfera da Terra, como o di�xido de carbono, ret�m calor.",
        "Teoria do Big Bang: Sugere que o universo come�ou h� cerca de 13,8 bilh�es de anos a partir de uma explos�o."
    };

    private string[] cienciaPerguntas = {
        "Isto implica...",
        "Portanto o aumento do efeito de estufa leva ao...",
        "O que � a Teoria do Big Bang?"
    };

    private string[] cienciaRespostasCertas = {
        " que teoricamente todos os organismos t�m um ancestral comum.",
        " Aumento das temperaturas m�dias da Terra, causando mudan�as clim�ticas.",
        " A sugest�o de que o universo come�ou a partir de uma explos�o."
    };

    private string[][] cienciaRespostasIncorretas = new string[][] {
        new string[] { " que cada esp�cie desenvolve seu pr�prio DNA sem rela��o com outras.", " que o DNA muda completamente em cada gera��o." },
        new string[] { " Resfriamento global, pois a camada de oz�nio se torna mais espessa.", " Melhoria na qualidade do ar devido � redu��o das emiss�es de carbono." },
        new string[] { " A ideia de que o universo sempre existiu.", " Uma teoria sobre a forma��o de estrelas." }
    };

    private string[] financasFactos = {
        "A infla��o � o aumento geral dos pre�os de bens e servi�os em uma economia ao longo do tempo. Isso significa que, com a infla��o, o dinheiro perde seu poder de compra.",
        "Segundo a lei da oferta quanto maior o pre�o, maior a oferta e menor a procura e, consequentemente, quanto menor o pre�o, menor a oferta e maior a procura. ",
        "Ter uma reserva de emerg�ncia � fundamental para evitar endividamento em situa��es inesperadas. A recomenda��o geral � que essa reserva cubra de 3 a 6 meses de despesas mensais b�sicas."
    };

    private string[] financasPerguntas = {
        "Qual dos seguintes fatores pode contribuir para o aumento da infla��o?",
        "O Jo�o tem uma venda de sumo de laranja e pretende aumentar o pre�o do seu produto o que acontecer� � venda do Jo�o?",
        "Qual � o principal objetivo de uma reserva de emerg�ncia?"
    };

    private string[] financasRespostasCertas = {
        " Aumento nos pre�os do petr�leo.",
        " A procura diminui.",
        " Fornecer um suporte financeiro durante per�odos de imprevistos ou perda de renda."
    };

    private string[][] financasRespostasIncorretas = new string[][] {
        new string[] { " Redu��o dos sal�rios.", " Aumento do desemprego." },
        new string[] { " A oferta diminui.", " A procura aumenta." },
        new string[] { " Possibilitar investimentos em produtos financeiros de alto rendimento.", " Proteger o patrim�nio contra flutua��es de mercado." }
    };

    private string[] historiaFactos = {
        "A Revolu��o Industrial, que come�ou no final do s�culo XVIII na Inglaterra, marcou uma transi��o significativa de economias agr�rias e artesanais para economias industriais e urbanas, alterando a maneira como as pessoas trabalhavam e viviam   .",
        "A Queda do Muro de Berlim, em 1989, foi um evento simb�lico que marcou o fim da Guerra Fria. O muro, que dividia a Alemanha Oriental e Ocidental desde 1961, foi um s�mbolo da separa��o ideol�gica entre o bloco sovi�tico e o ocidental.",
        "A Revolu��o dos Cravos, que ocorreu em 25 de abril de 1974, foi um movimento pac�fico que resultou na queda da ditadura do Estado Novo em Portugal. Este evento � comemorado anualmente como o Dia da Liberdade e simboliza a transi��o do pa�s para a democracia.."
    };

    private string[] historiaPerguntas = {
        "Qual das seguintes mudan�as sociais foi uma consequ�ncia direta da Revolu��o Industrial?",
        "Qual foi uma das principais consequ�ncias da queda do Muro de Berlim?",
        "Qual foi o principal objetivo da Revolu��o dos Cravos em 1974?"
    };

    private string[] historiaRespostasCertas = {
        " Crescimento das cidades e urbaniza��o.",
        " Unifica��o da Alemanha.",
        " Derrubar a ditadura do Estado Novo e restaurar a democracia"
    };

    private string[][] historiaRespostasIncorretas = new string[][] {
        new string[] { " Redu��o do com�rcio internacional.", " Aumento da popula��o rural." },
        new string[] { " Aumento da imigra��o para a Alemanha Oriental.", " Redu��o do com�rcio entre a Alemanha e a Fran�a." },
        new string[] { " Aumentar a influ�ncia de Portugal nas col�nias africanas.", " Expandir o territ�rio portugu�s na Europa." }
    };

    

    // Vari�veis para rastrear o �ndice do tema e do fato atual
    private int currentThemeIndex;
    private int currentFactIndex;
    private void Start()
    {
        temasFactos = new string[][] { cienciaFactos, financasFactos, historiaFactos };
        temasPerguntas = new string[][] { cienciaPerguntas, financasPerguntas, historiaPerguntas };
        temasRespostasCertas = new string[][] { cienciaRespostasCertas, financasRespostasCertas, historiaRespostasCertas };

        // Inicializando os dados dos temas


        if (collisionFunction != null)
        {
            // Access the bookID and tema
            string bookId = collisionFunction.bookID;
            string tema = collisionFunction.tema;

            // Output the values to the console (for testing)
            Debug.Log("Book ID: " + bookId);
            Debug.Log("Tema: " + tema);
        }
        else
        {
            Debug.LogWarning("CollisionFunction reference is not set!");
        }
        // Configurar a primeira p�gina
        PopulateLists();
        SetRandomTheme();
        DisplayRandomFact();
    }

    private void PopulateLists()
    {
        List<int> tempList = new List<int>()
        {
            0, 1, 2
        };

        for (int i = 0; i < 3; i++)
        {
            listaGrande.Add(tempList.ToList());
        }

    }

    private void ResetList(int index)
    {
        listaGrande[index] = new List<int> { 0, 1, 2 };
    }
    private void RemoveFromList(int tema, int pergunta)
    {
        listaGrande[tema].Remove(pergunta);

        if (listaGrande[tema].Count <= 0)
        {
            ResetList(tema);
        }
    }

    private void SetRandomTheme()
    {
        currentThemeIndex = Random.Range(0, listaGrande.Count); // 0 = Ci�ncia, 1 = Finan�as, 2 = Hist�ria
    }

    private void SetRandomFactIndex(int tema)
    {
        int tempIndex = Random.Range(0, listaGrande[tema].Count);

        currentFactIndex = listaGrande[tema][tempIndex];
    }

    private void DisplayRandomFact()
    {
        // Verifica se o tema � v�lido
        if (currentThemeIndex < 0) return;

        // Escolher um fato aleat�rio do tema selecionado
        SetRandomFactIndex(currentThemeIndex);
        factText.text = GetCurrentFacts()[currentFactIndex]; // Atualizar o texto do fato

        // Configurar o painel
        factPanel.SetActive(true);
        questionPanel.SetActive(false);

        RemoveFromList(currentThemeIndex, currentFactIndex);
    }
    private string[] GetCurrentFacts()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaFactos; // Ci�ncia
            case 1: return financasFactos; // Finan�as
            case 2: return historiaFactos; // Hist�ria
            default: return new string[0]; // Retornar um array vazio se o tema for inv�lido
        }
    }
    public void OnNextButtonClicked()
    {
        // Quando o bot�o "Seguinte" � clicado
        factPanel.SetActive(false);
        questionPanel.SetActive(true);

        // Mostrar uma pergunta aleat�ria do mesmo tema do fato atual
        questionText.text = GetCurrentQuestions()[currentFactIndex];
        SetupAnswers(); // Configurar resposta
    }
    private string[] GetCurrentQuestions()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaPerguntas; // Ci�ncia
            case 1: return financasPerguntas; // Finan�as
            case 2: return historiaPerguntas; // Hist�ria
            default: return new string[0]; // Retornar um array vazio se o tema for inv�lido
        }
    }
    private void SetupAnswers()
    {
        // Obter a resposta correta
        string correctAnswer = GetCurrentCorrectAnswers()[currentFactIndex];

        // Obter as respostas incorretas
        string[] incorrectAnswers = GetCurrentIncorrectAnswers();

        // Criar uma lista para todas as respostas
        List<string> allAnswers = new List<string>(incorrectAnswers);

        // Adicionar a resposta correta em uma posi��o aleat�ria
        int correctAnswerPosition = Random.Range(0, allAnswers.Count + 1); // Gera uma posi��o aleat�ria entre 0 e 2 (inclusive)
        allAnswers.Insert(correctAnswerPosition, correctAnswer);

        // Atualizar os bot�es com as respostas mantendo a ordem a, b, c
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Atualizar o texto dos bot�es com as respostas
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];

            // Remover listeners anteriores para evitar cliques repetidos
            answerButtons[i].onClick.RemoveAllListeners();

            // Adicionar o listener correto dependendo se a resposta � certa ou errada
            bool isCorrect = (i == correctAnswerPosition);
            answerButtons[i].onClick.AddListener(() => AnswerSelected(isCorrect));
        }
    }

    private string[] GetCurrentCorrectAnswers()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaRespostasCertas; // Ci�ncia
            case 1: return financasRespostasCertas; // Finan�as
            case 2: return historiaRespostasCertas; // Hist�ria
            default: return new string[0]; // Retornar um array vazio se o tema for inv�lido
        }
    }

    private string[] GetCurrentIncorrectAnswers()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaRespostasIncorretas[currentFactIndex]; // Ci�ncia
            case 1: return financasRespostasIncorretas[currentFactIndex]; // Finan�as
            case 2: return historiaRespostasIncorretas[currentFactIndex]; // Hist�ria
            default: return new string[0]; // Retornar um array vazio se o tema for inv�lido
        }
    }

    private void AnswerSelected(bool isCorrect)
    {
        if (bookIndex >= books.Length)
        {
            return;
        }

        if (isCorrect)
        {
            correctSound.Play(); // Tocar som de acerto
            Debug.Log("Resposta correta!");

            // Desativa o BoxCollider2D do objeto com bookID de book1
            DisableBookCollider(books[bookIndex]); // Passando a refer�ncia diretamente

            ++bookIndex;

            correctSound.Play();

            fechadoSom.Play();

            Canva.SetActive(false);
            playerMovement.collidedStop = false;

            // Exibe um novo fato ou pergunta
            DisplayRandomFact();
            ResetQuiz();
        }
        else
        {
            ghostText.HandleMiss();

            fechadoSom.Play();

            incorrectSound.Play(); // Tocar som de erro
            Debug.Log("Resposta incorreta!");

            playerMovement.collidedStop = false;




            Canva.SetActive(false);

            if (player != null)
            {
                player.transform.position = new Vector3(87.2f, -682f, player.transform.position.z); // Alterar posi��o
                Debug.Log("Jogador reposicionado para a posi��o (87.2, -682).");
            }

            ResetQuiz(); // Chama ResetQuiz apenas quando a resposta estiver incorreta
        }
    }



    private void ResetQuiz()
    {
        // Limpa a configura��o anterior
        factPanel.SetActive(true);
        questionPanel.SetActive(false);
        SetRandomTheme(); // Redefine o tema aleat�rio
        DisplayRandomFact(); // Exibe um novo fato
    }

    private void DisableBookCollider(GameObject book)
    {
        if (book != null)
        {
            BoxCollider2D bookCollider = book.GetComponent<BoxCollider2D>();

            Rigidbody2D bookRigidbody = book.GetComponent<Rigidbody2D>();

            if (bookCollider != null)
            {
                bookCollider.enabled = false; // Desativa o Box Collider 2D
                Debug.Log($"Box Collider 2D do livro {book.name} desativado.");
            }
            else
            {
                Debug.LogWarning($"Box Collider 2D n�o encontrado no livro {book.name}.");
            }
            if (bookRigidbody != null)
            {
                bookRigidbody.bodyType = RigidbodyType2D.Dynamic; // Set body type to Dynamic
                bookRigidbody.gravityScale = 10;
                Debug.Log($"Rigidbody2D of the book {book.name} set to Dynamic.");
            }
            else
            {
                Debug.LogWarning($"Rigidbody2D not found on the book {book.name}.");
            }
            Destroy(book, 5);
        }
        else
        {
            Debug.LogWarning($"Livro n�o encontrado.");
        }
    }
}

