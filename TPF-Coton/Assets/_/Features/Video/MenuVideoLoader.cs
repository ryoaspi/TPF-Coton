using System;
using UnityEngine;
using UnityEngine.Video;

namespace Video.Runtime
{
    public class MenuVideoLoader : MonoBehaviour
    {
        public VideoPlayer m_videoPlayer;
        private void Start()
        {
            m_videoPlayer.Prepare();
            m_videoPlayer.prepareCompleted += OnVideoPrepared;
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            vp.Play();
        }
    }
}
