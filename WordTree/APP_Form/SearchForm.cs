﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WordTree.Model;
using WordTree.Service;

namespace APP_Form
{
    public partial class SearchForm : Form
    {
        private const string Address = @"http://open.iciba.com/dsapi/";

        ///<summary>Search的目标集合</summary>
        List<string> searchTarget = new List<string>();
        ///<summary>Search的结果集合</summary>
        List<string> searchResult = new List<string>();

        WordInfoForm wordInfoForm;

        WordAndDicManager wordAndDicManager = WordAndDicManager.getInstance();
        
        public SearchForm()
        {
            InitializeComponent();
            SentenceInit();
            VocabularyDic allWordsDic = wordAndDicManager.getVocabularyDic("AllWords");
            searchTarget = new List<string>(allWordsDic.List);
        }
        /**<summary>
         * 输入内容时自动触发 Search 方法,
         * 并设置相关的属性.
         * </summary>
         */
        private void comboBox_Searcher_TextUpdate(object sender, EventArgs e)
        {
            Search();
            //使光标保持在最后一栏
            try
            {
                this.comboBox_Searcher.SelectionStart = this.comboBox_Searcher.Text.Length;
            }catch(Exception e2)
            {
                
            }
            //使光标不被遮挡
            Cursor = Cursors.Default;
        }

        /**<summary>
         * 在 searchTarget 中查询,结果存入searchResult,
         * 并更新到comboBox_Searcher.
         * </summary>
         */
        private void Search()
        {
            try
            {
                if (comboBox_Searcher.Text == "")
                {
                    pnl_wordInfo.Controls.Clear();
                    comboBox_Searcher.Items.Clear();
                    comboBox_Searcher.DroppedDown = false;
                    pnl_wordInfo.Controls.Add(panel_sentence);
                }
                else
                {
                    comboBox_Searcher.Items.Clear();
                    searchResult.Clear();
                    comboBox_Searcher.DroppedDown = true;
                    foreach (var wordstr in searchTarget)
                    {
                        if (wordstr.StartsWith(comboBox_Searcher.Text.ToLower()))
                        {
                            searchResult.Add(wordstr);
                        }
                    }
                    comboBox_Searcher.Items.AddRange(searchResult.ToArray());
                }
            }catch(Exception e)
            {
                pnl_wordInfo.Controls.Add(panel_sentence);
            }
        }

        private void comboBox_Searcher_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (wordInfoForm != null)
                {
                    wordInfoForm.Dispose();
                }
                if (comboBox_Searcher.SelectedIndex >= 0)
                {
                    pnl_wordInfo.Controls.Clear();
                    string wordStr = comboBox_Searcher.SelectedItem.ToString().Replace(' ', '_');
                    Word targetWord = wordAndDicManager.getWord(wordStr);
                    wordInfoForm = new WordInfoForm(targetWord);
                    wordInfoForm.TopLevel = false;
                    wordInfoForm.FormBorderStyle = FormBorderStyle.None;
                    wordInfoForm.Dock = DockStyle.Fill;
                    pnl_wordInfo.Controls.Add(wordInfoForm);
                    wordInfoForm.Show();
                }
            }
            catch(Exception ex)
            {

            }
        }
        /// <summary>
        /// 初始化每日一句
        /// </summary>
        private void SentenceInit()
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string apiResult = webClient.DownloadString(Address);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(apiResult); ;
            object content;
            object note;
            object picture;
            try
            {
                json.TryGetValue("content", out content);
                json.TryGetValue("note", out note);
                json.TryGetValue("picture2", out picture);
                label_content.Text = content.ToString();
                label_note.Text = note.ToString();
                var webC = new System.Net.WebClient();
                var bmp = new Bitmap(webC.OpenRead(picture.ToString()));
                pictureBox1.Image = bmp;
            }
            catch (Exception laz)
            {

            }
         }

    }
}
