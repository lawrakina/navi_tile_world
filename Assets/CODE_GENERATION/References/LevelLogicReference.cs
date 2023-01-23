using NavySpade._PJ71.Level;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class LevelLogicReference : BaseReference<LevelLogic, LevelLogicVariable>
	{
	    public LevelLogicReference() : base() { }
	    public LevelLogicReference(LevelLogic value) : base(value) { }
	}
}