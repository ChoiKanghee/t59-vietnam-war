using UnityEngine;

namespace T59VietnamWar.UI
{
    // Integration point only. M0 does not create, read, or overwrite saves.
    public abstract class MenuSaveProvider : MonoBehaviour
    {
        public abstract bool TryGetContinueScene(out string sceneName);
    }
}
