using System;
using System.Collections.Generic;

namespace GuiPainter.Common
{
    /// <summary>
    /// Undo/Redo manager
    /// </summary>
    public class UndoRedoManager
    {
        readonly LinkedList<UndoableCommand> _history;
        readonly Stack<UndoableCommand> _redoStack = new Stack<UndoableCommand>();
        readonly int _maxHistoryLength;
        int _updating;

        public static int DefaultMaxHistoryLength = 20;

        public static UndoRedoManager Instance
        {
            get
            {
                if (Sessions.Count == 0)
                    CreateNewSession();
                return Sessions.Peek();
            }
        }

        private static readonly Stack<UndoRedoManager> Sessions = new Stack<UndoRedoManager>();

        public static int SessionsCount
        {
            get { return Sessions.Count; }
        }

        public static void CreateNewSession(int maxHistoryLength = -1)
        {
            Sessions.Push(new UndoRedoManager(maxHistoryLength));
        }

        public static void ReturnToPrevSession()
        {
            Sessions.Pop();
        }

        public UndoRedoManager(int maxHistoryLength = -1)
        {
            _maxHistoryLength = maxHistoryLength;
            if (maxHistoryLength == -1)
                _maxHistoryLength = DefaultMaxHistoryLength;
            _history = new LinkedList<UndoableCommand>();
        }

        public virtual void Add(UndoableCommand cmd)
        {
            if (_updating > 0)
                return;

            _history.AddLast(cmd);
            if (_history.Count > _maxHistoryLength)
                _history.RemoveFirst();

            _redoStack.Clear();
        }

        public virtual void Add(Action undo, Action redo)
        {
            Add(new ActionCommand(undo, redo));
        }

        public void Undo()
        {
            if (_history.Count <= 0) return;
            var cmd = _history.Last.Value;
            _history.RemoveLast();
            //
            _updating++; // prevent text changing into handlers
            try
            {
                cmd.Undo();
            }
            finally
            {
                _updating--;
            }
            //
            _redoStack.Push(cmd);
        }


        public void ClearHistory()
        {
            _history.Clear();
            _redoStack.Clear();
        }

        public void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            _updating++; // prevent text changing into handlers
            try
            {
                var cmd = _redoStack.Pop();
                cmd.Redo();

                _history.AddLast(cmd);
                if (_history.Count > _maxHistoryLength)
                    _history.RemoveFirst();
            }
            finally
            {
                _updating--;
            }

        }

        public bool CanUndo
        {
            get
            {
                return _history.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return _redoStack.Count > 0;
            }
        }
    }
}
