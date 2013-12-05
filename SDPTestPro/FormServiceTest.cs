using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDP.Client;
using SDP.Common;
using SDP;
using System.Configuration;
using System.Collections;
using SDP.Util;
using SDP.Data.Trans;

namespace SDPForm
{
    public partial class Form_ServiceTest : Form
    {
        public Form_ServiceTest()
        {
            InitializeComponent();

            SystemContext.Init();

            string webAddress = ConfigurationManager.AppSettings["webAddress"];
            if (webAddress == null || webAddress.Equals(""))
                webAddress = "http://localhost:8181";

            string webName = ConfigurationManager.AppSettings["webName"];
            if (webName == null || webName.Equals(""))
                webName = "SdpFrameworkWeb";

            textBox_serverHost.Text = string.Format("{0}/{1}", webAddress, webName);
        }


        private void comboBox_proCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_proCode.SelectedIndex > -1)
                SDPClient.ProCode = comboBox_proCode.SelectedItem.ToString();
            else
                SDPClient.ProCode = "";
        }

        private bool CallService(InParams inparams, OutParams outparams)
        {
            DateTime begin = DateTime.Now;
            try
            {
                toolStripStatusLabel_time.Text = "正在调用远程服务，请稍候...";
                statusStrip_state.Update();
                inparams.SetRouteType(textBox_routeType.Text);
                SDPClient.RouteService(inparams, outparams);
                toolStripStatusLabel_time.Text = String.Format("远程服务执行成功，耗时：{0:c}", DateTime.Now - begin);
                ShowServiceResult(outparams);
                return true;
            }
            catch (Exception e)
            {
                toolStripStatusLabel_time.Text = String.Format("远程服务执行失败，耗时：{0:c}", DateTime.Now - begin);
                richTextBox_result.Text = String.Format("远程服务执行失败：\n\r{0}", e);
                ClearResultPage();
                return false;
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            string addr = textBox_serverHost.Text.Trim();
            SDPClient.InitComRequest(String.Format("{0}/services/ComRequest", addr));
            this.Text = string.Format("远程服务测试 - ({0})", addr);

            comboBox_proCode.Items.Clear();
            try
            {
                InParams inparams = SDPClient.NewInParams(null);
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetServiceName("EnterServices");
                inparams.SetServiceItem("GetProjectNames");

                if (CallService(inparams, outparams))
                {
                    DataTable table = outparams.GetTableParamValue(0);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        comboBox_proCode.Items.Add(table.Rows[i]["F_PROJ_CODE"]);
                    }
                    if (comboBox_proCode.Items.Count > 0)
                        comboBox_proCode.SelectedIndex = 0;

                    comboBox_proCode.Items.Add("");
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine("初始化项目名称列表失败：{0}", ex);
            }
        }

        private void button_serverVer_Click(object sender, EventArgs e)
        {
            InParams inparams = SDPClient.NewInParams(null);
            OutParams outparams = SDPClient.NewOutParams();

            inparams.SetServiceName("GetSystemVersion");

            CallService(inparams, outparams);
        }

        private void button_refreshServer_Click(object sender, EventArgs e)
        {
            InParams inparams = SDPClient.NewInParams("SystemEnter");
            OutParams outparams = SDPClient.NewOutParams();

            inparams.SetServiceName("RefreshServices");

            CallService(inparams, outparams);
        }

        private void button_callService_Click(object sender, EventArgs e)
        {
            InParams inparams = SDPClient.NewInParams(comboBox_proCode.Text.Trim());
            OutParams outparams = SDPClient.NewOutParams();

            inparams.SetRequestBody(textBox_reqBody.Text.Trim());
            inparams.SetRequestParams(textBox_reqParams.Text.Trim());

            CallService(inparams, outparams);
        }


        private void button_datarule_Click(object sender, EventArgs e)
        {
            InParams inparams = SDPClient.NewInParams(comboBox_proCode.Text.Trim());
            OutParams outparams = SDPClient.NewOutParams();

            string proCode = comboBox_proCode.Text.Trim();

            inparams.SetServiceName("DataRuleServices");
            inparams.SetServiceItem("SaveDataRuleToFile");
            inparams.SetRequestBody("TargetProCode", proCode);
            inparams.SetRequestBody("RuleName", "");
            inparams.SetRequestBody("Level", "");

            CallService(inparams, outparams);
        }

        private void button_refreshProj_Click(object sender, EventArgs e)
        {
            InParams inparams = SDPClient.NewInParams(comboBox_proCode.Text.Trim());
            OutParams outparams = SDPClient.NewOutParams();

            inparams.SetServiceName("RefreshServices");

            CallService(inparams, outparams);
        }

        private void ClearResultPage()
        {
            tabControl_result.TabPages.Clear();
            tabPage_result.Parent = tabControl_result;
        }

        private void ShowServiceResult(OutParams outparams)
        {
            richTextBox_result.Text = outparams.GetParams();

            ArrayList list = new ArrayList();
            list.Add(tabPage_result.Name);

            string tabname = "";

            int count = outparams.GetStrParamsCount();
            if (count > 0)
            {
                tabname = "__ResultParamList";

                if (!tabControl_result.TabPages.ContainsKey(tabname))
                {
                    TabPage page = new TabPage();
                    page.Name = tabname;
                    page.Text = "变量列表";

                    DataTable table = new DataTable();
                    DataColumn column = table.Columns.Add();
                    column.Caption = "变量名称";
                    column.ColumnName = "变量名称";
                    column.DataType = System.Type.GetType("System.String");

                    column = table.Columns.Add();
                    column.Caption = "变量值";
                    column.ColumnName = "变量值";
                    column.DataType = System.Type.GetType("System.String");

                    DataGridView grid = new DataGridView();
                    grid.Parent = page;
                    grid.Dock = DockStyle.Fill;
                    grid.DataSource = table.DefaultView;
                    page.Tag = table;

                    for (int i = 0; i < count; i++)
                    {
                        IParam param = outparams.GetStrParam(i);
                        DataRow row = table.NewRow();
                        row["变量名称"] = param.GetName();
                        row["变量值"] = param.GetValue();
                        table.Rows.Add(row);
                    }
                    table.AcceptChanges();

                    page.Parent = tabControl_result;
                }
                else
                {
                    DataTable table = tabControl_result.TabPages[tabname].Tag as DataTable;
                    if (table != null)
                    {
                        table.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            IParam param = outparams.GetStrParam(i);
                            DataRow row = table.NewRow();
                            row["变量名称"] = param.GetName();
                            row["变量值"] = param.GetValue();
                            table.Rows.Add(row);
                        }
                        table.AcceptChanges();
                    }
                }

                list.Add(tabname);
            }

            count = outparams.GetTableParamsCount();
            if (count > 0)
            {                
                for (int i = 0; i < count; i++)
                {
                    DataTable table = outparams.GetTableParamValue(i);

                    TableUtil.SetProperty(table, "DataSource", outparams.GetStrParamValue("DataSource"));
                    TableUtil.SetProperty(table, "Command", outparams.GetStrParamValue("Command"));

                    tabname = table.TableName.ToUpper();

                    list.Add(tabname);

                    if (!tabControl_result.TabPages.ContainsKey(tabname))
                    {
                        TabPage page = new TabPage();
                        page.Name = tabname;
                        page.Text = tabname;
                        DataGridView grid = new DataGridView();
                        grid.Parent = page;
                        grid.Dock = DockStyle.Fill;
                        grid.Tag = table;

                        //grid.DataSource = table.DefaultView;
                        DataUIUtil.InitDataGridViewColumns(grid, table);

                        page.Tag = grid;
                        page.Parent = tabControl_result;
                    }
                    else
                    {
                        DataGridView grid = tabControl_result.TabPages[tabname].Tag as DataGridView;
                        if (grid != null)
                        {
                            grid.Tag = table;

                            //grid.DataSource = table.DefaultView;
                            DataUIUtil.InitDataGridViewColumns(grid, table);
                        }
                    }
                }
            }

            IEnumerator it = tabControl_result.TabPages.GetEnumerator();
            while (it.MoveNext())
            {
                TabPage page = it.Current as TabPage;
                if (page!=null && !list.Contains(page.Name))
                {
                    tabControl_result.TabPages.Remove(page);
                }
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            DateTime begin = DateTime.Now;
            TabPage page = tabControl_result.SelectedTab;
            if (page != null)
            {
                try
                {
                    DataGridView grid = page.Tag as DataGridView;
                    if (grid != null)
                    {
                        DataTable table = grid.Tag as DataTable;
                        if (table != null)
                        {
                            toolStripStatusLabel_time.Text = "正在调用远程服务，请稍候...";
                            statusStrip_state.Update();

                            int count = 0;
                            ITransactior tran = new Transactior();
                            tran.Begin();
                            try
                            {
                                tran.Post(table);
                                if (!tran.IsChanged())
                                {
                                    toolStripStatusLabel_time.Text = string.Format("数据集[{0}]没有需要保存的数据.", page.Text);
                                    statusStrip_state.Update();
                                    tran.Commit();
                                    return;
                                }
                                count = tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }

                            toolStripStatusLabel_time.Text = String.Format("保存数据集[{0}]执行成功({1})，耗时：{2:c}", page.Text, count, DateTime.Now - begin);
                        }
                    }
                }
                catch (Exception exc)
                {
                    toolStripStatusLabel_time.Text = String.Format("保存数据集[{0}]执行失败，耗时：{0:c}", page.Text, DateTime.Now - begin);
                    richTextBox_result.Text = String.Format("保存数据集[{0}]执行失败：\n\r{0}", page.Text, exc);
                    ClearResultPage();
                }
            }
        }
    }
}
