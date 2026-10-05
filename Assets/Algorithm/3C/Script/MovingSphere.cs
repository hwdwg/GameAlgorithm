using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    Vector2 playerInput;

    Vector3 velocity;

    [Range(0,100)]
    [SerializeField]
    float maxMoveSpeed;

    [Range(0,100)]
    [SerializeField]
    float maxAcceleration;

    /// <summary>
    /// 移动边界
    /// </summary>
    [SerializeField]
    Rect allowedArea = new Rect(-20f,-20f,40f,40f);
    /// <summary>
    /// 边界越界弹跳因子
    /// </summary>
    [Range(0,1)]
    [SerializeField]
    float bounciness;
    private void Awake()
    {
        velocity = Vector3.zero;
    }
    private void Update()
    {
        //获取输入
        SetInput();
    }

    private void FixedUpdate()
    {
        //物理移动
        Move();
    }
    /// <summary>
    /// 获取输入
    /// </summary>
    private void SetInput()
    {
        //获取横竖轴输入
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        //限制模长（防止对角加速）
        playerInput=Vector2.ClampMagnitude(playerInput, 1f);
    }
    /// <summary>
    /// 消费输入进行移动
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(playerInput.x,0,playerInput.y);
        //加速度算速度
        Vector3 desireVelocity=moveDir*maxMoveSpeed;
        float maxSpeedDelta = maxAcceleration * Time.deltaTime ;
        velocity.x = Mathf.MoveTowards(velocity.x,desireVelocity.x,maxSpeedDelta);
        velocity.z = Mathf.MoveTowards(velocity.z,desireVelocity.z,maxSpeedDelta);
        //速度算位移
        Vector3 moveDelta=velocity*Time.deltaTime;
        Vector3 newPosition=transform.position+moveDelta;
        //边界约束
        BoundaryConstrain(ref newPosition);
        transform.position = newPosition;
    }
    /// <summary>
    /// 边界约束
    /// </summary>
    private void BoundaryConstrain(ref Vector3 newPosition)
    {
        //越界则截止在边界，速度反向
         if(newPosition.x>allowedArea.xMax)
         {
            newPosition.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
         }
        else if(newPosition.x<allowedArea.xMin)
        {
            newPosition.x=allowedArea.xMin;
            velocity.x= -velocity.x * bounciness;
        }

         if(newPosition.z>allowedArea.yMax)
         {
            newPosition.z=allowedArea.yMax;
            velocity.z= -velocity.z * bounciness;
         }
        else if(newPosition.z<allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
    }
}
