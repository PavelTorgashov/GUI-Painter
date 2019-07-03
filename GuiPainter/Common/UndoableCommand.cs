namespace GuiPainter.Common
{
    /// <summary>
    /// Определение команды, обладающей возможностью отмены
    /// </summary>
    public abstract class UndoableCommand
    {
        public string Name { get; set; }
        public abstract void Undo();
        public abstract void Redo();
    }
}
