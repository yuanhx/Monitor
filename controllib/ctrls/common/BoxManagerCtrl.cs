using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace UICtrls
{
    public partial class BoxManagerCtrl<T> : UserControl
        where T : Control, new()
    {
        protected CBoxManager<T> mBoxManager = new CBoxManager<T>();

        public event InitBoxEventHandle<T> OnInitBox = null;
        public event BoxEventHandle<T> OnActiveBoxChanging = null;
        public event BoxEventHandle<T> OnActiveBoxChanged = null;

        public BoxManagerCtrl()
        {
            InitializeComponent();

            InitBoxManager();
        }

        protected virtual void InitBoxManager()
        {
            mBoxManager.Container = this;
            mBoxManager.OnInitBox += new InitBoxEventHandle<T>(InitBox);

            mBoxManager.OnActiveBoxChanging += new BoxEventHandle<T>(DoActiveBoxChanging);
            mBoxManager.OnActiveBoxChanged += new BoxEventHandle<T>(DoActiveBoxChanged);

            mBoxManager.OnShowModeChanged += new ShowModeChangedEventHandle(DoShowModeChanged);
            mBoxManager.OnShowIndexChanged += new ShowIndexChangedEventHandle(DoShowIndexChanged);
        }

        protected virtual bool InitBox(T box)
        {
            box.MouseClick += new MouseEventHandler(PlayBoxMouseClick);
            box.MouseDoubleClick += new MouseEventHandler(PlayBoxMouseDoubleClick);

            if (OnInitBox != null)
                return OnInitBox(box);
            else
                return true;
        }

        public IBoxManager BoxManager
        {
            get { return mBoxManager; }
        }

        public int BoxCount
        {
            get { return mBoxManager.BoxCount; }
            set { mBoxManager.BoxCount = value; }
        }

        public int MaxIndex
        {
            get { return mBoxManager.MaxIndex; }
        }

        public void SetShowMode(int showRow, int showColumn)
        {
            mBoxManager.SetShowMode(showRow, showColumn);
        }

        public string ShowMode
        {
            get { return mBoxManager.ShowMode; }
            set { mBoxManager.ShowMode = value; }
        }

        public int ShowRow
        {
            get { return mBoxManager.ShowRow; }
        }

        public int ShowColumn
        {
            get { return mBoxManager.ShowColumn; }
        }

        public int ShowIndex
        {
            get { return mBoxManager.ShowIndex; }
            set { mBoxManager.ShowIndex = value; }
        }

        public T ActiveBox
        {
            get { return mBoxManager.ActiveBox; }
            set { mBoxManager.ActiveBox = value; }
        }

        public void Init()
        {
            Init(16);
        }

        public void Init(int boxCount)
        {
            Init(boxCount, 2, 2);
        }

        public void Init(int boxCount, int showRow, int showColumn)
        {
            mBoxManager.BoxCount = boxCount;
            mBoxManager.SetShowMode(showRow, showColumn);
            mBoxManager.ShowIndex = 0;
        }

        public void RefreshShow()
        {
            mBoxManager.RefreshShow();
        }

        protected virtual void DoShowModeChanged(int showRow, int showColumn)
        {
            DoShowIndexChanged(0);
        }

        protected virtual void DoShowIndexChanged(int showIndex)
        {
            
        }

        protected virtual void DoActiveBoxChanging(T box)
        {
            if (OnActiveBoxChanging != null)
            {
                OnActiveBoxChanging(box);
            }
        }

        protected virtual void DoActiveBoxChanged(T box)
        {
            if (OnActiveBoxChanged != null)
            {
                OnActiveBoxChanged(box);
            }
        }

        protected virtual void PlayBoxMouseClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as T;
            this.OnMouseClick(e);
        }

        protected virtual void PlayBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as T;
            this.OnMouseDoubleClick(e);
        }
    }
}
