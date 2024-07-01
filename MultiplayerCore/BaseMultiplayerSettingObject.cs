using TMPro;
using UnityEngine.UIElements;

namespace TootTallyMultiplayer.MultiplayerCore
{
    public abstract class BaseMultiplayerSettingObject
    {
        public bool isDispoed;
        public string name;
        public bool isInitialized;

        public BaseMultiplayerSettingObject(string name)
        {
            this.name = name;
            
        }


        public virtual void Initialize()
        {
            isInitialized = true;
        }


        public void Remove()
        {
            Dispose();
            isDispoed = true;
        }


        public abstract void Dispose();
    }
}
