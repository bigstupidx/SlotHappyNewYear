using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BingoManager : MonoBehaviour {

    public UILabel[] labels_winscore;

    public SpriteCollection bingoRects;

    string[] Cards;

    Dictionary<int, int[]> table_LineIDFindGrids;

    void Start()
    {
        table_LineIDFindGrids = new Dictionary<int, int[]>();

        table_LineIDFindGrids.Add(1, new int[] { 2, 5, 8, 11, 14 });
        table_LineIDFindGrids.Add(2, new int[] { 1,4,7,10,13 });
        table_LineIDFindGrids.Add(3, new int[] { 3,6,9,12,15 });
        table_LineIDFindGrids.Add(4, new int[] { 1,5,9,11,13 });
        table_LineIDFindGrids.Add(5, new int[] { 3,5,7,11,15 });
        table_LineIDFindGrids.Add(6, new int[] { 2,4,8,10, 14 });
        table_LineIDFindGrids.Add(7, new int[] { 2,6,8,12, 14 });
        table_LineIDFindGrids.Add(8, new int[] { 1,5,7,11, 13 });
        table_LineIDFindGrids.Add(9, new int[] { 3,5,9,11, 15 });
        table_LineIDFindGrids.Add(10, new int[] { 2,4,7,10, 14 });
        table_LineIDFindGrids.Add(11, new int[] { 2,6,9,12, 14 });
        table_LineIDFindGrids.Add(12, new int[] { 3,6,8,12, 15 });
        table_LineIDFindGrids.Add(13, new int[] { 1,4,8,10, 13 });
        table_LineIDFindGrids.Add(14, new int[] { 3,5,8,11, 15 });
        table_LineIDFindGrids.Add(15, new int[] { 1,5,8,11, 14 });
        table_LineIDFindGrids.Add(16, new int[] { 1,6,7,12, 13 });
        table_LineIDFindGrids.Add(17, new int[] { 3,4,9,10, 15 });
        table_LineIDFindGrids.Add(18, new int[] { 2,5,7,11, 14 });
        table_LineIDFindGrids.Add(19, new int[] { 2,5,9,11, 14 });
        table_LineIDFindGrids.Add(20, new int[] { 3,6,7,12, 15 });
        table_LineIDFindGrids.Add(21, new int[] { 1, 4, 9, 10, 13 });
        table_LineIDFindGrids.Add(22, new int[] { 1, 4, 8, 12, 15 });
        table_LineIDFindGrids.Add(23, new int[] { 3, 6, 8, 10, 13 });
        table_LineIDFindGrids.Add(24, new int[] { 2,4,9,10, 14 });
        table_LineIDFindGrids.Add(25, new int[] { 2,6,7,12,14 });
        table_LineIDFindGrids.Add(26, new int[] { 2,4,8,10,13 });
        table_LineIDFindGrids.Add(27, new int[] { 2,4,8,12,15 });
        table_LineIDFindGrids.Add(28, new int[] { 1,5,9,12,15 });
        table_LineIDFindGrids.Add(29, new int[] { 3,5,7,10,13 });
        table_LineIDFindGrids.Add(30, new int[] { 1,4,7,11,15 });
        table_LineIDFindGrids.Add(31, new int[] { 3,6,9,11,13 });
        table_LineIDFindGrids.Add(32, new int[] { 2,4,8,12,14 });
        table_LineIDFindGrids.Add(33, new int[] { 2,6,8,10,14 });
        table_LineIDFindGrids.Add(34, new int[] { 1,5,8,11,14 });
        table_LineIDFindGrids.Add(35, new int[] { 3,5,8,11,14 });
        table_LineIDFindGrids.Add(36, new int[] { 1,4,8,11,14 });
        table_LineIDFindGrids.Add(37, new int[] { 3,6,8,11,14 });
        table_LineIDFindGrids.Add(38, new int[] { 3,5,9,11,13 });
        table_LineIDFindGrids.Add(39, new int[] { 1,5,7,11,15 });
        table_LineIDFindGrids.Add(40, new int[] { 2,4,7,10,13 });
        table_LineIDFindGrids.Add(41, new int[] { 2,6,9,12,15 });
        table_LineIDFindGrids.Add(42, new int[] { 1,4,7,11,13 });
        table_LineIDFindGrids.Add(43, new int[] { 3,6,9,11,15 });
        table_LineIDFindGrids.Add(44, new int[] { 1,5,7,10,13 });
        table_LineIDFindGrids.Add(45, new int[] { 3,5,9,12,15 });
        table_LineIDFindGrids.Add(46, new int[] { 2,4,8,11,14 });
        table_LineIDFindGrids.Add(47, new int[] { 2,6,8,11,14 });
        table_LineIDFindGrids.Add(48, new int[] { 1,4,7,12,15 });
        table_LineIDFindGrids.Add(49, new int[] { 3,6,9,12,13 });
        table_LineIDFindGrids.Add(50, new int[] { 2,5,8,10,14 });

        labels_winscore[0].enabled = false;
        labels_winscore[1].enabled = false;
        labels_winscore[2].enabled = false;
    }

    public void OpenBingoLine(int id_line)
    {
        bingoRects.OpenSprites(table_LineIDFindGrids[id_line]);
    }
    public void CloseAllBingoLine()
    {
        bingoRects.CloseAllSprtie();
    }
    public void ShowPayoff(int lineid,string payoff)
    {
        int idx = (table_LineIDFindGrids[lineid])[0];

        labels_winscore[idx - 1].text = payoff;
        labels_winscore[idx - 1].enabled = true;
    }

    public void ClosePayoff()
    {
        labels_winscore[0].enabled = false;
        labels_winscore[1].enabled = false;
        labels_winscore[2].enabled = false;
    }
}
