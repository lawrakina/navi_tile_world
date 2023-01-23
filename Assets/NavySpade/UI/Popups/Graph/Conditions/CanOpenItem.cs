using System;
using Core.Meta;

namespace Core.UI.Popups.Graph.Conditions
{
    [Serializable]
    [CustomSerializeReferenceName("CanOpenItem")]
    public class CanOpenItem : ICondition
    {
        public bool Check()
        {
            return MetaGameConfig.Instance.CanUnlock;
        }
    }
}