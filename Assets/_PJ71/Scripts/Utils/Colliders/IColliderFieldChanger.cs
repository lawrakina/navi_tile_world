using System;

namespace NavySpade._PJ71.Utils.Colliders
{
    public interface IColliderFieldChanger
    {
        public event Action FieldsChanged;
    }
}