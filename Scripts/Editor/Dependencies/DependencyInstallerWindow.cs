#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace STIGRADOR.Editor.Dependencies
{
    /// <summary>
    /// Shows the optional dependency installer once per editor session
    /// while some of the optional packages are missing.
    /// </summary>
    [InitializeOnLoad]
    internal static class DependencyInstallerBootstrap
    {
        private const string ShownThisSessionKey = "STIGLIB.DependencyInstaller.Shown";

        static DependencyInstallerBootstrap()
        {
            if (Application.isBatchMode)
                return;

            EditorApplication.delayCall += TryShow;
        }

        private static void TryShow()
        {
            if (SessionState.GetBool(ShownThisSessionKey, false))
                return;

            SessionState.SetBool(ShownThisSessionKey, true);

            if (DependencyInstallerWindow.IsDismissed)
                return;

            if (DependencyInstallerWindow.Dependencies.All(d => d.IsInstalled))
                return;

            DependencyInstallerWindow.Open();
        }
    }

    public class DependencyInstallerWindow : EditorWindow
    {
        private const string DismissedConfigKey = "STIGLIB.DependencyInstaller.Dismissed";

        internal class Dependency
        {
            public readonly string DisplayName;
            public readonly string PackageName;
            public readonly string Url;
            public readonly string Description;
            public bool Selected = true;

            public Dependency(string displayName, string packageName, string url, string description)
            {
                DisplayName = displayName;
                PackageName = packageName;
                Url = url;
                Description = description;
            }

            public bool IsInstalled => PackageInfo.FindForPackageName(PackageName) != null;
        }

        internal static readonly Dependency[] Dependencies =
        {
            new Dependency(
                "VContainer",
                "jp.hadashikick.vcontainer",
                "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.19.0",
                "DI container for Unity"),
            new Dependency(
                "UniTask",
                "com.cysharp.unitask",
                "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.5.11",
                "Allocation-free async/await for Unity"),
        };

        internal static bool IsDismissed
        {
            get => EditorUserSettings.GetConfigValue(DismissedConfigKey) == "1";
            set => EditorUserSettings.SetConfigValue(DismissedConfigKey, value ? "1" : null);
        }

        private AddAndRemoveRequest _request;
        private bool _dontAskAgain;

        [MenuItem("Tools/STIGLIB/Install Dependencies")]
        public static void Open()
        {
            var window = GetWindow<DependencyInstallerWindow>(true, "STIGLIB Dependencies", true);
            window.minSize = new Vector2(420, 200);
            window._dontAskAgain = IsDismissed;
            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("STIGLIB can use the following packages.", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Select the ones to install via Package Manager:", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(_request != null))
            {
                foreach (var dependency in Dependencies)
                    DrawDependency(dependency);

                EditorGUILayout.Space();
                _dontAskAgain = EditorGUILayout.ToggleLeft("Don't ask again for this project", _dontAskAgain);
            }

            GUILayout.FlexibleSpace();

            if (_request != null)
            {
                EditorGUILayout.HelpBox("Installing packages...", MessageType.Info);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                var toInstall = Dependencies.Where(d => d.Selected && !d.IsInstalled).ToArray();
                using (new EditorGUI.DisabledScope(toInstall.Length == 0))
                {
                    if (GUILayout.Button("OK", GUILayout.Width(100)))
                        Install(toInstall);
                }

                if (GUILayout.Button("Cancel", GUILayout.Width(100)))
                {
                    IsDismissed = _dontAskAgain;
                    Close();
                }
            }
        }

        private static void DrawDependency(Dependency dependency)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (dependency.IsInstalled)
                {
                    using (new EditorGUI.DisabledScope(true))
                        EditorGUILayout.ToggleLeft($"{dependency.DisplayName} (installed)", true);
                }
                else
                {
                    dependency.Selected = EditorGUILayout.ToggleLeft(dependency.DisplayName, dependency.Selected);
                }
            }

            EditorGUILayout.LabelField(dependency.Description, EditorStyles.miniLabel);
        }

        private void Install(Dependency[] toInstall)
        {
            IsDismissed = _dontAskAgain;
            _request = Client.AddAndRemove(toInstall.Select(d => d.Url).ToArray());
            EditorApplication.update += PollRequest;
        }

        private void PollRequest()
        {
            if (_request == null || !_request.IsCompleted)
                return;

            EditorApplication.update -= PollRequest;

            if (_request.Status == StatusCode.Success)
                Debug.Log("[STIGLIB] Dependencies installed.");
            else
                Debug.LogError($"[STIGLIB] Failed to install dependencies: {_request.Error?.message}");

            _request = null;
            Close();
        }

        private void OnDestroy()
        {
            EditorApplication.update -= PollRequest;
        }
    }
}
#endif
