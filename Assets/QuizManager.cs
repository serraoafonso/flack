using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class QuizManager : MonoBehaviour
{
    public GameObject factPanel; // Referência para o painel do fato
    public GameObject questionPanel; // Referência para o painel da pergunta
    public TextMeshProUGUI factText; // Texto do fato
    public TextMeshProUGUI questionText; // Texto da pergunta
    public Button[] answerButtons; // Botões de resposta
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

        "O DNA contém as instruções genéticas que regulam o desenvolvimento e funcionamento dos organismos. Ele é passado de geração em geração, garantindo a hereditariedade das características entre os seres vivos.",
        "Efeito de Estufa: Fenómeno em que gases na atmosfera da Terra, como o dióxido de carbono, retêm calor.",
        "Teoria do Big Bang: Sugere que o universo começou há cerca de 13,8 bilhões de anos a partir de uma explosão."
    };

    private string[] cienciaPerguntas = {
        "Isto implica...",
        "Portanto o aumento do efeito de estufa leva ao...",
        "O que é a Teoria do Big Bang?"
    };

    private string[] cienciaRespostasCertas = {
        " que teoricamente todos os organismos têm um ancestral comum.",
        " Aumento das temperaturas médias da Terra, causando mudanças climáticas.",
        " A sugestão de que o universo começou a partir de uma explosão."
    };

    private string[][] cienciaRespostasIncorretas = new string[][] {
        new string[] { " que cada espécie desenvolve seu próprio DNA sem relação com outras.", " que o DNA muda completamente em cada geração." },
        new string[] { " Resfriamento global, pois a camada de ozônio se torna mais espessa.", " Melhoria na qualidade do ar devido à redução das emissões de carbono." },
        new string[] { " A ideia de que o universo sempre existiu.", " Uma teoria sobre a formação de estrelas." }
    };

    private string[] financasFactos = {
        "A inflação é o aumento geral dos preços de bens e serviços em uma economia ao longo do tempo. Isso significa que, com a inflação, o dinheiro perde seu poder de compra.",
        "Segundo a lei da oferta quanto maior o preço, maior a oferta e menor a procura e, consequentemente, quanto menor o preço, menor a oferta e maior a procura. ",
        "Ter uma reserva de emergência é fundamental para evitar endividamento em situações inesperadas. A recomendação geral é que essa reserva cubra de 3 a 6 meses de despesas mensais básicas."
    };

    private string[] financasPerguntas = {
        "Qual dos seguintes fatores pode contribuir para o aumento da inflação?",
        "O João tem uma venda de sumo de laranja e pretende aumentar o preço do seu produto o que acontecerá à venda do João?",
        "Qual é o principal objetivo de uma reserva de emergência?"
    };

    private string[] financasRespostasCertas = {
        " Aumento nos preços do petróleo.",
        " A procura diminui.",
        " Fornecer um suporte financeiro durante períodos de imprevistos ou perda de renda."
    };

    private string[][] financasRespostasIncorretas = new string[][] {
        new string[] { " Redução dos salários.", " Aumento do desemprego." },
        new string[] { " A oferta diminui.", " A procura aumenta." },
        new string[] { " Possibilitar investimentos em produtos financeiros de alto rendimento.", " Proteger o patrimônio contra flutuações de mercado." }
    };

    private string[] historiaFactos = {
        "A Revolução Industrial, que começou no final do século XVIII na Inglaterra, marcou uma transição significativa de economias agrárias e artesanais para economias industriais e urbanas, alterando a maneira como as pessoas trabalhavam e viviam   .",
        "A Queda do Muro de Berlim, em 1989, foi um evento simbólico que marcou o fim da Guerra Fria. O muro, que dividia a Alemanha Oriental e Ocidental desde 1961, foi um símbolo da separação ideológica entre o bloco soviético e o ocidental.",
        "A Revolução dos Cravos, que ocorreu em 25 de abril de 1974, foi um movimento pacífico que resultou na queda da ditadura do Estado Novo em Portugal. Este evento é comemorado anualmente como o Dia da Liberdade e simboliza a transição do país para a democracia.."
    };

    private string[] historiaPerguntas = {
        "Qual das seguintes mudanças sociais foi uma consequência direta da Revolução Industrial?",
        "Qual foi uma das principais consequências da queda do Muro de Berlim?",
        "Qual foi o principal objetivo da Revolução dos Cravos em 1974?"
    };

    private string[] historiaRespostasCertas = {
        " Crescimento das cidades e urbanização.",
        " Unificação da Alemanha.",
        " Derrubar a ditadura do Estado Novo e restaurar a democracia"
    };

    private string[][] historiaRespostasIncorretas = new string[][] {
        new string[] { " Redução do comércio internacional.", " Aumento da população rural." },
        new string[] { " Aumento da imigração para a Alemanha Oriental.", " Redução do comércio entre a Alemanha e a França." },
        new string[] { " Aumentar a influência de Portugal nas colônias africanas.", " Expandir o território português na Europa." }
    };

    

    // Variáveis para rastrear o índice do tema e do fato atual
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
        // Configurar a primeira página
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
        currentThemeIndex = Random.Range(0, listaGrande.Count); // 0 = Ciência, 1 = Finanças, 2 = História
    }

    private void SetRandomFactIndex(int tema)
    {
        int tempIndex = Random.Range(0, listaGrande[tema].Count);

        currentFactIndex = listaGrande[tema][tempIndex];
    }

    private void DisplayRandomFact()
    {
        // Verifica se o tema é válido
        if (currentThemeIndex < 0) return;

        // Escolher um fato aleatório do tema selecionado
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
            case 0: return cienciaFactos; // Ciência
            case 1: return financasFactos; // Finanças
            case 2: return historiaFactos; // História
            default: return new string[0]; // Retornar um array vazio se o tema for inválido
        }
    }
    public void OnNextButtonClicked()
    {
        // Quando o botão "Seguinte" é clicado
        factPanel.SetActive(false);
        questionPanel.SetActive(true);

        // Mostrar uma pergunta aleatória do mesmo tema do fato atual
        questionText.text = GetCurrentQuestions()[currentFactIndex];
        SetupAnswers(); // Configurar resposta
    }
    private string[] GetCurrentQuestions()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaPerguntas; // Ciência
            case 1: return financasPerguntas; // Finanças
            case 2: return historiaPerguntas; // História
            default: return new string[0]; // Retornar um array vazio se o tema for inválido
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

        // Adicionar a resposta correta em uma posição aleatória
        int correctAnswerPosition = Random.Range(0, allAnswers.Count + 1); // Gera uma posição aleatória entre 0 e 2 (inclusive)
        allAnswers.Insert(correctAnswerPosition, correctAnswer);

        // Atualizar os botões com as respostas mantendo a ordem a, b, c
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Atualizar o texto dos botões com as respostas
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];

            // Remover listeners anteriores para evitar cliques repetidos
            answerButtons[i].onClick.RemoveAllListeners();

            // Adicionar o listener correto dependendo se a resposta é certa ou errada
            bool isCorrect = (i == correctAnswerPosition);
            answerButtons[i].onClick.AddListener(() => AnswerSelected(isCorrect));
        }
    }

    private string[] GetCurrentCorrectAnswers()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaRespostasCertas; // Ciência
            case 1: return financasRespostasCertas; // Finanças
            case 2: return historiaRespostasCertas; // História
            default: return new string[0]; // Retornar um array vazio se o tema for inválido
        }
    }

    private string[] GetCurrentIncorrectAnswers()
    {
        switch (currentThemeIndex)
        {
            case 0: return cienciaRespostasIncorretas[currentFactIndex]; // Ciência
            case 1: return financasRespostasIncorretas[currentFactIndex]; // Finanças
            case 2: return historiaRespostasIncorretas[currentFactIndex]; // História
            default: return new string[0]; // Retornar um array vazio se o tema for inválido
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
            DisableBookCollider(books[bookIndex]); // Passando a referência diretamente

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
                player.transform.position = new Vector3(87.2f, -682f, player.transform.position.z); // Alterar posição
                Debug.Log("Jogador reposicionado para a posição (87.2, -682).");
            }

            ResetQuiz(); // Chama ResetQuiz apenas quando a resposta estiver incorreta
        }
    }



    private void ResetQuiz()
    {
        // Limpa a configuração anterior
        factPanel.SetActive(true);
        questionPanel.SetActive(false);
        SetRandomTheme(); // Redefine o tema aleatório
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
                Debug.LogWarning($"Box Collider 2D não encontrado no livro {book.name}.");
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
            Debug.LogWarning($"Livro não encontrado.");
        }
    }
}

