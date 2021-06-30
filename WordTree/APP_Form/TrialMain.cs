﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APP_Form.Controller;
using HZH_Controls.Forms;
using Reader;
using StatTracer;
using WordTree.Service;

namespace APP_Form
{
    
    
    public partial class TrialMain : FrmWithTitle
    {
        public TraceForm traceForm = new TraceForm();
        public SearchForm searchForm = new SearchForm();
        public ReaderForm readerForm = new ReaderForm();
        public MemoryForm memoryForm = new MemoryForm();
        public TransferController transferController = TransferController.GetController();
        public SelectWordsForm selectform = new SelectWordsForm();

        TreeNode dictNode = new TreeNode("         词典");
        TreeNode statNode = new TreeNode("         统计");
        TreeNode readNode = new TreeNode("         阅读");
        TreeNode mryNode = new TreeNode("         记忆");
        public TrialMain()
        {
            InitializeComponent();
            
        }


        private async void TrialMain_Load(object sender, EventArgs e)
        {
            this.tvMenu.Nodes.Add(dictNode);
            this.tvMenu.Nodes.Add(statNode);
            this.tvMenu.Nodes.Add(mryNode);
            this.tvMenu.Nodes.Add(readNode);

            //await Task.Run(() => 
            //{
            //    traceForm.FormInit(traceForm.TimeLine1, traceForm.yesterday);
            //    traceForm.FormInit(traceForm.TimeLine2, traceForm.today);
            //    traceForm.FormInit(traceForm.TimeLine3, traceForm.tomorrow);
            //});

        }

        private void tvMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panControl.Controls.Clear();
            string strName = e.Node.Text.Trim();
            this.Title = strName;
            switch (strName)
            {
                case "词典":
                    transferController.Transfer(panControl, searchForm);
                    break;
                case "统计":
                    transferController.Transfer(panControl, traceForm);
                    break;
                case "阅读":
                    transferController.Transfer(panControl, readerForm);
                    break;
                case "记忆":
                    transferController.Transfer(panControl, memoryForm);
                    memoryForm.Memory(null, null);
                    break;
                
            }
            memoryForm.GenerateInfo('a');
        }

        private void SetParaType(string selectedtype)
        {
            if (readerForm != null)
            {
                readerForm.Dispose();
            }
            readerForm = new ReaderForm(selectedtype);
        }

        private void TomorrowRecordUpdate()
        {
            traceForm.TomorrowRecordInit();
            traceForm.TomorrowFormUpdate();
        }

        private void btnSetting_BtnClick(object sender, EventArgs e)
        {
            var settingForm = new SettingForm();
            settingForm.setType += new SetType(SetParaType);
            settingForm.Show();
        }

        private void TrialMain_Shown(object sender, EventArgs e)
        {
            //初始时，弹出设置窗口
            using (PlannedWordContext ctx = new PlannedWordContext())
            {
                if (ctx.DictionaryWords.Count() == 0)
                {
                    var settingForm = new SettingForm();
                    settingForm.setType += new SetType(SetParaType);
                    settingForm.Show();
                }
            }
        }

        private void btnSelect_BtnClick(object sender, EventArgs e)
        {
            selectform = new SelectWordsForm();
            selectform.set += new SetRecord(TomorrowRecordUpdate);
            selectform.Show();
        }

        private void btnExit_BtnClick(object sender, EventArgs e)
        {
            memoryForm.GenerateInfo('b');
            Application.Exit();
        }
    }
}
