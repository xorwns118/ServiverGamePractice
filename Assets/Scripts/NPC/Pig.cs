using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeekAnimal
{
    protected override void Reset()
    {
        base.Reset();
        RandomAction();
    }

    // 다음 랜덤 행동 개시
    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4); // 대기 0, 풀뜯기 1, 두리번 2, 걷기 3 (Random.Range(0f,4f)일때는 4까지 포함)

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }

    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }

    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
}
