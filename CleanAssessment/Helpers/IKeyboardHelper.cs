namespace CleanAssessment.Helpers
{
    public interface IKeyboardHelper
    {
        public bool IsKeyHeld(string keyCode);
        public bool IsKeyPressed(string keyCode);

        public Func<string, Task> OnKeyDown { get; set; }
        public Func<string, Task> OnKeyUp { get; set; }
    }
}
