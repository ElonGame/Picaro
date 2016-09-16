using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTutorial_05_26MAR14
{
    class MainMenuStartScreen : MenuScreen
    {
        MenuEntry gameplayEntry;
        MenuEntry returnPlayEntry;
        MenuEntry exitEntry;

        public MainMenuStartScreen(Color title_color, SpriteFont font, Vector2 pos)
            : base(title_color, font, pos)
        {
            gameplayEntry = new MenuEntry("Play");
            gameplayEntry.description = "Go to game play screen";
            gameplayEntry.position = new Vector2(position.X, position.Y - 25);
            gameplayEntry.Selected += PlayGameEntrySelected;
            MenuEntries.Add(gameplayEntry);

            returnPlayEntry = new MenuEntry("Return");
            returnPlayEntry.description = "Return to game play screen";
            returnPlayEntry.position = new Vector2(position.X, gameplayEntry.position.Y + 50);
            returnPlayEntry.Selected += ReturnGameEntrySelected;
            MenuEntries.Add(returnPlayEntry);

            exitEntry = new MenuEntry("Quit");
            exitEntry.description = "Exit Game";
            exitEntry.position = new Vector2(returnPlayEntry.position.X, returnPlayEntry.position.Y + 50);
            exitEntry.Selected += OnCancel;
            MenuEntries.Add(exitEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.ExitGame))            
            {
                ExitScreen();
                return;
            }

            base.HandleInput();
        }

        void PlayGameEntrySelected(object sender, EventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new GamePlayScreen());
        }

        void ReturnGameEntrySelected(object sender, EventArgs e)
        {
            if (!MapEngine.IsMapNull())
                ScreenManager.RemoveScreen(this);
        }

        protected override void OnCancel()
        {
            ScreenManager.thisGame.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.spriteBatch;

            sb.Begin();

            // draw the background images

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                
                menuEntry.Draw(this, isSelected, gameTime, true);
            }

            // draw the description text for the selected entry
            MenuEntry selectedMenuEntry = SelectedMenuEntry;
            if ((selectedMenuEntry != null) && !String.IsNullOrEmpty(selectedMenuEntry.description))
                Fonts.DrawCenteredText(ref sb, spriteFont, selectedMenuEntry.description, new Vector2(GameMain.VPCenter.X, 800), selectColor);
                //sb.DrawString(spriteFont, selectedMenuEntry.description, 
                //    new Vector2(((GameMain.vp.Width / 2) - spriteFont.MeasureString(selectedMenuEntry.description).X), 800), Color.White);

            sb.End();
        }
    }
}
