using System.Collections.Generic;
using System.Windows;

namespace DockWindowLib.Models
{
    class DragWindowService
    {
        List<IDropSurface> Surfaces = new List<IDropSurface>();
        List<IDropSurface> SurfacesWithDragOver = new List<IDropSurface>();

        DockManagerModel _owner;

        public DockManagerModel DockManager
        {
            get { return _owner; }
        }

        public DragWindowService(DockManagerModel owner)
        {
            _owner = owner;
        }

        public void Register(IDropSurface surface)
        {
            if (!Surfaces.Contains(surface))
                Surfaces.Add(surface);
        }

        public void Unregister(IDropSurface surface)
        {
            Surfaces.Remove(surface);
        }

        //public static void StartDrag(DockablePane pane, Point point)
        //{
        //    StartDrag(new FloatingWindow(_pane), point);
        //}
#if false
        Point Offset;

        public void StartDrag(FloatingWindowModel wnd, Point point, Point offset)
        {
            _pane = wnd.HostedPane;
            Offset = offset;

            _wnd = wnd;

            if (Offset.X >= _wnd.Width)
                Offset.X = _wnd.Width / 2;


            _wnd.Left = point.X - Offset.X;
            _wnd.Top = point.Y - Offset.Y;
            _wnd.Show();

            foreach (IDropSurface surface in Surfaces)
            {
                if (surface.SurfaceRectangle.Contains(point))
                {
                    SurfacesWithDragOver.Add(surface);
                    surface.OnDragEnter(point);
                }
            }
        }

        public void MoveDrag(Point point)
        {
            if (_wnd == null)
                return;

            _wnd.Left = point.X - Offset.X;
            _wnd.Top = point.Y - Offset.Y;

            List<IDropSurface> enteringSurfaces = new List<IDropSurface>();
            foreach (IDropSurface surface in Surfaces)
            {
                if (surface.SurfaceRectangle.Contains(point))
                {
                    if (!SurfacesWithDragOver.Contains(surface))
                        enteringSurfaces.Add(surface);
                    else
                        surface.OnDragOver(point);
                }
                else if (SurfacesWithDragOver.Contains(surface))
                {
                    SurfacesWithDragOver.Remove(surface);
                    surface.OnDragLeave(point);
                }
            }

            foreach (IDropSurface surface in enteringSurfaces)
            {
                SurfacesWithDragOver.Add(surface);
                surface.OnDragEnter(point);
            }
        }

        public void EndDrag(Point point)
        {
            IDropSurface dropSufrace = null;
            foreach (IDropSurface surface in Surfaces)
            {
                if (surface.SurfaceRectangle.Contains(point))
                {
                    if (surface.OnDrop(point))
                    {
                        dropSufrace = surface;
                        break;
                    }
                }
            }

            foreach (IDropSurface surface in SurfacesWithDragOver)
            {
                if (surface != dropSufrace)
                {
                    surface.OnDragLeave(point);
                }
            }

            SurfacesWithDragOver.Clear();

            if (dropSufrace != null)
                _wnd.Close();

            _wnd = null;
            _pane = null;
        }

        FloatingWindowModel _wnd;
        public FloatingWindowModel FloatingWindow
        {
            get { return _wnd; }
        }


        DockablePane _pane;
        public DockablePane DockablePane
        {
            get { return _pane; }
        }
#endif
    }
}
