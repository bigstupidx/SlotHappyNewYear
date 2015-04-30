using UnityEngine;
using System;
using System.Collections;

public class SpriteCollection : MonoBehaviour
{

    public SpriteObject[] spriteObjects;

    private bool sw_Ani;

    
    public void OpenSprite(int id_bingoline)
    {
        SetSpriteOpen(id_bingoline);
    }

    public void CloseSprite(int id_bingolines)
    {
        SetSpriteClose(id_bingolines);
    }

    public void OpenSprites(int[] ids_bingolines)
    {
        foreach (int integer in ids_bingolines)
            SetSpriteOpen(integer);
    }
    
    public void StartPlayAnimation(int[] spriteids)
    {
        sw_Ani = true;
        StartCoroutine(DoAnimation(spriteids));
    }

    public void OpenSpriteFromZeroToIdx(int idx)
    {

        try
        {
            for (int i = 1; i <= spriteObjects.Length; i++)
            {
                if (i <= idx)
                    SetSpriteOpen(i);
                else
                    SetSpriteClose(i);
            }
        }
        catch(Exception ex)
        {
            print(ex);
        }
    }

    public void CloseAllSprtie()
    {
        foreach (SpriteObject bingo in spriteObjects)
            bingo.Close();
    }

    // 開啟某一條線
    private void SetSpriteOpen(int idx)
    {
        spriteObjects[idx - 1].Open();
    }

    // 關閉某一條線
    private void SetSpriteClose(int idx)
    {
        spriteObjects[idx - 1].Close();
    }
    
    IEnumerator DoAnimation(int[] lineids)
    {
        int nowidx = 0;
        do
        {
            SetSpriteOpen(lineids[nowidx]);

            yield return new WaitForSeconds(1.0f);
            SetSpriteClose(lineids[nowidx]);


            nowidx++;
            if (nowidx >= lineids.Length)
                nowidx = 0;

        } while (sw_Ani);

        //this.CloseBingoLine(lineids);
    }

    public void StopDoAnimation()
    {
        // Stop DoAnimation Coroutine
        sw_Ani = false;

        this.CloseAllSprtie();
    }    

}
