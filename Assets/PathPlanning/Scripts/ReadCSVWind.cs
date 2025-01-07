using Crest;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSVWind : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public GameObject wave;
    int index = 0;
    List<float> windSpeedList = new List<float>();
    void Start()
    {
        ReadData();
        StartCoroutine(SetWindWaitForSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetWindWaitForSeconds()
    {
        while (true)
        {
            // 每隔1秒执行一次 WriteData()
            if (index<windSpeedList.Count)
            {
                //transform.GetComponent<ShapeFFT>()._spectrum._multiplier = windSpeedList[index] / 5 + 1;
                transform.GetComponent<OceanRenderer>()._globalWindSpeed = windSpeedList[index] + Random.Range(-2, 2);
                index++;
            }
            // 等待1秒钟
            yield return new WaitForSeconds(1f);
        }
    }

    public void ReadData()
    {
        string path = Application.dataPath + "/Data/" + "wind_data.xlsx";
        FileInfo fileInfo = new FileInfo(path);

        if (!fileInfo.Exists)
        {
            Debug.LogError("文件不存在！");
            //return waveHeightList;
        }

        // 使用EPPlus打开Excel文件
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // 获取第一个sheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            for (int i = 2; i <= rowCount; i++) // 从第二行开始读取数据（跳过表头）
            {
                var waveHeightValue = worksheet.Cells[i, 5].Value;
                if (waveHeightValue != null && float.TryParse(waveHeightValue.ToString(), out float waveHeight))
                {
                    windSpeedList.Add(waveHeight);
                }
            }
        }

    }
}
