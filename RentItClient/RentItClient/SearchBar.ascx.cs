using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class SearchBar : System.Web.UI.UserControl
    {
        public event EventHandler Search_Click;
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (Search_Click != null)
            {
                Search_Click(this, EventArgs.Empty);
            }
        }

        protected void bth_searchAdvanced_Click(object sender, EventArgs e)
        {

        }
    }
}