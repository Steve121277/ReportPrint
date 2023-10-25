using ReportPrint.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ReportPrint.OwnControls
{
    public partial class UserDataListView : ListView
    {
        private IEnumerable<Model.IUserData> userDatas = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal IEnumerable<Model.IUserData> UserDatas
        {
            get { return this.userDatas; }
            set
            {
                this.userDatas = value;

                if (this.userDatas != null)
                {
                    this.VirtualListSize = this.userDatas.Count();
                }
                else
                {
                    this.VirtualListSize = 0;
                }

                myCache = null;
                this.Invalidate();
            }
        }

        internal Model.IUserData SelectedUserData
        {
            get
            {
                if (SelectedIndices.Count > 0)
                {
                    int selectedIndex = SelectedIndices[0];

                    IUserData userData = (IUserData)Items[selectedIndex].Tag;

                    return userData;
                }
                else
                {
                    return null;
                }
            }
        }

        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache

        public UserDataListView()
        {
            InitializeComponent();

            this.VirtualMode = true;

            this.FullRowSelect = true;

            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                new System.Windows.Forms.ColumnHeader() { Text = "日付", Width = 90, TextAlign = HorizontalAlignment.Center },
                new System.Windows.Forms.ColumnHeader() { Text = "測定", Width = 80, TextAlign = HorizontalAlignment.Center },
                new System.Windows.Forms.ColumnHeader() { Text = "結果", Width = 70, TextAlign = HorizontalAlignment.Right }
            });

            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(OnRetrieveVirtualItem);
            this.CacheVirtualItems += new CacheVirtualItemsEventHandler(OnCacheVirtualItems);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            //Caching is not required but improves performance on large sets.
            //To leave out caching, don't connect the CacheVirtualItems event 
            //and make sure myCache is null.

            //check to see if the requested item is currently in the cache
            if (myCache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + myCache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = myCache[e.ItemIndex - firstItem];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                e.Item = GetListViewItemFromUserIndex(e.ItemIndex);
            }
        }

        void OnCacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            //We've gotten a request to refresh the cache.
            //First check if it's really neccesary.
            if (myCache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + myCache.Length)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            firstItem = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1; //indexes are inclusive
            myCache = new ListViewItem[length];

            //Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; i++)
            {
                myCache[i] = GetListViewItemFromUserIndex(i + firstItem);
            }
        }

        private ListViewItem GetListViewItemFromUserIndex(int Index)
        {
            if (this.userDatas == null ||
                this.userDatas.Count() <= Index ||
                Index < 0)
            {
                return null;
            }

            Model.IUserData userData = userDatas.ElementAt(Index);

            ListViewItem item = new ListViewItem(userData.MeasureTime.ToString("yyyy-M-d"));

            item.SubItems.Add(userData.GameTitle);
            item.SubItems.Add(userData.GaneScore.ToString("#,#.0#"));
            item.Tag = userData;

            return item;
        }
    }
}
