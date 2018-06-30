using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.IO;

/*
    Web Service Privacy, Compatibility and k-Anonymity
   - Integration of k-Anonymity of a WS method into the compatibility checking process of an existing privacy framework     
   Michael Edwards 
   CIS 695 - Graduate Project
  
*/
public partial class About : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatusMessage_AddRuleItemToClient.Text = "";
        lblStatusMessage_ClientPrivacyRules.Text = "";

        AssertionsGrid.DataKeyNames = new string[] 
        { 
            "target_WSID", "method_id", "resource_id", "rule_id", "rule_item_id", 
            "topic_id", "level_id", "domain_id", "scope_id"
        };

        // make certain coluns invisible to user
        AssertionsGrid.Columns[0].Visible = false; // target_WSID
        AssertionsGrid.Columns[2].Visible = false; // method_id
        AssertionsGrid.Columns[4].Visible = false; // resource_id

        AssertionsGrid.Columns[6].Visible = false; // rule_id
        AssertionsGrid.Columns[8].Visible = false; // rule_item_id
        AssertionsGrid.Columns[9].Visible = false; // topic_id
        AssertionsGrid.Columns[11].Visible = false; // level_id
        AssertionsGrid.Columns[13].Visible = false; // domain_id
        AssertionsGrid.Columns[15].Visible = false; // scope_id        

        if (!Page.IsPostBack)
        {
            drpWebService.DataBind();
            drpWebService.Items.Insert(0, new ListItem("", ""));
            drpMethods.DataBind();
            drpMethods.Items.Insert(0, new ListItem("", ""));
            drpResources.DataBind();
            drpResources.Items.Insert(0, new ListItem("", ""));
        }        

    }
    protected void grdPrivacyRuleItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        // add selected rule item to set of client privacy rule items (if it is not there already)
        GridViewRow row = grdPrivacyRuleItems.SelectedRow;
                
        string strWSID = drpWebService_forWS.SelectedItem.Value;
        string strRuleID = row.Cells[2].Text;
        string strRuleItemID = row.Cells[3].Text;

        int intWS_ID, intRule_ID, intRule_Item_ID;
        bool success1 = int.TryParse(strRuleID, out intRule_ID);
        bool success2 = int.TryParse(strRuleItemID, out intRule_Item_ID);
        bool success3 = int.TryParse(strWSID, out intWS_ID);

        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();
            
            // get the info for the web service
            SqlCommand cmd = new SqlCommand("spAdd_PrivacyRuleSetItem_forWS_FromGrid");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ws_id", SqlDbType.Int)).Value = intWS_ID;
            cmd.Parameters.Add(new SqlParameter("@rule_id", SqlDbType.Int)).Value = intRule_ID;
            cmd.Parameters.Add(new SqlParameter("@rule_item_id", SqlDbType.Int)).Value = intRule_Item_ID;
            cmd.Connection = conn;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();

            reader.Read();
            string strReturnMessage = reader.GetValue(0).ToString();
            lblStatusMessage_AddRuleItemToClient.Text = "* Message: " + strReturnMessage;
            grdWSPrivPolicyItems.DataBind();
            conn.Close();
        }
        catch (Exception exc)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Privacy Policies, grdPrivacyRuleItems_SelectedIndexChanged: " + exc.Message.ToString());
        }
        finally
        {
            conn.Close();
        }
    }
    protected void grdWSPrivPolicyItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = grdWSPrivPolicyItems.SelectedRow;

        drpWebService.SelectedValue = row.Cells[1].Text;
        txtRuleName.Text = row.Cells[2].Text;
        txtRuleID.Text = row.Cells[3].Text;
        txtRuleItemID.Text = row.Cells[4].Text;
        txtTopicID.Text = row.Cells[5].Text;
        txtTopicDesc.Text = row.Cells[6].Text;
        txtLevelID.Text = row.Cells[7].Text;
        txtLevelDesc.Text = row.Cells[8].Text;
        txtDomainID.Text = row.Cells[9].Text;
        txtDomainDesc.Text = row.Cells[10].Text;
        txtScopeID.Text = row.Cells[11].Text;
        txtScopeDesc.Text = row.Cells[12].Text;

    }
    protected void grdWSPrivPolicyItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int intRowIndex = e.RowIndex;
        grdWSPrivPolicyItems.SelectRow(intRowIndex);
        GridViewRow row = grdWSPrivPolicyItems.SelectedRow;

        string strWSID = row.Cells[1].Text;
        string strRuleID = row.Cells[3].Text;
        string strRuleItemID = row.Cells[4].Text;

        int intRule_ID, intRule_Item_ID;
        bool success1 = int.TryParse(strRuleID, out intRule_ID);
        bool success2 = int.TryParse(strRuleItemID, out intRule_Item_ID);

        try
        {
            SqlDataSource3.DeleteCommand = "spDelete_PrivacyRuleSetItem_ForWS_FromGrid";
            SqlDataSource3.DeleteCommandType = SqlDataSourceCommandType.StoredProcedure;
            SqlDataSource3.DeleteParameters["ws_id"].DefaultValue = strWSID;
            SqlDataSource3.DeleteParameters["rule_id"].DefaultValue = strRuleID;
            SqlDataSource3.DeleteParameters["rule_item_id"].DefaultValue = strRuleItemID;            
            SqlDataSource3.Delete();
           
            lblStatusMessage_ClientPrivacyRules.Text = "* Item has been deleted (unless it is being used by an assertion).";
        }
        catch (Exception exc)
        {
            lblStatusMessage_ClientPrivacyRules.Text = "* Message: Error Deleting Rule Item From WS";

            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Privacy Policies, grdWSPrivPolicyItems_RowDeleting: " + exc.Message.ToString());
        }
        finally
        {
        }
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strWSID = drpWebService.SelectedValue;
        string strMethodID = drpMethods.SelectedItem.Value;
        string strRuleID = txtRuleID.Text;
        string strRuleItemID = txtRuleItemID.Text;
        string strResourceID = drpResources.SelectedItem.Value;

        int intWS_ID, intMethodID, intRule_ID, intRule_Item_ID, intResource_ID;
        bool success = false;
        success = int.TryParse(strWSID, out intWS_ID);
        success = int.TryParse(strMethodID, out intMethodID);
        success = int.TryParse(strRuleID, out intRule_ID);
        success = int.TryParse(strRuleItemID, out intRule_Item_ID);
        success = int.TryParse(strResourceID, out intResource_ID);

        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();
            //***************************************
            // get the info for the web service

            SqlCommand cmd = new SqlCommand("spAdd_AssertionWS_FromGrid");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ws_id", SqlDbType.Int)).Value = intWS_ID;
            cmd.Parameters.Add(new SqlParameter("@method_id", SqlDbType.Int)).Value = intMethodID;
            cmd.Parameters.Add(new SqlParameter("@rule_id", SqlDbType.Int)).Value = intRule_ID;
            cmd.Parameters.Add(new SqlParameter("@rule_item_id", SqlDbType.Int)).Value = intRule_Item_ID;
            cmd.Parameters.Add(new SqlParameter("@resource_id", SqlDbType.Int)).Value = intResource_ID;
            SqlParameter param = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar);            
            param.Direction = ParameterDirection.Output;
            param.Size = 500;
            cmd.Parameters.Add(param);
            cmd.Connection = conn;

            var retInfo = cmd.ExecuteNonQuery();
            string strReturnMessage = cmd.Parameters["@ReturnMessage"].Value.ToString();  
            lblAssertionAdd.Text = strReturnMessage;
            AssertionsGrid.DataBind();

        }
        catch (Exception exc)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Privacy Policies, btnSubmit_Click: " + exc.Message.ToString());
        }
        finally
        {
            conn.Close();
        }

    }
    protected void drpWebService_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpWebService.SelectedIndex > 0)
        {
            string strValue = drpWebService.SelectedItem.Value;
        }
    }
    protected void AssertionsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int intRowIndex = e.RowIndex;
        AssertionsGrid.SelectRow(intRowIndex);
        GridViewRow row = AssertionsGrid.SelectedRow;

        string strWS_ID = ""; // = "6";   
        strWS_ID = AssertionsGrid.DataKeys[row.RowIndex]["target_WSID"].ToString();

        string strMethodID = AssertionsGrid.DataKeys[row.RowIndex]["method_id"].ToString();
        string strRuleID = AssertionsGrid.DataKeys[row.RowIndex]["rule_id"].ToString();
        string strRuleItemID = AssertionsGrid.DataKeys[row.RowIndex]["rule_item_id"].ToString();
        string strResourceID = AssertionsGrid.DataKeys[row.RowIndex]["resource_id"].ToString();

        try
        {
            AssertionsGrid.DataBind();
            SqlDataSource4.DeleteCommand = "spDelete_AssertionWS_FromGrid";
            SqlDataSource4.DeleteCommandType = SqlDataSourceCommandType.StoredProcedure;
            SqlDataSource4.DeleteParameters["ws_id"].DefaultValue = strWS_ID;
            SqlDataSource4.DeleteParameters["method_id"].DefaultValue = strMethodID;
            SqlDataSource4.DeleteParameters["rule_id"].DefaultValue = strRuleID;
            SqlDataSource4.DeleteParameters["rule_item_id"].DefaultValue = strRuleItemID;
            SqlDataSource4.DeleteParameters["resource_id"].DefaultValue = strResourceID;

            // ***************  extra items to avoid bug (Microsoft counts added Key values as part of procedure params)
            SqlDataSource4.DeleteParameters["target_WSID"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["topic_id"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["level_id"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["domain_id"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["scope_id"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["weight"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["mandatory_flag"].DefaultValue = "";
            SqlDataSource4.DeleteParameters["target_ws_id"].DefaultValue = "";
            //********************************************

            SqlDataSource4.Delete();

        }
        catch (Exception exc)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Privacy Policies, AssertionsGrid_RowDeleting: " + exc.Message.ToString());
        }
        finally
        {

        }
    }    

    int GetColumnIndexByName(GridViewRow row, string columnName)
    {
        int columnIndex = 0;
        foreach (DataControlFieldCell cell in row.Cells)
        {
            if (cell.ContainingField is BoundField)
                if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                    break;
            columnIndex++; // keep adding 1 while we don't have the correct name
        }
        return columnIndex;
    }    
 
    protected void grdWSPrivPolicyItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = (GridViewRow)e.Row;
        TableCell selectCell = row.Cells[0];
        LinkButton selectControl = null;

        if (selectCell.Controls.Count > 0)
        {
            selectControl = selectCell.Controls[2] as LinkButton;
            if (selectControl != null)
            {
                selectControl.Text = "SelForAssert";
            }
        }
    }
    protected void grdPrivacyRuleItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = (GridViewRow)e.Row;
        TableCell selectCell = row.Cells[0];
        LinkButton selectControl = null;

        if (selectCell.Controls.Count > 0)
        {
            selectControl = selectCell.Controls[0] as LinkButton;
            if (selectControl != null)
            {
                selectControl.Text = "AddToWS";
            }
        }
    }
    
}
