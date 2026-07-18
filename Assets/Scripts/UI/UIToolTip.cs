using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolTip : MonoBehaviour
{
    [SerializeField] private float xOffset = 100;
    [SerializeField] private float yOffset= 100;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 动态获取屏幕分辨率的一半作为 limit
        float xLimit = Screen.width / 2f;
        float yLimit = Screen.height / 2f;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }

    //这里必须要修改，是写死的，适应不了不同分辨率的屏幕，应该是根据屏幕分辨率来计算的
    //[SerializeField] private float xLimit;
    //[SerializeField] private float yLimit;

    //[SerializeField] private float xOffset;
    //[SerializeField] private float yOffset;

    //public virtual void AdjustPosition()
    //{
    //    Vector2 mousePosition = Input.mousePosition;

    //    float newXOffset = 0;
    //    float newYOffset = 0;

    //    if (mousePosition.x > xLimit)
    //        newXOffset = -xOffset;
    //    else
    //        newXOffset = xOffset;
    //    if (mousePosition.y > yLimit)
    //        newYOffset = -yOffset;
    //    else
    //        newYOffset = yOffset;

    //    transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    //}
}
