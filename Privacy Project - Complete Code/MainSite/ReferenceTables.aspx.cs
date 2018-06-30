using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Reflection;

/*
    Web Service Privacy, Compatibility and k-Anonymity
   - Integration of k-Anonymity of a WS method into the compatibility checking process of an existing privacy framework     
   Michael Edwards 
   CIS 695 - Graduate Project
  
*/

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblRuleAdd.Text = "";
        if (!Page.IsPostBack)
        {
            drpRule.DataBind();
            drpRule.Items.Insert(0, new ListItem("", ""));

            drpTopic.DataBind();
            drpTopic.Items.Insert(0, new ListItem("", ""));

            drpDomain.DataBind();
            drpDomain.Items.Insert(0, new ListItem("", ""));

            drpLevel.DataBind();
            drpLevel.Items.Insert(0, new ListItem("", ""));

            drpScope.DataBind();
            drpScope.Items.Insert(0, new ListItem("", ""));
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if ((drpRule.Text == "") ||
           (drpTopic.Text == "") ||
           (drpLevel.Text == "") ||
           (drpDomain.Text == "") ||
           (drpScope.Text == ""))
        {
            lblRuleAdd.Text = "Please select a value in each dropdown";
            return;
        }

        string strRuleValue = drpRule.SelectedValue.ToString();
        string strTopicValue = drpTopic.SelectedValue.ToString();
        string strLevel = drpLevel.SelectedValue.ToString();
        string strDomain = drpDomain.SelectedValue.ToString();
        string strScope = drpScope.SelectedValue.ToString();

        int intRuleValue, intTopicValue, intLevel, intDomain, intScope;

        bool blnSuccess = false;
        blnSuccess = int.TryParse(strRuleValue, out intRuleValue);
        blnSuccess = int.TryParse(strTopicValue, out intTopicValue);
        blnSuccess = int.TryParse(strLevel, out intLevel);
        blnSuccess = int.TryParse(strDomain, out intDomain);
        blnSuccess = int.TryParse(strScope, out intScope);

        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();
            //***************************************
            // get the info for the web service

            SqlCommand cmd = new SqlCommand("spAdd_PrivacyRuleSetItem");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@rule_id", SqlDbType.Int)).Value = intRuleValue;
            cmd.Parameters.Add(new SqlParameter("@topic_id", SqlDbType.Int)).Value = intTopicValue;
            cmd.Parameters.Add(new SqlParameter("@level_id", SqlDbType.Int)).Value = intLevel;
            cmd.Parameters.Add(new SqlParameter("@domain_id", SqlDbType.Int)).Value = intDomain;
            cmd.Parameters.Add(new SqlParameter("@scope_id", SqlDbType.Int)).Value = intScope;
            SqlParameter param = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar);
            param.Direction = ParameterDirection.Output;
            param.Size = 500;
            cmd.Parameters.Add(param);

            cmd.Connection = conn;
            var retInfo = cmd.ExecuteNonQuery();
            string strReturnMessage = cmd.Parameters["@ReturnMessage"].Value.ToString();
            lblRuleAdd.Text = strReturnMessage;

            grdPrivacyRuleSetItems.DataBind();
        }
        catch (Exception exc)
        {            
            lblRuleAdd.Text = "Error: " + exc.Message.ToString();

            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Reference Tables Page, btnSubmit_Click: " + exc.Message.ToString());
        }
        finally
        {
            conn.Close();
        }
    }

    protected void grdPrivacyRuleSetItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int intRowIndex = e.RowIndex;
        grdPrivacyRuleSetItems.SelectRow(intRowIndex);
        GridViewRow row = grdPrivacyRuleSetItems.SelectedRow;        
        string strRuleValue = row.Cells[2].Text;
        string strTopicValue = row.Cells[4].Text;
        string strLevel = row.Cells[6].Text;
        string strDomain = row.Cells[8].Text;
        string strScope = row.Cells[10].Text;
                
        try
        {
            SqlDataSource10.DeleteCommand = "spDelete_PrivacyRuleSetItem";
            SqlDataSource10.DeleteCommandType = SqlDataSourceCommandType.StoredProcedure;
            SqlDataSource10.DeleteParameters["rule_id"].DefaultValue = strRuleValue;
            SqlDataSource10.DeleteParameters["topic_id"].DefaultValue = strTopicValue;
            SqlDataSource10.DeleteParameters["level_id"].DefaultValue = strLevel;
            SqlDataSource10.DeleteParameters["domain_id"].DefaultValue = strDomain;
            SqlDataSource10.DeleteParameters["scope_id"].DefaultValue = strScope;

            SqlDataSource10.Delete();
        }
        catch (Exception exc)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Reference Tables Page, grdPrivacyRuleSetItems_RowDeleting: " + exc.Message.ToString());
        }
        finally
        {           
        }
    }
}
