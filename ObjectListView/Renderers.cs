/*
 * Renderers - A collection of useful renderers that are used to owner draw a cell in an ObjectListView
 *
 * Author: Phillip Piper
 * Date: 27/09/2008 9:15 AM
 *
 * Copyright (C) 2006-2010 Phillip Piper
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HAC2Beta2
{
    public interface IRenderer
    {
        /// <summary>
        /// Render the whole item within an ObjectListView.
        /// </summary>
        /// <param name="e">The event</param>
        /// <param name="g">A Graphics for rendering</param>
        /// <param name="itemBounds">The bounds of the item</param>
        /// <param name="rowObject">The model object to be drawn</param>
        /// <returns>Return true to indicate that the event was handled and no further processing is needed.</returns>
        bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, Object rowObject);

        /// <summary>
        /// Render one cell within an ObjectListView when it is in Details mode.
        /// </summary>
        /// <param name="e">The event</param>
        /// <param name="g">A Graphics for rendering</param>
        /// <param name="cellBounds">The bounds of the cell</param>
        /// <param name="rowObject">The model object to be drawn</param>
        /// <returns>Return true to indicate that the event was handled and no further processing is needed.</returns>
        bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, Object rowObject);

        /// <summary>
        /// What is under the given point?
        /// </summary>
        /// <param name="hti"></param>
        /// <param name="x">x co-ordinate</param>
        /// <param name="y">y co-ordinate</param>
        /// <remarks>This method should only alter HitTestLocation and/or UserData.</remarks>
        void HitTest(OlvListViewHitTestInfo hti, int x, int y);

        /// <summary>
        /// When the value in the given cell is to be edited, where should the edit rectangle be placed?
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellBounds"></param>
        /// <param name="item"></param>
        /// <param name="subItemIndex"></param>
        /// <returns></returns>
        Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex);
    }

    /// <summary>
    /// An AbstractRenderer is a do-nothing implementation of the IRenderer interface.
    /// </summary>
    [Browsable(true),
    ToolboxItem(false)]
    public class AbstractRenderer : Component, IRenderer
    {
        #region IRenderer Members

        public virtual bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object rowObject) {
            return true;
        }

        public virtual bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
            return false;
        }

        public virtual void HitTest(OlvListViewHitTestInfo hti, int x, int y) {
        }

        public virtual Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex) {
            return cellBounds;
        }

        #endregion
    }

    /// <summary>
    /// This class provides compatibility for v1 RendererDelegates
    /// </summary>
    [ToolboxItem(false)]
    internal class Version1Renderer : AbstractRenderer
    {
        public Version1Renderer(RenderDelegate renderDelegate) {
            this.RenderDelegate = renderDelegate;
        }
        /// <summary>
        /// The renderer delegate that this renderer wraps
        /// </summary>
        public RenderDelegate RenderDelegate;

        #region IRenderer Members

        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
            if (this.RenderDelegate == null)
                return base.RenderSubItem(e, g, cellBounds, rowObject);
            else
                return this.RenderDelegate(e, g, cellBounds, rowObject);
        }

        #endregion
    }

    /// <summary>
    /// A BaseRenderer provides useful base level functionality for any custom renderer.
    /// </summary>
    /// <remarks>
    /// <para>Subclasses will normally override the Render or OptionalRender method, and use the other
    /// methods as helper functions.</para>
    /// </remarks>
    [Browsable(true),
     ToolboxItem(true)]
    public class BaseRenderer : AbstractRenderer
    {
        #region Configuration Properties

        /// <summary>
        /// Can the renderer wrap lines that do not fit completely within the cell?
        /// </summary>
        /// <remarks>Wrapping text doesn't work with the GDI renderer.</remarks>
        [Category("Appearance"),
         Description("Can the renderer wrap text that does not fit completely within the cell"),
         DefaultValue(false)]
        public bool CanWrap {
            get { return canWrap; }
            set { canWrap = value; }
        }
        private bool canWrap;

        /// <summary>
        /// Gets or sets the image list from which keyed images will be fetched
        /// </summary>
        [Category("Appearance"),
         Description("The image list from which keyed images will be fetched for drawing."),
         DefaultValue(null)]
        public ImageList ImageList {
            get { return imageList; }
            set { imageList = value; }
        }
        private ImageList imageList;

        /// <summary>
        /// When rendering multiple images, how many pixels should be between each image?
        /// </summary>
        [Category("Appearance"),
         Description("When rendering multiple images, how many pixels should be between each image?"),
         DefaultValue(1)]
        public int Spacing {
            get { return spacing; }
            set { spacing = value; }
        }
        private int spacing = 1;

        /// <summary>
        /// Should text be rendered using GDI routines? This makes the text look more
        /// like a native List view control.
        /// </summary>
        [Category("Appearance"),
         Description("Should text be rendered using GDI routines?"),
         DefaultValue(true)]
        public bool UseGdiTextRendering {
            get {
                if (this.IsPrinting)
                    return false; // Can't use GDI routines on a GDI+ printer context
                else
                    return useGdiTextRendering;
            }
            set { useGdiTextRendering = value; }
        }
        private bool useGdiTextRendering = true;

        #endregion

        #region State Properties

        /// <summary>
        /// Get or set the aspect of the model object that this renderer should draw
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object Aspect {
            get {
                if (aspect == null)
                    aspect = column.GetValue(this.rowObject);
                return aspect;
            }
            set { aspect = value; }
        }
        private Object aspect;

        /// <summary>
        /// What are the bounds of the cell that is being drawn?
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        private Rectangle bounds;

        /// <summary>
        /// Get or set the OLVColumn that this renderer will draw
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVColumn Column {
            get { return column; }
            set { column = value; }
        }
        private OLVColumn column;

        /// <summary>
        /// Get/set the event that caused this renderer to be called
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DrawListViewItemEventArgs DrawItemEvent {
            get { return drawItemEventArgs; }
            set { drawItemEventArgs = value; }
        }
        private DrawListViewItemEventArgs drawItemEventArgs;

        /// <summary>
        /// Get/set the event that caused this renderer to be called
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DrawListViewSubItemEventArgs Event {
            get { return eventArgs; }
            set { eventArgs = value; }
        }
        private DrawListViewSubItemEventArgs eventArgs;

        /// <summary>
        /// Return the font to be used for text in this cell
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font Font {
            get {
                if (this.font != null || this.ListItem == null)
                    return this.font;

                if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
                    return this.ListItem.Font;
                else
                    return this.SubItem.Font;
            }
            set {
                this.font = value;
            }
        }
        private Font font;

        /// <summary>
        /// Gets the image list from which keyed images will be fetched
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageList ImageListOrDefault {
            get { return this.ImageList ?? this.ListView.BaseSmallImageList; }
        }

        /// <summary>
        /// Should this renderer fill in the background before drawing?
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDrawBackground {
            get { return !this.IsPrinting; }
        }

        /// <summary>
        /// Cache whether or not our item is selected
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsItemSelected {
            get { return isItemSelected; }
            set { isItemSelected = value; }
        }
        private bool isItemSelected;

        /// <summary>
        /// Is this renderer being used on a printer context?
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPrinting {
            get { return isPrinting; }
            set { isPrinting = value; }
        }
        private bool isPrinting;

        /// <summary>
        /// Get or set the listitem that this renderer will be drawing
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVListItem ListItem {
            get { return listItem; }
            set { listItem = value; }
        }
        private OLVListItem listItem;

        /// <summary>
        /// Get/set the listview for which the drawing is to be done
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObjectListView ListView {
            get { return objectListView; }
            set { objectListView = value; }
        }
        private ObjectListView objectListView;

        /// <summary>
        /// Get the specialized OLVSubItem that this renderer is drawing
        /// </summary>
        /// <remarks>This returns null for column 0.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVListSubItem OLVSubItem {
            get { return listSubItem as OLVListSubItem; }
        }

        /// <summary>
        /// Get or set the model object that this renderer should draw
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object RowObject {
            get { return rowObject; }
            set { rowObject = value; }
        }
        private Object rowObject;

        /// <summary>
        /// Get or set the list subitem that this renderer will be drawing
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVListSubItem SubItem {
            get { return listSubItem; }
            set { listSubItem = value; }
        }
        private OLVListSubItem listSubItem;

        /// <summary>
        /// The brush that will be used to paint the text
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush TextBrush {
            get {
                if (textBrush == null)
                    return new SolidBrush(this.GetForegroundColor());
                else
                    return this.textBrush;
            }
            set { textBrush = value; }
        }
        private Brush textBrush;

        private void ClearState() {
            this.Event = null;
            this.DrawItemEvent = null;
            this.Aspect = null;
            this.Font = null;
            this.TextBrush = null;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Align the second rectangle with the first rectangle,
        /// according to the alignment of the column
        /// </summary>
        /// <param name="outer">The cell's bounds</param>
        /// <param name="inner">The rectangle to be aligned within the bounds</param>
        /// <returns>An aligned rectangle</returns>
        protected virtual Rectangle AlignRectangle(Rectangle outer, Rectangle inner) {
            Rectangle r = new Rectangle(outer.Location, inner.Size);

            // Centre horizontally depending on the column alignment
            if (inner.Width < outer.Width) {
                switch (this.Column.TextAlign) {
                    case HorizontalAlignment.Left:
                        r.X = outer.Left;
                        break;
                    case HorizontalAlignment.Center:
                        r.X = outer.Left + ((outer.Width - inner.Width) / 2);
                        break;
                    case HorizontalAlignment.Right:
                        r.X = outer.Right - inner.Width - 1;
                        break;
                }
            }
            // Centre vertically too
            if (inner.Height < outer.Height)
                r.Y = outer.Top + ((outer.Height - inner.Height) / 2);

            return r;
        }

        /// <summary>
        /// Calculate the space that our rendering will occupy and then align that space
        /// with the given rectangle, according to the Column alignment
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected virtual Rectangle CalculateAlignedRectangle(Graphics g, Rectangle r) {
            if (this.Column.TextAlign == HorizontalAlignment.Left)
                return r;

            int width = this.CalculateCheckBoxWidth(g);
            width += this.CalculateImageWidth(g, this.GetImageSelector());
            width += this.CalculateTextWidth(g, this.GetText());

            // If the combined width is greater than the whole cell, 
            // we just use the cell itself
            if (width >= r.Width)
                return r;

            return this.AlignRectangle(r, new Rectangle(0, 0, width, r.Height));
        }

        /// <summary>
        /// How much space will the check box for this cell occupy?
        /// </summary>
        /// <remarks>Only column 0 can have check boxes. Sub item checkboxes are
        /// treated as images</remarks>
        /// <param name="g"></param>
        /// <returns></returns>
        protected virtual int CalculateCheckBoxWidth(Graphics g) {
            if (this.ListView.CheckBoxes && this.Column.Index == 0)
                return CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width + 6;
            else
                return 0;
        }

        /// <summary>
        /// How much horizontal space will the image of this cell occupy?
        /// </summary>
        /// <param name="g"></param>
        /// <param name="imageSelector"></param>
        /// <returns></returns>
        protected virtual int CalculateImageWidth(Graphics g, object imageSelector) {
            if (imageSelector == null || imageSelector == System.DBNull.Value)
                return 0;

            // Draw from the image list (most common case)
            ImageList il = this.ImageListOrDefault;
            if (il != null) {
                int selectorAsInt = -1;

                if (imageSelector is Int32)
                    selectorAsInt = (Int32)imageSelector;
                else {
                    String selectorAsString = imageSelector as String;
                    if (selectorAsString != null)
                        selectorAsInt = il.Images.IndexOfKey(selectorAsString);
                }
                if (selectorAsInt >= 0)
                    return il.ImageSize.Width;
            }

            // Is the selector actually an image?
            Image image = imageSelector as Image;
            if (image != null)
                return image.Width;

            return 0;
        }

        /// <summary>
        /// How much horizontal space will the text of this cell occupy?
        /// </summary>
        /// <param name="g"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        protected virtual int CalculateTextWidth(Graphics g, string txt) {
            if (String.IsNullOrEmpty(txt))
                return 0;

            if (this.UseGdiTextRendering) {
                Size proposedSize = new Size(int.MaxValue, int.MaxValue);
                return TextRenderer.MeasureText(g, txt, this.Font, proposedSize, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix).Width;
            } else {
                using (StringFormat fmt = new StringFormat()) {
                    fmt.Trimming = StringTrimming.EllipsisCharacter;
                    return 1 + (int)g.MeasureString(txt, this.Font, int.MaxValue, fmt).Width;
                }
            }
        }

        /// <summary>
        /// Return the Color that is the background color for this item's cell
        /// </summary>
        /// <returns>The background color of the subitem</returns>
        protected virtual Color GetBackgroundColor() {
            if (!this.ListView.Enabled)
                return SystemColors.Control;
            if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && this.ListView.FullRowSelect) {
                if (this.ListView.Focused)
                    return this.ListView.HighlightBackgroundColorOrDefault;
                else
                    if (!this.ListView.HideSelection)
                        return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
            }
            if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
                return this.ListItem.BackColor;
            else
                return this.SubItem.BackColor;
        }

        /// <summary>
        /// Return the color to be used for text in this cell
        /// </summary>
        /// <returns>The text color of the subitem</returns>
        protected virtual Color GetForegroundColor() {
            if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && 
                (this.Column.Index == 0 || this.ListView.FullRowSelect)) {
                if (this.ListView.Focused)
                    return this.ListView.HighlightForegroundColorOrDefault;
                else
                    if (!this.ListView.HideSelection)
                        return this.ListView.UnfocusedHighlightForegroundColorOrDefault;
            }
            if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
                return this.ListItem.ForeColor;
            else
                return this.SubItem.ForeColor;
        }

        /// <summary>
        /// Return the image that should be drawn against this subitem
        /// </summary>
        /// <returns>An Image or null if no image should be drawn.</returns>
        protected virtual Image GetImage() {
            return this.GetImage(this.GetImageSelector());
        }

        /// <summary>
        /// Return the actual image that should be drawn when keyed by the given image selector.
        /// An image selector can be: <list>
        /// <item>an int, giving the index into the image list</item>
        /// <item>a string, giving the image key into the image list</item>
        /// <item>an Image, being the image itself</item>
        /// </list>
        /// </summary>
        /// <param name="imageSelector">The value that indicates the image to be used</param>
        /// <returns>An Image or null</returns>
        protected virtual Image GetImage(Object imageSelector) {
            if (imageSelector == null || imageSelector == System.DBNull.Value)
                return null;

            ImageList il = this.ImageListOrDefault;
            if (il != null) {
                if (imageSelector is Int32) {
                    Int32 index = (Int32)imageSelector;
                    if (index < 0 || index >= il.Images.Count)
                        return null;
                    else
                        return il.Images[index];
                }

                String str = imageSelector as String;
                if (str != null) {
                    if (il.Images.ContainsKey(str))
                        return il.Images[str];
                    else
                        return null;
                }
            }

            return imageSelector as Image;
        }

        /// <summary>
        /// </summary>
        protected virtual Object GetImageSelector() {
            if (this.Column.Index == 0)
                return this.ListItem.ImageSelector;
            else
                return this.OLVSubItem.ImageSelector;
        }

        /// <summary>
        /// Return the string that should be drawn within this
        /// </summary>
        /// <returns></returns>
        protected virtual string GetText() {
            if (this.SubItem == null)
                return this.ListItem.Text;
            else
                return this.SubItem.Text;
        }

        /// <summary>
        /// Return the Color that is the background color for this item's text
        /// </summary>
        /// <returns>The background color of the subitem's text</returns>
        protected virtual Color GetTextBackgroundColor() {
            //TODO: Refactor with GetBackgroundColor() - they are almost identical
            if (this.IsItemSelected && !this.ListView.UseTranslucentSelection 
                && (this.Column.Index == 0 || this.ListView.FullRowSelect)) {
                if (this.ListView.Focused)
                    return this.ListView.HighlightBackgroundColorOrDefault;
                else
                    if (!this.ListView.HideSelection)
                        return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
            }

            if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
                return this.ListItem.BackColor;
            else
                return this.SubItem.BackColor;
        }

        #endregion

        #region IRenderer members

        /// <summary>
        /// Render the whole item in a non-details view.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="g"></param>
        /// <param name="itemBounds"></param>
        /// <param name="rowObject"></param>
        /// <returns></returns>
        public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object rowObject) {
            this.ClearState();

            this.DrawItemEvent = e;
            this.ListItem = (OLVListItem)e.Item;
            this.SubItem = null;
            this.ListView = (ObjectListView)this.ListItem.ListView;
            this.Column = this.ListView.GetColumn(0);
            this.RowObject = rowObject;
            this.Bounds = itemBounds;
            this.IsItemSelected = this.ListItem.Selected;

            return this.OptionalRender(g, itemBounds);
        }

        /// <summary>
        /// Render one cell
        /// </summary>
        /// <param name="e"></param>
        /// <param name="g"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowObject"></param>
        /// <returns></returns>
        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
            this.ClearState();

            this.Event = e;
            this.ListItem = (OLVListItem)e.Item;
            this.SubItem = (OLVListSubItem)e.SubItem;
            this.ListView = (ObjectListView)this.ListItem.ListView;
            this.Column = (OLVColumn)e.Header;
            this.RowObject = rowObject;
            this.Bounds = cellBounds;
            this.IsItemSelected = this.ListItem.Selected;

            return this.OptionalRender(g, cellBounds);
        }

        /// <summary>
        /// Calculate which part of this cell was hit
        /// </summary>
        /// <param name="hti"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void HitTest(OlvListViewHitTestInfo hti, int x, int y) {
            this.ClearState();

            this.ListView = hti.ListView;
            this.ListItem = hti.Item;
            this.SubItem = hti.SubItem;
            this.Column = hti.Column;
            this.RowObject = hti.RowObject;
            this.IsItemSelected = this.ListItem.Selected;
            if (this.SubItem == null)
                this.Bounds = this.ListItem.Bounds;
            else
                this.Bounds = this.ListItem.GetSubItemBounds(this.Column.Index);
                //this.Bounds = this.ListView.CalculateCellBounds(this.ListItem, this.Column.Index);

            using (Graphics g = this.ListView.CreateGraphics()) {
                this.HandleHitTest(g, hti, x, y);
            }
        }

        public override Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex) {
            this.ClearState();

            this.ListView = (ObjectListView)item.ListView;
            this.ListItem = item;
            this.SubItem = item.GetSubItem(subItemIndex);
            this.Column = this.ListView.GetColumn(subItemIndex);
            this.RowObject = item.RowObject;
            this.IsItemSelected = this.ListItem.Selected;
            this.Bounds = cellBounds;

            return this.HandleGetEditRectangle(g, cellBounds, item, subItemIndex);
        }

        #endregion

        #region IRenderer implementation

        // Subclasses will probably want to override these methods rather than the IRenderer
        // interface methods.

        /// <summary>
        /// Draw our data into the given rectangle using the given graphics context.
        /// </summary>
        /// <remarks>
        /// <para>Subclasses should override this method.</para></remarks>
        /// <param name="g">The graphics context that should be used for drawing</param>
        /// <param name="r">The bounds of the subitem cell</param>
        /// <returns>Returns whether the renderering has already taken place.
        /// If this returns false, the default processing will take over.
        /// </returns>
        public virtual bool OptionalRender(Graphics g, Rectangle r) {
            if (this.ListView.View == View.Details) {
                this.Render(g, r);
                return true;
            } else
                return false;
        }

        /// <summary>
        /// Draw our data into the given rectangle using the given graphics context.
        /// </summary>
        /// <remarks>
        /// <para>Subclasses should override this method if they never want
        /// to fall back on the default processing</para></remarks>
        /// <param name="g">The graphics context that should be used for drawing</param>
        /// <param name="r">The bounds of the subitem cell</param>
        public virtual void Render(Graphics g, Rectangle r) {
            this.StandardRender(g, r);
        }

        /// <summary>
        /// Do the actual work of hit testing. Subclasses should override this rather than HitTest()
        /// </summary>
        /// <param name="g"></param>
        /// <param name="hti"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected virtual void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y) {
            Rectangle r = this.CalculateAlignedRectangle(g, this.Bounds);
            this.StandardHitTest(g, hti, r, x, y);
        }

        /// <summary>
        /// Handle a HitTest request after all state information has been initialized
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellBounds"></param>
        /// <param name="item"></param>
        /// <param name="subItemIndex"></param>
        /// <returns></returns>
        protected virtual Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex) {
            // MAINTAINER NOTE: This type testing is wrong (design-wise). The base class should return cell bounds,
            // and a more specialized class should return StandardGetEditRectangle(). But BaseRenderer is used directly
            // to draw most normal cells, as well as being directly subclassed for user implemented renderers. And this
            // method needs to return different bounds in each of those cases. We should have a StandardRenderer and make
            // BaseRenderer into an ABC -- but that would break too much existing code. And so we have this hack :(

            // If we are a standard renderer, return the position of the text, otherwise, use the whole cell.
            if (this.GetType() == typeof(BaseRenderer))
                return this.StandardGetEditRectangle(g, cellBounds);
            else
                return cellBounds;
        }

        #endregion

        #region Standard IRenderer implementations

        /// <summary>
        /// Draw the standard "[checkbox] [image] [text]" cell after the state properties have been initialized.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected void StandardRender(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            // Adjust the first columns rectangle to match the padding used by the native mode of the ListView
            if (this.Column.Index == 0) {
                r.X += 3;
                r.Width -= 1;
            }
            this.DrawAlignedImageAndText(g, r);
        }

        /// <summary>
        /// Perform normal hit testing relative to the given bounds
        /// </summary>
        /// <param name="g"></param>
        /// <param name="hti"></param>
        /// <param name="bounds"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void StandardHitTest(Graphics g, OlvListViewHitTestInfo hti, Rectangle bounds, int x, int y) {
            Rectangle r = bounds;

            // Did they hit a check box?
            int width = this.CalculateCheckBoxWidth(g);
            Rectangle r2 = r;
            r2.Width = width;
            if (r2.Contains(x, y)) {
                hti.HitTestLocation = HitTestLocation.CheckBox;
                return;
            }

            // Did they hit the image? If they hit the image of a 
            // non-primary column that has a checkbox, it counts as a 
            // checkbox hit
            r.X += width;
            r.Width -= width;
            width = this.CalculateImageWidth(g, this.GetImageSelector());
            r2 = r;
            r2.Width = width;
            if (r2.Contains(x, y)) {
                if (this.Column.Index > 0 && this.Column.CheckBoxes)
                    hti.HitTestLocation = HitTestLocation.CheckBox;
                else
                    hti.HitTestLocation = HitTestLocation.Image;
                return;
            }

            // Did they hit the text?
            r.X += width;
            r.Width -= width;
            width = this.CalculateTextWidth(g, this.GetText());
            r2 = r;
            r2.Width = width;
            if (r2.Contains(x, y)) {
                hti.HitTestLocation = HitTestLocation.Text;
                return;
            }

            hti.HitTestLocation = HitTestLocation.InCell;
        }

        /// <summary>
        /// This method calculates the bounds of the text within a standard layout
        /// (i.e. optional checkbox, optional image, text)
        /// </summary>
        /// <remarks>This method only works correctly if the state of the renderer
        /// has been fully initialized (see BaseRenderer.GetEditRectangle)</remarks>
        /// <param name="g"></param>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        protected Rectangle StandardGetEditRectangle(Graphics g, Rectangle cellBounds) {
            Rectangle r = this.CalculateAlignedRectangle(g, cellBounds);

            int width = this.CalculateCheckBoxWidth(g);
            width += this.CalculateImageWidth(g, this.GetImageSelector());

            // Indent the primary column by the required amount
            if (this.Column.Index == 0 && this.ListItem.IndentCount > 0) {
                int indentWidth = this.ListView.SmallImageSize.Width;
                width += (indentWidth * this.ListItem.IndentCount);
            }

            // If there wasn't either a check box or an image, just use the whole cell
            if (width == 0)
                return cellBounds;

            // Take the check box and the image out of the rectangle, but ensure that
            // there is minimum width to the editor
            r.X += width;
            r.Width = Math.Max(r.Width - width, 40);

            return r;
        }

        #endregion

        #region Drawing routines

        /// <summary>
        /// Draw the given image aligned horizontally within the column.
        /// </summary>
        /// <remarks>
        /// Over tall images are scaled to fit. Over-wide images are
        /// truncated. This is by design!
        /// </remarks>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        /// <param name="image">The image to be drawn</param>
        protected virtual void DrawAlignedImage(Graphics g, Rectangle r, Image image) {
            if (image == null)
                return;

            // By default, the image goes in the top left of the rectangle
            Rectangle imageBounds = new Rectangle(r.Location, image.Size);

            // If the image is too tall to be drawn in the space provided, proportionally scale it down.
            // Too wide images are not scaled.
            if (image.Height > r.Height) {
                float scaleRatio = (float)r.Height / (float)image.Height;
                imageBounds.Width = (int)((float)image.Width * scaleRatio);
                imageBounds.Height = r.Height - 1;
            }

            // Align and draw our (possibly scaled) image
            g.DrawImage(image, this.AlignRectangle(r, imageBounds));
        }

        /// <summary>
        /// Draw our subitems image and text
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        protected virtual void DrawAlignedImageAndText(Graphics g, Rectangle r) {
            this.DrawImageAndText(g, this.CalculateAlignedRectangle(g, r));
        }

        /// <summary>
        /// Fill in the background of this cell
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        protected virtual void DrawBackground(Graphics g, Rectangle r) {
            if (!this.IsDrawBackground)
                return;

            Color backgroundColor = this.GetBackgroundColor();

            using (Brush brush = new SolidBrush(backgroundColor)) {
                g.FillRectangle(brush, r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
            }
        }

        /// <summary>
        /// Draw the check box of this row
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        protected virtual int DrawCheckBox(Graphics g, Rectangle r) {
            int imageIndex = this.ListItem.StateImageIndex;

            if (this.IsPrinting) {
                if (this.ListView.StateImageList == null || imageIndex < 0)
                    return 0;
                else
                    return this.DrawImage(g, r, this.ListView.StateImageList.Images[imageIndex]) + 4;
            }

            CheckBoxState boxState = CheckBoxState.UncheckedNormal;
            int switchValue = (imageIndex << 4); // + (this.IsItemHot ? 1 : 0);
            switch (switchValue) {
                case 0x00:
                    boxState = CheckBoxState.UncheckedNormal;
                    break;
                case 0x01:
                    boxState = CheckBoxState.UncheckedHot;
                    break;
                case 0x10:
                    boxState = CheckBoxState.CheckedNormal;
                    break;
                case 0x11:
                    boxState = CheckBoxState.CheckedHot;
                    break;
                case 0x20:
                    boxState = CheckBoxState.MixedNormal;
                    break;
                case 0x21:
                    boxState = CheckBoxState.MixedHot;
                    break;
            }

            // The odd constants are to match checkbox placement in native mode (on XP at least)
            CheckBoxRenderer.DrawCheckBox(g, new Point(r.X + 3, r.Y + (r.Height / 2) - 6), boxState);
            return CheckBoxRenderer.GetGlyphSize(g, boxState).Width + 6;
        }

        /// <summary>
        /// Draw the given text and optional image in the "normal" fashion
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        /// <param name="txt">The string to be drawn</param>
        /// <param name="image">The optional image to be drawn</param>
        protected virtual int DrawImage(Graphics g, Rectangle r, Object imageSelector) {
            if (imageSelector == null || imageSelector == System.DBNull.Value)
                return 0;

            // Draw from the image list (most common case)
            ImageList il = this.ListView.BaseSmallImageList;
            if (il != null) {
                int selectorAsInt = -1;

                if (imageSelector is Int32)
                    selectorAsInt = (Int32)imageSelector;
                else {
                    String selectorAsString = imageSelector as String;
                    if (selectorAsString != null)
                        selectorAsInt = il.Images.IndexOfKey(selectorAsString);
                }
                if (selectorAsInt >= 0) {
                    if (this.IsPrinting) {
                        // For some reason, printing from an image list doesn't work onto a printer context
                        // So get the image from the list and fall through to the "print an image" case
                        imageSelector = il.Images[selectorAsInt];
                    } else {
                        // If we are not printing, it's probable that the given Graphics object is double buffered using a BufferedGraphics object.
                        // But the ImageList.Draw method doesn't honor the Translation matrix that's probably in effect on the buffered
                        // graphics. So we have to calculate our drawing rectangle, relative to the cells natural boundaries.
                        // This effectively simulates the Translation matrix.
                        Rectangle r2 = new Rectangle(r.X - this.Bounds.X, r.Y - this.Bounds.Y, r.Width, r.Height);
                        il.Draw(g, r2.Location, selectorAsInt);

                        // Use this call instead of the above if you want to images to appear blended when selected
                        //NativeMethods.DrawImageList(g, il, selectorAsInt, r.X, r.Y, this.IsItemSelected);
                        return il.ImageSize.Width;
                    }
                }
            }

            // Is the selector actually an image?
            Image image = imageSelector as Image;
            if (image != null) {
                int top = r.Y;
                if (image.Size.Height < r.Height)
                    top += ((r.Height - image.Size.Height) / 2);

                g.DrawImageUnscaled(image, r.X, top);
                return image.Width;
            }

            return 0;
        }

        /// <summary>
        /// Draw our subitems image and text
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        protected virtual void DrawImageAndText(Graphics g, Rectangle r) {
            int offset = 0;
            if (this.ListView.CheckBoxes && this.Column.Index == 0) {
                offset = this.DrawCheckBox(g, r);
                r.X += offset;
                r.Width -= offset;
            }

            offset = this.DrawImage(g, r, this.GetImageSelector());
            r.X += offset;
            r.Width -= offset;

            this.DrawText(g, r, this.GetText());
        }

        /// <summary>
        /// Draw the given collection of image selectors
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="imageSelectors"></param>
        protected virtual void DrawImages(Graphics g, Rectangle r, ICollection imageSelectors) {
            // Collect the non-null images
            List<Image> images = new List<Image>();
            foreach (Object selector in imageSelectors) {
                Image image = this.GetImage(selector);
                if (image != null)
                    images.Add(image);
            }

            // Figure out how much space they will occupy
            int width = 0;
            int height = 0;
            foreach (Image image in images) {
                width += (image.Width + this.Spacing);
                height = Math.Max(height, image.Height);
            }

            // Align the collection of images within the cell
            Rectangle r2 = this.AlignRectangle(r, new Rectangle(0, 0, width, height));

            // Finally, draw all the images in their correct location
            Point pt = r2.Location;
            foreach (Image image in images) {
                g.DrawImage(image, pt);
                pt.X += (image.Width + this.Spacing);
            }
        }

        /// <summary>
        /// Draw the given text and optional image in the "normal" fashion
        /// </summary>
        /// <param name="g">Graphics context to use for drawing</param>
        /// <param name="r">Bounds of the cell</param>
        /// <param name="txt">The string to be drawn</param>
        protected virtual void DrawText(Graphics g, Rectangle r, String txt) {
            if (String.IsNullOrEmpty(txt))
                return;

            if (this.UseGdiTextRendering)
                this.DrawTextGdi(g, r, txt);
            else
                this.DrawTextGdiPlus(g, r, txt);
        }

        /// <summary>
        /// Print the given text in the given rectangle using only GDI routines
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="txt"></param>
        /// <remarks>
        /// The native list control uses GDI routines to do its drawing, so using them
        /// here makes the owner drawn mode looks more natural.
        /// <para>This method doesn't honour the CanWrap setting on the renderer. All
        /// text is single line</para>
        /// </remarks>
        protected virtual void DrawTextGdi(Graphics g, Rectangle r, String txt) {
            Color backColor = Color.Transparent;
            if (this.IsDrawBackground && this.IsItemSelected && this.Column.Index == 0 && !this.ListView.FullRowSelect)
                backColor = this.GetTextBackgroundColor();

            TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix |
                TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsTranslateTransform;
            
            // BUG: Setting or not setting SingleLine doesn't make any difference -- it is always single line.
            if (!this.CanWrap)
                flags |= TextFormatFlags.SingleLine;
            TextRenderer.DrawText(g, txt, this.Font, r, this.GetForegroundColor(), backColor, flags);
        }

        protected virtual StringFormat StringFormatForGdiPlus {
            get {
                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Center;
                fmt.Trimming = StringTrimming.EllipsisCharacter;
                fmt.Alignment = this.Column.TextStringAlign;
                if (!this.CanWrap)
                    fmt.FormatFlags = StringFormatFlags.NoWrap;
                return fmt;
            }
        }

        /// <summary>
        /// Print the given text in the given rectangle using normal GDI+ .NET methods
        /// </summary>
        /// <remarks>Printing to a printer dc has to be done using this method.</remarks>
        protected virtual void DrawTextGdiPlus(Graphics g, Rectangle r, String txt) {
            using (StringFormat fmt = this.StringFormatForGdiPlus) {
                // Draw the background of the text as selected, if it's the primary column
                // and it's selected and it's not in FullRowSelect mode.
                Font f = this.Font;
                if (this.IsDrawBackground && this.IsItemSelected && this.Column.Index == 0 && !this.ListView.FullRowSelect) {
                    SizeF size = g.MeasureString(txt, f, r.Width, fmt);
                    Rectangle r2 = r;
                    r2.Width = (int)size.Width + 1;
                    using (Brush brush = new SolidBrush(this.ListView.HighlightBackgroundColorOrDefault)) {
                        g.FillRectangle(brush, r2);
                    }
                }
                RectangleF rf = r;
                g.DrawString(txt, f, this.TextBrush, rf, fmt);
            }

            // We should put a focus rectange around the column 0 text if it's selected --
            // but we don't because:
            // - I really dislike this UI convention
            // - we are using buffered graphics, so the DrawFocusRecatangle method of the event doesn't work

            //if (this.Column.Index == 0) {
            //    Size size = TextRenderer.MeasureText(this.SubItem.Text, this.ListView.ListFont);
            //    if (r.Width > size.Width)
            //        r.Width = size.Width;
            //    this.Event.DrawFocusRectangle(r);
            //}
        }

        #endregion
    }

    /// <summary>
    /// This renderer highlights substrings that match a given text string. 
    /// </summary>
    public class HighlightTextRenderer : BaseRenderer
    {
        #region Life and death

        public HighlightTextRenderer() {
            this.StringComparison = StringComparison.CurrentCultureIgnoreCase;
            this.FramePen = Pens.DarkBlue;
            this.FillBrush = new SolidBrush(Color.FromArgb(96, Color.CornflowerBlue));
        }

        public HighlightTextRenderer(string text) : this() {
            this.TextToHighlight = text;
        }

        #endregion

        #region Configuration properties

        /// <summary>
        /// Gets or set the text that will be highlighted
        /// </summary>
        public string TextToHighlight {
            get { return textToHighlight; }
            set { textToHighlight = value; }
        }
        private string textToHighlight;

        /// <summary>
        /// Gets or sets the manner in which substring will be compared.
        /// </summary>
        /// <remarks>
        /// Use this to control if substring matches are case sensitive or insensitive.</remarks>
        public StringComparison StringComparison {
            get { return stringComparison; }
            set { stringComparison = value; }
        }
        private StringComparison stringComparison;

        /// <summary>
        /// Gets or set the pen will be used to frame the matched substrings.
        /// Set this to null to not draw a frame.
        /// </summary>
        public Pen FramePen {
            get { return framePen; }
            set { framePen = value; }
        }
        private Pen framePen;

        /// <summary>
        /// Gets or set the brush will be used to paint over the matched substrings.
        /// Set this to null to not fill the frame.
        /// </summary>
        /// <remarks>
        /// This is used to literally paint over the substring, so it must have be 
        /// transluscent if you want to substring to be legible.
        /// </remarks>
        public Brush FillBrush {
            get { return fillBrush; }
            set { fillBrush = value; }
        }
        private Brush fillBrush;

        #endregion

        #region Rendering

        // This class has two implement two highlighting schemes: one for GDI, another for GDI+.
        // Naturally, GDI+ makes the task easier, but we have to provide something for GDI
        // since that it is what is normally used.

        protected override void DrawTextGdi(Graphics g, Rectangle r, string txt) {
            base.DrawTextGdi(g, r, txt);
            if (!String.IsNullOrEmpty(this.TextToHighlight))
                this.DrawGdiTextHighlighting(g, r, txt);
        }

        protected virtual void DrawGdiTextHighlighting(Graphics g, Rectangle r, string txt) {
            TextFormatFlags flags = TextFormatFlags.NoPrefix |
                TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsTranslateTransform;

            // TextRenderer puts horizontal padding around the strings, so we need to take
            // that into account when measuring strings
            int paddingAdjustment = 6;

            // Find the substrings we want to highlight
            int start = 0;
            int found = txt.IndexOf(this.TextToHighlight, start, this.StringComparison);
            while (found >= 0) {
                // Measure the text that comes before our substring
                Size precedingTextSize = Size.Empty;
                if (found > 0) {
                    string precedingText = txt.Substring(0, found);
                    precedingTextSize = TextRenderer.MeasureText(g, precedingText, this.Font, r.Size, flags);
                    precedingTextSize.Width -= paddingAdjustment;
                }

                // Measure the length of our substring (may be different each time due to case differences)
                string highlightText = txt.Substring(found, this.TextToHighlight.Length);
                Size textToHighlightSize = TextRenderer.MeasureText(g, highlightText, this.Font, r.Size, flags);
                textToHighlightSize.Width -= paddingAdjustment;

                // Draw a filled frame around our substring
                Rectangle bounds = new Rectangle(r.X + precedingTextSize.Width + 1, r.Top, textToHighlightSize.Width, r.Height - 2);
                if (this.FramePen != null)
                    g.DrawRectangle(this.FramePen, bounds);
                if (this.FillBrush != null)
                    g.FillRectangle(this.FillBrush, bounds);

                // Find our next match
                start = found + this.TextToHighlight.Length;
                found = txt.IndexOf(this.TextToHighlight, start, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        protected override void DrawTextGdiPlus(Graphics g, Rectangle r, string txt) {
            base.DrawTextGdiPlus(g, r, txt);
            if (!String.IsNullOrEmpty(this.TextToHighlight))
                this.DrawGdiPlusTextHighlighting(g, r, txt);
        }

        protected virtual void DrawGdiPlusTextHighlighting(Graphics g, Rectangle r, string txt) {
            // Find the substrings we want to highlight
            List<CharacterRange> ranges = new List<CharacterRange>();
            int start = 0;
            int found = 0;
            while (found >= 0) {
                found = txt.IndexOf(this.TextToHighlight, start, this.StringComparison);
                if (found >= 0) {
                    ranges.Add(new CharacterRange(found, this.TextToHighlight.Length));
                    start = found + this.TextToHighlight.Length;
                } 
            }
            
            if (ranges.Count == 0)
                return;

            using (StringFormat fmt = this.StringFormatForGdiPlus) {
                RectangleF rf = r;
                fmt.SetMeasurableCharacterRanges(ranges.ToArray());
                Region[] stringRegions = g.MeasureCharacterRanges(txt, this.Font, rf, fmt);

                foreach (Region region in stringRegions) {
                    RectangleF bounds = region.GetBounds(g);
                    if (this.FramePen != null)
                        g.DrawRectangle(this.FramePen, bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height);
                    if (this.FillBrush != null)
                        g.FillRectangle(this.FillBrush, bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// This class maps a data value to an image that should be drawn for that value.
    /// </summary>
    /// <remarks><para>It is useful for drawing data that is represented as an enum or boolean.</para></remarks>
    public class MappedImageRenderer : BaseRenderer
    {
        /// <summary>
        /// Return a renderer that draw boolean values using the given images
        /// </summary>
        /// <param name="trueImage">Draw this when our data value is true</param>
        /// <param name="falseImage">Draw this when our data value is false</param>
        /// <returns>A Renderer</returns>
        static public MappedImageRenderer Boolean(Object trueImage, Object falseImage) {
            return new MappedImageRenderer(true, trueImage, false, falseImage);
        }

        /// <summary>
        /// Return a renderer that draw tristate boolean values using the given images
        /// </summary>
        /// <param name="trueImage">Draw this when our data value is true</param>
        /// <param name="falseImage">Draw this when our data value is false</param>
        /// <param name="nullImage">Draw this when our data value is null</param>
        /// <returns>A Renderer</returns>
        static public MappedImageRenderer TriState(Object trueImage, Object falseImage, Object nullImage) {
            return new MappedImageRenderer(new Object[] { true, trueImage, false, falseImage, null, nullImage });
        }

        /// <summary>
        /// Make a new empty renderer
        /// </summary>
        public MappedImageRenderer() {
            map = new System.Collections.Hashtable();
        }

        /// <summary>
        /// Make a new renderer that will show the given image when the given key is the aspect value
        /// </summary>
        /// <param name="key">The data value to be matched</param>
        /// <param name="image">The image to be shown when the key is matched</param>
        public MappedImageRenderer(Object key, Object image)
            : this() {
            this.Add(key, image);
        }

        /// <summary>
        /// Make a new renderer that will show the given images when it receives the given keys
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="image1"></param>
        /// <param name="key2"></param>
        /// <param name="image2"></param>
        public MappedImageRenderer(Object key1, Object image1, Object key2, Object image2)
            : this() {
            this.Add(key1, image1);
            this.Add(key2, image2);
        }

        /// <summary>
        /// Build a renderer from the given array of keys and their matching images
        /// </summary>
        /// <param name="keysAndImages">An array of key/image pairs</param>
        public MappedImageRenderer(Object[] keysAndImages)
            : this() {
            if ((keysAndImages.GetLength(0) % 2) != 0)
                throw new ArgumentException("Array must have key/image pairs");

            for (int i = 0; i < keysAndImages.GetLength(0); i += 2)
                this.Add(keysAndImages[i], keysAndImages[i + 1]);
        }

        /// <summary>
        /// Register the image that should be drawn when our Aspect has the data value.
        /// </summary>
        /// <param name="value">Value that the Aspect must match</param>
        /// <param name="image">An ImageSelector -- an int, string or image</param>
        public void Add(Object value, Object image) {
            if (value == null)
                this.nullImage = image;
            else
                map[value] = image;
        }

        /// <summary>
        /// Render our value
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            ICollection aspectAsCollection = this.Aspect as ICollection;
            if (aspectAsCollection == null)
                this.RenderOne(g, r, this.Aspect);
            else
                this.RenderCollection(g, r, aspectAsCollection);
        }

        protected void RenderCollection(Graphics g, Rectangle r, ICollection imageSelectors) {
            ArrayList images = new ArrayList();
            Image image = null;
            foreach (Object selector in imageSelectors) {
                if (selector == null)
                    image = this.GetImage(this.nullImage);
                else if (map.ContainsKey(selector))
                    image = this.GetImage(map[selector]);
                else
                    image = null;

                if (image != null)
                    images.Add(image);
            }

            this.DrawImages(g, r, images);
        }

        protected void RenderOne(Graphics g, Rectangle r, Object selector) {
            Image image = null;
            if (selector == null)
                image = this.GetImage(this.nullImage);
            else
                if (map.ContainsKey(selector))
                    image = this.GetImage(map[selector]);

            if (image != null)
                this.DrawAlignedImage(g, r, image);
        }

        #region Private variables

        private Hashtable map; // Track the association between values and images
        private Object nullImage; // image to be drawn for null values (since null can't be a key)

        #endregion
    }

    /// <summary>
    /// This renderer draws just a checkbox to match the check state of our model object.
    /// </summary>

    public class CheckStateRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);
            r = this.CalculateCheckBoxBounds(g, r);
            this.DrawImage(g, r, this.Column.GetCheckStateImage(this.RowObject));
        }

        protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex) {
            return cellBounds;
        }

        protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y) {
            Rectangle r = this.CalculateCheckBoxBounds(g, this.Bounds);
            if (r.Contains(x, y))
                hti.HitTestLocation = HitTestLocation.CheckBox;
        }

        private Rectangle CalculateCheckBoxBounds(Graphics g, Rectangle cellBounds) {
            // We don't use this because the checkbox images were drawn into the small image list
            //Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.CheckedNormal);
            Size checkBoxSize = this.ListView.SmallImageSize;
            return this.AlignRectangle(cellBounds,
                new Rectangle(0, 0, checkBoxSize.Width, cellBounds.Height));
        }
    }

    /// <summary>
    /// Render an image that comes from our data source.
    /// </summary>
    /// <remarks>The image can be sourced from:
    /// <list>
    /// <item>a byte-array (normally when the image to be shown is
    /// stored as a value in a database)</item>
    /// <item>an int, which is treated as an index into the image list</item>
    /// <item>a string, which is treated first as a file name, and failing that as an index into the image list</item>
    /// <item>an ICollection of ints or strings, which will be drawn as consecutive images</item>
    /// </list>
    /// <para>If an image is an animated GIF, it's state is stored in the SubItem object.</para>
    /// <para>By default, the image renderer does not render animations (it begins life with animations paused).
    /// To enable animations, you must call Unpause().</para>
    /// <para>In the current implementation (2009-09), each column showing animated gifs must have a 
    /// different instance of ImageRenderer assigned to it. You cannot share the same instance of
    /// an image renderer between two animated gif columns. If you do, only the last column will be
    /// animated.</para>
    /// </remarks>
    public class ImageRenderer : BaseRenderer
    {
        /// <summary>
        /// Make an empty image renderer
        /// </summary>
        public ImageRenderer() {
            this.tickler = new System.Threading.Timer(new TimerCallback(this.OnTimer), null, Timeout.Infinite, Timeout.Infinite);
            this.stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Make an empty image renderer that begins life ready for animations
        /// </summary>
        public ImageRenderer(bool startAnimations)
            : this() {
            this.Paused = !startAnimations;
        }

        #region Properties

        /// <summary>
        /// Should the animations in this renderer be paused?
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Paused {
            get { return isPaused; }
            set {
                if (isPaused != value) {
                    isPaused = value;
                    if (isPaused) {
                        this.tickler.Change(Timeout.Infinite, Timeout.Infinite);
                        this.stopwatch.Stop();
                    } else {
                        this.tickler.Change(1, Timeout.Infinite);
                        this.stopwatch.Start();
                    }
                }
            }
        }
        private bool isPaused = true;

        #endregion

        #region Commands

        /// <summary>
        /// Pause any animations
        /// </summary>
        public void Pause() {
            this.Paused = true;
        }

        /// <summary>
        /// Unpause any animations
        /// </summary>
        public void Unpause() {
            this.Paused = false;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draw our image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            if (this.Aspect == null || this.Aspect == System.DBNull.Value)
                return;

            if (this.Aspect is System.Byte[]) {
                this.DrawAlignedImage(g, r, this.GetImageFromAspect());
            } else {
                ICollection imageSelectors = this.Aspect as ICollection;
                if (imageSelectors == null)
                    this.DrawAlignedImage(g, r, this.GetImageFromAspect());
                else
                    this.DrawImages(g, r, imageSelectors);
            }
        }

        /// <summary>
        /// Translate our Aspect into an image.
        /// </summary>
        /// <remarks>The strategy is:<list type="unordered">
        /// <item>If its a byte array, we treat it as an in-memory image</item>
        /// <item>If it's an int, we use that as an index into our image list</item>
        /// <item>If it's a string, we try to load a file by that name. If we can't, 
        /// we use the string as an index into our image list.</item>
        ///</list></remarks>
        /// <returns>An image</returns>
        protected Image GetImageFromAspect() {
            // If we've already figured out the image, don't do it again
            if (this.OLVSubItem != null && this.OLVSubItem.ImageSelector is Image) {
                if (this.OLVSubItem.AnimationState == null)
                    return (Image)this.OLVSubItem.ImageSelector;
                else
                    return this.OLVSubItem.AnimationState.image;
            }

            // Try to convert our Aspect into an Image
            // If its a byte array, we treat it as an in-memory image
            // If it's an int, we use that as an index into our image list
            // If it's a string, we try to find a file by that name.
            //    If we can't, we use the string as an index into our image list.
            Image image = null;
            if (this.Aspect is System.Byte[]) {
                using (MemoryStream stream = new MemoryStream((System.Byte[])this.Aspect)) {
                    try {
                        image = Image.FromStream(stream);
                    }
                    catch (ArgumentException) {
                        // ignore
                    }
                }
            } else if (this.Aspect is Int32) {
                image = this.GetImage(this.Aspect);
            } else {
                String str = this.Aspect as String;
                if (!String.IsNullOrEmpty(str)) {
                    try {
                        image = Image.FromFile(str);
                    }
                    catch (FileNotFoundException) {
                        image = this.GetImage(this.Aspect);
                    }
                    catch (OutOfMemoryException) {
                        image = this.GetImage(this.Aspect);
                    }
                }
            }

            // If this image is an animation, initialize the animation process
            if (this.OLVSubItem != null && AnimationState.IsAnimation(image)) {
                this.OLVSubItem.AnimationState = new AnimationState(image);
            }

            // Cache the image so we don't repeat this dreary process
            if (this.OLVSubItem != null)
                this.OLVSubItem.ImageSelector = image;

            return image;
        }

        #endregion

        #region Events

        /// <summary>
        /// This is the method that is invoked by the timer. It basically switches control to the listview thread.
        /// </summary>
        /// <param name="state">not used</param>
        public void OnTimer(Object state) {
            if (this.ListView == null || this.Paused)
                this.tickler.Change(1000, Timeout.Infinite);
            else {
                if (this.ListView.InvokeRequired)
                    this.ListView.Invoke((MethodInvoker)delegate { this.OnTimer(state); });
                else
                    this.OnTimerInThread();
            }
        }

        /// <summary>
        /// This is the OnTimer callback, but invoked in the same thread as the creator of the ListView.
        /// This method can use all of ListViews methods without creating a CrossThread exception.
        /// </summary>
        protected void OnTimerInThread() {
            // MAINTAINER NOTE: This method must renew the tickler. If it doesn't the animations will stop.

            // If this listview has been destroyed, we can't do anything, so we return without
            // renewing the tickler, effectively killing all animations on this renderer
            if (this.ListView.IsDisposed)
                return;

            // If we're not in Detail view or our column has been removed from the list,
            // we can't do anything at the moment, but we still renew the tickler because the view may change later.
            if (this.ListView.View != System.Windows.Forms.View.Details || this.Column.Index < 0) {
                this.tickler.Change(1000, Timeout.Infinite);
                return;
            }

            long elapsedMilliseconds = this.stopwatch.ElapsedMilliseconds;
            int subItemIndex = this.Column.Index;
            long nextCheckAt = elapsedMilliseconds + 1000; // wait at most one second before checking again
            Rectangle updateRect = new Rectangle(); // what part of the view must be updated to draw the changed gifs?

            // Run through all the subitems in the view for our column, and for each one that
            // has an animation attached to it, see if the frame needs updating.
            foreach (OLVListItem lvi in this.ListView.Items) {
                // Get the animation state from the subitem. If there isn't an animation state, skip this row.
                OLVListSubItem lvsi = lvi.GetSubItem(subItemIndex);
                AnimationState state = lvsi.AnimationState;
                if (state == null || !state.IsValid)
                    continue;

                // Has this frame of the animation expired?
                if (elapsedMilliseconds >= state.currentFrameExpiresAt) {
                    state.AdvanceFrame(elapsedMilliseconds);

                    // Track the area of the view that needs to be redrawn to show the changed images
                    if (updateRect.IsEmpty)
                        updateRect = lvsi.Bounds;
                    else
                        updateRect = Rectangle.Union(updateRect, lvsi.Bounds);
                }

                // Remember the minimum time at which a frame is next due to change
                nextCheckAt = Math.Min(nextCheckAt, state.currentFrameExpiresAt);
            }

            // Update the part of the listview where frames have changed
            if (!updateRect.IsEmpty)
                this.ListView.Invalidate(updateRect);

            // Renew the tickler in time for the next frame change
            this.tickler.Change(nextCheckAt - elapsedMilliseconds, Timeout.Infinite);
        }

        #endregion

        /// <summary>
        /// Instances of this class kept track of the animation state of a single image.
        /// </summary>
        internal class AnimationState
        {
            const int PropertyTagTypeShort = 3;
            const int PropertyTagTypeLong = 4;
            const int PropertyTagFrameDelay = 0x5100;
            const int PropertyTagLoopCount = 0x5101;

            /// <summary>
            /// Is the given image an animation
            /// </summary>
            /// <param name="image">The image to be tested</param>
            /// <returns>Is the image an animation?</returns>
            static public bool IsAnimation(Image image) {
                if (image == null)
                    return false;
                else
                    return (new List<Guid>(image.FrameDimensionsList)).Contains(FrameDimension.Time.Guid);
            }

            /// <summary>
            /// Create an AnimationState in a quiet state
            /// </summary>
            public AnimationState() {
                this.imageDuration = new List<int>();
            }

            /// <summary>
            /// Create an animation state for the given image, which may or may not
            /// be an animation
            /// </summary>
            /// <param name="image">The image to be rendered</param>
            public AnimationState(Image image)
                : this() {
                if (!AnimationState.IsAnimation(image))
                    return;

                // How many frames in the animation?
                this.image = image;
                this.frameCount = this.image.GetFrameCount(FrameDimension.Time);

                // Find the delay between each frame.
                // The delays are stored an array of 4-byte ints. Each int is the
                // number of 1/100th of a second that should elapsed before the frame expires
                foreach (PropertyItem pi in this.image.PropertyItems) {
                    if (pi.Id == PropertyTagFrameDelay) {
                        for (int i = 0; i < pi.Len; i += 4) {
                            //TODO: There must be a better way to convert 4-bytes to an int
                            int delay = (pi.Value[i + 3] << 24) + (pi.Value[i + 2] << 16) + (pi.Value[i + 1] << 8) + pi.Value[i];
                            this.imageDuration.Add(delay * 10); // store delays as milliseconds
                        }
                        break;
                    }
                }

                // There should be as many frame durations as frames
                Debug.Assert(this.imageDuration.Count == this.frameCount, "There should be as many frame durations as there are frames.");
            }

            /// <summary>
            /// Does this state represent a valid animation
            /// </summary>
            public bool IsValid {
                get {
                    return (this.image != null && this.frameCount > 0);
                }
            }

            /// <summary>
            /// Advance our images current frame and calculate when it will expire
            /// </summary>
            public void AdvanceFrame(long millisecondsNow) {
                this.currentFrame = (this.currentFrame + 1) % this.frameCount;
                this.currentFrameExpiresAt = millisecondsNow + this.imageDuration[this.currentFrame];
                this.image.SelectActiveFrame(FrameDimension.Time, this.currentFrame);
            }

            internal int currentFrame;
            internal long currentFrameExpiresAt;
            internal Image image;
            internal List<int> imageDuration;
            internal int frameCount;
        }

        #region Private variables

        private System.Threading.Timer tickler; // timer used to tickle the animations
        private Stopwatch stopwatch; // clock used to time the animation frame changes

        #endregion
    }

    /// <summary>
    /// Render our Aspect as a progress bar
    /// </summary>
    public class BarRenderer : BaseRenderer
    {
        #region Constructors

        /// <summary>
        /// Make a BarRenderer
        /// </summary>
        public BarRenderer()
            : base() {
        }

        /// <summary>
        /// Make a BarRenderer for the given range of data values
        /// </summary>
        public BarRenderer(int minimum, int maximum)
            : this() {
            this.MinimumValue = minimum;
            this.MaximumValue = maximum;
        }

        /// <summary>
        /// Make a BarRenderer using a custom bar scheme
        /// </summary>
        public BarRenderer(Pen pen, Brush brush)
            : this() {
            this.Pen = pen;
            this.Brush = brush;
            this.UseStandardBar = false;
        }

        /// <summary>
        /// Make a BarRenderer using a custom bar scheme
        /// </summary>
        public BarRenderer(int minimum, int maximum, Pen pen, Brush brush)
            : this(minimum, maximum) {
            this.Pen = pen;
            this.Brush = brush;
            this.UseStandardBar = false;
        }

        /// <summary>
        /// Make a BarRenderer that uses a horizontal gradient
        /// </summary>
        public BarRenderer(Pen pen, Color start, Color end)
            : this() {
            this.Pen = pen;
            this.SetGradient(start, end);
        }

        /// <summary>
        /// Make a BarRenderer that uses a horizontal gradient
        /// </summary>
        public BarRenderer(int minimum, int maximum, Pen pen, Color start, Color end)
            : this(minimum, maximum) {
            this.Pen = pen;
            this.SetGradient(start, end);
        }

        #endregion

        #region Configuration Properties

        /// <summary>
        /// Should this bar be drawn in the system style?
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("Should this bar be drawn in the system style?"),
         DefaultValue(true)]
        public bool UseStandardBar {
            get { return useStandardBar; }
            set { useStandardBar = value; }
        }
        private bool useStandardBar = true;

        /// <summary>
        /// How many pixels in from our cell border will this bar be drawn
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("How many pixels in from our cell border will this bar be drawn"),
         DefaultValue(2)]
        public int Padding {
            get { return padding; }
            set { padding = value; }
        }
        private int padding = 2;

        /// <summary>
        /// What color will be used to fill the interior of the control before the 
        /// progress bar is drawn?
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("The color of the interior of the bar"),
         DefaultValue(typeof(Color), "AliceBlue")]
        public Color BackgroundColor {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        private Color backgroundColor = Color.AliceBlue;

        /// <summary>
        /// What color should the frame of the progress bar be?
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("What color should the frame of the progress bar be"),
         DefaultValue(typeof(Color), "Black")]
        public Color FrameColor {
            get { return frameColor; }
            set { frameColor = value; }
        }
        private Color frameColor = Color.Black;

        /// <summary>
        /// How many pixels wide should the frame of the progress bar be?
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("How many pixels wide should the frame of the progress bar be"),
         DefaultValue(1.0f)]
        public float FrameWidth {
            get { return frameWidth; }
            set { frameWidth = value; }
        }
        private float frameWidth = 1.0f;

        /// <summary>
        /// What color should the 'filled in' part of the progress bar be?
        /// </summary>
        /// <remarks>This is only used if GradientStartColor is Color.Empty</remarks>
        [Category("Appearance - ObjectListView"),
         Description("What color should the 'filled in' part of the progress bar be"),
         DefaultValue(typeof(Color), "BlueViolet")]
        public Color FillColor {
            get { return fillColor; }
            set { fillColor = value; }
        }
        private Color fillColor = Color.BlueViolet;

        /// <summary>
        /// Use a gradient to fill the progress bar starting with this color
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("Use a gradient to fill the progress bar starting with this color"),
         DefaultValue(typeof(Color), "CornflowerBlue")]
        public Color GradientStartColor {
            get { return startColor; }
            set {
                startColor = value;
            }
        }
        private Color startColor = Color.CornflowerBlue;

        /// <summary>
        /// Use a gradient to fill the progress bar ending with this color
        /// </summary>
        [Category("Appearance - ObjectListView"),
         Description("Use a gradient to fill the progress bar ending with this color"),
         DefaultValue(typeof(Color), "DarkBlue")]
        public Color GradientEndColor {
            get { return endColor; }
            set {
                endColor = value;
            }
        }
        private Color endColor = Color.DarkBlue;

        /// <summary>
        /// Regardless of how wide the column become the progress bar will never be wider than this
        /// </summary>
        [Category("Behavior"),
        Description("The progress bar will never be wider than this"),
        DefaultValue(100)]
        public int MaximumWidth {
            get { return maximumWidth; }
            set { maximumWidth = value; }
        }
        private int maximumWidth = 100;

        /// <summary>
        /// Regardless of how high the cell is  the progress bar will never be taller than this
        /// </summary>
        [Category("Behavior"),
        Description("The progress bar will never be taller than this"),
        DefaultValue(16)]
        public int MaximumHeight {
            get { return maximumHeight; }
            set { maximumHeight = value; }
        }
        private int maximumHeight = 16;

        /// <summary>
        /// The minimum data value expected. Values less than this will given an empty bar
        /// </summary>
        [Category("Behavior"),
        Description("The minimum data value expected. Values less than this will given an empty bar"),
        DefaultValue(0.0)]
        public double MinimumValue {
            get { return minimumValue; }
            set { minimumValue = value; }
        }
        private double minimumValue = 0.0;

        /// <summary>
        /// The maximum value for the range. Values greater than this will give a full bar
        /// </summary>
        [Category("Behavior"),
        Description("The maximum value for the range. Values greater than this will give a full bar"),
        DefaultValue(100.0)]
        public double MaximumValue {
            get { return maximumValue; }
            set { maximumValue = value; }
        }
        private double maximumValue = 100.0;

        #endregion

        #region Public Properties (non-IDE)

        /// <summary>
        /// The Pen that will draw the frame surrounding this bar
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Pen Pen {
            get {
                if (this.pen == null && !this.FrameColor.IsEmpty)
                    return new Pen(this.FrameColor, this.FrameWidth);
                else
                    return this.pen;
            }
            set {
                this.pen = value;
            }
        }
        private Pen pen;

        /// <summary>
        /// The brush that will be used to fill the bar
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush Brush {
            get {
                if (this.brush == null && !this.FillColor.IsEmpty)
                    return new SolidBrush(this.FillColor);
                else
                    return this.brush;
            }
            set {
                this.brush = value;
            }
        }
        private Brush brush;

        /// <summary>
        /// The brush that will be used to fill the background of the bar
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush BackgroundBrush {
            get {
                if (this.backgroundBrush == null && !this.BackgroundColor.IsEmpty)
                    return new SolidBrush(this.BackgroundColor);
                else
                    return this.backgroundBrush;
            }
            set {
                this.backgroundBrush = value;
            }
        }
        private Brush backgroundBrush;

        #endregion

        /// <summary>
        /// Draw this progress bar using a gradient
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SetGradient(Color start, Color end) {
            this.GradientStartColor = start;
            this.GradientEndColor = end;
        }

        /// <summary>
        /// Draw our aspect
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            Rectangle frameRect = Rectangle.Inflate(r, 0 - this.Padding, 0 - this.Padding);
            frameRect.Width = Math.Min(frameRect.Width, this.MaximumWidth);
            frameRect.Height = Math.Min(frameRect.Height, this.MaximumHeight);
            frameRect = this.AlignRectangle(r, frameRect);

            // Convert our aspect to a numeric value
            IConvertible convertable = this.Aspect as IConvertible;
            if (convertable == null)
                return;
            double aspectValue = convertable.ToDouble(NumberFormatInfo.InvariantInfo);

            Rectangle fillRect = Rectangle.Inflate(frameRect, -1, -1);
            if (aspectValue <= this.MinimumValue)
                fillRect.Width = 0;
            else if (aspectValue < this.MaximumValue)
                fillRect.Width = (int)(fillRect.Width * (aspectValue - this.MinimumValue) / this.MaximumValue);

            // MS-themed progress bars don't work when printing
            if (this.UseStandardBar && ProgressBarRenderer.IsSupported && !this.IsPrinting) {
                ProgressBarRenderer.DrawHorizontalBar(g, frameRect);
                ProgressBarRenderer.DrawHorizontalChunks(g, fillRect);
            } else {
                g.FillRectangle(this.BackgroundBrush, frameRect);
                if (fillRect.Width > 0) {
                    // FillRectangle fills inside the given rectangle, so expand it a little
                    fillRect.Width++;
                    fillRect.Height++;
                    if (this.GradientStartColor == Color.Empty)
                        g.FillRectangle(this.Brush, fillRect);
                    else {
                        using (LinearGradientBrush gradient = new LinearGradientBrush(frameRect, this.GradientStartColor, this.GradientEndColor, LinearGradientMode.Horizontal)) {
                            g.FillRectangle(gradient, fillRect);
                        }
                    }
                }
                g.DrawRectangle(this.Pen, frameRect);
            }
        }
    }

    /// <summary>
    /// An ImagesRenderer draws zero or more images depending on the data returned by its Aspect.
    /// </summary>
    /// <remarks><para>This renderer's Aspect must return a ICollection of ints, strings or Images,
    /// each of which will be drawn horizontally one after the other.</para>
    /// <para>As of v2.1, this functionality has been absorbed into ImageRenderer and this is now an
    /// empty shell, solely for backwards compatibility.</para>
    /// </remarks>
    [ToolboxItem(false)]
    public class ImagesRenderer : ImageRenderer
    {
    }

    /// <summary>
    /// A MultiImageRenderer draws the same image a number of times based on our data value
    /// </summary>
    /// <remarks><para>The stars in the Rating column of iTunes is a good example of this type of renderer.</para></remarks>
    public class MultiImageRenderer : BaseRenderer
    {
        /// <summary>
        /// Make a quiet rendererer
        /// </summary>
        public MultiImageRenderer()
            : base() {
        }

        /// <summary>
        /// Make an image renderer that will draw the indicated image, at most maxImages times.
        /// </summary>
        /// <param name="imageSelector"></param>
        /// <param name="maxImages"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public MultiImageRenderer(Object imageSelector, int maxImages, int minValue, int maxValue)
            : this() {
            this.ImageSelector = imageSelector;
            this.MaxNumberImages = maxImages;
            this.MinimumValue = minValue;
            this.MaximumValue = maxValue;
        }

        #region Configuration Properties

        /// <summary>
        /// The index of the image that should be drawn
        /// </summary>
        [Category("Behavior"),
         Description("The index of the image that should be drawn"),
         DefaultValue(-1)]
        public int ImageIndex {
            get {
                if (imageSelector is Int32)
                    return (Int32)imageSelector;
                else
                    return -1;
            }
            set { imageSelector = value; }
        }

        /// <summary>
        /// The name of the image that should be drawn
        /// </summary>
        [Category("Behavior"),
         Description("The index of the image that should be drawn"),
         DefaultValue(null)]
        public string ImageName {
            get {
                return imageSelector as String;
            }
            set { imageSelector = value; }
        }

        /// <summary>
        /// The image selector that will give the image to be drawn
        /// </summary>
        /// <remarks>Like all image selectors, this can be an int, string or Image</remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object ImageSelector {
            get { return imageSelector; }
            set { imageSelector = value; }
        }
        private Object imageSelector;

        /// <summary>
        /// What is the maximum number of images that this renderer should draw?
        /// </summary>
        [Category("Behavior"),
         Description("The maximum number of images that this renderer should draw"),
         DefaultValue(10)]
        public int MaxNumberImages {
            get { return maxNumberImages; }
            set { maxNumberImages = value; }
        }
        private int maxNumberImages = 10;

        /// <summary>
        /// Values less than or equal to this will have 0 images drawn
        /// </summary>
        [Category("Behavior"),
         Description("Values less than or equal to this will have 0 images drawn"),
         DefaultValue(0)]
        public int MinimumValue {
            get { return minimumValue; }
            set { minimumValue = value; }
        }
        private int minimumValue = 0;

        /// <summary>
        /// Values greater than or equal to this will have MaxNumberImages images drawn
        /// </summary>
        [Category("Behavior"),
         Description("Values greater than or equal to this will have MaxNumberImages images drawn"),
         DefaultValue(100)]
        public int MaximumValue {
            get { return maximumValue; }
            set { maximumValue = value; }
        }
        private int maximumValue = 100;

        #endregion

        /// <summary>
        /// Draw our data value
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            Image image = this.GetImage(this.ImageSelector);
            if (image == null)
                return;

            // Convert our aspect to a numeric value
            IConvertible convertable = this.Aspect as IConvertible;
            if (convertable == null)
                return;
            double aspectValue = convertable.ToDouble(NumberFormatInfo.InvariantInfo);

            // Calculate how many images we need to draw to represent our aspect value
            int numberOfImages;
            if (aspectValue <= this.MinimumValue)
                numberOfImages = 0;
            else if (aspectValue < this.MaximumValue)
                numberOfImages = 1 + (int)(this.MaxNumberImages * (aspectValue - this.MinimumValue) / this.MaximumValue);
            else
                numberOfImages = this.MaxNumberImages;

            // If we need to shrink the image, what will its on-screen dimensions be?
            int imageScaledWidth = image.Width;
            int imageScaledHeight = image.Height;
            if (r.Height < image.Height) {
                imageScaledWidth = (int)((float)image.Width * (float)r.Height / (float)image.Height);
                imageScaledHeight = r.Height;
            }
            // Calculate where the images should be drawn
            Rectangle imageBounds = r;
            imageBounds.Width = (this.MaxNumberImages * (imageScaledWidth + this.Spacing)) - this.Spacing;
            imageBounds.Height = imageScaledHeight;
            imageBounds = this.AlignRectangle(r, imageBounds);

            // Finally, draw the images
            for (int i = 0; i < numberOfImages; i++) {
                g.DrawImage(image, imageBounds.X, imageBounds.Y, imageScaledWidth, imageScaledHeight);
                imageBounds.X += (imageScaledWidth + this.Spacing);
            }
        }
    }


    /// <summary>
    /// A class to render a value that contains a bitwise-OR'ed collection of values.
    /// </summary>
    public class FlagRenderer : BaseRenderer
    {
        /// <summary>
        /// Register the given image to the given value
        /// </summary>
        /// <param name="key">When this flag is present...</param>
        /// <param name="imageSelector">...draw this image</param>
        public void Add(Object key, Object imageSelector) {
            Int32 k2 = ((IConvertible)key).ToInt32(NumberFormatInfo.InvariantInfo);

            this.imageMap[k2] = imageSelector;
            this.keysInOrder.Remove(k2);
            this.keysInOrder.Add(k2);
        }

        /// <summary>
        /// Draw the flags
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);

            IConvertible convertable = this.Aspect as IConvertible;
            if (convertable == null)
                return;

            Int32 v2 = convertable.ToInt32(NumberFormatInfo.InvariantInfo);

            Point pt = r.Location;
            foreach (Int32 key in this.keysInOrder) {
                if ((v2 & key) == key) {
                    Image image = this.GetImage(this.imageMap[key]);
                    if (image != null) {
                        g.DrawImage(image, pt);
                        pt.X += (image.Width + this.Spacing);
                    }
                }
            }
        }

        /// <summary>
        /// Do the actual work of hit testing. Subclasses should override this rather than HitTest()
        /// </summary>
        /// <param name="g"></param>
        /// <param name="hti"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y) {
            IConvertible convertable = this.Aspect as IConvertible;
            if (convertable == null)
                return;

            Int32 v2 = convertable.ToInt32(NumberFormatInfo.InvariantInfo);

            Point pt = this.Bounds.Location;
            foreach (Int32 key in this.keysInOrder) {
                if ((v2 & key) == key) {
                    Image image = this.GetImage(this.imageMap[key]);
                    if (image != null) {
                        Rectangle imageRect = new Rectangle(pt, image.Size);
                        if (imageRect.Contains(x, y)) {
                            hti.UserData = key;
                            return;
                        }
                        pt.X += (image.Width + this.Spacing);
                    }
                }
            }
        }

        private List<Int32> keysInOrder = new List<Int32>();
        private Dictionary<Int32, Object> imageMap = new Dictionary<Int32, object>();
    }

    /// <summary>
    /// This renderer draws an image, a single line title, and then multi-line descrition
    /// under the title.
    /// </summary>
    /// <remarks>
    /// <para>This class works best with FullRowSelect = true.</para>
    /// <para>It's not designed to work with cell editing -- it will work but will look odd.</para>
    /// <para>
    /// This class is experimental. It may not work properly and may disappear from
    /// future versions.
    /// </para>
    /// </remarks>
    public class DescribedTaskRenderer : BaseRenderer
    {
        public DescribedTaskRenderer() {
        }

        #region Configuration properties

        /// <summary>
        /// Gets or set the font that will be used to draw the title of the task
        /// </summary>
        /// <remarks>If this is null, the ListView's font will be used</remarks>
        [Category("Appearance - ObjectListView"),
        Description("The font that will be used to draw the title of the task"),
        DefaultValue(null)]
        public Font TitleFont {
            get { return titleFont; }
            set { titleFont = value; }
        }
        private Font titleFont;

        /// <summary>
        /// Return a font that has been set for the title or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Font TitleFontOrDefault {
            get {
                return this.TitleFont ?? this.ListView.Font;
            }
        }

        /// <summary>
        /// Gets or set the color of the title of the task
        /// </summary>
        /// <remarks>This color is used when the task is not selected or when the listview
        /// has a translucent selection mechanism.</remarks>
        [Category("Appearance - ObjectListView"),
        Description("The color of the title"),
        DefaultValue(typeof(Color), "")]
        public Color TitleColor {
            get { return titleColor; }
            set { titleColor = value; }
        }
        private Color titleColor;

        /// <summary>
        /// Return the color of the title of the task or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Color TitleColorOrDefault {
            get {
                if (this.IsItemSelected || this.TitleColor.IsEmpty)
                    return this.GetForegroundColor();
                else
                    return this.TitleColor;
            }
        }

        /// <summary>
        /// Gets or set the font that will be used to draw the description of the task
        /// </summary>
        /// <remarks>If this is null, the ListView's font will be used</remarks>
        [Category("Appearance - ObjectListView"),
        Description("The font that will be used to draw the description of the task"),
        DefaultValue(null)]
        public Font DescriptionFont {
            get { return descriptionFont; }
            set { descriptionFont = value; }
        }
        private Font descriptionFont;

        /// <summary>
        /// Return a font that has been set for the title or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Font DescriptionFontOrDefault {
            get {
                return this.DescriptionFont ?? this.ListView.Font;
            }
        }

        /// <summary>
        /// Gets or set the color of the description of the task
        /// </summary>
        /// <remarks>This color is used when the task is not selected or when the listview
        /// has a translucent selection mechanism.</remarks>
        [Category("Appearance - ObjectListView"),
        Description("The color of the description"),
        DefaultValue(typeof(Color), "DimGray")]
        public Color DescriptionColor {
            get { return descriptionColor; }
            set { descriptionColor = value; }
        }
        private Color descriptionColor = Color.DimGray;

        /// <summary>
        /// Return the color of the description of the task or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Color DescriptionColorOrDefault {
            get {
                if (this.DescriptionColor.IsEmpty || (this.IsItemSelected && !this.ListView.UseTranslucentSelection))
                    return this.GetForegroundColor();
                else
                    return this.DescriptionColor;
            }
        }

        /// <summary>
        /// Gets or sets the number of pixels that renderer will leave empty around the edge of the cell
        /// </summary>
        [Category("Appearance - ObjectListView"),
        Description("The number of pixels that renderer will leave empty around the edge of the cell"),
        DefaultValue(typeof(Size), "2,2")]
        public Size CellPadding {
            get { return cellPadding; }
            set { cellPadding = value; }
        }
        private Size cellPadding = new Size(2, 2);

        /// <summary>
        /// Gets or sets the number of pixels that will be left between the image and the text
        /// </summary>
        [Category("Appearance - ObjectListView"),
        Description("The number of pixels that that will be left between the image and the text"),
        DefaultValue(4)]
        public int ImageTextSpace {
            get { return imageTextSpace; }
            set { imageTextSpace = value; }
        }
        private int imageTextSpace = 4;

        /// <summary>
        /// Gets or sets the name of the aspect of the model object that contains the task description
        /// </summary>
        [Category("Appearance - ObjectListView"),
        Description("The name of the aspect of the model object that contains the task description"),
        DefaultValue(null)]
        public string DescriptionAspectName {
            get { return descriptionAspectName; }
            set { descriptionAspectName = value; }
        }
        private string descriptionAspectName;

        #endregion

        #region Calculating

        /// <summary>
        /// Fetch the description from the model class
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDescription() {
            if (String.IsNullOrEmpty(this.DescriptionAspectName))
                return String.Empty;

            if (this.descriptionGetter == null)
                this.descriptionGetter = new Munger(this.DescriptionAspectName);

            return this.descriptionGetter.GetValue(this.RowObject) as String;
        }
        Munger descriptionGetter;

        #endregion

        #region Rendering

        public override void Render(Graphics g, Rectangle r) {
            this.DrawBackground(g, r);
            this.DrawDescribedTask(g, r, this.Aspect as String, this.GetDescription(), this.GetImage());
        }

        public virtual void DrawDescribedTask(Graphics g, Rectangle r, string title, string description, Image image) {
            Rectangle cellBounds = r;
            cellBounds.Inflate(-this.CellPadding.Width, -this.CellPadding.Height);
            Rectangle textBounds = cellBounds;

            if (image != null) {
                g.DrawImage(image, cellBounds.Location);
                int gapToText = image.Width + this.ImageTextSpace;
                textBounds.X += gapToText;
                textBounds.Width -= gapToText;
            }

            // Color the background if the row is selected and we're not using a translucent selection
            if (this.IsItemSelected && !this.ListView.UseTranslucentSelection) {
                using (SolidBrush b = new SolidBrush(this.GetTextBackgroundColor())) {
                    g.FillRectangle(b, textBounds);
                }
            }

            // Draw the title
            if (!String.IsNullOrEmpty(title)) {
                using (StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap)) {
                    fmt.Trimming = StringTrimming.EllipsisCharacter;
                    fmt.Alignment = StringAlignment.Near;
                    fmt.LineAlignment = StringAlignment.Near;
                    Font f = this.TitleFontOrDefault;
                    using (SolidBrush b = new SolidBrush(this.TitleColorOrDefault)) {
                        g.DrawString(title, f, b, textBounds, fmt);
                    }

                    // How tall was the title?
                    SizeF size = g.MeasureString(title, f, (int)textBounds.Width, fmt);
                    textBounds.Y += (int)size.Height;
                    textBounds.Height -= (int)size.Height;
                }
            }

            // Draw the description
            if (!String.IsNullOrEmpty(description)) {
                using (StringFormat fmt2 = new StringFormat()) {
                    fmt2.Trimming = StringTrimming.EllipsisCharacter;
                    using (SolidBrush b = new SolidBrush(this.DescriptionColorOrDefault)) {
                        g.DrawString(description, this.DescriptionFontOrDefault, b, textBounds, fmt2);
                    }
                }
            }
        }

        #endregion

        #region Hit Testing

        protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y) {
            if (this.Bounds.Contains(x, y))
                hti.HitTestLocation = HitTestLocation.Text;
        }

        #endregion
    }
}
