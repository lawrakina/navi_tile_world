using NavySpade._PJ71.Level;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class LevelLogicEvent : UnityEvent<LevelLogic> { }

	[CreateAssetMenu(
	    fileName = "LevelLogicVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "LevelLogic",
	    order = 120)]
	public class LevelLogicVariable : BaseVariable<LevelLogic, LevelLogicEvent>
	{
	}
}