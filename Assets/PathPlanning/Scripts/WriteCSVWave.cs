using OfficeOpenXml;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WriteCSVWave : MonoBehaviour
{
    List<float> wave_height = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WriteWaveHeightWaitForSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        wave_height.Add(transform.position.y);
    }

    IEnumerator WriteWaveHeightWaitForSeconds()
    {
        yield return new WaitForSeconds(500.0f);
        WriteData();
    }

    public void WriteData()
    {
        string file_name = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string path = Application.dataPath + "/Data/"+ file_name+ "wave_height.xlsx";
        FileInfo newFile = new FileInfo(path);
        if (newFile.Exists)
        {
            //创建一个新的excel文件
            newFile.Delete();
            newFile = new FileInfo(path);
        }
        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //在excel空文件添加新sheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            //添加列名

            worksheet.Cells[1, 1].Value = "Wave_Height";
            for (int i = 0; i < wave_height.Count; i++)
            {
                //添加一行数据
                worksheet.Cells["A" + (i + 2).ToString()].Value = wave_height[i];
            }
            //for (int i = 0; i < positionDict.Count; i++)
            //{
            //    //添加一行数据
            //    worksheet.Cells["A" + (i + 2).ToString()].Value = (i + 1).ToString();
            //    worksheet.Cells["B" + (i + 2).ToString()].Value = positionDict[i + 1].pos_x;
            //    worksheet.Cells["C" + (i + 2).ToString()].Value = positionDict[i + 1].pos_y;

            //}

            //保存excel
            package.Save();
        }
    }
}
