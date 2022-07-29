using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator anim;       // Armazena o controlador da animação
    bool isWalkingFlag;         // Armazena o estado do parâmetro "isWalking"
    bool isRunningFlag;         // Armazena o estado do parâmetro "isRunning"

    PlayerControls input;

    Vector2 Movimento = new Vector2();  // Armazena os controles de direção
    bool movimentoPressionado;          // Armazena o estado de Mover
    bool runPressionado;          // Armazena o estado de Correr

    private void Awake()
    {
        input = new PlayerControls();

        input.Player.Move.performed += ctx =>
        {
            Movimento = ctx.ReadValue<Vector2>();
            movimentoPressionado = Movimento.x != 0 || Movimento.y != 0;
        };
        input.Player.Run.performed += ctx => runPressionado = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("IsWalking", isWalkingFlag);
        anim.SetBool("IsRunning", isRunningFlag);
    }

    void Mover()
    {
        if(movimentoPressionado && !isWalkingFlag)
        {
            isWalkingFlag = true;
            anim.SetBool("IsWalking", isWalkingFlag);
        }
        if (!movimentoPressionado && isWalkingFlag)
        {
            isWalkingFlag = false;
            anim.SetBool("IsWalking", isWalkingFlag);
        }

        if ((movimentoPressionado && runPressionado) && !isRunningFlag)
        {
            isRunningFlag = true;
            anim.SetBool("IsRunning", isRunningFlag);
        }
        if ((!movimentoPressionado || !isRunningFlag) && isRunningFlag)
        {
            isRunningFlag = false;
            anim.SetBool("IsRunning", isRunningFlag);
        }
    }

    void Rotacionar()
    {
        Vector3 atualPosition = transform.position;     // Armazena a posição atual
        Vector3 novaPosition = new Vector3(Movimento.x, 0f, Movimento.y);   // Mudança de posição
        Vector3 positionToLookAt = atualPosition + novaPosition;    // Atualização da posição
        transform.LookAt(positionToLookAt);
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Rotacionar();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
