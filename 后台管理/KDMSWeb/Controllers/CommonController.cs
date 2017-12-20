using KDMSModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace KDMSWeb.Controllers
{
    public class CommonController : Controller
    {

        // GET: Common

        KDMSEntities db = new KDMSEntities();
        /// <summary>  
        /// 分页查询 + 条件查询 + 排序  
        /// </summary>  
        /// <typeparam name="Tkey">泛型</typeparam>  
        /// <param name="pageSize">每页大小</param>  
        /// <param name="pageIndex">当前页码</param>  
        /// <param name="total">总数量</param>  
        /// <param name="whereLambda">查询条件</param>  
        /// <param name="orderbyLambda">排序条件</param>  
        /// <param name="isAsc">是否升序</param>  
        /// <returns>IQueryable 泛型集合</returns>  
        public IQueryable<T> LoadPageItems<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc)
        {
            total = db.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                var temp = db.Set<T>().Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda)
                             .Skip(pageSize * (pageIndex - 1))
                             .Take(pageSize);
                return temp.AsQueryable();
            }
            else
            {
                var temp = db.Set<T>().Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize);
                return temp.AsQueryable();
            }
        }
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dpid"></param>
        /// <param name="address"></param>
        /// <param name="created"></param>
        /// <returns></returns>
        public static IEnumerable<Restaurant> Search(string name = null, string dpid = null, string address = null, DateTime? created = null)
        {
            //为了模拟EF查询，转换为IEnumerable,在EF中此处为数据库上下文的表对象  
            IEnumerable<Restaurant> result = new List<Restaurant>();

            /*下列代码不会立即执行查询，而是生成查询计划 
             * 若参数不存在则不添加查询条件，从而可以无限制的添加查询条件 
             */
            if (!string.IsNullOrEmpty(name)) { result = result.Where(d => d.Name.Contains(name)); }
            if (!string.IsNullOrEmpty(dpid)) { result = result.Where(d => d.DPId.Contains(dpid)); }
            if (!string.IsNullOrEmpty(address)) { result = result.Where(d => d.Address.Contains(address)); }
            if (created.HasValue) { result = result.Where(d => d.created == created); }


            return result;
        }
    }

    //public class LoadPage<T> : Controller where T : new()
    //{
    //    KDMSEntities db = new KDMSEntities();
    //    public static IQueryable<T> LoadPageItems<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc)
    //    {            
    //        total = db.Set<T>().Where(whereLambda).Count();
    //        if (isAsc)
    //        {
    //            var temp = db.Set<T>().Where(whereLambda)
    //                         .OrderBy<T, Tkey>(orderbyLambda)
    //                         .Skip(pageSize * (pageIndex - 1))
    //                         .Take(pageSize);
    //            return temp.AsQueryable();
    //        }
    //        else
    //        {
    //            var temp = db.Set<T>().Where(whereLambda)
    //                       .OrderByDescending<T, Tkey>(orderbyLambda)
    //                       .Skip(pageSize * (pageIndex - 1))
    //                       .Take(pageSize);
    //            return temp.AsQueryable();
    //        }
    //    }
    //}
}