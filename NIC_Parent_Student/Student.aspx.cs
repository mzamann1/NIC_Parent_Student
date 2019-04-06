﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NIC_Parent_Student
{
    public partial class Student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                HideGrid();
                GetData();
                RowWork();
            }
            HideGrid();
            

            // GetGuardian();
        }
        int i;
        private void GetGuardian()
        {
            using (SampleDataContext dbContext = new SampleDataContext())
            {
                dbContext.guardians.ToList();
            }
        }

        private void GetData()
        {
            using (SampleDataContext dbContext = new SampleDataContext())
            {
                GridView1.DataSource = dbContext.students.ToList();
                GridView1.DataBind();
            }
           
            
        }

       

        private void InsertOrUpdate(int ind)
        {
            
            using (SampleDataContext dbContext = new SampleDataContext())
            {
                if (ind == 0)
                {
                    student newstd = new student()
                    {
                        std_Fname = txtFirstName.Text,
                        std_Lname = txtLastName.Text,
                        std_class = Convert.ToInt32(txtClass.Text)
                    };
                    dbContext.students.InsertOnSubmit(newstd);
                }
                else
                {
                    student student_obj = dbContext.students.Single(x => x.std_id == ind);
                    if (student_obj == null)
                    {
                        Response.Write("<script>alert('Not Found')</script>");
                    }
                    else
                    {
                        student_obj.std_id = ind;
                        student_obj.std_Fname = txtFirstName.Text;
                        student_obj.std_Lname = txtLastName.Text;
                        student_obj.std_class = Convert.ToInt32(txtClass.Text);
                    }
                }

                
                dbContext.SubmitChanges();
                Response.Write("<script>alert('Changes Saved!')</script>");
                Clear();
                GetData();
                RowWork();
            }
        }


        private void RowWork()
        {
            if (GridView1.DataSource!=null)
            {
                using (SampleDataContext context = new SampleDataContext())
                {
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        LinkButton lb_add = (LinkButton)GridView1.Rows[i].FindControl("AddGuardian");
                        int id = Convert.ToInt32(lb_add.CommandArgument);


                        LinkButton lb_view= (LinkButton)GridView1.Rows[i].FindControl("ViewSingleGuardian");
                        guardian g_check = context.guardians.Where(x => x.std_id == id).SingleOrDefault();
                        if (g_check!=null)
                        {
                            lb_add.Enabled = false;
                            lb_view.Enabled = true;

                        }
                        else
                        {
                            lb_add.Enabled = true;
                            lb_view.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                InsertOrUpdate(0);
            }
            else
            {
                Response.Write("<script>alert('Cannot Update Data Due to Validations Error!!')</script>");
            }
            
        }
        private bool isValid()
        {
            if ((!string.IsNullOrWhiteSpace(txtFirstName.Text) && txtFirstName.Text.All(char.IsLetter)) && (!string.IsNullOrWhiteSpace(txtLastName.Text) && txtLastName.Text.All(char.IsLetter)) && (!string.IsNullOrWhiteSpace(txtClass.Text) && txtClass.Text.All(char.IsDigit)))
            {
                return true;
            }
            else return false;
        }

       

        protected void AddGuardian_Click(object sender, EventArgs e)
        {

        }
       
        protected void AddGuardian_Click1(object sender, EventArgs e)
        {
           
                hf1.Value = (sender as LinkButton).CommandArgument;
            Server.Transfer("~/AddGuardian.aspx");
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            btnInsert.Enabled = false;
            HideGrid();
            hf1.Value= (sender as LinkButton).CommandArgument;
            i = Convert.ToInt32(hf1.Value);
            using (SampleDataContext context = new SampleDataContext())
            {
                student std = context.students.SingleOrDefault(x => x.std_id == i);

                if (std!=null)
                {
                    txtFirstName.Text = std.std_Fname;
                    txtLastName.Text = std.std_Lname;
                    txtClass.Text = std.std_class.ToString();
                    
                }
            }

        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            HideGrid();
            hf1.Value = (sender as LinkButton).CommandArgument;
            i = Convert.ToInt32(hf1.Value);
            using (SampleDataContext context = new SampleDataContext())
            {

                guardian g = context.guardians.Where(x => x.std_id == i).SingleOrDefault();
                if (g != null)
                {
                    g.std_id = null;

                    context.SubmitChanges();
                }
                student std = context.students.SingleOrDefault(x=>x.std_id==i);
                context.students.DeleteOnSubmit(std);
                context.SubmitChanges();
                Clear();
                GetData();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }
        private void Clear()
        {
            txtFirstName.Text  = txtLastName.Text=txtClass.Text   = "";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           int  id = Convert.ToInt32(hf1.Value);
            if (isValid())
            {
                InsertOrUpdate(id);
            }
            else
            {
                Response.Write("<script>alert('Cannot Update Data Due to Validations Error!!')</script>");
            }
        }

        
        private void HideGrid()
        {
            GridView2.Visible = false;
        }

        protected void ViewSingleGuardian_Click1(object sender, EventArgs e)
        {
            hf1.Value = (sender as LinkButton).CommandArgument;
            i = Convert.ToInt32(hf1.Value);
            using (SampleDataContext context = new SampleDataContext())
            {
                GridView2.DataSource = context.guardians.Where(x => x.std_id == i).ToList();
                GridView2.DataBind();
                GridView2.Visible = true;
            }
        }
    }
}