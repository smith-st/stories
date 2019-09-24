namespace MVC {
    public interface IGameViewListener {
        void ShowFirstScene();
        void ShowNextScene();
        void PlayerChoseAnswer(int answerId);
    }
}