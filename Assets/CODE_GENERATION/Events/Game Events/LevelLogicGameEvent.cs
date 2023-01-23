using NavySpade._PJ71.Level;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "LevelLogicGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "LevelLogic",
	    order = 120)]
	public sealed class LevelLogicGameEvent : GameEventBase<LevelLogic>
	{
	}
}