using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; // 目标位置
    public float offset_y;
    private float targetRotationAngle;  //目标的旋转角度
    private float myRotationAngle; //需要的旋转角度

    private Quaternion adjustedRotation; //调整后的Rotation

    private Vector3 offsetPostion;  //位置偏置

    private void Start()
    {
        offsetPostion = new Vector3(0, offset_y, -10); //偏置y方向5个单位，z方向-10个单位
    }

    private void LateUpdate()
    { //Update之后执行，使摄像机移动更平滑
        if (!target)
        {
            return;
        }

        //根据自身与目标的旋转角度平滑变动
        targetRotationAngle = target.eulerAngles.y;
        myRotationAngle = transform.eulerAngles.y;
        myRotationAngle = Mathf.LerpAngle(myRotationAngle, targetRotationAngle, 0.3f);

        adjustedRotation = Quaternion.Euler(0, myRotationAngle, 0); //调整过后的Rotation

        transform.position = target.position + (adjustedRotation * offsetPostion);   //根据目标位置调整自身
        transform.LookAt(target);   //看向目标
    }
}
