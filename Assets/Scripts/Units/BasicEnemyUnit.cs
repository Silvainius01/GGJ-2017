using UnityEngine;
using System.Collections;

public class BasicEnemyUnit : Unit {

	public enum EnemyEffect
	{
		FAT,
		DRUNK,
		NONE
	}

	private BitSet activeEffects = new BitSet();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void  Update () {
        base.Update();
	}

	public override void Attack (Unit unit){

	}

	protected override Unit GetTarget (){
		return null;
	}

	public void FocusOnUnit(Unit unit){

	}

	public void AnalyzeNewSurroundings(Vector2 gridPos){

	}

	public void EnterBuilding(SpecialBuilding building){
		building.EnterBuilding (this);
	}

	public void BuildingExited(){

	}

	public bool WantsToEnterBuilding(){
		return true;
	}

	void OnTriggerEnter2D(Collider2D other){
		// check to see if collided with target building
	}

	public void Slow(float percent){

	}

	public void GetDrunk(float slowPercent, float drunkTime, float enterBuldingPercent){

	}

	public void AddEffect(EnemyEffect effect){
		activeEffects.SetBit ((int)effect);
	}

	public void RemoveEffect(EnemyEffect effect){
		activeEffects.ClearBit((int) effect);
	}

	public bool HasEffectActive(EnemyEffect effect){
		return activeEffects.IsSet ((int)effect);
	}

	public bool HasEffectsActive(BitSet effects){
		return effects.IsSubsetOf (activeEffects);
	}
}
