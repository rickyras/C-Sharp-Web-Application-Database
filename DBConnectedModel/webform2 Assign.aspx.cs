using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace A3RitchiePerera
{
    public partial class webform2_Assign : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["NorthwindConnectionString1"].ConnectionString;
        
        DataSet ds = new DataSet();


        protected void Page_Load(object sender, EventArgs e)
        {

           
            string query1 = "Select CategoryID, CategoryName from Categories;";

           

            try
            {
                if (!IsPostBack)
                {
                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand(query1, conn);
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        lbCat.DataSource = reader;
                        lbCat.DataTextField = "CategoryName";
                        lbCat.DataValueField = "CategoryID";
                        lbCat.DataMember = "Order";
                        lbCat.DataBind();
                        ddlProducts.Enabled = false;
                        ListItem liSelect = new ListItem("Select a Product", "-1");
                        ddlProducts.Items.Insert(0, liSelect);
                        ds.Clear();

                    }

                    

                }
            }
            catch (HttpParseException ex)
            {

            }
            catch (HttpCompileException ex)
            {

            }
            catch (InvalidOperationException ex)
            {

            }
            finally
            {

                ds.Clear();

            }



        }//enable postback on the dropdownlist


protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string index = lbCat.SelectedValue;
            ddlProducts.Items.Clear();
            ds.Clear();
            grdOrder.AutoGenerateColumns = false;
            ddlProducts.Enabled = true;
           
            //something to do with index value and value of productID!!!!!!!
            string query = "SELECT ProductID, ProductName FROM Products WHERE (Products.CategoryID ='" + index + "');";
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlProducts.DataSource = reader;


                    ddlProducts.DataTextField = "ProductName";
                    ddlProducts.DataValueField = "ProductID";
                    ddlProducts.DataMember = "Order";

                    ddlProducts.DataBind();

                }
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                grdOrder.AutoGenerateColumns = false;
            }
   
        }

        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)

        {

            string index = lbCat.SelectedItem.Value;

            string index2 = ddlProducts.SelectedValue;
            //must set Auto Generate Coluumns to true on gridview
            //make sure enable view state of false so that it does not inherit previous displaying

            string query = "SELECT Orders.OrderID, Orders.OrderDate, Orders.ShippedDate, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipCountry AS ShipCountry FROM Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID INNER JOIN Products ON [Order Details].ProductID = Products.ProductID INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID WHERE (Products.ProductID ='" + index2 + "');";


            string curItem = lbCat.SelectedItem.ToString();


            try
            {
                if (!ddlProducts.SelectedValue.Equals(-1))
                {
                    grdOrder.AutoGenerateColumns = true;
                    SqlConnection conn = new SqlConnection(cs);
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(ds);
                    ds.Tables[0].TableName = "Orders.OrderID";



                    grdOrder.DataSource = ds.Tables["Orders.OrderID"];
                    grdOrder.DataBind();


                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                grdOrder.AutoGenerateColumns = true;               

            }

        }

        protected void grdOrder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}