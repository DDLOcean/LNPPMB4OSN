using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using OfficeOpenXml;


public class WriteUNsPosition : MonoBehaviour {

    // Use this for initialization
    //Dictionary<int, Vector2> sensor_node_position_dict = new Dictionary<int, Vector2>();
    private GameObject[] sensor_nodes;

    void Start () {
       
    }

    //public void MobileNodeReset()
    //{
    //    WriteData(file_no.ToString());
    //    num = 0;
    //    //transform.GetComponent<MobileAgent>().AgentReset();
    //    file_no++;
    //    positionDict.Clear();
    //}
    public void WriteData()
    {
        string file_name = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string path = Application.dataPath + "/Data/"+ file_name+ ".xlsx";
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

            worksheet.Cells[1, 1].Value = "No";
            worksheet.Cells[1, 2].Value = "Pos_x";
            worksheet.Cells[1, 3].Value = "Pos_y";

            sensor_nodes = GameObject.FindGameObjectsWithTag("sensor_node");
            for (int i = 0; i < sensor_nodes.Length; i++)
            {
                //添加一行数据
                worksheet.Cells["A" + (i + 2).ToString()].Value = (i + 1).ToString();
                worksheet.Cells["B" + (i + 2).ToString()].Value = sensor_nodes[i].transform.localPosition.x;
                worksheet.Cells["C" + (i + 2).ToString()].Value = sensor_nodes[i].transform.localPosition.z;
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
