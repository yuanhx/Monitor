using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UICtrls;
using System.Drawing;
using Utils;

namespace UICtrls
{
    public delegate bool InitBoxEventHandle<T>(T box);
    public delegate void BoxEventHandle<T>(T box);
    public delegate void SetBoxEventHandle<T>(T box, int showRow, int showColumn);

    public delegate void ShowModeChangedEventHandle(int showRow, int showColumn);
    public delegate void ShowIndexChangedEventHandle(int showIndex);

    public enum TArrayType { None = 0, Vertical = 1, Horizontal = 2 }

    public interface IBoxManager
    {
        Control Container { get; }
        int BoxCount { get; set; }
        int ShowIndex { get; set; }
        int ShowRow { get; }
        int ShowColumn { get; }
        int ShowCount { get; }
        int MaxIndex { get; }

        int Interval { get; set; }

        int ActiveIndex { get; set; }

        void SetShowMode(int showRow, int showColumn);
        void RefreshShow();
    }

    public class CBoxManager<T> : IBoxManager
        where T : Control, new()
    {
        protected T[] mBoxList = null;
        private int mBoxCount = 0;
        private int mShowIndex = -1;
        private Control mContainer = null;
        private T mActiveBox = null;

        private int mShowRow = 0;
        private int mShowColumn = 0;

        private int mInterval = 2;

        public event InitBoxEventHandle<T> OnInitBox = null;
        public event SetBoxEventHandle<T> OnSetBox = null;

        public event BoxEventHandle<T> OnActiveBoxChanging = null;
        public event BoxEventHandle<T> OnActiveBoxChanged = null;

        public event ShowModeChangedEventHandle OnShowModeChanged = null;
        public event ShowIndexChangedEventHandle OnShowIndexChanged = null;

        public CBoxManager()
        {
            //
        }

        public CBoxManager(Control container)
        {
            Container = container;
        }

        public T[] BoxList
        {
            get { return mBoxList; }
        }

        public Control Container
        {
            get { return mContainer; }
            set
            {                
                if (mContainer != value)
                {
                    if (mContainer != null)
                    {
                        mContainer.Resize -= new EventHandler(DoContainerResize);
                    }

                    mContainer = value;

                    if (mContainer != null)
                    {
                        mContainer.Resize += new EventHandler(DoContainerResize);
                    }
                }
            }
        }

        public int Interval
        {
            get { return mInterval; }
            set 
            {
                if (mInterval != value)
                {
                    mInterval = value;
                    RefreshShow();
                }
            }
        }

        public int ActiveIndex
        {
            get { return (ActiveBox != null && ActiveBox.Tag!=null) ? (int)ActiveBox.Tag : -1; }
            set
            {
                if (mBoxList != null && value < BoxCount)
                {
                    ActiveBox = mBoxList[value];
                }
            }
        }

        public T ActiveBox
        {
            get { return mActiveBox; }
            set 
            {
                if (mActiveBox != value)
                {                    
                    if (mContainer != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            SetActiveBox(value);
                        };
                        mContainer.Invoke(form_invoker);
                    }
                    else
                    {
                        SetActiveBox(value);
                    }
                }
            }
        }

        private void SetActiveBox(T box)
        {
            lock (this)
            {
                if (OnActiveBoxChanging != null)
                    OnActiveBoxChanging(mActiveBox);

                mActiveBox = box;

                if (mActiveBox != null)
                    SetActiveIndex(mActiveBox.TabIndex);                

                if (OnActiveBoxChanged != null)
                    OnActiveBoxChanged(mActiveBox);
            }
        }

        private void SetActiveIndex(int index)
        {
            if (index < BoxCount)
            {
                int showCount = ShowCount;
                int curIndex = index + 1;
                int showIndex = 0;

                if (curIndex > showCount)
                    showIndex = curIndex / showCount + ((curIndex % showCount) > 0 ? 1 : 0) - 1;

                if (ShowIndex != showIndex)
                    ShowIndex = showIndex;
            }
        }

        public void InitBoxs(int boxCount)
        {
            Container.Controls.Clear();

            mBoxList = new T[boxCount];

            T box;
            for (int i = 0; i < boxCount; i++)
            {
                box = new T();
                box.TabIndex = i;

                if (InitBox(box))
                {
                    box.Parent = Container;

                    mBoxList[i] = box;
                }
            }
        }

        public int BoxCount
        {
            get { return mBoxCount; }
            set
            {
                if (mBoxCount != value)
                {
                    mBoxCount = value;

                    InitBoxs(mBoxCount);
                }
            }
        }

        public int ShowCount
        {
            get { return ShowColumn * ShowRow; }
        }

        public int MaxIndex
        {
            get
            {
                int n = ShowRow * ShowColumn;
                if (n == 0) n = 1;

                return BoxCount % n == 0 ? BoxCount / n : BoxCount / n + 1;
            }
        }

        public int ShowIndex
        {
            get { return mShowIndex; }
            set
            {
                if (mShowIndex != value)
                {
                    if (value >= MaxIndex)
                        mShowIndex = MaxIndex - 1;
                    else mShowIndex = value;

                    SetBoxShowMode(ShowRow, ShowColumn, mShowIndex);

                    if (OnShowIndexChanged != null)
                        OnShowIndexChanged(mShowIndex);
                }
            }
        }

        public string ShowMode
        {
            get 
            {
                return string.Format("{0}X{1}", ShowRow, ShowColumn);
            }
            set 
            {
                if (value == null || value.Equals(""))
                {
                    SetShowMode(1, 1);
                    return;
                }
                else if (value.ToUpper().IndexOf("X") > 0)
                {
                    string[] sm = StrUtil.GetSplitArray(value.ToUpper(), "X");
                    if (sm.Length == 2)
                    {
                        SetShowMode(Convert.ToInt32(sm[0]), Convert.ToInt32(sm[1]));
                        return;
                    }
                }
                throw new Exception(string.Format("ShowMode({0})是不合法的格式！",value));
            }
        }

        public void SetShowMode(int showRow, int showColumn)
        {
            if (mShowRow != showRow || mShowColumn != showColumn)
            {
                mShowRow = showRow;
                mShowColumn = showColumn;

                if (mShowIndex >= MaxIndex)
                    mShowIndex = MaxIndex - 1;

                SetBoxShowMode(ShowRow, ShowColumn, ShowIndex);

                if (OnShowModeChanged != null)
                    OnShowModeChanged(mShowRow, mShowColumn);
            }
        }

        public int ShowRow
        {
            get { return mShowRow; }
        }

        public int ShowColumn
        {
            get { return mShowColumn; }
        }

        private void SetBoxShowMode(int showRow, int showColumn, int showIndex)
        {
            if (mBoxList == null) return;
            
            T curBox;

            int boxCount = mBoxList.Length;
            for (int i = 0; i < boxCount; i++)
            {
                curBox = mBoxList[i];

                curBox.Visible = false;
                curBox.Location = new Point(0, 0);
                curBox.Size = new Size(0, 0);                
            }

            if (showRow <= 0) showRow = 2;
            if (showColumn <= 0) showColumn = 2;
            if (showIndex < 0) showIndex = 0;

            int showMode = showRow * showColumn;
            int n;
            for (int i = 0; i < showRow; i++)
            {
                for (int j = 0; j < showColumn; j++)
                {
                    n = (showMode * showIndex) + i * showColumn + j;
                    if (n < boxCount)
                    {
                        curBox = mBoxList[n];

                        curBox.Location = new Point(j * (Container.Width / showColumn) + Interval, i * (Container.Height / showRow) + Interval);
                        curBox.Size = new Size(Container.Width / showColumn - (Interval * 2), Container.Height / showRow - (Interval * 2));
                        curBox.Visible = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            T curActiveBox = ActiveBox;

            for (int i = 0; i < boxCount; i++)
            {
                curBox = mBoxList[i];

                if (!curBox.Visible)
                {
                    SetBox(curBox, showRow, showColumn);

                    if (curBox == curActiveBox)
                    {
                        ActiveBox = null;
                        curActiveBox = null;
                    }
                }
            }

            for (int i = 0; i < boxCount; i++)
            {
                curBox = mBoxList[i];

                if (curBox.Visible)
                {
                    SetBox(curBox, showRow, showColumn);

                    if (curActiveBox == null)
                    {
                        curActiveBox = curBox;
                    }
                }
            }

            if (ActiveBox != curActiveBox)
            {
                ActiveBox = curActiveBox;
            }
        }

        public void RefreshShow()
        {
            SetBoxShowMode(ShowRow, ShowColumn, ShowIndex);
        }

        protected bool InitBox(T box)
        {
            if (OnInitBox != null)
            {
                return OnInitBox(box);
            }
            return true;
        }

        protected void SetBox(T box, int showRow, int showColumn)
        {
            if (OnSetBox != null)
            {
                OnSetBox(box, showRow, showColumn);
            }
            else
            {
                int showMode = showRow * showColumn;
                switch (showMode)
                {
                    case 1:
                        box.Font = new Font("宋体", 19);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        box.Font = new Font("宋体", 13);
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        box.Font = new Font("宋体", 11);
                        break;
                    default:
                        box.Font = new Font("宋体", 9);
                        break;
                }
            }
        }

        protected void DoContainerResize(object sender, EventArgs e)
        {
            RefreshShow();
        }
    }
}
