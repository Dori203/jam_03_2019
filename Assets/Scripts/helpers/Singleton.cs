using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helpers {
    [DisallowMultipleComponent]
    public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour, IDestroyable {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object m_Lock = new object();
        private static T m_Instance;

        public static T Instance {
            get {
                lock (m_Lock) {
                    if (m_Instance != null) {
                        if (!m_Instance.ShouldDestroyOnLoad()) DontDestroyOnLoad(m_Instance);
                        return m_Instance;
                    }

                    m_Instance = (T) FindObjectOfType(typeof(T));
                    if (m_Instance != null) {
                        if (!m_Instance.ShouldDestroyOnLoad()) DontDestroyOnLoad(m_Instance);
                        return m_Instance;
                    }

                    GameObject singletonObject = new GameObject();
                    m_Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T) + " (Singleton)";
                    if (!m_Instance.ShouldDestroyOnLoad()) DontDestroyOnLoad(singletonObject);

                    return m_Instance;
                }
            }
        }

        private void Awake() {
            if (Instance == this) return;
            Debug.LogWarning("Destroying duplicate singleton instance");
            Destroy(gameObject);
        }

        private void OnEnable() {
            if (Instance != null && Instance != this) return;
            SceneManager.activeSceneChanged += Instance.OnSceneChanged;
        }

        private void OnDisable() {
            if (Instance != null && Instance != this) return;
            SceneManager.activeSceneChanged -= Instance.OnSceneChanged;
        }
    }

    public interface IDestroyable {
        bool ShouldDestroyOnLoad();
        void OnSceneChanged(Scene previousScene, Scene nextScene);
    }
}