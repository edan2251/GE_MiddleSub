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
        Debug.Log($"{stageIndex}로변경됨");
        RefreshRankList();
    }

    void RefreshRankList()
    {
        //기존의 모든 자식 오브젝트 삭제
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        //랭크 데이터 정렬
        var sortedData = allData.results.Where(r => r.stage == stageIndex).OrderByDescending(x => x.score).ToList();

        //랭크 데이터 생성
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}
