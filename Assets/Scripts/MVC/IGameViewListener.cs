namespace MVC {
    public interface IGameViewListener {
        void ShowNextScene();
        void PlayerChoseAnswer(int answerId);
    }
}