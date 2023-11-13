namespace SSnake.GameSession
{
    public class GameSessionService
    {
        private static GameSessionService i;

        public static GameSessionService I
        {
            get
            {
                if (i == null)
                {
                    i = new GameSessionService();
                }

                return i;
            }
        }

        public SessionData SessionData { get; private set; }

        public void StartSession()
        {
            SessionData = new SessionData();
            SessionData.SetSessionState(SessionState.Playing);
        }

        public void EndSession()
        {
            SessionData.SetSessionState(SessionState.GameOver);
        }

        public void AddEdible()
        {
            SessionData.AddEdible();
        }
    }

    public interface ISessionDataSource
    {
        public int EatenEdibles { get; }
        public float GameTime { get; }
        public SessionState State { get; }
    }

    [System.Serializable]
    public enum SessionState
    {
        Playing = 0,
        GameOver = 1,
    }

}

