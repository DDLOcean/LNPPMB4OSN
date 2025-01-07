using Crest;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WindCurrentForce : MonoBehaviour
{
    Rigidbody agentRB;
    //NavAcademy academy;

    float densityAir = 1.293f;
    float dragAir = 0.065f;
    float areaAir = 1;
    public Vector2 vWind = new Vector2(3f, 2f);

    float densityWater = 1025f;
    float dragWater = 0.065f;
    float areaWater = 1;
    public Vector2 vCurrent = new Vector2(0.03f, 0.02f);

    //Vector2 fWave = new Vector2();
    //float widthMobileNode = 0;

    float lamda = 0;
    int index = 0;
    List<float> windSpeedList = new List<float>();
    List<float> windDirList = new List<float>();
    List<float> currentSpeedList = new List<float>();
    List<float> currentDirList = new List<float>();
    float wind_speed = 0;
    float wind_dir = 0;
    float current_speed = 0;
    float current_dir = 0;
    //public float windLevel;
    //public float currentLevel;

    // Use this for initialization
    void Start()
    {
        ReadWindData();
        ReadCurrentData();
        StartCoroutine(SetWindWaitForSeconds());
        agentRB = GetComponent<Rigidbody>();
        lamda = Mathf.Sqrt((densityAir * dragAir * areaAir) / (densityWater * dragWater * areaWater));

        Vector2 vMobileNode = (1 / (1 + lamda)) * vCurrent + (lamda / (1 + lamda)) * vWind;
        transform.GetComponent<Rigidbody>().linearVelocity = new Vector3(vMobileNode.x, 0, vMobileNode.y);
    }

    // Update is called once per frame
    void Update()
    {
        vWind = new Vector2(Random.Range(-2, 2)+CalculateVector(wind_speed, wind_dir).x, Random.Range(-2, 2)+CalculateVector(wind_speed, wind_dir).y);
        vCurrent =new Vector2(Random.Range(-2, 2)+CalculateVector(current_speed, current_dir).x, Random.Range(-2, 2)+CalculateVector(current_speed, current_dir).y);
        Vector2 agentRB2D = new Vector2();
        agentRB2D.x = agentRB.linearVelocity.x;
        agentRB2D.y = agentRB.linearVelocity.z;

        Vector2 fWind = 0.5f * densityAir * dragAir * areaAir * (vWind - agentRB2D).magnitude * (vWind - agentRB2D);
        Vector2 fCurrent = 0.5f * densityWater * dragWater * areaWater * (agentRB2D - vCurrent).magnitude * (vCurrent - agentRB2D);
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(fWind.x, 0, fWind.y));
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(fCurrent.x, 0, fCurrent.y));
    }
    IEnumerator SetWindWaitForSeconds()
    {
        while (true)
        {
            // 每隔1秒执行一次 WriteData()
            if (index<windSpeedList.Count)
            {
                //transform.GetComponent<ShapeFFT>()._spectrum._multiplier = windSpeedList[index] / 5 + 1;
                wind_speed = windSpeedList[index];
                wind_dir = windDirList[index];
                current_speed = currentSpeedList[index];
                current_dir = currentDirList[index];
                index++;
            }
            // 等待1秒钟
            yield return new WaitForSeconds(1f);
        }
    }
    public void ReadWindData()
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
                var windSpeedValue = worksheet.Cells[i, 1].Value;
                var windDirValue = worksheet.Cells[i, 2].Value;
                if (windSpeedValue != null && float.TryParse(windSpeedValue.ToString(), out float windSpeed))
                {
                    windSpeedList.Add(windSpeed);
                }
                if (windDirValue != null && float.TryParse(windDirValue.ToString(), out float windDir))
                {
                    windDirList.Add(windDir);
                }
            }
        }

    }
    public void ReadCurrentData()
    {
        string path = Application.dataPath + "/Data/" + "current_data.xlsx";
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
                var currentSpeedValue = worksheet.Cells[i, 1].Value;
                var currentDirValue = worksheet.Cells[i, 2].Value;
                if (currentSpeedValue != null && float.TryParse(currentSpeedValue.ToString(), out float currentSpeed))
                {
                    currentSpeedList.Add(currentSpeed);
                }
                if (currentDirValue != null && float.TryParse(currentDirValue.ToString(), out float currentDir))
                {
                    currentDirList.Add(currentDir);
                }
            }
        }

    }
    Vector2 CalculateVector(float speed, float direction)
    {
        // 将角度从度转换为弧度
        float directionRadians = direction * Mathf.Deg2Rad;

        // 计算X和Z组件
        float xComponent = speed * Mathf.Cos(directionRadians);
        float zComponent = speed * Mathf.Sin(directionRadians);

        // 返回向量，因为是风向，所以需要反向
        return new Vector2(-xComponent, -zComponent);
    }
}
