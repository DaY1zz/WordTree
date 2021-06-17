﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordTree.Model;

namespace WordTree.Service
{
    /// <summary>
    /// 管理单词计划，增删改查数据库
    /// </summary>
    public class MmryPlanManager
    {
        /// <summary>
        /// 增加计划记录
        /// </summary>
        /// <param name="wordStr"></param>
        public void AddPlan(string wordStr)
        {
            PlannedWord plannedWord = new PlannedWord(wordStr, DateTime.Now, -1);
            using (var ctx = new PlannedWordContext())
            {
                if (ctx.PlannedWords.Where(o => o.Wordstr == wordStr).Any())
                {
                    throw new ApplicationException("该单词已经在计划中");
                }
                ctx.PlannedWords.Add(plannedWord);
                ctx.SaveChanges();

                //Console.WriteLine("Successfully add!");
            }
        }

        /// <summary>
        /// 清空计划
        /// </summary>
        public void ClearAll()
        {
            using (var ctx = new PlannedWordContext())
            {
                var list = ctx.PlannedWords.ToList();
                ctx.PlannedWords.RemoveRange(list);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="wordStr"></param>
        public void DelPlan(string wordStr)
        {
            using(var ctx = new PlannedWordContext())
            {
                var targetWord = ctx.PlannedWords.SingleOrDefault(o => o.Wordstr == wordStr);

                if (targetWord == null)
                    throw new ApplicationException("该单词不在计划中");
                else
                {
                    ctx.PlannedWords.Remove(targetWord);
                    ctx.SaveChanges();
                }
                
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="plannedWord"></param>
        public void UpdatePlan(PlannedWord plannedWord)
        {
            if (plannedWord == null)
                throw new NullReferenceException("空记录");

            using(var ctx = new PlannedWordContext())
            {
                DelPlan(plannedWord.Wordstr);
                ctx.PlannedWords.Add(plannedWord);
                ctx.SaveChanges();
                
            }
        }

        /// <summary>
        /// 按上次记忆时间查询所有记录
        /// </summary>
        /// <returns></returns>
        public List<PlannedWord> QueryAll()
        {
            using(var ctx = new PlannedWordContext())
            {
                return ctx.PlannedWords.OrderBy(o=>o.LastMmryTime).ToList();
            }
        }

        /// <summary>
        /// 按单词查询记录
        /// </summary>
        /// <param name="wordStr"></param>
        /// <returns></returns>
        public PlannedWord QueryByWordStr(string wordStr)
        {
            using(var ctx = new PlannedWordContext())
            {
                var targetWord = ctx.PlannedWords.SingleOrDefault(o => o.Wordstr == wordStr);

                if (targetWord == null)
                    throw new ApplicationException("该单词不在计划中");
                return targetWord;
            }
        }
        


    }
}