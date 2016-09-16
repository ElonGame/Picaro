using System;

namespace GameTutorial_05_26MAR14
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry pos_vec for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameMain game = new GameMain())
            {
                game.Run();
            }
        }
    }
#endif
}

