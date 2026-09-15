using UnityEngine;
using UnityEngine.SceneManagement;

public enum TipoArmadilha
{
    Espeto,
    Torreta
}

public enum EstadoEspeto
{
    SemEspinhos,
    Ativando,
    EspinhosParados,
    Desativando
}

public class EspetoETorreta : MonoBehaviour
{
    [Header("Configuração Geral")]
    [SerializeField] private TipoArmadilha tipo = TipoArmadilha.Espeto;


    // =================================================
    // CONFIGURAÇÕES DO ESPETO
    // =================================================

    [Header("⚙️ Configurações do Espeto")]
    [SerializeField] private float tempoAtivado = 4f;
    [SerializeField] private float tempoDesativado = 3f;

    private EstadoEspeto estadoEspeto;


    // =================================================
    // CONFIGURAÇÕES DA TORRETA
    // =================================================

    [Header("🔫 Configurações da Torreta")]
    [SerializeField] private float tempoEntreDisparos = 2f;
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private Transform pontoDisparo;


    // =================================================
    // VARIÁVEIS GERAIS
    // =================================================

    private float cronometro;
    private Animator anim;


    // =================================================
    // START
    // =================================================

    private void Start()
    {
        anim = GetComponent<Animator>();

        cronometro = 0f;

        // Inicializa o espeto
        if (tipo == TipoArmadilha.Espeto)
        {
            estadoEspeto = EstadoEspeto.SemEspinhos;

            anim.Play("SemEspinhos");
        }

        // Inicializa a torreta
        if (tipo == TipoArmadilha.Torreta)
        {
            anim.Play("torretinhaparada");
        }
    }


    // =================================================
    // UPDATE
    // =================================================

    private void Update()
    {
        switch (tipo)
        {
            case TipoArmadilha.Espeto:

                ControlarEspeto();

                break;


            case TipoArmadilha.Torreta:

                ControlarTorreta();

                break;
        }
    }


    // =================================================
    // CONTROLE DO ESPETO
    // =================================================

    private void ControlarEspeto()
    {
        cronometro += Time.deltaTime;

        switch (estadoEspeto)
        {
            // =================================================
            // SEM ESPINHOS
            // =================================================

            case EstadoEspeto.SemEspinhos:

                if (cronometro >= tempoDesativado)
                {
                    cronometro = 0f;

                    estadoEspeto = EstadoEspeto.Ativando;

                    anim.Play("EspinhosAtivandoDesativando");
                }

                break;


            // =================================================
            // ATIVANDO
            // =================================================

            case EstadoEspeto.Ativando:

                if (AnimacaoTerminou("EspinhosAtivandoDesativando"))
                {
                    cronometro = 0f;

                    estadoEspeto = EstadoEspeto.EspinhosParados;

                    anim.Play("EspinhosParadinhos");
                }

                break;


            // =================================================
            // ESPINHOS PARADOS
            // =================================================

            case EstadoEspeto.EspinhosParados:

                if (cronometro >= tempoAtivado)
                {
                    cronometro = 0f;

                    estadoEspeto = EstadoEspeto.Desativando;

                    anim.Play("EspinhosDesativandoAtivando");
                }

                break;


            // =================================================
            // DESATIVANDO
            // =================================================

            case EstadoEspeto.Desativando:

                if (AnimacaoTerminou("EspinhosDesativandoAtivando"))
                {
                    cronometro = 0f;

                    estadoEspeto = EstadoEspeto.SemEspinhos;

                    anim.Play("SemEspinhos");
                }

                break;
        }
    }


    // =================================================
    // CONTROLE DA TORRETA
    // =================================================

    private void ControlarTorreta()
    {
        cronometro += Time.deltaTime;

        // Verifica se chegou a hora de disparar
        if (cronometro >= tempoEntreDisparos)
        {
            // Reinicia o cronômetro
            cronometro = 0f;

            // Toca a animação de disparo
            anim.Play("torretinha");

            // Cria o projétil
            DispararProjetil();
        }
    }


    // =================================================
    // DISPARAR PROJÉTIL
    // =================================================

    public void DispararProjetil()
    {
        Debug.Log("🔫 DISPAROU!");

        // Verifica se o prefab foi configurado
        if (projetilPrefab == null)
        {
            Debug.LogError(
                "❌ Projetil Prefab não foi configurado na torreta!"
            );

            return;
        }

        // Verifica se o ponto de disparo foi configurado
        if (pontoDisparo == null)
        {
            Debug.LogError(
                "❌ Ponto Disparo não foi configurado na torreta!"
            );

            return;
        }

        // Cria o projétil
        GameObject novoProjetil = Instantiate(
            projetilPrefab,
            pontoDisparo.position,
            pontoDisparo.rotation
        );

        Debug.Log(
            "💥 Projétil criado: " + novoProjetil.name
        );
    }


    // =================================================
    // VERIFICAR SE A ANIMAÇÃO TERMINOU
    // =================================================

    private bool AnimacaoTerminou(string nomeAnimacao)
    {
        AnimatorStateInfo estadoAtual =
            anim.GetCurrentAnimatorStateInfo(0);

        if (estadoAtual.IsName(nomeAnimacao))
        {
            return estadoAtual.normalizedTime >= 1f;
        }

        return false;
    }


    // =================================================
    // COLISÃO DO ESPETO COM O PLAYER
    // =================================================

    private void OnTriggerStay2D(Collider2D other)
    {
        // Só funciona se for o Player
        if (other.CompareTag("Player"))
        {
            switch (estadoEspeto)
            {
                case EstadoEspeto.SemEspinhos:

                    break;


                case EstadoEspeto.Ativando:

                    break;


                case EstadoEspeto.EspinhosParados:

                    MatarPlayer();

                    break;


                case EstadoEspeto.Desativando:

                    break;
            }
        }
    }


    // =================================================
    // REINICIAR CENA
    // =================================================

    private void MatarPlayer()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}