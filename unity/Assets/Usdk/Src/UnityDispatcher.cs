//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace Usdk {
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    /// <summary>
    /// The unity thread dispatcher.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class UnityDispatcher : MonoBehaviour {
        private static UnityDispatcher instance;

        // The thread safe task queue.
        private static List<Action> postTasks = new List<Action> ();

        // The executing buffer.
        private static List<Action> executing = new List<Action> ();

        private static UnityDispatcher Instance {
            get {
                CheckInstance ();
                return instance;
            }
        }

        /// <summary>
        /// Work thread post a task to the main thread.
        /// </summary>
        public static void PostTask (Action task) {
            lock (postTasks) {
                postTasks.Add (task);
            }
        }

        /// <summary>
        /// Start to run this dispatcher.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void CheckInstance () {
            if (instance == null && Application.isPlaying) {
                var go = new GameObject (
                    "UsdkUnityDispatcher", typeof (UnityDispatcher));
                GameObject.DontDestroyOnLoad (go);

                instance = go.GetComponent<UnityDispatcher> ();
            }
        }

        private void Awake () {
            GameObject.DontDestroyOnLoad (this);
        }

        private void OnDestroy () {
            postTasks.Clear ();
            executing.Clear ();
            instance = null;
        }

        private void Update () {
            lock (postTasks) {
                if (postTasks.Count > 0) {
                    for (int i = 0; i < postTasks.Count; ++i) {
                        executing.Add (postTasks[i]);
                    }

                    postTasks.Clear ();
                }
            }

            for (int i = 0; i < executing.Count; ++i) {
                var task = executing[i];
                try {
                    task ();
                } catch (Exception e) {
                    Debug.LogError (e.Message, this);
                }
            }

            executing.Clear ();
        }
    }
}