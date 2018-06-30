<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="ReferenceTables.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
          <tr>
              <td><font size="5"><b>Reference Tables - Topic, Level, Domain, Scope, Privacy Rule Set Items</b></font></td>
          </tr>
          <tr>             
              <td> <font color="red"><b><asp:Label ID="lblMessageToUser" runat="server" Text=""></asp:Label>                  
                  </b></font>
              </td>
          </tr>              
          <tr><td>&nbsp</td></tr>  
           <tr><td><b>Topic</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource1" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="Topic_ID" HeaderText="Topic_ID" 
                          SortExpression="Topic_ID" InsertVisible="False" ReadOnly="True" />
                      <asp:BoundField DataField="Description" HeaderText="Description" 
                          SortExpression="Description" />
                  </Columns>
                   <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="SELECT [Topic_ID], [Description] FROM [Topic]"></asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp</td></tr>    
           <tr><td><b>Level</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource2" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="Level_ID" HeaderText="Level_ID" 
                          SortExpression="Level_ID" InsertVisible="False" ReadOnly="True" />
                      <asp:BoundField DataField="Description" HeaderText="Description" 
                          SortExpression="Description" />
                  </Columns>
                   <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="SELECT [Level_ID], [Description] FROM [Level]"></asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp</td></tr> 
          <tr><td><b>Domain</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource3" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="Domain_ID" HeaderText="Domain_ID" 
                          SortExpression="Domain_ID" InsertVisible="False" ReadOnly="True" />
                      <asp:BoundField DataField="Description" HeaderText="Description" 
                          SortExpression="Description" />
                  </Columns>
                   <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  
                  
                  SelectCommand="SELECT [Domain_ID], [Description] FROM [Domain]"></asp:SqlDataSource>
              </td></tr> 
          <tr><td>&nbsp</td></tr> 
           <tr><td><b>Scope</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource4" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="Scope_ID" HeaderText="Scope_ID" 
                          SortExpression="Scope_ID" InsertVisible="False" ReadOnly="True" />
                      <asp:BoundField DataField="Description" HeaderText="Description" 
                          SortExpression="Description" />
                  </Columns>
                   <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="SELECT [Scope_ID], [Description] FROM [Scope]"></asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp</td></tr>           
          <tr><td>&nbsp</td></tr> 
          <tr><td>&nbsp;</td></tr>                                                        
            <tr><td><b>Add New Rule Item</b></td></tr>
            <tr><td>
                <font color="red"><asp:Label ID="lblRuleAdd" runat="server" Text=""></asp:Label></font>
                </td></tr> 
            <tr><td><b>Rule</b></td></tr>
            <tr><td>
                <asp:DropDownList ID="drpRule" runat="server" Width="163px" 
                    DataSourceID="SqlDataSource5" DataTextField="Name" DataValueField="Rule_ID">
                </asp:DropDownList>
                </td>
            </tr>
            <tr><td>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                    SelectCommand="SELECT [Rule_ID], [Name] FROM [PrivacyRuleSet]">
                </asp:SqlDataSource>
                </td></tr> 
            <tr><td><b>Topic</b></td></tr>
            <tr><td>
                <asp:DropDownList ID="drpTopic" runat="server" Width="163px" 
                    DataSourceID="SqlDataSource6" DataTextField="Description" 
                    DataValueField="Topic_ID">
                </asp:DropDownList>
                </td>
            </tr>  
            <tr><td>
                <asp:SqlDataSource ID="SqlDataSource6" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                    SelectCommand="SELECT [Topic_ID], [Description] FROM [Topic]">
                </asp:SqlDataSource>
                </td></tr> 
            <tr><td><b>Level</b></td></tr>
            <tr><td>
                <asp:DropDownList ID="drpLevel" runat="server" Width="163px" 
                    DataSourceID="SqlDataSource7" DataTextField="Description" 
                    DataValueField="Level_ID">
                </asp:DropDownList>
                </td>
            </tr>  
            <tr><td>
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                    SelectCommand="SELECT [Level_ID], [Description] FROM [Level]">
                </asp:SqlDataSource>
                </td></tr> 
            <tr><td><b>Domain</b></td></tr>
            <tr><td>
                <asp:DropDownList ID="drpDomain" runat="server" Width="163px" 
                    DataSourceID="SqlDataSource8" DataTextField="Description" 
                    DataValueField="Domain_ID">
                </asp:DropDownList>
                </td>
            </tr>  
            <tr><td>
                <asp:SqlDataSource ID="SqlDataSource8" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                    SelectCommand="SELECT [Domain_ID], [Description] FROM [Domain]">
                </asp:SqlDataSource>
                </td></tr> 
            <tr><td><b>Scope</b></td></tr>
            <tr><td>
                <asp:DropDownList ID="drpScope" runat="server" Width="163px" 
                    DataSourceID="SqlDataSource9" DataTextField="Description" 
                    DataValueField="Scope_ID">
                </asp:DropDownList>
                </td>
            </tr>  
            <tr><td>
                <asp:SqlDataSource ID="SqlDataSource9" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                    SelectCommand="SELECT [Scope_ID], [Description] FROM [Scope]">
                </asp:SqlDataSource>
                </td></tr>
            <tr><td>&nbsp</td></tr>
            
            <tr>
                <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" /></td>
              </tr>  
            <tr><td>&nbsp;</td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td><b>Privacy Rules Set Items (Available to Be Assigned to Client or Web Service)</b></td></tr>
            <tr><td>
                <font color="red"><asp:Label ID="lblPrivacyRuleSetItems" runat="server" Text=""></asp:Label></font>
            </td></tr> 
            <tr><td>&nbsp;</td></tr>
            <tr><td class="style1">
                <asp:GridView ID="grdPrivacyRuleSetItems" runat="server" Width="796px" 
                    AutoGenerateColumns="False" DataSourceID="SqlDataSource10" 
                    AutoGenerateDeleteButton="True" 
                    onrowdeleting="grdPrivacyRuleSetItems_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="rule_name" HeaderText="rule_name" 
                            SortExpression="rule_name" />
                        <asp:BoundField DataField="Rule_ID" HeaderText="Rule_ID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="Rule_ID" />
                        <asp:BoundField DataField="rule_item_id" HeaderText="rule_item_id" 
                            InsertVisible="False" ReadOnly="True" SortExpression="rule_item_id" />
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
                 <asp:SqlDataSource ID="SqlDataSource10" runat="server" 
                     ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                     SelectCommand="spGetPrivacyRuleSetItems" 
                     SelectCommandType="StoredProcedure" DeleteCommand="spDelete_PrivacyRuleSetItem" 
                     DeleteCommandType="StoredProcedure">
                     <DeleteParameters>
                         <asp:Parameter Name="rule_id" Type="Int32" />
                         <asp:Parameter Name="topic_id" Type="Int32" />
                         <asp:Parameter Name="level_id" Type="Int32" />
                         <asp:Parameter Name="domain_id" Type="Int32" />
                         <asp:Parameter Name="scope_id" Type="Int32" />
                     </DeleteParameters>
                 </asp:SqlDataSource>
                 </td></tr>
                 <tr><td>&nbsp</td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>&nbsp</td></tr>
            <tr><td>&nbsp;</td></tr>
        </table>    

</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .style1
        {
            height: 125px;
        }
    </style>
    </asp:Content>


