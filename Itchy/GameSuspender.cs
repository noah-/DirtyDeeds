﻿using System;
using WhiteMagic;

namespace Itchy
{
    public class GameSuspender : IDisposable
    {
        protected D2Game game;

        public GameSuspender(D2Game game)
        {
            this.game = game;

            //game.Debugger.SuspendAllThreads(game.MainThreadId);
            game.Debugger.SuspendAllThreads();
            game.ResumeStormThread();
        }

        public void Dispose()
        {
            game.Debugger.ResumeAllThreads();
        }
    }
}
