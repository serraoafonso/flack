using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class FinalTest : MonoBehaviour
{
    public GameObject factPanel; // Referência para o painel do fato
    public GameObject questionPanel; // Referência para o painel da pergunta
    public TextMeshProUGUI factText; // Texto do fato
    public TextMeshProUGUI questionText; // Texto da pergunta

    public TextMeshProUGUI resultText; // Texto do resultado (Certa/Errada)

    public Button[] answerButtons; // Botões de resposta
    public AudioSource audioSource; // AudioSource para tocar os sons
    public AudioClip correctSound; // Som de acerto
    public AudioClip incorrectSound; // Som de erro

    public GameObject CanvaFT;

    public CollisionFunction collisionFunction;
    [SerializeField]
    private GhostText ghostText;


    public PlayerMovement playerMovement;

    public GameObject canvasResult; 
    public GameObject player;

    private List<int> perguntasFeitas;


    private int bookIndex = 0;
    private string[][] temasFactos;
    private string[][] temasPerguntas;
    private string[][] temasRespostasCertas;


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


    private int currentThemeIndex;
    private int currentFactIndex;

    
    private int correctAnswersInARow = 0;
    private int correctAnswersCount;



    private void Start()
    {
        // Configurar a primeira página
        SetRandomTheme();
        DisplayRandomFact();

        perguntasFeitas = new List<int>();
    }


    private void SetRandomTheme()
    {
        currentThemeIndex = Random.Range(0, 3); // 0 = Ciência, 1 = Finanças, 2 = História
    }


    private void DisplayRandomFact()
        
    {

        // Verifica se o tema é válido
        if (currentThemeIndex < 0) return;


        // Escolher um fato aleatório do tema selecionado
        currentFactIndex = Random.Range(0, 3/*GetCurrentFacts().Length*/);
        factText.text = GetCurrentFacts()[currentFactIndex]; // Atualizar o texto do fato


        // Configurar o painel
        factPanel.SetActive(true);
        questionPanel.SetActive(false);

        if (resultText != null)
        {
            resultText.gameObject.SetActive(false); // Esconder texto de resultado

        }
    }

    private int GetUniqueRandomFactIndex()
    {
        // Verifica se todas as perguntas já foram feitas
        if (perguntasFeitas.Count >= GetCurrentFacts().Length)
        {
            perguntasFeitas.Clear(); // Reseta a lista se todas as perguntas já foram feitas
        }

        int randomIndex;
        // Continua gerando índices aleatórios até encontrar um que não foi utilizado
        do
        {
            randomIndex = Random.Range(0, GetCurrentFacts().Length);
        } while (perguntasFeitas.Contains(randomIndex));

        // Adiciona o índice à lista de perguntas feitas
        perguntasFeitas.Add(randomIndex);

        return randomIndex;
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
        // Exibir resultado
        resultText.gameObject.SetActive(true); // Certifique-se de que o texto de resultado está ativo primeiro


        if (isCorrect)
        {
            
            //audioSource.PlayOneShot(correctSound); // Tocar som de acerto



            Debug.Log("Resposta correta!");

            //CanvaFT.SetActive(false);

           



            correctAnswersCount++; // Incrementar o contador de respostas corretas
            


            
            Debug.Log("Resposta correta!");
            correctAnswersInARow++; // Incrementar o contador de respostas corretas


            if (correctAnswersInARow >= 3)
            {

                playerMovement.colidirEscada = false;
                playerMovement.collidedStop = false;
                Debug.Log("Completou o nível! Três respostas corretas seguidas.");

                CanvaFT.SetActive(false);

                resultText.text = "Parabéns! Completou a estante 1!!!";

                canvasResult.SetActive(true);

                correctAnswersInARow = 0; // Resetar o contador
            }
            else
            {
                // Exibe a próxima pergunta de um tema diferente
                SetRandomTheme(); // Escolher um novo tema aleatório
                DisplayRandomFact(); // Exibe um fato e a pergunta do novo tema
            }
            
            
            
        }
        else
        {
            playerMovement.collidedStop = false;

            //ghostText.HandleMiss(); // Lógica de erro do GhostText
            //audioSource.PlayOneShot(incorrectSound); // Tocar som de erro
            
            Debug.Log("Resposta incorreta!");

            CanvaFT.SetActive(false);


            if (player != null)
            {
                player.transform.position = new Vector3(355f, -682f, 0); // Reposicionar jogador
                Debug.Log("Jogador reposicionado para a posição (355, -682, 0).");
            }


            //ResetQuiz(); // Reiniciar o quiz ao errar
            Debug.Log("Resposta incorreta!");
            correctAnswersInARow = 0; // Resetar o contador ao errar
            //ResetQuiz(); // Reinicia o quiz se o jogador errar


        }
    }


    private IEnumerator WaitAndDisplayNewFact(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Esperar antes de prosseguir
        DisplayRandomFact(); // Exibir um novo fato
    }




    private void ResetQuiz()
    {
        // Limpa a configuração anterior
        correctAnswersCount = 0; // Reiniciar contador de respostas corretas
        CanvaFT.SetActive(true);


        SetRandomTheme(); // Redefine o tema aleatório
        DisplayRandomFact(); // Exibe um novo fato
    }


    private void Colisao()
    {
        // Verifica se a colisão com a escada ocorreu
        if (playerMovement.colidirEscada)
        {
            // Exibir a pergunta e respostas imediatamente
            CanvaFT.SetActive(true);
            Debug.Log("Colidiu com a escada.");
            questionText.text = GetCurrentQuestions()[currentFactIndex]; // A pergunta é baseada no fato atual
            SetupAnswers(); // Configura as respostas
        }
    }
}



