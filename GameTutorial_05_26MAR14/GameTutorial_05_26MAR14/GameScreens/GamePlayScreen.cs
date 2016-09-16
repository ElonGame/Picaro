using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    // updates GameSession, Draws Session, get input for session?
    class GamePlayScreen : GameScreen
    {
        public bool playerMoved = false;
        public bool drawingLOS = false;

        public Point prevPlayerPos = Point.Zero;
        public Point curPlayerPos = Point.Zero;

        public GamePlayScreen()//ref ScreenManager sm)
        {
            //base.Initialize(ref sm);
            GameSession.StartNewSession(ref ScreenManager.contentMan);
            VisionHelper.Initialize();
            HUDHelper.Initialize();

        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive && !coveredByOtherScreen)
                GameSession.Update(gt);       
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.spriteBatch;
            if (playerMoved)
                VisionHelper.getVisibleSqrs(GameSession.curPlayer.tilePosition, GameSession.curPlayer.vision);
            
            sb.Begin();
            HUDHelper.Draw(ref sb);
            MapEngine.Draw(sb);
            sb.End();

            // draw altitude tiles
            //if (singleton.mapLoaded)
            //    MapEngine.Draw(sb, ref ScreenManager.graphicDevMan, ref singleton.normalmapEffect);

            sb.Begin();
            GameSession.curPlayer.Draw(ref sb, ref MapEngine.mEngVportWLoc);
            sb.End();
        }

        public override void HandleInput()
        {
            prevPlayerPos = curPlayerPos;

            if (InputManager.IsActionTriggered(InputManager.Action.Back))
            {
                ScreenManager.AddScreen(new MainMenuStartScreen(Color.Blue, Fonts.normFont, GameMain.VPCenter));
                return;
            }

            // player input
            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharN, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, 0, -1))
                {
                    GameSession.curPlayer.SetMapPosition(0, -55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }        
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharNE, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, 1, -1))
                {
                    GameSession.curPlayer.SetMapPosition(55, -55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharE, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, 1, 0))
                {
                    GameSession.curPlayer.SetMapPosition(55, 0, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharSE, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, 1, 1))
                {
                    GameSession.curPlayer.SetMapPosition(55, 55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharS, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, 0, 1))
                {
                    GameSession.curPlayer.SetMapPosition(0, 55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharSW, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, -1, 1))
                {
                    GameSession.curPlayer.SetMapPosition(-55, 55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharW, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, -1, 0))
                {
                    GameSession.curPlayer.SetMapPosition(-55, 0, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.MoveCharNW, 100))
            {
                if (!VisionHelper.isBlocked(GameSession.curPlayer.tilePosition, -1, -1))
                {
                    GameSession.curPlayer.SetMapPosition(-55, -55, MapEngine.currMap.widthTiles, MapEngine.currMap.heightTiles);
                    VisionHelper.centerViewport(GameSession.curPlayer.mapPosition);
                }
            }

            // move mapEngineViewport up
            if (InputManager.IsActionPressedTimed(InputManager.Action.CursorUp, 100))
                MapEngine.MoveMapEngineViewportLocation(0, -55);

            // move mapEngineViewport right
            if (InputManager.IsActionPressedTimed(InputManager.Action.PageRight, 100))
                MapEngine.MoveMapEngineViewportLocation(55, 0);

            // move mapEngineViewport down
            if (InputManager.IsActionPressedTimed(InputManager.Action.CursorDown, 100))
                MapEngine.MoveMapEngineViewportLocation(0, 55);

            // move mapEngineViewport left
            if (InputManager.IsActionPressedTimed(InputManager.Action.PageLeft, 100))
                MapEngine.MoveMapEngineViewportLocation(-55, 0);

            //if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            //    MapEngine.ShowBlankStates = true;

            // save
            if (InputManager.IsActionTriggered(InputManager.Action.Save))
                MapEngine.SaveMap();

            // load
            if (InputManager.IsActionTriggered(InputManager.Action.Load))
                MapEngine.LoadMap();

            // new
            if (InputManager.IsActionTriggered(InputManager.Action.New))
            {
                MapEngine.NewMap();
                MapEngine.LoadMap();
            }

            if (InputManager.IsActionPressedTimed(InputManager.Action.Ok, 100))
            {
                if (!drawingLOS)
                    drawingLOS = true;
                else
                {
                    drawingLOS = false;
                    VisionHelper.clearLOS();
                }
            }

            if (InputManager.IsActionTriggered(InputManager.Action.ShowDebug))
                Globals.GetMapData = (Globals.GetMapData) ? false : true;

            //if(InputManager.IsActionPressedTimed(InputManager.Action.SpritePrev, 100))
            //{
            //    GameSession.curPlayer.MapSprite.
            //}

            curPlayerPos = GameSession.curPlayer.tilePosition;

            if (drawingLOS)
                VisionHelper.drawLOS();

            if (curPlayerPos != prevPlayerPos)
                playerMoved = true;
            else
                playerMoved = false;
        }



        static public void clampSqr(ref Point sqr)
        {
            if (sqr.X < 0)
                sqr.X = 0;
            if (sqr.Y < 0)
                sqr.Y = 0;
            if (sqr.X >= MapEngine.currMap.widthTiles)
                sqr.X = MapEngine.currMap.widthTiles - 1;
            if (sqr.Y >= MapEngine.currMap.heightTiles)
                sqr.Y = MapEngine.currMap.heightTiles - 1;
        }
    }
}
