﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagement
{
    
    public partial class adminbookinventory : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        static string global_filepath;
        static int global_actual_stock, global_current_stock, global_issued_books;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillAuthorPublisherValues();
            }
            GridView1.DataBind();
        }

        //add button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                Response.Write("<script>alert('Book ID already exists');</script>");
            }
            else
            {
                addNewBook();
            }    
        }
        //update button
        protected void Button1_Click(object sender, EventArgs e)
        {
            updateBookByID();
        }
        //delete button
        protected void Button3_Click(object sender, EventArgs e)
        {
            deleteBookByID();
        }
        //search button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getBookByID();
        }

        void deleteBookByID()
        {
            if (checkBookExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE FROM book_master_tbl WHERE book_id='" + TextBox3.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Book deleted successfully');</script>");
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid book ID');</script>");
            }
        }
        void updateBookByID()
        {
            if (checkBookExists())
            {
                try
                {
                    int actual_stock = Convert.ToInt32(TextBox9.Text.Trim());
                    int current_stock = Convert.ToInt32(TextBox10.Text.Trim());

                    if (global_actual_stock == actual_stock)
                    {

                    }
                    else
                    {
                        if (actual_stock < global_issued_books)
                        {
                            Response.Write("<script>alert('Actual Stock value cannot be less than the Issued books');</script>");
                            return;
                        }
                        else
                        {
                            current_stock = actual_stock - global_issued_books;
                            TextBox10.Text = "" + current_stock;
                        }
                    }



                    string filepath = "~/book_inventory/book1";
                    string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (string.IsNullOrEmpty(filename))
                    {
                        filepath = global_filepath;
                    }
                    else
                    {
                        FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                        filepath = "~/book_inventory/" + filename;
                    }

                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl SET book_name=@book_name, genre=@genre, " +
                        "author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, language=@language, " +
                        "edition=@edition, book_cost=@book_cost, no_of_pages=@no_of_pages, book_description=@book_description, " +
                        "actual_stock=@actual_stock, current_stock=@current_stock, book_img_link=@book_img_link where book_id='"
                        + TextBox3.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", DropDownList2.SelectedValue);
                    cmd.Parameters.AddWithValue("@author_name", DropDownList4.SelectedValue);
                    cmd.Parameters.AddWithValue("@publisher_name", DropDownList3.SelectedValue);
                    cmd.Parameters.AddWithValue("@publish_date", TextBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedValue);
                    cmd.Parameters.AddWithValue("@edition", TextBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_cost", TextBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_of_pages", TextBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_description", TextBox11.Text.Trim());
                    cmd.Parameters.AddWithValue("@actual_stock", actual_stock.ToString());
                    cmd.Parameters.AddWithValue("@current_stock", current_stock.ToString());
                    cmd.Parameters.AddWithValue("@book_img_link", filepath);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    getBookByID();
                    GridView1.DataBind();
                    Response.Write("<script>alert('Book updated succesfully');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message.Replace("'", "\\'") + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid book ID');</script>");
            }
        }
        
        void getBookByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id = '" + TextBox3.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) 
                {
                    TextBox4.Text = dt.Rows[0]["book_name"].ToString();
                    TextBox2.Text = dt.Rows[0]["publish_date"].ToString();
                    TextBox8.Text = dt.Rows[0]["edition"].ToString();
                    TextBox6.Text = dt.Rows[0]["book_cost"].ToString().Trim();
                    TextBox5.Text = dt.Rows[0]["no_of_pages"].ToString().Trim();
                    TextBox9.Text = dt.Rows[0]["actual_stock"].ToString().Trim();
                    TextBox10.Text = dt.Rows[0]["current_stock"].ToString().Trim();
                    TextBox11.Text = dt.Rows[0]["book_description"].ToString();
                    TextBox7.Text = "" + (Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString()) - Convert.ToInt32(dt.Rows[0]["current_stock"].ToString()));

                    DropDownList1.SelectedValue = dt.Rows[0]["language"].ToString().Trim();
                    DropDownList2.SelectedValue = dt.Rows[0]["genre"].ToString().Trim();
                    DropDownList3.SelectedValue = dt.Rows[0]["publisher_name"].ToString().Trim();
                    DropDownList4.SelectedValue = dt.Rows[0]["author_name"].ToString().Trim();

                    global_actual_stock = Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString().Trim());
                    global_current_stock = Convert.ToInt32(dt.Rows[0]["current_stock"].ToString().Trim());
                    global_issued_books = global_actual_stock - global_current_stock;
                    global_filepath = dt.Rows[0]["book_img_link"].ToString();
                }
                else
                {
                    Response.Write("<script>alert('Invalid Book ID');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


        void fillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT author_name from author_master_tbl;", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DropDownList4.DataSource = dt;
                DropDownList4.DataValueField = "author_name";
                DropDownList4.DataBind();

                cmd = new SqlCommand("SELECT publisher_name from publisher_master_tbl", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                DropDownList3.DataSource = dt;
                DropDownList3.DataValueField = "publisher_name";
                DropDownList3.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        bool checkBookExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id='" + TextBox3.Text.Trim() + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }

        }

        void addNewBook()
        {
            try
            {
                string filepath = "~/book_inventory/book1.jpg";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                filepath = "~/book_inventory/" + filename;

                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl (book_id,book_name,genre,author_name,publisher_name,publish_date," +
                    "language,edition,book_cost,no_of_pages,book_description,actual_stock,current_stock,book_img_link) " +
                    "VALUES (@book_id,@book_name,@genre,@author_name,@publisher_name,@publish_date,@language,@edition,@book_cost," +
                    "@no_of_pages,@book_description,@actual_stock,@current_stock,@book_img_link)", con);

                cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", DropDownList2.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@author_name", DropDownList4.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publisher_name", DropDownList3.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publish_date", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition", TextBox8.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_pages", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", TextBox11.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@current_stock", TextBox10.Text.Trim());
                cmd.Parameters.AddWithValue("@book_img_link", filepath);


                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book added succesfully');</script>");
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message.Replace("'", "\\'") + "');</script>");

            }
        }
    }
}