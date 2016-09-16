using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    static public class VisionHelper
    {
        static public List<Point> prevLOSSqrs;

        static public void Initialize()
        {
            prevLOSSqrs = new List<Point>();

            centerViewport(GameSession.curPlayer.mapPosition);
            getVisibleSqrs(GameSession.curPlayer.tilePosition, GameSession.curPlayer.vision);
        }

        static public bool isBlocked(Point char_pos, int x_offset, int y_offset)
        {
            if (MapEngine.currMap.MSGrid[char_pos.Y + y_offset][char_pos.X + x_offset].MSStates[(int)MSFlagIndex.PASSABLE] == MSFlag.BLKD)
                return true;
            return false;
        }

        static public void centerViewport(Point char_pos)
        {
            int x_char_pos = char_pos.X;
            int y_char_pos = char_pos.Y;

            int x_offset = -440;
            int y_offset = -440;

            MapEngine.SetMapEngineViewportLocation(x_char_pos + x_offset, y_char_pos + y_offset);
        }

        static public void getVisibleSqrs(Point char_pos, int vis_max)
        {
            clearVisible(ref char_pos, vis_max);

            for (int d = 0; d < Globals.NumDirects; d += Globals.NESWItr)
                linearDirCheck(ref char_pos, Globals.Directions[d], vis_max, false);

            for (int diag = 1; diag < Globals.NumDirects; diag += Globals.NESWItr)
                diagCheck(ref char_pos, Globals.Directions[diag], vis_max, diag);
        }

        static public void diagCheck(ref Point start_pos, Point dir, int vis_max, int cur_dur)
        {
            int dir1, dir2;
            Point cur_dsqr, cur_vsqr, dir1p, dir2p;
            cur_dsqr = cur_vsqr = Point.Zero;

            dir1 = cur_dur - 1; dir2 = ((cur_dur + 1) >= Globals.NumDirects) ? 0 : (cur_dur + 1);
            dir1p = Globals.Directions[dir1]; dir2p = Globals.Directions[dir2];
            cur_dsqr.X = start_pos.X + dir.X; cur_dsqr.Y = start_pos.Y + dir.Y;

            GamePlayScreen.clampSqr(ref cur_dsqr);
            rayCheck(ref start_pos, ref cur_dsqr, ref dir, ref dir1p, ref dir2p, vis_max);
        }

        static public void rayCheck(ref Point start_sqr, ref Point cur_sqr, ref Point diag_dir,
            ref Point dir1, ref Point dir2, int vis_max)
        {
            Point cur_dir, new_cur_sqr;
            int block_c, low_vis_c, vis_max_diag, vis_max_diag_dir_line;
            block_c = low_vis_c = 0;
            bool is_blocked = false;
            cur_dir = new_cur_sqr = Point.Zero;

            vis_max_diag = (vis_max > 3) ? vis_max - 2 : 1;
            vis_max_diag_dir_line = (vis_max > 3) ? vis_max_diag + 1 : 0;

            for (int v = 0; v < vis_max_diag /*vis_max*/; v++) // (vis_max - vis_count)
            {
                vis_max_diag_dir_line += (-1 * v);
                for (int d = 0; d < 2; d++)
                {
                    cur_dir = (d == 0) ? dir1 : dir2;
                    for (int dv = d; dv < (vis_max_diag_dir_line /*vis_max - v*/); dv++) // (vis_max - vis_count - v)
                    {
                        block_c = 0;
                        is_blocked = false;
                        new_cur_sqr.X = (cur_sqr.X + (v * diag_dir.X)) + (dv * cur_dir.X); // CUR TILE TO CHECK
                        new_cur_sqr.Y = (cur_sqr.Y + (v * diag_dir.Y)) + (dv * cur_dir.Y);
                        GamePlayScreen.clampSqr(ref new_cur_sqr);

                        //getCorners(ref new_cur_sqr, ref VecRays); // returns map vectors, in actual pixel (not tile) size
                        is_blocked = isSightBlocked(ref start_sqr, ref new_cur_sqr, ref block_c);

                        if (is_blocked)
                            MapEngine.currMap.MSGrid[(int)new_cur_sqr.Y][(int)new_cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.NO_VIS;
                        else
                        {
                            if (MapEngine.currMap.MSGrid[(int)new_cur_sqr.Y][(int)new_cur_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
                                low_vis_c = 2;
                            else
                                low_vis_c = 1;

                            if (block_c > low_vis_c)
                                MapEngine.currMap.MSGrid[(int)new_cur_sqr.Y][(int)new_cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.NO_VIS;
                            else if (block_c == low_vis_c)
                                MapEngine.currMap.MSGrid[(int)new_cur_sqr.Y][(int)new_cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.LOW_VIS;
                            else
                                MapEngine.currMap.MSGrid[(int)new_cur_sqr.Y][(int)new_cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.HIGH_VIS;
                        }
                    }
                }
            }
        }

        static public bool isSightBlocked(ref Point start_sqr, ref Point tar_sqr, ref int block_c)
        {
            bool do_block_check, do_x_step, do_y_step, reached_x, reached_y;
            int next_x_sqr_pixl, next_y_sqr_pixl, delta_x, delta_y, x_sqr_step, y_sqr_step,
                 x_mid_offset, y_mid_offset, xs1, xs2, ys1, ys2, step_c, retries;
            Vector2 cur_pixel_loc, ray_start, ray_end, ray_start_backup, ray_end_backup;
            Point chk_sqr, cur_step_sqr;

            do_block_check = do_x_step = do_y_step = reached_x = reached_y = false;
            cur_pixel_loc = ray_start = ray_end = ray_start_backup = ray_end_backup = Vector2.Zero;
            chk_sqr = cur_step_sqr = Point.Zero;
            delta_x = delta_y = next_x_sqr_pixl = next_y_sqr_pixl = x_sqr_step = y_sqr_step =
                 x_mid_offset = y_mid_offset = xs1 = xs2 = ys1 = ys2 = step_c = retries = 0;

            xs1 = ys1 = 30; xs2 = ys2 = 25;
            x_mid_offset = (tar_sqr.X < start_sqr.X) ? xs1 : xs2; y_mid_offset = (tar_sqr.Y < start_sqr.Y) ? ys1 : ys2;
            ray_start.X = (start_sqr.X * 55) + x_mid_offset; ray_start.Y = (start_sqr.Y * 55) + y_mid_offset;
            ray_end.X = (tar_sqr.X * 55) + x_mid_offset; ray_end.Y = (tar_sqr.Y * 55) + y_mid_offset;
            delta_x = (int)(((ray_end.X - ray_start.X) / 55)); delta_y = (int)(((ray_end.Y - ray_start.Y) / 55));
            x_sqr_step = Math.Sign(delta_x); y_sqr_step = Math.Sign(delta_y);
            next_x_sqr_pixl = (int)ray_start.X + (x_sqr_step * 30); next_y_sqr_pixl = (int)ray_start.Y + (y_sqr_step * 30);

            ray_start_backup = ray_start;
            ray_end_backup = ray_end;
            cur_step_sqr = start_sqr;
            cur_pixel_loc = ray_start;

            while (true)
            {
                do_block_check = do_x_step = do_y_step = false;

                cur_pixel_loc.X += delta_x;
                do_x_step = ((x_sqr_step < 0) ? ((cur_pixel_loc.X <= next_x_sqr_pixl) ? true : false) : ((cur_pixel_loc.X >= next_x_sqr_pixl) ? true : false));
                reached_x = ((x_sqr_step < 0) ? ((cur_pixel_loc.X <= ray_end.X) ? true : false) : ((cur_pixel_loc.X >= ray_end.X) ? true : false));

                if (do_x_step)
                {
                    cur_step_sqr.X += x_sqr_step;
                    next_x_sqr_pixl += (x_sqr_step * 55);
                    do_block_check = true;
                }

                cur_pixel_loc.Y += delta_y;
                do_y_step = ((y_sqr_step < 0) ? ((cur_pixel_loc.Y <= next_y_sqr_pixl) ? true : false) : ((cur_pixel_loc.Y >= next_y_sqr_pixl) ? true : false));
                reached_y = ((y_sqr_step < 0) ? ((cur_pixel_loc.Y <= ray_end.Y) ? true : false) : ((cur_pixel_loc.Y >= ray_end.Y) ? true : false));

                if (do_y_step)
                {
                    cur_step_sqr.Y += y_sqr_step;
                    next_y_sqr_pixl += (y_sqr_step * 55);
                    do_block_check = true;
                }

                if (reached_x)
                    if (reached_y)
                        return false;

                if (do_block_check)
                {
                    step_c++;
                    if (cur_step_sqr != tar_sqr)
                    {
                        if (MapEngine.currMap.MSGrid[cur_step_sqr.Y][cur_step_sqr.X].MSStates[(int)MSFlagIndex.PASSABLE] == MSFlag.BLKD)
                        {
                            block_c++;
                            if (step_c == 1)
                                return true;

                            if (MapEngine.currMap.MSGrid[tar_sqr.Y][tar_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
                            {
                                if (retries < 1)
                                {
                                    // shift to corner opposite of direction
                                    ray_start.X = ray_start_backup.X - (x_sqr_step * 25); ray_start.Y = ray_start_backup.Y - (y_sqr_step * 25);

                                    if (do_x_step)
                                        if (do_y_step)
                                            return true;

                                    switch (retries)
                                    {
                                        case 0: // shift to corner closer x, closer y
                                            ray_end.X = ray_end_backup.X - (x_sqr_step * 25); ray_end.Y = ray_end_backup.Y - (y_sqr_step * 25);
                                            break;
                                        case 1: // shift to corner further x, closer y
                                            //if (MapEngine.currMap.MSGrid[tar_sqr.Y][tar_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
                                            //{
                                            ray_end.X = ray_end_backup.X + (x_sqr_step * 30); ray_end.Y = ray_end_backup.Y - (y_sqr_step * 25);
                                            //}
                                            //else
                                            //    return true;
                                            break;
                                    }

                                    delta_x = (int)(((ray_end.X - ray_start.X) / 55)); delta_y = (int)(((ray_end.Y - ray_start.Y) / 55));
                                    next_x_sqr_pixl = (int)ray_start.X + (x_sqr_step * 55); next_y_sqr_pixl = (int)ray_start.Y + (y_sqr_step * 55);
                                    cur_step_sqr = start_sqr;
                                    cur_pixel_loc = ray_start;
                                    reached_x = reached_y = false;
                                    retries++;
                                }
                                else
                                    return true;
                            }
                            else
                                return true;
                        }
                    }
                    else
                        return false;
                }
            }
        }

        static public void getCorners(ref Point end_sqr, ref Vector2[] vec_rays)
        {
            vec_rays[0].X = end_sqr.X * 55;
            vec_rays[0].Y = end_sqr.Y * 55;

            vec_rays[1].X = (end_sqr.X + 1) * 55;
            vec_rays[1].Y = end_sqr.Y * 55;

            vec_rays[2].X = end_sqr.X * 55;
            vec_rays[2].Y = (end_sqr.Y + 1) * 55;

            vec_rays[3].X = (end_sqr.X + 1) * 55;
            vec_rays[3].Y = (end_sqr.Y + 1) * 55;
        }

        static public bool linearDirCheck(ref Point start_pos, Point dir, int vis_max, bool break_early)
        {
            bool set_block, blockage;
            set_block = blockage = false;

            Point cur_sqr, block_sqr;
            cur_sqr = Point.Zero;
            block_sqr = new Point(-1, -1);

            for (int s = 1; s <= vis_max; s++)
            {
                cur_sqr.X = start_pos.X + (s * dir.X); cur_sqr.Y = start_pos.Y + (s * dir.Y);

                GamePlayScreen.clampSqr(ref cur_sqr);
                if (cur_sqr == block_sqr)
                    continue;

                if (MapEngine.currMap.MSGrid[cur_sqr.Y][cur_sqr.X].MSStates[(int)MSFlagIndex.PASSABLE] == MSFlag.BLKD)
                {
                    set_block = true;
                    block_sqr = cur_sqr;
                }

                MapEngine.currMap.MSGrid[(int)cur_sqr.Y][(int)cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = (blockage) ? MSFlag.NO_VIS : MSFlag.HIGH_VIS;

                if (set_block)
                    blockage = true;

                if (blockage)
                    if (break_early)
                        return true;
            }
            return false;
        }

        static public void getTargetLOS(Point start_sqr, ref Point tar_sqr, out List<Point> returned_sqr_line)
        {
            bool do_block_check, do_x_step, do_y_step, reached_x, reached_y;
            int next_x_sqr_pixl, next_y_sqr_pixl, delta_x, delta_y, x_sqr_step, y_sqr_step,
                 x_mid_offset, y_mid_offset, xs1, xs2, ys1, ys2, step_c, retries;
            Vector2 cur_pixel_loc, ray_start, ray_end, ray_start_backup, ray_end_backup;
            Point chk_sqr, cur_step_sqr;

            do_block_check = do_x_step = do_y_step = reached_x = reached_y = false;
            cur_pixel_loc = ray_start = ray_end = ray_start_backup = ray_end_backup = Vector2.Zero;
            chk_sqr = cur_step_sqr = Point.Zero;
            delta_x = delta_y = next_x_sqr_pixl = next_y_sqr_pixl = x_sqr_step = y_sqr_step =
                 x_mid_offset = y_mid_offset = xs1 = xs2 = ys1 = ys2 = step_c = retries = 0;
            returned_sqr_line = new List<Point>();

            xs1 = ys1 = 30; xs2 = ys2 = 25;
            x_mid_offset = (tar_sqr.X < start_sqr.X) ? xs1 : xs2; y_mid_offset = (tar_sqr.Y < start_sqr.Y) ? ys1 : ys2;
            ray_start.X = (start_sqr.X * 55) + x_mid_offset; ray_start.Y = (start_sqr.Y * 55) + y_mid_offset;
            ray_end.X = (tar_sqr.X * 55) + x_mid_offset; ray_end.Y = (tar_sqr.Y * 55) + y_mid_offset;
            delta_x = (int)(((ray_end.X - ray_start.X) / 55)); delta_y = (int)(((ray_end.Y - ray_start.Y) / 55));
            x_sqr_step = Math.Sign(delta_x); y_sqr_step = Math.Sign(delta_y);
            next_x_sqr_pixl = (int)ray_start.X + (x_sqr_step * 30); next_y_sqr_pixl = (int)ray_start.Y + (y_sqr_step * 30);

            ray_start_backup = ray_start;
            ray_end_backup = ray_end;
            cur_step_sqr = start_sqr;
            cur_pixel_loc = ray_start;

            while (true)
            {
                do_block_check = do_x_step = do_y_step = false;

                cur_pixel_loc.X += delta_x;
                do_x_step = ((x_sqr_step < 0) ? ((cur_pixel_loc.X <= next_x_sqr_pixl) ? true : false) : ((cur_pixel_loc.X >= next_x_sqr_pixl) ? true : false));
                reached_x = ((x_sqr_step < 0) ? ((cur_pixel_loc.X <= ray_end.X) ? true : false) : ((cur_pixel_loc.X >= ray_end.X) ? true : false));

                if (do_x_step)
                {
                    cur_step_sqr.X += x_sqr_step;
                    next_x_sqr_pixl += (x_sqr_step * 55);
                    do_block_check = true;
                }

                cur_pixel_loc.Y += delta_y;
                do_y_step = ((y_sqr_step < 0) ? ((cur_pixel_loc.Y <= next_y_sqr_pixl) ? true : false) : ((cur_pixel_loc.Y >= next_y_sqr_pixl) ? true : false));
                reached_y = ((y_sqr_step < 0) ? ((cur_pixel_loc.Y <= ray_end.Y) ? true : false) : ((cur_pixel_loc.Y >= ray_end.Y) ? true : false));

                if (do_y_step)
                {
                    cur_step_sqr.Y += y_sqr_step;
                    next_y_sqr_pixl += (y_sqr_step * 55);
                    do_block_check = true;
                }

                if (reached_x)
                    if (reached_y)
                    {
                        returned_sqr_line.Add(tar_sqr);
                        return;
                    }

                if (do_block_check)
                {
                    step_c++;
                    if (cur_step_sqr != tar_sqr)
                    {
                        returned_sqr_line.Add(cur_step_sqr);

                        if (MapEngine.currMap.MSGrid[cur_step_sqr.Y][cur_step_sqr.X].MSStates[(int)MSFlagIndex.PASSABLE] == MSFlag.BLKD)
                        {
                            returned_sqr_line.Clear();
                            if (step_c == 1)
                                return;

                            //if (MapEngine.currMap.MSGrid[tar_sqr.Y][tar_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
                            //{
                            if (retries < 1)
                            {
                                // shift to corner opposite of direction
                                ray_start.X = ray_start_backup.X - (x_sqr_step * 25); ray_start.Y = ray_start_backup.Y - (y_sqr_step * 25);

                                if (do_x_step)
                                    if (do_y_step)
                                        return;

                                switch (retries)
                                {
                                    case 0: // shift to corner closer x, closer y
                                        ray_end.X = ray_end_backup.X - (x_sqr_step * 25); ray_end.Y = ray_end_backup.Y - (y_sqr_step * 25);
                                        break;
                                    case 1: // shift to corner further x, closer y
                                        //if (MapEngine.currMap.MSGrid[tar_sqr.Y][tar_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
                                        //{
                                        ray_end.X = ray_end_backup.X + (x_sqr_step * 30); ray_end.Y = ray_end_backup.Y - (y_sqr_step * 25);
                                        //}
                                        //else
                                        //    return true;
                                        break;
                                }

                                delta_x = (int)(((ray_end.X - ray_start.X) / 55)); delta_y = (int)(((ray_end.Y - ray_start.Y) / 55));
                                next_x_sqr_pixl = (int)ray_start.X + (x_sqr_step * 55); next_y_sqr_pixl = (int)ray_start.Y + (y_sqr_step * 55);
                                cur_step_sqr = start_sqr;
                                cur_pixel_loc = ray_start;
                                reached_x = reached_y = false;
                                retries++;
                            }
                            else
                                return;
                            //}
                            //else
                            //    return;// true;
                        }
                    }
                    else
                    {
                        returned_sqr_line.Add(tar_sqr);
                        return;
                    }
                }
            }
        }

        static public void drawLOS()
        {
            List<Point> cur_los_sqrs;

            Point tar_sqr = MapEngine.curMouseSqr;
            GamePlayScreen.clampSqr(ref tar_sqr);

            clearLOS();

            getTargetLOS(GameSession.curPlayer.tilePosition, ref tar_sqr, out cur_los_sqrs);
            if (cur_los_sqrs.Count > 0)
            {
                for (int i = 0; i < cur_los_sqrs.Count; i++)
                {
                    MapEngine.currMap.MSGrid[cur_los_sqrs[i].Y][cur_los_sqrs[i].X].AltColor = new Color(0, 200, 0, 10);
                    prevLOSSqrs.Add(cur_los_sqrs[i]);
                }
            }
        }

        static public void clearLOS()
        {
            if (prevLOSSqrs.Count > 0)
            {
                for (int p = 0; p < prevLOSSqrs.Count; p++)
                    MapEngine.currMap.MSGrid[prevLOSSqrs[p].Y][prevLOSSqrs[p].X].AltColor = Color.White;
                prevLOSSqrs.Clear();
            }
        }

        static public void clearVisible(ref Point char_pos, int vis_max)
        {
            int row_size = 33;
            Point start_sqr = new Point(char_pos.X - 17, char_pos.Y - 17);
            Point cur_sqr = start_sqr;

            for (int r = 0; r < row_size; r++)
            {
                cur_sqr.Y = start_sqr.Y + r;
                for (int c = 0; c < row_size; c++)
                {
                    GamePlayScreen.clampSqr(ref cur_sqr);
                    MapEngine.currMap.MSGrid[cur_sqr.Y][cur_sqr.X].MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.NO_VIS;
                    cur_sqr.X = start_sqr.X + c;
                }
            }
        }
    }
}
