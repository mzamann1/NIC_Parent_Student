using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NIC_Parent_Student
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            using (SampleDataContext dbcontext = new SampleDataContext())
            {
                GridView1.DataSource = dbcontext.guardians.ToList();
                GridView1.DataBind();
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            int id =Convert.ToInt32((sender as LinkButton).CommandArgument);

            using (SampleDataContext dbContext = new SampleDataContext())
            {
                GridView2.DataSource = dbContext.students.FirstOrDefault(x => x.std_id == id);
                GridView2.DataBind();
            }
        }
    }
}