﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerYoungWoo : DummyPlayerParent
{
    protected override IEnumerator KnockBack(Direction direction, float ccTime, float ccPower)
    {
        if (isKnockBack)
            yield break;

        StartCoroutine(KnockBackDelay());

        rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        rb2D.AddForce((Vector2.right * (int)direction + Vector2.up).normalized * (ccPower), ForceMode2D.Impulse);
        float time = 0f;
        while (time < ccTime)
        {
            time += Time.deltaTime;

            yield return null;
        }
        yield return null;
        knockBackCoroutine = null;
    }

    protected override IEnumerator Stun(float ccTime)
    {
        // 분노모드 취소
        playerData.Rage -= 999;
        efcAnimator.gameObject.SetActive(true);

        float time = 0f;
        while (time < ccTime * 0.3f)
        {
            time += Time.deltaTime;

            yield return null;
        }
        efcAnimator.SetTrigger("TrgEnd");
        stunCoroutine = null;
    }
}
