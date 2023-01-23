using NavySpade._PJ71.Level;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "LevelLogic")]
	public sealed class LevelLogicGameEventListener : BaseGameEventListener<LevelLogic, LevelLogicGameEvent, LevelLogicUnityEvent>
	{
	}
}