using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Movement : AnimatorProperty
{
    public LayerMask crashMask;
    public float moveSpeed = 2.0f;
    public float rotSpeed = 360.0f;
    NavMeshPath myPath;
    Coroutine corMove = null;
    Coroutine corRotate = null;
    Coroutine corByPathMove = null;

    protected void StopMoveCoroutine(){
        if (corMove != null) {
            StopCoroutine(corMove);
            corMove = null;
        }
        if (corRotate != null) {
            StopCoroutine(corRotate);
            corRotate = null;
        }
        if (corByPathMove != null) {
            StopCoroutine(corByPathMove);
            corByPathMove = null;
        }
    }    

    public void MoveToPos(Vector3 pos){
        if(myPath == null) myPath = new NavMeshPath();
        if(NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, myPath)){
            switch(myPath.status){
                case NavMeshPathStatus.PathComplete:
                case NavMeshPathStatus.PathPartial:
                    StopMoveCoroutine();
                    corByPathMove = StartCoroutine(MovingByPath(myPath.corners));
                    break;
                case NavMeshPathStatus.PathInvalid:
                    break;
            }
        }
    }

    IEnumerator MovingByPath(Vector3[] path){
        int curIdx = 1;
        while(curIdx < path.Length){
            yield return corMove = StartCoroutine(MovingToPos(path[curIdx++]));
        }
    }

    IEnumerator MovingToPos(Vector3 pos){
        if(myAnim.gameObject.GetComponentInParent<Unit>()){
            myAnim.SetBool("IsMoving", true);
        }
        Vector3 moveDir = pos - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        corRotate = StartCoroutine(RotatingToPos(moveDir));

        while(moveDist > 0.0f){
            float delta = moveSpeed * Time.deltaTime;
            // Ray ray = new Ray(transform.position, transform.forward);
            // if(Physics.Raycast(ray, out RaycastHit hit, delta, crashMask)){
            //     CapsuleCollider col = transform.GetComponent<CapsuleCollider>();
            //     delta = hit.distance - col.radius;
            // }
            if(moveDist < delta) delta = moveDist;
            transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;
            yield return null;
        }
        if(myAnim.gameObject.GetComponentInParent<Unit>()){
            myAnim.SetBool("IsMoving", false);
        }
    }
    IEnumerator RotatingToPos(Vector3 dir){
        float rotAngle = Vector3.Angle(transform.forward, dir);
        
        float rotDir = 1.0f;
        if(Vector3.Dot(transform.right, dir) < 0.0f) rotDir = -1.0f;

        while(rotAngle > 0.0f){
            float delta = rotSpeed * Time.deltaTime;
            if(rotAngle < delta) delta = rotAngle;
            transform.Rotate(Vector3.up * rotDir * delta);
            rotAngle -= delta;
            yield return null;
        }
    }
}
