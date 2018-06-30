<%@ Page Title="Privacy Policies" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PrivacyPolicies.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     <table border="0" align=left>
          <tr>
              <td class="style1"><b><font size="5">Web Service Privacy Policies</font></b></td>
          </tr>
           <tr><td></td></tr>
          <tr><td><b><asp:Label ID="lblStatusMessage_AddRuleItemToClient" runat="server"></asp:Label></b></td></tr>
          <tr>
              <td class="style1"><br /><b>Privacy Policy Items - Possibilities Available for WS (full set of privacy rules and 
        constituent items to choose from)</b></td>
          </tr>
          <tr><td class="style1"><u>AddToWS</u> adds the item to the grid Privacy Rules Items for WS below</td></tr>
          <tr>
              <td class="style1">
                         <asp:DropDownList ID="drpWebService_forWS" runat="server" 
                             DataSourceID="sqlWebService_forWS" DataTextField="Name" DataValueField="WS_ID" 
                             Width="109px" 
                             onselectedindexchanged="drpWebService_SelectedIndexChanged">
                         </asp:DropDownList>
                     <asp:SqlDataSource ID="sqlWebService_forWS" runat="server" 
                             ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                             
                      SelectCommand="SELECT [WS_ID], [Name] FROM [ClientAndServices] WHERE ([WS_ID] &lt;&gt; @WS_ID) ORDER BY [Name]">
                         <SelectParameters>
                             <asp:Parameter DefaultValue="6" Name="WS_ID" Type="Int32" />
                         </SelectParameters>
                         </asp:SqlDataSource></td>
          </tr>
     </table>   
     
    <p>&nbsp;</p> 
     <p>
         <asp:GridView ID="grdPrivacyRuleItems" runat="server" AutoGenerateColumns="False" 
             DataSourceID="SqlDataSource1" Width="915px" 
             AutoGenerateSelectButton="True" 
             onselectedindexchanged="grdPrivacyRuleItems_SelectedIndexChanged" 
             onrowdatabound="grdPrivacyRuleItems_RowDataBound">
             <Columns>
                 <asp:BoundField DataField="rule_name" HeaderText="rule_name" 
                     SortExpression="rule_name" />
                 <asp:BoundField DataField="Rule_ID" HeaderText="Rule_ID" 
                     SortExpression="Rule_ID" InsertVisible="False" ReadOnly="True" />
                 <asp:BoundField DataField="rule_item_id" HeaderText="rule_item_id" 
                     SortExpression="rule_item_id" InsertVisible="False" ReadOnly="True" />
                 <asp:BoundField DataField="topic_id" HeaderText="topic_id" 
                     SortExpression="topic_id" />
                 <asp:BoundField DataField="topic_desc" HeaderText="topic_desc" 
                     SortExpression="topic_desc" />
                 <asp:BoundField DataField="level_id" HeaderText="level_id" 
                     SortExpression="level_id" />
                 <asp:BoundField DataField="level_desc" HeaderText="level_desc" 
                     SortExpression="level_desc" />
                 <asp:BoundField DataField="domain_id" HeaderText="domain_id" 
                     SortExpression="domain_id" />
                 <asp:BoundField DataField="domain_desc" HeaderText="domain_desc" 
                     SortExpression="domain_desc" />
                 <asp:BoundField DataField="scope_id" HeaderText="scope_id" 
                     SortExpression="scope_id" />
                 <asp:BoundField DataField="scope_desc" HeaderText="scope_desc" 
                     SortExpression="scope_desc" />
             </Columns>
             <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
         </asp:GridView>
         <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
             ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
             SelectCommand="spGetPrivacyRuleSetItems" 
             SelectCommandType="StoredProcedure">
         </asp:SqlDataSource>
</p>     
    <p>
        
     </p>  
        <table border="0" align=left>
          <tr><td></td></tr>
          <tr><td></td></tr>
          <tr><td><font color="red"><b><asp:Label ID="lblStatusMessage_ClientPrivacyRules" runat="server"></asp:Label></b></label></td></tr>
           <tr><td class="style1"><br /><b>Privacy Policy Items for WS (Rule Items available to be used in 
                 a propositional formula).</td>
          </tr>
         <tr><td class="style1"><u>SelectForAssertion</u> adds  adds the item to the Assertion Form below</td></tr>
         <tr><td class="style1"></td></tr>
          <tr>
             <td>
                 <asp:GridView ID="grdWSPrivPolicyItems" runat="server" AutoGenerateColumns="False" 
                     DataSourceID="SqlDataSource3" Width="915px" 
                     AutoGenerateDeleteButton="True" 
                     onrowdeleting="grdWSPrivPolicyItems_RowDeleting" 
                     onselectedindexchanged="grdWSPrivPolicyItems_SelectedIndexChanged" AutoGenerateSelectButton="True" 
                     onrowdatabound="grdWSPrivPolicyItems_RowDataBound">
                     <Columns>
                         <asp:BoundField DataField="ws_id" HeaderText="ws_id" 
                             SortExpression="ws_id" />
                         <asp:BoundField DataField="rule_name" HeaderText="rule_name" 
                             SortExpression="rule_name" />
                         <asp:BoundField DataField="Rule_ID" HeaderText="Rule_ID" 
                             InsertVisible="False" ReadOnly="True" SortExpression="Rule_ID" />
                         <asp:BoundField DataField="rule_item_id" HeaderText="rule_item_id" 
                             SortExpression="rule_item_id" InsertVisible="False" ReadOnly="True" />
                         <asp:BoundField DataField="topic_id" HeaderText="topic_id" 
                             SortExpression="topic_id" />
                         <asp:BoundField DataField="topic_desc" HeaderText="topic_desc" 
                             SortExpression="topic_desc" />
                         <asp:BoundField DataField="level_id" HeaderText="level_id" 
                             SortExpression="level_id" />
                         <asp:BoundField DataField="level_desc" HeaderText="level_desc" 
                             SortExpression="level_desc" />
                         <asp:BoundField DataField="domain_id" HeaderText="domain_id" 
                             SortExpression="domain_id" />
                         <asp:BoundField DataField="domain_desc" HeaderText="domain_desc" 
                             SortExpression="domain_desc" />
                         <asp:BoundField DataField="scope_id" HeaderText="scope_id" 
                             SortExpression="scope_id" />
                         <asp:BoundField DataField="scope_desc" HeaderText="scope_desc" 
                             SortExpression="scope_desc" />
                     </Columns>
                     <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
                 </asp:GridView>
                 <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                     ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>"                      
                     SelectCommand="spGetPrivacyRuleSetItems_WS" 
                     SelectCommandType="StoredProcedure" 
                     DeleteCommand="spDelete_PrivacyRuleSetItem_ForWS_FromGrid" 
                     DeleteCommandType="StoredProcedure">
                     <DeleteParameters>
                         <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                         <asp:Parameter Name="ws_id" Type="Int32" />
                         <asp:Parameter Name="rule_id" Type="Int32" />
                         <asp:Parameter Name="rule_item_id" Type="Int32" />
                     </DeleteParameters>
                     <SelectParameters>
                         <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                     </SelectParameters>
                 </asp:SqlDataSource>
              </td>
          </tr>          
          <tr>
              <td class="style1">&nbsp;&nbsp;&nbsp;<br /></td>
              <td class="style1"></td>              
               <td class="style1">&nbsp;&nbsp;</td>             
          </tr>           
          <tr>
                <td class="style1"><b>Add WS Assertion</td>
          </tr>
          <tr><td class="style1"><u>Select rule above, fill in other fields, then click submit</u></td></tr>
          <tr><td></td></tr>
          <tr>
                <td class="style1"><font color="red"><asp:Label ID="lblAssertionAdd" runat="server"></asp:Label></font></td>
          </tr>
           <tr><td></td></tr>
          </table>          
          <table border="0" style="border-width: thin; border-color:#000000;border-style: solid;">
              <tr>
                    <td><asp:Label ID="lbl1" runat="server" Text="Web Service:"></asp:Label></td>  
                     <td>
                         <asp:DropDownList ID="drpWebService" runat="server" 
                             DataSourceID="sqlWebService" DataTextField="Name" DataValueField="WS_ID" 
                             Width="109px" AutoPostBack="True" 
                             onselectedindexchanged="drpWebService_SelectedIndexChanged" 
                             Enabled="False">
                         </asp:DropDownList>
                     </td>  
                     <td><asp:SqlDataSource ID="sqlWebService" runat="server" 
                             ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                             SelectCommand="SELECT [WS_ID], [Name] FROM [ClientAndServices] WHERE ([WS_ID] &lt;&gt; @WS_ID) ORDER BY [Name]">
                         <SelectParameters>
                             <asp:Parameter DefaultValue="6" Name="WS_ID" Type="Int32" />
                         </SelectParameters>
                         </asp:SqlDataSource></td>                 
              </tr> 
              <tr><td></td></tr> 
               <tr>
                    <td><asp:Label ID="lbl2" runat="server" Text="Method:"></asp:Label></td>  
                    <td><asp:DropDownList ID="drpMethods" runat="server" Height="23px" Width="128px" 
                            DataSourceID="sqlMethods" DataTextField="Name" DataValueField="Method_ID" 
                            AutoPostBack="True">
                        </asp:DropDownList></td> 
                    <td><asp:SqlDataSource ID="sqlMethods" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                            
                            SelectCommand="SELECT [Name], [Method_ID] FROM [WSMethods] WHERE ([WS_ID] = @WS_ID)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="drpWebService" Name="WS_ID" 
                                PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>                   
              </tr> 
              <tr><td></td></tr> 
               <tr>
                    <td><asp:Label ID="lbl3" runat="server" Text="Resource:"></asp:Label></td>   
                    <td><asp:DropDownList ID="drpResources" runat="server" DataSourceID="sqlResources" 
                            DataTextField="Name" DataValueField="Resource_ID" Height="21px" 
                            Width="198px"></asp:DropDownList></td> 
                    <td><asp:SqlDataSource ID="sqlResources" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                            SelectCommand="SELECT [Name], [Resource_ID], [WS_ID] FROM [WSResources]"></asp:SqlDataSource></td>                  
              </tr>               
              <tr><td></td></tr>               
              <tr>
                    <td><asp:Label ID="Label0" runat="server" Text="Rule Name:"></asp:Label></td>
                    <td><asp:TextBox ID="txtRuleName" runat="server" Width="150px" Enabled="False"></asp:TextBox> </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label1" runat="server" Text="Rule ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtRuleID" runat="server" Width="150px" Enabled="False"></asp:TextBox></td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label2" runat="server" Text="Rule Item ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtRuleItemID" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label3" runat="server" Text="Topic ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtTopicID" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label4" runat="server" Text="Topic Desc:"></asp:Label></td>
                    <td><asp:TextBox ID="txtTopicDesc" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label5" runat="server" Text="Level ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtLevelID" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label6" runat="server" Text="Level Desc:"></asp:Label></td>
                    <td><asp:TextBox ID="txtLevelDesc" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label7" runat="server" Text="Domain ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtDomainID" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label8" runat="server" Text="Domain Desc:"></asp:Label></td>
                    <td><asp:TextBox ID="txtDomainDesc" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label9" runat="server" Text="Scope ID:"></asp:Label></td>
                    <td><asp:TextBox ID="txtScopeID" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr> 
              <tr>
                    <td><asp:Label ID="Label10" runat="server" Text="Scope Desc:"></asp:Label></td>
                    <td><asp:TextBox ID="txtScopeDesc" runat="server" Width="150px" Enabled="False"></asp:TextBox>                  
                    </td>
              </tr>  
              <tr><td class="style1"></td></tr>  
              <tr>
                <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" /></td>
              </tr>                
          </table>  
          <tr><td class="style1"></td></tr>     
          <tr><td class="style1"></td></tr> 
          <table>
          <tr><td class="style1"></td></tr> 
          <tr><td class="style1"><b>Assertions for WS</b></td></tr>          
          <tr>
                <td>
                    <asp:GridView ID="AssertionsGrid" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="SqlDataSource4" AutoGenerateDeleteButton="True" 
                        onrowdeleting="AssertionsGrid_RowDeleting" 
                        DataKeyNames="target_WSID,Method_ID,resource_id">
                        <Columns>
                            <asp:BoundField DataField="target_WSID" HeaderText="target_WSID" 
                                InsertVisible="False" ReadOnly="True" SortExpression="target_WSID" />
                            <asp:BoundField DataField="target_WS" HeaderText="target_WS" 
                                SortExpression="target_WS" />
                            <asp:BoundField DataField="Method_ID" HeaderText="Method_ID" 
                                InsertVisible="False" ReadOnly="True" SortExpression="Method_ID" />
                            <asp:BoundField DataField="method_name" HeaderText="method_name" 
                                SortExpression="method_name" />
                            <asp:BoundField DataField="resource_id" HeaderText="resource_id" 
                                InsertVisible="False" ReadOnly="True" SortExpression="resource_id" />
                            <asp:BoundField DataField="resource_name" HeaderText="resource_name" 
                                SortExpression="resource_name" />
                            <asp:BoundField DataField="Rule_ID" HeaderText="Rule_ID" 
                                SortExpression="Rule_ID" ReadOnly="True" InsertVisible="False" />
                            <asp:BoundField DataField="rule_name" HeaderText="rule_name" 
                                SortExpression="rule_name" />
                            <asp:BoundField DataField="rule_item_id" HeaderText="rule_item_id" 
                                SortExpression="rule_item_id" InsertVisible="False" ReadOnly="True" />
                            <asp:BoundField DataField="topic_id" HeaderText="topic_id" 
                                SortExpression="topic_id" />
                            <asp:BoundField DataField="topic_desc" HeaderText="topic_desc" 
                                SortExpression="topic_desc" />
                            <asp:BoundField DataField="level_id" HeaderText="level_id" 
                                SortExpression="level_id" />
                            <asp:BoundField DataField="level_desc" HeaderText="level_desc" 
                                SortExpression="level_desc" />
                            <asp:BoundField DataField="domain_id" HeaderText="domain_id" 
                                SortExpression="domain_id" />
                            <asp:BoundField DataField="domain_desc" HeaderText="domain_desc" 
                                SortExpression="domain_desc" />
                            <asp:BoundField DataField="scope_id" HeaderText="scope_id" 
                                SortExpression="scope_id" />
                            <asp:BoundField DataField="scope_desc" HeaderText="scope_desc" 
                                SortExpression="scope_desc" />
                        </Columns>
                        <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
                    </asp:GridView>
                    
                </td>  
          </tr>        
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="spGetAssertions_WS" SelectCommandType="StoredProcedure" 
                  DeleteCommand="spDelete_AssertionWS_FromGrid" 
                  DeleteCommandType="StoredProcedure">
                  <DeleteParameters>
                      <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                      <asp:Parameter Name="ws_id" Type="Int32" />
                      <asp:Parameter Name="method_id" Type="Int32" />
                      <asp:Parameter Name="rule_id" Type="Int32" />
                      <asp:Parameter Name="rule_item_id" Type="Int32" />
                      <asp:Parameter Name="resource_id" Type="Int32" />

                      <asp:Parameter Name="target_WSID" Type="Int32" />
                      <asp:Parameter Name="topic_id" Type="Int32" />
                      <asp:Parameter Name="level_id" Type="Int32" />
                      <asp:Parameter Name="domain_id" Type="Int32" />
                      <asp:Parameter Name="scope_id" Type="Int32" />
                      <asp:Parameter Name="weight" Type="Double" />
                      <asp:Parameter Name="mandatory_flag" Type="Boolean" />
                      <asp:Parameter Name="target_ws_id" Type="Int32" />
                  </DeleteParameters>
                   <SelectParameters>
                         <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                     </SelectParameters>
              </asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp;</td></tr>
          <tr>
             <td>&nbsp;</td>
          </tr> 
          <tr><td>&nbsp;</td></tr>      
        </table>    

</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .style1
        {
            height: 21px;
        }
        </style>
</asp:Content>


