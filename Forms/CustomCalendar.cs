using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace neoStockMasterv2.Controls
{
    public class CustomCalendar : Control
    {
        // ── Renkler ─────────────────────────────────────────────────────────────
        private readonly Color _bgColor          = Color.FromArgb(18, 18, 30);
        private readonly Color _headerBg         = Color.FromArgb(25, 25, 45);
        private readonly Color _headerText       = Color.FromArgb(200, 200, 255);
        private readonly Color _weekdayText      = Color.FromArgb(120, 120, 180);
        private readonly Color _dayText          = Color.FromArgb(220, 220, 240);
        private readonly Color _todayRing        = Color.FromArgb(100, 120, 255);
        private readonly Color _selectedBg       = Color.FromArgb(80, 100, 240);
        private readonly Color _selectedText     = Color.White;
        private readonly Color _orderDotColor    = Color.FromArgb(255, 90, 120);     // kırmızımsı nokta
        private readonly Color _orderHighlight   = Color.FromArgb(40, 255, 130, 90); // yarı şeffaf arka plan
        private readonly Color _otherMonthText   = Color.FromArgb(60, 60, 90);
        private readonly Color _navBtnHover      = Color.FromArgb(50, 50, 80);

        // ── Durum ────────────────────────────────────────────────────────────────
        private DateTime _displayMonth;          // görüntülenen ay/yıl
        private DateTime _selectedDate;          // seçili gün
        private HashSet<DateTime> _orderDates = new HashSet<DateTime>(); // sipariş olan günler

        private bool _prevHover, _nextHover;

        // ── Sabitler ─────────────────────────────────────────────────────────────
        private const int HeaderHeight  = 52;
        private const int WeekRowHeight = 28;
        private const int CellPadding   = 4;

        // ── Olaylar ──────────────────────────────────────────────────────────────
        public event EventHandler<DateTime> DateSelected;

        // ── Özellikler ───────────────────────────────────────────────────────────
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value.Date; _displayMonth = new DateTime(value.Year, value.Month, 1); Invalidate(); }
        }

        public HashSet<DateTime> OrderDates
        {
            get => _orderDates;
            set { _orderDates = value ?? new HashSet<DateTime>(); Invalidate(); }
        }

        // ── Constructor ──────────────────────────────────────────────────────────
        public CustomCalendar()
        {
            DoubleBuffered  = true;
            ResizeRedraw    = true;
            Cursor          = Cursors.Hand;
            _selectedDate   = DateTime.Today;
            _displayMonth   = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint           |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        // ── Boyutlar ─────────────────────────────────────────────────────────────
        private RectangleF GetCellBounds(int col, int row)
        {
            float cellW = Width  / 7f;
            float cellH = (Height - HeaderHeight - WeekRowHeight) / 6f;
            float x = col * cellW;
            float y = HeaderHeight + WeekRowHeight + row * cellH;
            return new RectangleF(x + CellPadding, y + CellPadding,
                                  cellW - CellPadding * 2, cellH - CellPadding * 2);
        }

        private RectangleF PrevButtonRect =>
            new RectangleF(10, (HeaderHeight - 28) / 2f, 28, 28);

        private RectangleF NextButtonRect =>
            new RectangleF(Width - 38, (HeaderHeight - 28) / 2f, 28, 28);

        // ── Paint ─────────────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = TextRenderingHint.ClearTypeGridFit;

            DrawBackground(g);
            DrawHeader(g);
            DrawWeekdays(g);
            DrawDays(g);
        }

        private void DrawBackground(Graphics g)
        {
            using var bg = new SolidBrush(_bgColor);
            using var path = RoundedRect(new RectangleF(0, 0, Width, Height), 16);
            g.FillPath(bg, path);
        }

        private void DrawHeader(Graphics g)
        {
            // Header arka planı
            using var hBrush = new SolidBrush(_headerBg);
            using var hPath  = RoundedRectTop(new RectangleF(0, 0, Width, HeaderHeight), 16);
            g.FillPath(hBrush, hPath);

            // Başlık yazısı: Ay Yıl
            string title = _displayMonth.ToString("MMMM yyyy",
                System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
            using var titleFont  = new Font("Segoe UI", 13f, FontStyle.Bold);
            using var titleBrush = new SolidBrush(_headerText);
            var titleRect = new RectangleF(40, 0, Width - 80, HeaderHeight);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(title, titleFont, titleBrush, titleRect, sf);

            // Önceki ay butonu
            DrawNavButton(g, PrevButtonRect, "‹", _prevHover);
            DrawNavButton(g, NextButtonRect, "›", _nextHover);
        }

        private void DrawNavButton(Graphics g, RectangleF rect, string symbol, bool hover)
        {
            if (hover)
            {
                using var hBrush = new SolidBrush(_navBtnHover);
                using var p = RoundedRect(rect, 8);
                g.FillPath(hBrush, p);
            }
            using var font  = new Font("Segoe UI", 16f, FontStyle.Bold);
            using var brush = new SolidBrush(_headerText);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(symbol, font, brush, rect, sf);
        }

        private void DrawWeekdays(Graphics g)
        {
            string[] days = { "Pt", "Sa", "Ça", "Pe", "Cu", "Ct", "Pa" };
            float cellW = Width / 7f;
            using var font  = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var brush = new SolidBrush(_weekdayText);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            for (int i = 0; i < 7; i++)
            {
                var r = new RectangleF(i * cellW, HeaderHeight, cellW, WeekRowHeight);
                g.DrawString(days[i], font, brush, r, sf);
            }

            // Alt çizgi
            using var pen = new Pen(Color.FromArgb(40, 80, 80, 120), 1);
            g.DrawLine(pen, 8, HeaderHeight + WeekRowHeight - 1, Width - 8, HeaderHeight + WeekRowHeight - 1);
        }

        private void DrawDays(Graphics g)
        {
            // Ayın ilk gününü bul (Pazartesi = 0 bazlı)
            int firstDayOfWeek = ((int)_displayMonth.DayOfWeek + 6) % 7; // Mon=0
            int daysInMonth    = DateTime.DaysInMonth(_displayMonth.Year, _displayMonth.Month);

            // Önceki ayın son günleri
            DateTime prevMonth   = _displayMonth.AddMonths(-1);
            int daysInPrevMonth  = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);

            using var dayFont    = new Font("Segoe UI", 10f, FontStyle.Regular);
            using var boldFont   = new Font("Segoe UI", 10f, FontStyle.Bold);
            using var otherBrush = new SolidBrush(_otherMonthText);
            using var dayBrush   = new SolidBrush(_dayText);
            using var selBrush   = new SolidBrush(_selectedText);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            int cellIndex = 0;

            // Önceki ay günleri
            for (int d = daysInPrevMonth - firstDayOfWeek + 1; d <= daysInPrevMonth; d++, cellIndex++)
            {
                int col = cellIndex % 7;
                int row = cellIndex / 7;
                var rc  = GetCellBounds(col, row);
                g.DrawString(d.ToString(), dayFont, otherBrush, rc, sf);
            }

            // Bu ayın günleri
            for (int d = 1; d <= daysInMonth; d++, cellIndex++)
            {
                int col  = cellIndex % 7;
                int row  = cellIndex / 7;
                var rc   = GetCellBounds(col, row);
                var date = new DateTime(_displayMonth.Year, _displayMonth.Month, d);
                bool isToday    = date == DateTime.Today;
                bool isSelected = date == _selectedDate.Date;
                bool hasOrder   = _orderDates.Contains(date);

                // Sipariş vurgulama (arka plan)
                if (hasOrder && !isSelected)
                {
                    using var oBrush = new SolidBrush(_orderHighlight);
                    using var oPath  = RoundedRect(rc, 8);
                    g.FillPath(oBrush, oPath);
                }

                // Seçili gün
                if (isSelected)
                {
                    using var sBrush = new LinearGradientBrush(
                        new PointF(rc.Left, rc.Top), new PointF(rc.Right, rc.Bottom),
                        _selectedBg, Color.FromArgb(120, 160, 255));
                    using var sPath = RoundedRect(rc, 10);
                    g.FillPath(sBrush, sPath);
                    g.DrawString(d.ToString(), boldFont, selBrush, rc, sf);
                }
                else
                {
                    g.DrawString(d.ToString(), isToday ? boldFont : dayFont, dayBrush, rc, sf);
                }

                // Bugün halkası
                if (isToday && !isSelected)
                {
                    using var todayPen = new Pen(_todayRing, 1.5f);
                    using var tPath    = RoundedRect(rc, 10);
                    g.DrawPath(todayPen, tPath);
                }

                // Sipariş noktası
                if (hasOrder)
                {
                    float dotR  = 3.5f;
                    float dotX  = rc.Left + rc.Width / 2f - dotR;
                    float dotY  = rc.Bottom - dotR * 2 - 2;
                    using var dotBrush = new SolidBrush(isSelected ? Color.White : _orderDotColor);
                    g.FillEllipse(dotBrush, dotX, dotY, dotR * 2, dotR * 2);
                }
            }

            // Sonraki ayın günleri
            int remaining = 42 - cellIndex;
            for (int d = 1; d <= remaining; d++, cellIndex++)
            {
                int col = cellIndex % 7;
                int row = cellIndex / 7;
                var rc  = GetCellBounds(col, row);
                g.DrawString(d.ToString(), dayFont, otherBrush, rc, sf);
            }
        }

        // ── Mouse Olayları ────────────────────────────────────────────────────────
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool pHover = PrevButtonRect.Contains(e.Location);
            bool nHover = NextButtonRect.Contains(e.Location);
            if (pHover != _prevHover || nHover != _nextHover)
            {
                _prevHover = pHover;
                _nextHover = nHover;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _prevHover = _nextHover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (PrevButtonRect.Contains(e.Location))
            {
                _displayMonth = _displayMonth.AddMonths(-1);
                Invalidate();
                return;
            }
            if (NextButtonRect.Contains(e.Location))
            {
                _displayMonth = _displayMonth.AddMonths(1);
                Invalidate();
                return;
            }

            // Gün hücresi tıklaması
            int firstDayOfWeek = ((int)_displayMonth.DayOfWeek + 6) % 7;
            int daysInMonth    = DateTime.DaysInMonth(_displayMonth.Year, _displayMonth.Month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                int cellIndex = firstDayOfWeek + d - 1;
                int col = cellIndex % 7;
                int row = cellIndex / 7;
                var rc  = GetCellBounds(col, row);
                if (rc.Contains(e.Location))
                {
                    _selectedDate = new DateTime(_displayMonth.Year, _displayMonth.Month, d);
                    Invalidate();
                    DateSelected?.Invoke(this, _selectedDate);
                    break;
                }
            }
            base.OnMouseClick(e);
        }

        // ── Yardımcı: Yuvarlatılmış köşe yolu ────────────────────────────────────
        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(r.Left,          r.Top,           d, d, 180, 90);
            path.AddArc(r.Right - d,     r.Top,           d, d, 270, 90);
            path.AddArc(r.Right - d,     r.Bottom - d,    d, d,   0, 90);
            path.AddArc(r.Left,          r.Bottom - d,    d, d,  90, 90);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath RoundedRectTop(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(r.Left,      r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddLine(r.Right, r.Top + radius, r.Right, r.Bottom);
            path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
            path.AddLine(r.Left, r.Bottom, r.Left, r.Top + radius);
            path.CloseFigure();
            return path;
        }
    }
}
