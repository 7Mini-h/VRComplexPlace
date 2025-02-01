using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FoxRandomMove : MonoBehaviour
{
    public static FoxRandomMove Instance;

    public Transform[] movePoints;
    public Transform trnSausage;

    private int currentPoint;
    private Animator animator;

    private bool canWandering;
    private bool isWanderingRestart;
    private bool isWaitingFood;
    private bool isWaitingFoodStart;
    private float wanderingResetTimer;

    public bool isEating;
    private bool isEatingStart;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canWandering = true;
        isWanderingRestart = false;
    }

    private void Update()
    {
        // �Դ� ������ �ֿ켱
        if (!isEating)
        {
            // 0. ��Ÿ�� ���� ������ ��ġ�� ��ȸ
            if (canWandering && !isWanderingRestart && !isWaitingFood)
            {
                StartCoroutine(IeMoveRandomPosition());
            }

            // 1. �Ҽ����� �Ĵٺ�
            // SetStartFoxWaitngFood()�� ���� ����
            if (isWaitingFood)
            {
                if (isWaitingFoodStart)
                {
                    StopAllCoroutines();

                    if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Anim_Fox_Move"))
                    {
                        animator.SetBool("isMove", false);
                    }

                    isWaitingFoodStart = false;
                }

                transform.LookAt(trnSausage.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

            // 2. �Ҽ����� ������ų� �� �Ծ��� ��� �ٽ� ��Ÿ���� ��ȸ�ϱ������ ���ð��� ���� �ٽ� 0������ �̵�
            // SetStopFoxWaitngFood()�� ���� ����
            if (isWanderingRestart && !canWandering && !isWaitingFood)
            {
                wanderingResetTimer += Time.deltaTime;
                if (wanderingResetTimer > 3)
                {
                    isWanderingRestart = false;
                    canWandering = true;
                }
            }
        }
        else
        {
            if (isEatingStart)
            {
                StopAllCoroutines();
                StartCoroutine(IeEatFood());
            }
        }
    }

    public void SetStartFoxWaitngFood()
    {
        isWaitingFood = true;
        isWaitingFoodStart = true;
        canWandering = false;
    }

    public void SetStopFoxWaitngFood()
    {
        isWaitingFood = false;
        isWaitingFoodStart = false;
        isWanderingRestart = true;
        wanderingResetTimer = 0;
    }
    public void SetStartEatFood()
    {
        isEating = true;
        isEatingStart = true;
    }

    private IEnumerator IeMoveRandomPosition()
    {
        canWandering = false;
        yield return new WaitForSeconds(1.5f);

        int randomPosIndex;
        do
        {
            randomPosIndex = Random.Range(0, movePoints.Length);
        } while (currentPoint == randomPosIndex);
        currentPoint = randomPosIndex;

        Vector3 startPos = transform.localPosition;
        Vector3 startForward = transform.forward;

        Coroutine coRot = StartCoroutine(IeRotate(startForward, randomPosIndex));
        yield return new WaitForSeconds(1f / (8 * 3));
        Coroutine coMove = StartCoroutine(IeMove(startPos, randomPosIndex));

        yield return coRot;
        yield return coMove;

        float randomWait = Random.Range(0, 2);
        yield return new WaitForSeconds(randomWait);
        canWandering = true;
    }

    private IEnumerator IeRotate(Vector3 _startForward, int _posidx, float _rotSpeed = 8f)
    {
        //for (float i = 0; i < 1f; i += Time.deltaTime)
        //{
        //    transform.forward = Vector3.Slerp(_startForward, movePoints[_posidx].localPosition, i * _rotSpeed);
        //    yield return null;
        //}

        transform.LookAt(movePoints[_posidx].position);

        yield break;
    }
    private IEnumerator IeMove(Vector3 _startPosition, int _posidx, float _moveSpeed = 1f)
    {
        animator.SetBool("isMove", true);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(_startPosition, movePoints[_posidx].localPosition, i * _moveSpeed);
            yield return null;
        }
        transform.localPosition = movePoints[_posidx].localPosition;

        animator.SetBool("isMove", false);
        yield break;
    }

    private IEnumerator IeEatFood()
    {
        isEatingStart = false;

        isWaitingFood = false;
        isWaitingFoodStart = false;
        isWanderingRestart = false;
        canWandering = false;

        animator.SetBool("isMove", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Anim_Fox_Move"));

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = new Vector3(trnSausage.localPosition.x, transform.localPosition.y, trnSausage.localPosition.z) + ((trnSausage.position - transform.position).normalized * -1.3f);
        targetPos.y = transform.localPosition.y;

        transform.LookAt(trnSausage.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        for (float i = 0; i < (1 / 3f); i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, i * 3);
            yield return null;
        }
        transform.localPosition = targetPos;


        animator.SetBool("isMove", false);
        animator.SetTrigger("ToEat");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Anim_Fox_Eat"));
        // �ִϸ��̼ǿ� ���߾� �Ҽ����� �۾������� ����
        SausageScript sausage = trnSausage.GetComponent<SausageScript>();
        sausage.EatenByFox(1.2f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        animator.SetTrigger("ToIdle");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Anim_Fox_Idle"));

        // �Ҽ��� ����
        sausage.ResetSausage();

        isEating = false;
        SetStopFoxWaitngFood();
        yield break;
    }
}
