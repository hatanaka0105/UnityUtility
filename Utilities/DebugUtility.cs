
namespace UnityCustomExtension
{
    public static class DebugUtility
    {
        private static DebugWindowManager _debugWindow;

        public static DebugWindowManager GetDebugWindow()
        {
            if(_debugWindow == null)
            {
                _debugWindow = UUtility.InstantiateFromResources<DebugWindowManager>("DebugWindow");
            }

            return _debugWindow;
        }

        public static void AddText(string text)
        {
            if (_debugWindow == null)
            {
                _debugWindow = UUtility.InstantiateFromResources<DebugWindowManager>("DebugWindow");
            }

            _debugWindow.AddDebugText(text);
        }

        public static void SetText(string text)
        {
            if (_debugWindow == null)
            {
                _debugWindow = UUtility.InstantiateFromResources<DebugWindowManager>("DebugWindow");
            }

            _debugWindow.SetDebugText(text);
        }
    } 
}