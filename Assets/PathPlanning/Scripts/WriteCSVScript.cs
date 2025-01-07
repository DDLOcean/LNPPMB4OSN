using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using OfficeOpenXml;


public class WriteCSVScript : MonoBehaviour {

    // Use this for initialization
    Dictionary<int, Position> positionDict = new Dictionary<int, Position>();
    int num = 0;
    int file_no = 0;
    class Position
    {
        public float pos_x;
        public float pos_y;
        public float vel_x;
        public float vel_y;
        public float rot;
        public float vel_rot;
        public float height;
        public float delta_time;
    }
    void Start () {
       
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        num++;
        Position pos = new Position();
        pos.pos_x = transform.position.x;
        pos.pos_y = transform.position.z;
        pos.height = transform.position.y;
        pos.vel_x = transform.GetComponent<Rigidbody>().linearVelocity.x;
        pos.vel_y = transform.GetComponent<Rigidbody>().linearVelocity.z;
        pos.rot = transform.localEulerAngles.y;
        pos.vel_rot = transform.GetComponent<Rigidbody>().angularVelocity.y;
        pos.delta_time = Time.deltaTime;
        positionDict.Add(num, pos);

    }
    public void MobileNodeReset()
    {
        WriteData(file_no.ToString());
        num = 0;
        //transform.GetComponent<MobileAgent>().AgentReset();
        file_no++;
        positionDict.Clear();
    }
    public void WriteData(string file_name)
    {
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

            worksheet.Cells[1, 1].Value = "Step";
            worksheet.Cells[1, 2].Value = "Pos_x";
            worksheet.Cells[1, 3].Value = "Pos_y";
            worksheet.Cells[1, 4].Value = "Vel_x";
            worksheet.Cells[1, 5].Value = "Vel_y";
            worksheet.Cells[1, 6].Value = "Height";
            worksheet.Cells[1, 7].Value = "rot";
            worksheet.Cells[1, 8].Value = "vel_rot";
            worksheet.Cells[1, 9].Value = "delta_time";

            for (int i = 0; i < positionDict.Count; i++)
            {
                //添加一行数据
                worksheet.Cells["A" + (i + 2).ToString()].Value = (i + 1).ToString();
                worksheet.Cells["B" + (i + 2).ToString()].Value = positionDict[i + 1].pos_x;
                worksheet.Cells["C" + (i + 2).ToString()].Value = positionDict[i + 1].pos_y;
                worksheet.Cells["D" + (i + 2).ToString()].Value = positionDict[i + 1].vel_x;
                worksheet.Cells["E" + (i + 2).ToString()].Value = positionDict[i + 1].vel_y;
                worksheet.Cells["F" + (i + 2).ToString()].Value = positionDict[i + 1].height;
                worksheet.Cells["G" + (i + 2).ToString()].Value = positionDict[i + 1].rot;
                worksheet.Cells["H" + (i + 2).ToString()].Value = positionDict[i + 1].vel_rot;
                worksheet.Cells["I" + (i + 2).ToString()].Value = positionDict[i + 1].delta_time;
            }
            
            //保存excel
            package.Save();
        }
    }
}
