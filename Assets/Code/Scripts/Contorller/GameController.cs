using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Monster selectMonster = null;

    private List<Unit> selectUnitList = new List<Unit>();

    void SelectUnit(Unit newUnit){
        newUnit.SelectUnit();
        selectUnitList.Add(newUnit);
    }

    void DeselectUnit(Unit newUnit){
        newUnit.DeselectUnit();
        selectUnitList.Remove(newUnit);
    }

    public void SelectMonster(Monster newMonster){
        DeselectAll();
        newMonster.SelectMonster();
        selectMonster = newMonster;
    }

    public void DeselectMonster(){
        if(selectMonster != null){
            selectMonster.DeselectMonster();
            selectMonster = null;
        }
    }

    public void DeselectAll(){
        if (selectUnitList.Count > 0)
        {
            for (int i = 0; i < selectUnitList.Count; i++)
            {
                selectUnitList[i].DeselectUnit();
            }
            selectUnitList.Clear();
        }
        DeselectMonster();
    }

    public void OneSelectUnit(Unit newUnit){
        DeselectAll();
        SelectUnit(newUnit);
    }

    public void MulSelectUnit(Unit newUnit){
        if(selectUnitList.Contains(newUnit)) DeselectUnit(newUnit);
        else SelectUnit(newUnit);
    }

    public void DragSelectUnit(Unit newUnit){
        if ( !selectUnitList.Contains(newUnit) )
		{
			SelectUnit(newUnit);
		}
    }

    public void MoveSelectUnit(Vector3 pos){
		for ( int i = 0; i < selectUnitList.Count; ++ i ){
			selectUnitList[i].Move(pos);
		}
    }
}