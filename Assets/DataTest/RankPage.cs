using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot; [SerializeField] GameObject rowPrefab;

    StageResultList allData;

    int stageIndex = 1;

    private void Awake()
    {
        allData = StageResultSaver.LoadRank();
        RefreshRankList();
    }

    public void ButtonAction(int index)
    {
        stageIndex = index;
        Debug.Log($"{stageIndex}�κ����");
        RefreshRankList();
    }

    void RefreshRankList()
    {
        //������ ��� �ڽ� ������Ʈ ����
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        //��ũ ������ ����
        var sortedData = allData.results.Where(r => r.stage == stageIndex).OrderByDescending(x => x.score).ToList();

        //��ũ ������ ����
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}
