using UnityEngine;
using System.Collections;

public class BasicEnemyUnit : Unit {

	public enum EnemyEffect
	{
		FAT,
		DRUNK,
		PLAGUE,
		DUEL_WINNER,
		NONE
	}

	private BitSet activeEffects = new BitSet();

	// Use this for initialization
	void Start () {
	
	}

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

	public void Slow(float percent)
    {
		AddEffect (EnemyEffect.FAT);
		speed = speed * percent;
	}

	public void GetDrunk(float slowPercent, float drunkTime, float enterBuldingPercent){
		AddEffect (EnemyEffect.DRUNK);
	}

	public void Plague(float deathTime){
		AddEffect (EnemyEffect.PLAGUE);
		// add plague componenet
		Plague plagueComp = gameObject.AddComponent<Plague>();
		plagueComp.Init (deathTime);
	}

	public void DuelStarted(){
		stunned = true;
	}

	public void DuelEnded(){
		AddEffect (EnemyEffect.DUEL_WINNER);
		stunned = false;
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
