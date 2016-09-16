using System;

namespace MapGeneratorProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry pos_vec for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MapGeneratorMain game = new MapGeneratorMain())
            {
                game.Run();
            }
        }
    }
#endif
}

