using UnityEngine;

namespace SSnake.GameSession
{
    [System.Serializable]
    public class SessionData : ISessionDataSource
    {
        private SessionState _sessionState;
        private float _startTime;
        private int _eatenEdibles;

        public SessionData()
        {
            _eatenEdibles = 0;
            _startTime = Time.time;
            _sessionState = SessionState.Playing;
        }

        public int EatenEdibles => _eatenEdibles;
        public float GameTime => Time.time - _startTime;
        public SessionState State => _sessionState;

        public void AddEdible()
        {
            _eatenEdibles++;
        }

        public void SetSessionState(SessionState state)
        {
            _sessionState = state;
        }
    }
}

