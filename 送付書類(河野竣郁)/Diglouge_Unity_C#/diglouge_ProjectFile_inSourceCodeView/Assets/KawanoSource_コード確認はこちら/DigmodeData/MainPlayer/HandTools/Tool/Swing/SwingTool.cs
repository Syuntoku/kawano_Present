using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Syuntoku.DigMode.Tool.Swing
{
    public class SwingTool
    {
        Vector3 _swingEnd;
        Vector3 _swingEndPosition;
        Vector3 _initPosition;
        Vector3 _forowPosition;
        const float SWINGEND_TIME = 0.15f;
        const float FOLLOW_THROUGH_TIME = 0.2f;

        public SwingTool()
        {
            _swingEnd = new Vector3(90.0f, 0.0f, 0.0f);
            _swingEndPosition = new Vector3(0.0f, -0.01f, 0.005f);
            _forowPosition = new Vector3(-0.3f, 0.1f, -0.3f);
        }

        //==================================
        //public
        //==================================
        public void SetInitPosition(Vector3 position)
        {
            _initPosition = position;
            _swingEndPosition += _initPosition;       
        }

        public void Swing(GameObject swingObject)
        {
            swingObject.transform.DOKill();
            swingObject.transform.rotation = swingObject.transform.parent.rotation;
            swingObject.transform.Rotate(-10.0f, 0.0f, 0, 0f);
            swingObject.transform.localPosition = _initPosition;
            swingObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            swingObject.transform.DOLocalMove(_swingEndPosition, SWINGEND_TIME).OnComplete(() =>
            {
                swingObject.transform.DOLocalMove(Vector3.Scale(_initPosition , (Vector3.forward + Vector3.right)), FOLLOW_THROUGH_TIME /2);
                swingObject.transform.DOLocalMoveY(_initPosition.y, FOLLOW_THROUGH_TIME);
            });
            swingObject.transform.DOLocalRotate(_swingEnd, SWINGEND_TIME).OnComplete(()=>
            {
                swingObject.transform.DOLocalRotate(Vector3.zero, FOLLOW_THROUGH_TIME);
            });
        }
    }
}
